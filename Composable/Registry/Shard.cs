// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;
using Soe.Collections.HashSet;

namespace Soe.Composable
{
    /// <summary>
    /// Represents an isolated managing instance of the identity and component registry
    /// </summary>
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
        /// <summary>
        /// Gets the managing instance of this <see cref="Shard"/>'s composable object identity registry
        /// </summary>
        public Entities Entities
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return entities; }
        }
        
        private readonly int id;
        /// <summary>
        /// Gets the unique identifier of this <see cref="Shard"/>
        /// </summary>
        public int Id
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return id; }
        }
        
        /// <summary>
        /// Initializes this shard with the provided memory allocator
        /// </summary>
        /// <param name="allocator">The memory allocator underlying registries will request data blocks from</param>
        public Shard(IMemoryAllocator allocator)
        {
            this.id = GetNextId(this);
            
            this.allocator = allocator;
            this.components = new HashSet<Type, ComponentContainer>(EqualityComparer<Type>.Default);
            this.entities = new Entities(this);
        }

        /// <inheritdoc/>
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
        
        /// <summary>
        /// Populates the component pool of a pure data structure of type <typeparamref name="T"/> defining
        /// specific properties of an entity
        /// </summary>
        /// <typeparam name="T">The pure data structure defining specific properties</typeparam>
        /// <returns>The initialized component pool for the given type <typeparamref name="T"/></returns>
        /// <exception cref="TypeAccessException">Thrown if the registry was unable to create or receive the designated component pool</exception>
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

        /// <summary>
        /// Tries to receive the instance of a component pool for the provided pure data structure of type <typeparamref name="T"/>
        /// </summary>
        /// <param name="component">The component pool belonging to the given type <typeparamref name="T"/></param>
        /// <typeparam name="T">The pure data structure defining specific properties</typeparam>
        /// <returns>True if the registry defines a component pool for <typeparamref name="T"/>, false otherwise</returns>
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