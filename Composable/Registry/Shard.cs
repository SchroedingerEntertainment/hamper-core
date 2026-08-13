// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;
using Soe.Collections.HashSet;

namespace Soe.Composable
{
    #if EXPORT_HAMPER_CORE_COMPOSITION
    public
    #else
    internal
    #endif
    partial class Shard : IMemoryAllocator
    {
        private readonly IMemoryAllocator allocator;
        private HashSet<Type, ComponentContainer> components;
        
        private readonly Entities entities;

        public Entities Entities
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return entities; }
        }
        
        private readonly int id;

        public int Id
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return id; }
        }
        
        public Shard(IMemoryAllocator allocator)
        {
            this.id = GetNextId(this);
            
            this.allocator = allocator;
            this.components = new HashSet<Type, ComponentContainer>(EqualityComparer<Type>.Default);
            this.entities = new Entities(this);
        }

        public void Dispose()
        {
            Span<ComponentContainer> registry = components.AsSpan();
            for(int i = 0; i < registry.Length; i++)
            {
                if (registry[i].IsValid)
                {
                    registry[i].Clear();
                    registry[i] = default;
                }
            }
            components.Clear();
            entities.Clear();
            ReturnId(id);
            
            allocator.Dispose();
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Component<T> RegisterComponent<T>()
            where T : struct
        {
            Type componentType = typeof(T);
            int hash = componentType.GetHashCode();
            
            ref ComponentContainer result = ref components.Emplace(componentType, hash);
            if (!result.IsValid)
            {
                result = new ComponentContainer(new Component<T>(this), hash, componentType);
            }
            if (result.GetInstance(out Component<T>? instance))
            {
                return instance!;
            }
            else throw new TypeAccessException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGetComponent<T>(out Component<T>? component)
            where T : struct
        {
            Type componentType = typeof(T);
            if (components.Find(componentType, componentType.GetHashCode(), out _, out _, out Ref<ComponentContainer> result) && result.Value.GetInstance(out component))
            {
                return true;
            }
            else
            {
                component = null;
                return false;
            }
        }
        
        #region IMemoryAllocator implementation
        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void IMemoryAllocator.Access(in MemoryHandle handle, int index, in EntityId entity)
        {
            allocator.Access(in handle, index, in entity);
        }
        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        EntityId IMemoryAllocator.Access(in MemoryHandle handle, int index)
        {
            return allocator.Access(in handle, index);
        }
        
        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        MemoryHandle IMemoryAllocator.Allocate(int size)
        {
            return allocator.Allocate(size);
        }
        
        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void IMemoryAllocator.Free(in MemoryHandle handle)
        {
            allocator.Free(in handle);
        }
        
        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void IMemoryAllocator.InitializeBlock(in MemoryHandle handle, int index, in EntityId entity)
        {
            allocator.InitializeBlock(in handle, index, in entity);
        }
        #endregion
    }
}