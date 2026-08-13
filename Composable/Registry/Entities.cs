// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Collections;
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
    class Entities : SparseArray, IReadOnlySequence<EntityId>
    {
        private readonly Shard shard;
        private EntityId freeList;
        private int maxID;

        private EmbeddedList<EntityId> entities;

        public int Capacity
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return Length * (MemoryAllocator.BlockSize >> MemoryAllocator.BlockShift); }
        }

        public int Count
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                // Requires at least immutable access when scheduled
                AccessManager.ThrowOnLessAccessible<Entities>(this, AccessType.Immutable);
                
                return entities.Count;
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal Entities(Shard shard)
        {
            this.shard = shard;
            this.freeList = EntityId.Invalid;
            this.maxID = 0;
            this.entities = default;
        }
        
        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlySpan<EntityId> AsReadOnlySpan()
        {
            // Requires at least immutable access when scheduled
            AccessManager.ThrowOnLessAccessible<Entities>(this, AccessType.Immutable);
            
            return entities.AsReadOnlySpan();
        }
        
        public void Clear()
        {
            IMemoryAllocator allocator = shard;
            for (int i = Length - 1; i >= 0; i--)
            {
                if (data?[i].IsValid ?? false)
                {
                    allocator.Free(data[i]);
                    data[i] = default; 
                }
            }
            entities.Clear();
            this.freeList = EntityId.Invalid;
            this.maxID = 0;
        }

        public EntityId Create()
        {
            // Requires mutable access when scheduled
            AccessManager.ThrowOnAccessViolation<Entities>(this, AccessType.Mutable);
            
            Ref<MemoryHandle> handle;
            EntityId entity;

            IMemoryAllocator allocator = shard;
            if (freeList == EntityId.Invalid)
            {
                // Create new entity from current max entity ID
                entity = new EntityId(maxID++, 0, shard.Id, EntityFlags.None);
                
                int slot = entity.Index >> MemoryAllocator.BlockShift;
                if (!Find(slot, out handle))
                {
                    // Entity does not exist, add it to the sparse array
                    ref MemoryHandle tmp = ref Emplace(slot, Version);
                    if (!tmp.IsValid)
                    {
                        // Block is uninitialized, we must initialize it first to prevent false positives
                        tmp = allocator.Allocate(MemoryAllocator.BlockSize);
                        allocator.InitializeBlock(tmp, 0, EntityId.Invalid);
                        
                        handle = new Ref<MemoryHandle>(ref tmp);
                    }
                }
            }
            else
            {
                // Use recyclable entity
                entity = new EntityId(freeList.Index, freeList.Version + 1, freeList.ShardId, EntityFlags.None);
                int slot = entity.Index >> MemoryAllocator.BlockShift;
                if (Find(slot, out handle))
                {
                    // Swap the recyclable entity with whatever is stored at its slot in the sparse array
                    freeList = allocator.Access(handle.Value, freeList.Index & MemoryAllocator.BlockMask);
                }
                else throw new AccessViolationException();
            }

            int index = entities.Count; 
            entities.Add(entity);

            // Write a modified version of entity to its slot in the sparse array so entity.Index -> dense index
            allocator.Access(handle.Value, entity.Index & MemoryAllocator.BlockMask, new EntityId(index, entity.Version, entity.ShardId, entity.Flags));
            return entity;
        }

        public bool Dispose(EntityId entity)
        {
            // Requires mutable access when scheduled
            AccessManager.ThrowOnAccessViolation<Entities>(this, AccessType.Mutable);
            
            if (Find(entity.Index >> MemoryAllocator.BlockShift, out Ref<MemoryHandle> handle))
            {
                IMemoryAllocator allocator = shard;
                
                // Check if entity is alive
                EntityId entityPtr = allocator.Access(handle.Value, entity.Index & MemoryAllocator.BlockMask);
                if (((~EntityId.Null & entity) ^ entityPtr) < EntityId.Null)
                {
                    allocator.Access(handle.Value, entity.Index & MemoryAllocator.BlockMask, freeList);
                    freeList = new EntityId(entity.Index, entity.Version, entity.ShardId, EntityFlags.Reserved);

                    if (entityPtr.Index < Count - 1)
                    {
                        // Swap entity data with last entity
                        EntityId swap = entities[Count - 1];
                        if (Find(swap.Index >> MemoryAllocator.BlockShift, out handle))
                        {
                            EntityId tmp = allocator.Access(handle.Value, swap.Index & MemoryAllocator.BlockMask);
                            allocator.Access(handle.Value, swap.Index & MemoryAllocator.BlockMask, new EntityId(entityPtr.Index, tmp.Version, tmp.ShardId, tmp.Flags));

                            Swap(entityPtr.Index, tmp.Index);
                        }
                        else throw new AccessViolationException();
                    }

                    entities.RemoveAt(Count - 1);
                    return true;
                }
            }
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void Swap(int oldIndex, int newIndex)
        {
            (entities[oldIndex], entities[newIndex]) = (entities[newIndex], entities[oldIndex]);
        }

        public bool TryGet(EntityId entity, out EntityId result)
        {
            // Requires at least immutable access when scheduled
            AccessManager.ThrowOnLessAccessible<Entities>(this, AccessType.Immutable);
            
            if (Find(entity.Index >> MemoryAllocator.BlockShift, out Ref<MemoryHandle> handle))
            {
                IMemoryAllocator allocator = shard;
                
                // Check if entity is alive
                EntityId entityPtr = allocator.Access(handle.Value, entity.Index & MemoryAllocator.BlockMask);
                if (((~EntityId.Null & entity) ^ entityPtr) < EntityId.Null)
                {
                    result = entities[entityPtr.Index];
                    return true;
                }
            }

            result = EntityId.Invalid;
            return false;
        }
    }
}