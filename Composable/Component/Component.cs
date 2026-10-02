// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;
using Soe.Collections.Embedded;
using Soe.Threading;

namespace Soe.Composable
{
    /// <summary>
    /// Stores pure data structures of type <typeparamref name="T"/> defining specific properties of an entity in a component pool 
    /// </summary>
    /// <typeparam name="T">The pure data structure defining specific properties</typeparam>
    #if EXPORT_HAMPER_CORE_COMPOSITION
    public
    #else
    internal
    #endif
    class Component<T> : SparseMap, IComponent, IReadOnlyIterable<EntityId, ReadOnlyIterator<EntityId>.DefaultStrategy>, IReadOnlySequence<EntityId>, IIterable<T, Iterator<T>.DefaultStrategy>, ISequence<T>, IBorrowAnchor
        where T : struct
    {
        private readonly Shard shard;
        /// <summary>
        /// Gets the <see cref="Shard"/> this component pool belongs to
        /// </summary>
        public int ShardId
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return shard.Id; }
        }
        
        private EmbeddedList<EntityId, HeapArray<EntityId>> entities;
        private EmbeddedList<T, HeapArray<T>> components;
        private IComponentGroup? group;
        
        /// <inheritdoc/>
        public new int Count
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                // Requires at least immutable access when scheduled
                AccessManager.ThrowOnLessAccessible<Component<T>>(this, AccessType.Immutable);
                
                return components.Count;
            }
        }

        /// <summary>
        /// Gets the component data at the specific index
        /// </summary>
        /// <param name="index">The index to get the component data for</param>
        internal ref T this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                // Requires at least immutable access when scheduled
                AccessManager.ThrowOnLessAccessible<Component<T>>(this, AccessType.Immutable);

                return ref components[index];
            }
        }

        /// <summary>
        /// Initializes the component pool on a specific <see cref="Shard"/>
        /// </summary>
        /// <param name="shard">The <see cref="Shard"/> this component pool belongs to</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal Component(Shard shard)
        {
            this.shard = shard;
            this.entities = default;
            this.components = default;
            this.group = null;
        }
        
        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Span<T> AsSpan()
        {
            // Requires at least immutable access when scheduled
            AccessManager.ThrowOnLessAccessible<Component<T>>(this, AccessType.Mutable);

            return components.AsSpan();
        }
        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlySpan<EntityId> AsReadOnlySpan()
        {
            // Requires at least immutable access when scheduled
            AccessManager.ThrowOnLessAccessible<Component<T>>(this, AccessType.Immutable);

            return entities.AsSpan();
        }

        /// <summary>
        /// Tries to add this component pool to the given <see cref="IComponentGroup"/> instance
        /// </summary>
        /// <param name="componentGroup">The group instance this component pool should belong to</param>
        /// <returns>True if the component pool was successfully added, false otherwise</returns>
        /// <remarks>A component pool can be managed by only a single group at a time</remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal bool AttachGroup(IComponentGroup componentGroup)
        {
            // Requires mutable access when scheduled
            AccessManager.ThrowOnAccessViolation<Component<T>>(this, AccessType.Mutable);
            
            if (group == null)
            {
                group = componentGroup;
                return true;
            }
            else return false;
        }
        
        /// <summary>
        /// Attempts to add a component of type <typeparamref name="T"/> to the given entity
        /// </summary>
        /// <param name="entity">The entity a component should be added to</param>
        /// <returns>A reference to the added or existing component data</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the entity belongs to a shard not managing
        /// this component pool</exception>
        public ref T Add(EntityId entity)
        {
            if(entity.ShardId == shard.Id)
            {
                // Requires mutable access when scheduled
                AccessManager.ThrowOnAccessViolation<Component<T>>(this, AccessType.Mutable);
                IMemoryAllocator allocator = shard;
                
                int slot = entity.Index >> MemoryAllocator.BlockShift;
                if (!Find(slot, out int index, out int distance, out Ref<MemoryHandle> handle))
                {
                    // Entity slot is new to this container, add it
                    ref MemoryHandle tmp = ref Emplace(slot, index, distance, Version);
                    if (!tmp.IsValid)
                    {
                        // Block is uninitialized, initialize it to prevent false positives
                        tmp = allocator.Allocate(MemoryAllocator.BlockSize);
                        allocator.InitializeBlock(tmp, 0, EntityId.Invalid);
                        
                        handle = new Ref<MemoryHandle>(ref tmp);
                    }
                }

                // Look the entity up in the sparse map
                EntityId entityPtr = allocator.Access(handle.Value, entity.Index & MemoryAllocator.BlockMask);
                if (Compare(entity, entityPtr))
                {
                    // Entity exists and is alive
                    index = entityPtr.Index;
                }
                else
                {
                    // Entity is new to this component, add it
                    index = components.Count;
                    components.Add(default);
                    entities.Add(entity);
                    Version++;

                    // Write a modified version of entity to its slot in the sparse map so entity.Index -> dense index
                    allocator.Access(handle.Value, entity.Index & MemoryAllocator.BlockMask, new EntityId(index, entity.Version, entity.ShardId, entity.Flags));
                    
                    // Notify listeners
                    Volatile.Read(ref group)?.ComponentAdded<T>(entity, ref index);
                }
                return ref components[index];
            }
            else throw new ArgumentOutOfRangeException(nameof(entity.ShardId));
        }
        
        /// <inheritdoc/>
        public void Clear()
        {
            IMemoryAllocator allocator = shard;
            for (int i = Capacity - 1; i >= 0; i--)
            {
                if (data?[i].IsValid ?? false)
                {
                    allocator.Free(data[i].Handle);
                    data[i] = default;   
                }
            }
            entities.Clear();
            components.Clear();
            Version++;
            count = 0;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        bool Compare(EntityId entity, EntityId entityPtr)
        {
            // Sanitize entities
            entity &= ~EntityId.FlagBits;
            entityPtr &= ~EntityId.FlagBits;
            
            // Check if entity matches version, shard and reserved bit
            return (((~EntityId.Null & entity) ^ entityPtr) < EntityId.Null);
        }

        /// <summary>
        /// Searches for the specified entity and returns the index of its component data in the pool
        /// </summary>
        /// <param name="entity">The entity to locate in the component pool</param>
        /// <returns>The index of the component data in the pool, if found; otherwise, the lower bound of the pool minus 1</returns>
        internal int IndexOf(EntityId entity)
        {
            // Requires mutable access when scheduled
            AccessManager.ThrowOnLessAccessible<Component<T>>(this, AccessType.Immutable);
            
            if (Find(entity.Index >> MemoryAllocator.BlockShift, out _, out _, out Ref<MemoryHandle> handle))
            {
                IMemoryAllocator allocator = shard;
                
                // Look the entity up in the sparse map
                EntityId entityPtr = allocator.Access(handle.Value, entity.Index & MemoryAllocator.BlockMask);
                if (Compare(entity, entityPtr))
                {
                    // Entity exists and is alive
                    return entityPtr.Index;
                }
            }
            return -1;
        }

        /// <summary>
        /// Attempts to remove a component of type <typeparamref name="T"/> and it's component data from the given entity
        /// </summary>
        /// <param name="entity">The entity the component should be removed from</param>
        /// <returns>True if the component data was successfully removed from the pool, false otherwise</returns>
        /// <exception cref="AccessViolationException">Thrown if pool was unable to reorder the component storage. This might
        /// indicate a corrupted memory block</exception>
        public bool Remove(EntityId entity)
        {
            // Requires mutable access when scheduled
            AccessManager.ThrowOnAccessViolation<Component<T>>(this, AccessType.Mutable);
            
            if (Find(entity.Index >> MemoryAllocator.BlockShift, out _, out _, out Ref<MemoryHandle> handle))
            {
                IMemoryAllocator allocator = shard;
                
                // Check if entity is alive
                EntityId entityPtr = allocator.Access(handle.Value, entity.Index & MemoryAllocator.BlockMask);
                if (Compare(entity, entityPtr) && !entityPtr.FlagSet(EntityFlags.Locked))
                {
                    // Mark component as removed by adding the reserved flag
                    allocator.Access(handle.Value, entity.Index & MemoryAllocator.BlockMask, new EntityId(entityPtr.Index, entityPtr.Version, entityPtr.ShardId, EntityFlags.Reserved));
                    
                    // Notify group
                    {
                        int componentIndex = entityPtr.Index;
                        Volatile.Read(ref group)?.ComponentRemoved<T>(entity, ref componentIndex);
                        entityPtr = new EntityId(componentIndex, entityPtr.Version, entityPtr.ShardId, entityPtr.Flags);
                    }
                    if (entityPtr.Index < components.Count - 1)
                    {
                        // Swap entity data with last entity
                        EntityId swap = entities[components.Count - 1];
                        if (Find(swap.Index >> MemoryAllocator.BlockShift, out _, out _, out handle))
                        {
                            EntityId tmp = allocator.Access(handle.Value, swap.Index & MemoryAllocator.BlockMask);
                            allocator.Access(handle.Value, swap.Index & MemoryAllocator.BlockMask, new EntityId(entityPtr.Index, tmp.Version, tmp.ShardId, tmp.Flags));

                            SwapValues(entityPtr.Index, tmp.Index);
                        }
                        else throw new AccessViolationException();
                    }

                    int index = components.Count - 1;
                    components.RemoveAt(index);
                    entities.RemoveAt(index);
                    Version++;
                    
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Tries to remove this component pool from the given <see cref="IComponentGroup"/> instance
        /// </summary>
        /// <param name="componentGroup">The group instance this component pool belongs to</param>
        /// <returns>True if the component pool was successfully removed, false otherwise</returns>
        /// <remarks>A component pool can be managed by only a single group at a time. Only the managing
        /// group can be removed</remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal bool ReleaseGroup(IComponentGroup componentGroup)
        {
            // Requires mutable access when scheduled
            AccessManager.ThrowOnAccessViolation<Component<T>>(this, AccessType.Mutable);

            if (group == componentGroup)
            {
                group = null;
                return true;
            }
            else return false;
        }
        
        /// <summary>
        /// Reorders the component pool by switching the provided indices with each other
        /// </summary>
        /// <param name="source">The source index</param>
        /// <param name="target">The target index</param>
        /// <returns>True if both source and target have been successfully switched with each other, false otherwise</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal bool Swap(int source, int target)
        {
            // Requires mutable access when scheduled
            AccessManager.ThrowOnAccessViolation<Component<T>>(this, AccessType.Mutable);
            
            if (Swap(entities[source], entities[target]))
            {
                SwapValues(source, target);
                Version++;
                return true;
            }
            else return false;
        }
        bool Swap(EntityId source, EntityId target)
        {
            if (Find(source.Index >> MemoryAllocator.BlockShift, out _, out _, out Ref<MemoryHandle> sourceHandle) &&
                Find(target.Index >> MemoryAllocator.BlockShift, out _, out _, out Ref<MemoryHandle> targetHandle))
            {
                IMemoryAllocator allocator = shard;

                EntityId sourcePtr = allocator.Access(sourceHandle.Value, source.Index & MemoryAllocator.BlockMask);
                EntityId targetPtr = allocator.Access(targetHandle.Value, target.Index & MemoryAllocator.BlockMask);
                
                allocator.Access(sourceHandle.Value, source.Index & MemoryAllocator.BlockMask, new EntityId(targetPtr.Index, sourcePtr.Version, sourcePtr.ShardId, sourcePtr.Flags));
                allocator.Access(targetHandle.Value, target.Index & MemoryAllocator.BlockMask, new EntityId(sourcePtr.Index, targetPtr.Version, targetPtr.ShardId, targetPtr.Flags));

                return true;
            }
            else return false;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void SwapValues(int oldIndex, int newIndex)
        {
            (components[oldIndex], components[newIndex]) = (components[newIndex], components[oldIndex]);
            (entities[oldIndex], entities[newIndex]) = (entities[newIndex], entities[oldIndex]);
        }

        /// <summary>
        /// Tries to set certain flag bits for this entity's component data
        /// </summary>
        /// <param name="entity">The entity to set flag bits for</param>
        /// <param name="flags">Any combination of bits received from <see cref="EntityFlags"/></param>
        /// <returns>True if the component flags were successfully set, false otherwise</returns>
        public bool TrySetFlags(EntityId entity, EntityFlags flags)
        {
            // Requires at least mutable access when scheduled
            AccessManager.ThrowOnLessAccessible<Component<T>>(this, AccessType.Mutable);
            
            if (Find(entity.Index >> MemoryAllocator.BlockShift, out _, out _, out Ref<MemoryHandle> handle))
            {
                IMemoryAllocator allocator = shard;
                
                // Check if the entity is alive
                EntityId entityPtr = allocator.Access(handle.Value, entity.Index & MemoryAllocator.BlockMask);
                if (Compare(entity, entityPtr))
                {
                    allocator.Access(handle.Value, entity.Index & MemoryAllocator.BlockMask, new EntityId(entityPtr.Index, entityPtr.Version, entityPtr.ShardId, flags & ~EntityFlags.Reserved));
                    return ((flags & ~EntityFlags.Reserved) == flags);
                }
            }
            return false;
        }
        
        /// <summary>
        /// Tries to receive a reference to an entity's component data
        /// </summary>
        /// <param name="entity">The entity the component should be received for</param>
        /// <param name="result">A reference to the entity's component data</param>
        /// <returns>True if the component pool contains data for the given entity, false otherwise</returns>
        public bool TryGet(EntityId entity, out Ref<T> result)
        {
            // Requires at least immutable access when scheduled
            AccessManager.ThrowOnLessAccessible<Component<T>>(this, AccessType.Immutable);
            
            if (Find(entity.Index >> MemoryAllocator.BlockShift, out _, out _, out Ref<MemoryHandle> handle))
            {
                IMemoryAllocator allocator = shard;
                
                // Check if the entity is alive
                EntityId entityPtr = allocator.Access(handle.Value, entity.Index & MemoryAllocator.BlockMask);
                if (Compare(entity, entityPtr))
                {
                    result = new Ref<T>(ref components[entityPtr.Index]);
                    return true;
                }
            }

            result = Ref<T>.CreateEmpty();
            return false;
        }
        
        #region IReadOnlyIterable Members
        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        ReadOnlyIterator<EntityId, ReadOnlyIterator<EntityId>.DefaultStrategy> IReadOnlyIterable<EntityId, ReadOnlyIterator<EntityId>.DefaultStrategy>.GetEnumerator()
        {
            return new ReadOnlyIterator<EntityId, ReadOnlyIterator<EntityId>.DefaultStrategy>(AsReadOnlySpan());
        }
        #endregion

        #region IIterable Members
        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        Iterator<T, Iterator<T>.DefaultStrategy> IIterable<T, Iterator<T>.DefaultStrategy>.GetEnumerator()
        {
            return new Iterator<T, Iterator<T>.DefaultStrategy>(AsSpan());
        }
        #endregion
        
        #region IBorrowAnchor Members
        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        bool IBorrowAnchor.OnNext(AccessManager.DependencyTreeResolver resolver, UInt32 uniqueId)
        {
            return Volatile.Read(ref group)?.OnRequest<T>(resolver, uniqueId) ?? false;
        }
        #endregion
    }
}