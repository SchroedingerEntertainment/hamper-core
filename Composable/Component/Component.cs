// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;
using Soe.Collections.Embedded;
using Soe.Threading;

namespace Soe.Composable
{
    #if EXPORT_HAMPER_CORE_COMPOSITION
    public
    #else
    internal
    #endif
    partial class Component<T> : SparseMap, IComponent, IReadOnlySequence<EntityId>, ISequence<T>
        where T : struct
    {
        private readonly Shard shard;

        public int ShardId
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return shard.Id; }
        }
        
        private EmbeddedList<EntityId> entities;
        private EmbeddedList<T> components;
        private IComponentGroup? group;
        
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
            AccessManager.ThrowOnLessAccessible<Component<T>>(this, AccessType.Immutable);

            return components.AsSpan();
        }
        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlySpan<EntityId> AsReadOnlySpan()
        {
            // Requires at least immutable access when scheduled
            AccessManager.ThrowOnLessAccessible<Component<T>>(this, AccessType.Immutable);

            return entities.AsReadOnlySpan();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal bool AttachGroup(IComponentGroup componentGroup)
        {
            return (Interlocked.CompareExchange(ref group, componentGroup, null) == null);
        }
        
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
                if (((~EntityId.Null & entity) ^ entityPtr) < EntityId.Null)
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

                    // Write a modified version of entity to its slot in the sparse map so entity.Index -> dense index
                    allocator.Access(handle.Value, entity.Index & MemoryAllocator.BlockMask, new EntityId(index, entity.Version, entity.ShardId, entity.Flags));
                    
                    // Notify listeners
                    Volatile.Read(ref group)?.ComponentAdded<T>(entity, ref index);
                }
                return ref components[index];
            }
            else throw new ArgumentOutOfRangeException(nameof(entity.ShardId));
        }
        
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
            count = 0;
        }

        internal int IndexOf(EntityId entity)
        {
            // Requires mutable access when scheduled
            AccessManager.ThrowOnLessAccessible<Component<T>>(this, AccessType.Immutable);
            
            if (Find(entity.Index >> MemoryAllocator.BlockShift, out _, out _, out Ref<MemoryHandle> handle))
            {
                IMemoryAllocator allocator = shard;
                
                // Look the entity up in the sparse map
                EntityId entityPtr = allocator.Access(handle.Value, entity.Index & MemoryAllocator.BlockMask);
                if (((~EntityId.Null & entity) ^ entityPtr) < EntityId.Null)
                {
                    // Entity exists and is alive
                    return entityPtr.Index;
                }
            }
            return -1;
        }

        public bool Remove(EntityId entity)
        {
            // Requires mutable access when scheduled
            AccessManager.ThrowOnAccessViolation<Component<T>>(this, AccessType.Mutable);
            
            if (Find(entity.Index, out _, out _, out Ref<MemoryHandle> handle))
            {
                IMemoryAllocator allocator = shard;
                
                // Check if entity is alive
                EntityId entityPtr = allocator.Access(handle.Value, entity.Index & MemoryAllocator.BlockMask);
                if (((~EntityId.Null & entity) ^ entityPtr) < EntityId.Null)
                {
                    // Mark component as removed by adding the reserved flag
                    allocator.Access(handle.Value, entity.Index & MemoryAllocator.BlockMask, new EntityId(entityPtr.Index, entityPtr.Version, entityPtr.ShardId, EntityFlags.Reserved));
                    if (entityPtr.Index < Count - 1)
                    {
                        // Notify group
                        {
                            int componentIndex = entityPtr.Index;
                            Volatile.Read(ref group)?.ComponentRemoved<T>(entity, ref componentIndex);
                            entityPtr = new EntityId(componentIndex, entityPtr.Version, entityPtr.ShardId, entityPtr.Flags);
                        }
                        
                        // Swap entity data with last entity
                        EntityId swap = entities[Count - 1];
                        if (Find(swap.Index >> MemoryAllocator.BlockShift, out _, out _, out handle))
                        {
                            EntityId tmp = allocator.Access(handle.Value, swap.Index & MemoryAllocator.BlockMask);
                            allocator.Access(handle.Value, swap.Index & MemoryAllocator.BlockMask, new EntityId(entityPtr.Index, tmp.Version, tmp.ShardId, tmp.Flags));

                            SwapValues(entityPtr.Index, tmp.Index);
                        }
                        else throw new AccessViolationException();
                    }

                    int index = Count - 1;
                    components.RemoveAt(index);
                    entities.RemoveAt(index);
                    
                    return true;
                }
            }
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal bool ReleaseGroup(IComponentGroup componentGroup)
        {
            return (Interlocked.CompareExchange(ref group, null, componentGroup) != null);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal bool Swap(int source, int target)
        {
            // Requires mutable access when scheduled
            AccessManager.ThrowOnAccessViolation<Component<T>>(this, AccessType.Mutable);
            
            if (Swap(entities[source], entities[target]))
            {
                SwapValues(source, target);
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
        
        public bool TryGet(EntityId entity, out Ref<T> result)
        {
            // Requires at least immutable access when scheduled
            AccessManager.ThrowOnLessAccessible<Component<T>>(this, AccessType.Immutable);
            
            if (Find(entity.Index >> MemoryAllocator.BlockShift, out _, out _, out Ref<MemoryHandle> handle))
            {
                IMemoryAllocator allocator = shard;
                
                // Check if the entity is alive
                EntityId entityPtr = allocator.Access(handle.Value, entity.Index & MemoryAllocator.BlockMask);
                if (((~EntityId.Null & entity) ^ entityPtr) < EntityId.Null)
                {
                    result = new Ref<T>(ref components[entityPtr.Index]);
                    return true;
                }
            }

            result = Ref<T>.CreateEmpty();
            return false;
        }
    }
}