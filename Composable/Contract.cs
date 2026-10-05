// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;

namespace Soe.Composable
{
    /// <summary>
    /// Represents a single property of a contract instance
    /// </summary>
    [method: MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal struct ContractProperty()
    {
        private int version = 0;
        private int index = -1;

        /// <summary>
        /// Accesses the component data bound to this property
        /// </summary>
        /// <param name="entity">The entity the contract was concluded with</param>
        /// <param name="propertyData">The component data bound to this property</param>
        /// <typeparam name="T">A pure data structure defining specific properties</typeparam>
        /// <returns>True if component data was successfully received, false otherwise</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Access<T>(EntityId entity, out Ref<T> propertyData)
            where T : struct
        {
            if (Shard.TryGetShard(entity.ShardId, out Shard? shard) && (shard?.TryGetComponent(out Component<T>? component) ?? false))
            {
                if(component!.Version != version || index == -1)
                {
                    index = component.IndexOf(entity);
                    version = component.Version;
                }
                if (index >= 0)
                {
                    propertyData = new Ref<T>(ref component[index]);
                    return true;
                }
            }
            
            propertyData = Ref<T>.CreateEmpty(); 
            return false;
        }
    }
    
    /// <summary>
    /// Represents a composable object identity that ensures a certain composition to be present
    /// </summary>
    /// <typeparam name="T">A pure data structure defining specific properties</typeparam>
    #if EXPORT_HAMPER_CORE_COMPOSABLE
    public
    #else
    internal
    #endif
    struct Contract<T> : IDisposable
        where T : struct
    {
        private readonly EntityId entity;
        
        private ContractProperty property;
        /// <summary>
        /// Gets the component data bound to this property
        /// </summary>
        /// <exception cref="AccessViolationException">Thrown if the contract was unable to access the component data for this property</exception>
        public ref T Property
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                if (property.Access(entity, out Ref<T> propertyData))
                {
                    return ref propertyData.Value;
                }
                else throw new AccessViolationException();
            }
        }

        /// <summary>
        /// Gets if the composable object identity's composition is present
        /// </summary>
        public bool IsValid
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return Shard.TryGetShard(entity.ShardId, out Shard? shard) &&
                       (shard?.TryGetComponent(out Component<T>? component) ?? false) && component!.TryGet(entity, out _);
                       
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        Contract(EntityId entity)
        {
            this.entity = entity;
            this.property = new ContractProperty();
        }
        
        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Dispose()
        {
            entity.TrySetComponentFlags<T>(entity.Flags & ~EntityFlags.Locked);
        }
        
        /// <summary>
        /// Tries to get the component data bound to this property
        /// </summary>
        /// <param name="propertyData">The component data bound to this property</param>
        /// <returns>True if component data was successfully received, false otherwise</returns>
        public bool TryGetProperty(out Ref<T> propertyData)
        {
            return property.Access(entity, out propertyData);
        }

        /// <summary>
        /// Creates a new contract for the provided object identity
        /// </summary>
        /// <param name="entity">The object identity to ensure a certain composition to be present</param>
        /// <param name="contract">The created composable object identity</param>
        /// <returns>True if the contract was successfully created, false otherwise</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryContractCreate(EntityId entity, out Contract<T> contract)
        {
            if (entity.TrySetComponentFlags<T>(entity.Flags & EntityFlags.Locked))
            {
                contract = new Contract<T>(entity);
                return true;
            }
            else
            {
                entity.TrySetComponentFlags<T>(entity.Flags & ~EntityFlags.Locked);
                
                contract = default;
                return false;
            }
        }

        /// <summary>
        /// Gets if the composable object identity's composition is present
        /// </summary>
        /// <param name="entity">The object identity to ensure a certain composition to be present</param>
        /// <returns>True if the composition fulfills the contract, false otherwise</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ValidateContract(EntityId entity)
        {
            return Shard.TryGetShard(entity.ShardId, out Shard? shard) &&
                   (shard?.TryGetComponent(out Component<T>? component) ?? false) && component!.TryGet(entity, out _);
        }
    }
    /// <summary>
    /// Represents a composable object identity that ensures a certain composition to be present
    /// </summary>
    /// <typeparam name="T1">A pure data structure defining specific properties</typeparam>
    /// <typeparam name="T2">A pure data structure defining specific properties</typeparam>
    #if EXPORT_HAMPER_CORE_COMPOSABLE
    public
    #else
    internal
    #endif
    struct Contract<T1, T2> : IDisposable
        where T1 : struct
        where T2 : struct
    {
        private readonly EntityId entity;
        
        private ContractProperty property1;
        /// <summary>
        /// Gets the component data bound to this property
        /// </summary>
        /// <exception cref="AccessViolationException">Thrown if the contract was unable to access the component data for this property</exception>
        public ref T1 Property1
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                if (property1.Access(entity, out Ref<T1> propertyData))
                {
                    return ref propertyData.Value;
                }
                else throw new AccessViolationException();
            }
        }

        private ContractProperty property2;
        /// <summary>
        /// Gets the component data bound to this property
        /// </summary>
        /// <exception cref="AccessViolationException">Thrown if the contract was unable to access the component data for this property</exception>
        public ref T2 Property2
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                if (property2.Access(entity, out Ref<T2> propertyData))
                {
                    return ref propertyData.Value;
                }
                else throw new AccessViolationException();
            }
        }
        
        /// <summary>
        /// Gets if the composable object identity's composition is present
        /// </summary>
        public bool IsValid
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return Shard.TryGetShard(entity.ShardId, out Shard? shard) &&
                       (shard?.TryGetComponent(out Component<T1>? c1) ?? false) && c1!.TryGet(entity, out _) &&
                       shard.TryGetComponent(out Component<T2>? c2) && c2!.TryGet(entity, out _);
                       
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        Contract(EntityId entity)
        {
            this.entity = entity;
            this.property1 = new ContractProperty();
            this.property2 = new ContractProperty();
        }
        
        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Dispose()
        {
            entity.TrySetComponentFlags<T1>(entity.Flags & ~EntityFlags.Locked);
            entity.TrySetComponentFlags<T2>(entity.Flags & ~EntityFlags.Locked);
        }
        
        /// <summary>
        /// Tries to get the component data bound to this property
        /// </summary>
        /// <param name="propertyData">The component data bound to this property</param>
        /// <returns>True if component data was successfully received, false otherwise</returns>
        public bool TryGetProperty(out Ref<T1> propertyData)
        {
            return property1.Access(entity, out propertyData);
        }
        /// <summary>
        /// Tries to get the component data bound to this property
        /// </summary>
        /// <param name="propertyData">The component data bound to this property</param>
        /// <returns>True if component data was successfully received, false otherwise</returns>
        public bool TryGetProperty(out Ref<T2> propertyData)
        {
            return property2.Access(entity, out propertyData);
        }

        /// <summary>
        /// Creates a new contract for the provided object identity
        /// </summary>
        /// <param name="entity">The object identity to ensure a certain composition to be present</param>
        /// <param name="contract">The created composable object identity</param>
        /// <returns>True if the contract was successfully created, false otherwise</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryContractCreate(EntityId entity, out Contract<T1, T2> contract)
        {
            if (entity.TrySetComponentFlags<T1>(entity.Flags & EntityFlags.Locked) &&
                entity.TrySetComponentFlags<T2>(entity.Flags & EntityFlags.Locked))
            {
                contract = new Contract<T1, T2>(entity);
                return true;
            }
            else
            {
                entity.TrySetComponentFlags<T1>(entity.Flags & ~EntityFlags.Locked);
                entity.TrySetComponentFlags<T2>(entity.Flags & ~EntityFlags.Locked);
                
                contract = default;
                return false;
            }
        }

        /// <summary>
        /// Gets if the composable object identity's composition is present
        /// </summary>
        /// <param name="entity">The object identity to ensure a certain composition to be present</param>
        /// <returns>True if the composition fulfills the contract, false otherwise</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ValidateContract(EntityId entity)
        {
            return Shard.TryGetShard(entity.ShardId, out Shard? shard) &&
                   (shard?.TryGetComponent(out Component<T1>? c1) ?? false) && c1!.TryGet(entity, out _) &&
                   shard.TryGetComponent(out Component<T2>? c2) && c2!.TryGet(entity, out _);
        }
    }
    /// <summary>
    /// Represents a composable object identity that ensures a certain composition to be present
    /// </summary>
    /// <typeparam name="T1">A pure data structure defining specific properties</typeparam>
    /// <typeparam name="T2">A pure data structure defining specific properties</typeparam>
    /// <typeparam name="T3">A pure data structure defining specific properties</typeparam>
    #if EXPORT_HAMPER_CORE_COMPOSABLE
    public
    #else
    internal
    #endif
    struct Contract<T1, T2, T3> : IDisposable
        where T1 : struct
        where T2 : struct
        where T3 : struct
    {
        private readonly EntityId entity;
        
        private ContractProperty property1;
        /// <summary>
        /// Gets the component data bound to this property
        /// </summary>
        /// <exception cref="AccessViolationException">Thrown if the contract was unable to access the component data for this property</exception>
        public ref T1 Property1
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                if (property1.Access(entity, out Ref<T1> propertyData))
                {
                    return ref propertyData.Value;
                }
                else throw new AccessViolationException();
            }
        }

        private ContractProperty property2;
        /// <summary>
        /// Gets the component data bound to this property
        /// </summary>
        /// <exception cref="AccessViolationException">Thrown if the contract was unable to access the component data for this property</exception>
        public ref T2 Property2
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                if (property2.Access(entity, out Ref<T2> propertyData))
                {
                    return ref propertyData.Value;
                }
                else throw new AccessViolationException();
            }
        }
        
        private ContractProperty property3;
        /// <summary>
        /// Gets the component data bound to this property
        /// </summary>
        /// <exception cref="AccessViolationException">Thrown if the contract was unable to access the component data for this property</exception>
        public ref T3 Property3
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                if (property3.Access(entity, out Ref<T3> propertyData))
                {
                    return ref propertyData.Value;
                }
                else throw new AccessViolationException();
            }
        }
        
        /// <summary>
        /// Gets if the composable object identity's composition is present
        /// </summary>
        public bool IsValid
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return Shard.TryGetShard(entity.ShardId, out Shard? shard) &&
                       (shard?.TryGetComponent(out Component<T1>? c1) ?? false) && c1!.TryGet(entity, out _) &&
                       shard.TryGetComponent(out Component<T2>? c2) && c2!.TryGet(entity, out _) &&
                       shard.TryGetComponent(out Component<T3>? c3) && c3!.TryGet(entity, out _);
                       
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        Contract(EntityId entity)
        {
            this.entity = entity;
            this.property1 = new ContractProperty();
            this.property2 = new ContractProperty();
            this.property3 = new ContractProperty();
        }
        
        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Dispose()
        {
            entity.TrySetComponentFlags<T1>(entity.Flags & ~EntityFlags.Locked);
            entity.TrySetComponentFlags<T2>(entity.Flags & ~EntityFlags.Locked);
            entity.TrySetComponentFlags<T3>(entity.Flags & ~EntityFlags.Locked);
        }
        
        /// <summary>
        /// Tries to get the component data bound to this property
        /// </summary>
        /// <param name="propertyData">The component data bound to this property</param>
        /// <returns>True if component data was successfully received, false otherwise</returns>
        public bool TryGetProperty(out Ref<T1> propertyData)
        {
            return property1.Access(entity, out propertyData);
        }
        /// <summary>
        /// Tries to get the component data bound to this property
        /// </summary>
        /// <param name="propertyData">The component data bound to this property</param>
        /// <returns>True if component data was successfully received, false otherwise</returns>
        public bool TryGetProperty(out Ref<T2> propertyData)
        {
            return property2.Access(entity, out propertyData);
        }
        /// <summary>
        /// Tries to get the component data bound to this property
        /// </summary>
        /// <param name="propertyData">The component data bound to this property</param>
        /// <returns>True if component data was successfully received, false otherwise</returns>
        public bool TryGetProperty(out Ref<T3> propertyData)
        {
            return property3.Access(entity, out propertyData);
        }

        /// <summary>
        /// Creates a new contract for the provided object identity
        /// </summary>
        /// <param name="entity">The object identity to ensure a certain composition to be present</param>
        /// <param name="contract">The created composable object identity</param>
        /// <returns>True if the contract was successfully created, false otherwise</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryContractCreate(EntityId entity, out Contract<T1, T2, T3> contract)
        {
            if (entity.TrySetComponentFlags<T1>(entity.Flags & EntityFlags.Locked) &&
                entity.TrySetComponentFlags<T2>(entity.Flags & EntityFlags.Locked) &&
                entity.TrySetComponentFlags<T3>(entity.Flags & EntityFlags.Locked))
            {
                contract = new Contract<T1, T2, T3>(entity);
                return true;
            }
            else
            {
                entity.TrySetComponentFlags<T1>(entity.Flags & ~EntityFlags.Locked);
                entity.TrySetComponentFlags<T2>(entity.Flags & ~EntityFlags.Locked);
                entity.TrySetComponentFlags<T3>(entity.Flags & ~EntityFlags.Locked);
                
                contract = default;
                return false;
            }
        }

        /// <summary>
        /// Gets if the composable object identity's composition is present
        /// </summary>
        /// <param name="entity">The object identity to ensure a certain composition to be present</param>
        /// <returns>True if the composition fulfills the contract, false otherwise</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ValidateContract(EntityId entity)
        {
            return Shard.TryGetShard(entity.ShardId, out Shard? shard) &&
                   (shard?.TryGetComponent(out Component<T1>? c1) ?? false) && c1!.TryGet(entity, out _) &&
                   shard.TryGetComponent(out Component<T2>? c2) && c2!.TryGet(entity, out _) &&
                   shard.TryGetComponent(out Component<T3>? c3) && c3!.TryGet(entity, out _);
        }
    }
    /// <summary>
    /// Represents a composable object identity that ensures a certain composition to be present
    /// </summary>
    /// <typeparam name="T1">A pure data structure defining specific properties</typeparam>
    /// <typeparam name="T2">A pure data structure defining specific properties</typeparam>
    /// <typeparam name="T3">A pure data structure defining specific properties</typeparam>
    /// <typeparam name="T4">A pure data structure defining specific properties</typeparam>
    #if EXPORT_HAMPER_CORE_COMPOSABLE
    public
    #else
    internal
    #endif
    struct Contract<T1, T2, T3, T4> : IDisposable
        where T1 : struct
        where T2 : struct
        where T3 : struct
        where T4 : struct
    {
        private readonly EntityId entity;
        
        private ContractProperty property1;
        /// <summary>
        /// Gets the component data bound to this property
        /// </summary>
        /// <exception cref="AccessViolationException">Thrown if the contract was unable to access the component data for this property</exception>
        public ref T1 Property1
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                if (property1.Access(entity, out Ref<T1> propertyData))
                {
                    return ref propertyData.Value;
                }
                else throw new AccessViolationException();
            }
        }

        private ContractProperty property2;
        /// <summary>
        /// Gets the component data bound to this property
        /// </summary>
        /// <exception cref="AccessViolationException">Thrown if the contract was unable to access the component data for this property</exception>
        public ref T2 Property2
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                if (property2.Access(entity, out Ref<T2> propertyData))
                {
                    return ref propertyData.Value;
                }
                else throw new AccessViolationException();
            }
        }
        
        private ContractProperty property3;
        /// <summary>
        /// Gets the component data bound to this property
        /// </summary>
        /// <exception cref="AccessViolationException">Thrown if the contract was unable to access the component data for this property</exception>
        public ref T3 Property3
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                if (property3.Access(entity, out Ref<T3> propertyData))
                {
                    return ref propertyData.Value;
                }
                else throw new AccessViolationException();
            }
        }
        
        private ContractProperty property4;
        /// <summary>
        /// Gets the component data bound to this property
        /// </summary>
        /// <exception cref="AccessViolationException">Thrown if the contract was unable to access the component data for this property</exception>
        public ref T4 Property4
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                if (property4.Access(entity, out Ref<T4> propertyData))
                {
                    return ref propertyData.Value;
                }
                else throw new AccessViolationException();
            }
        }
        
        /// <summary>
        /// Gets if the composable object identity's composition is present
        /// </summary>
        public bool IsValid
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return Shard.TryGetShard(entity.ShardId, out Shard? shard) &&
                       (shard?.TryGetComponent(out Component<T1>? c1) ?? false) && c1!.TryGet(entity, out _) &&
                       shard.TryGetComponent(out Component<T2>? c2) && c2!.TryGet(entity, out _) &&
                       shard.TryGetComponent(out Component<T3>? c3) && c3!.TryGet(entity, out _) &&
                       shard.TryGetComponent(out Component<T4>? c4) && c4!.TryGet(entity, out _);
                       
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        Contract(EntityId entity)
        {
            this.entity = entity;
            this.property1 = new ContractProperty();
            this.property2 = new ContractProperty();
            this.property3 = new ContractProperty();
            this.property4 = new ContractProperty();
        }
        
        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Dispose()
        {
            entity.TrySetComponentFlags<T1>(entity.Flags & ~EntityFlags.Locked);
            entity.TrySetComponentFlags<T2>(entity.Flags & ~EntityFlags.Locked);
            entity.TrySetComponentFlags<T3>(entity.Flags & ~EntityFlags.Locked);
            entity.TrySetComponentFlags<T4>(entity.Flags & ~EntityFlags.Locked);
        }
        
        /// <summary>
        /// Tries to get the component data bound to this property
        /// </summary>
        /// <param name="propertyData">The component data bound to this property</param>
        /// <returns>True if component data was successfully received, false otherwise</returns>
        public bool TryGetProperty(out Ref<T1> propertyData)
        {
            return property1.Access(entity, out propertyData);
        }
        /// <summary>
        /// Tries to get the component data bound to this property
        /// </summary>
        /// <param name="propertyData">The component data bound to this property</param>
        /// <returns>True if component data was successfully received, false otherwise</returns>
        public bool TryGetProperty(out Ref<T2> propertyData)
        {
            return property2.Access(entity, out propertyData);
        }
        /// <summary>
        /// Tries to get the component data bound to this property
        /// </summary>
        /// <param name="propertyData">The component data bound to this property</param>
        /// <returns>True if component data was successfully received, false otherwise</returns>
        public bool TryGetProperty(out Ref<T3> propertyData)
        {
            return property3.Access(entity, out propertyData);
        }
        /// <summary>
        /// Tries to get the component data bound to this property
        /// </summary>
        /// <param name="propertyData">The component data bound to this property</param>
        /// <returns>True if component data was successfully received, false otherwise</returns>
        public bool TryGetProperty(out Ref<T4> propertyData)
        {
            return property4.Access(entity, out propertyData);
        }

        /// <summary>
        /// Creates a new contract for the provided object identity
        /// </summary>
        /// <param name="entity">The object identity to ensure a certain composition to be present</param>
        /// <param name="contract">The created composable object identity</param>
        /// <returns>True if the contract was successfully created, false otherwise</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryContractCreate(EntityId entity, out Contract<T1, T2, T3, T4> contract)
        {
            if (entity.TrySetComponentFlags<T1>(entity.Flags & EntityFlags.Locked) &&
                entity.TrySetComponentFlags<T2>(entity.Flags & EntityFlags.Locked) &&
                entity.TrySetComponentFlags<T3>(entity.Flags & EntityFlags.Locked) &&
                entity.TrySetComponentFlags<T4>(entity.Flags & EntityFlags.Locked))
            {
                contract = new Contract<T1, T2, T3, T4>(entity);
                return true;
            }
            else
            {
                entity.TrySetComponentFlags<T1>(entity.Flags & ~EntityFlags.Locked);
                entity.TrySetComponentFlags<T2>(entity.Flags & ~EntityFlags.Locked);
                entity.TrySetComponentFlags<T3>(entity.Flags & ~EntityFlags.Locked);
                entity.TrySetComponentFlags<T4>(entity.Flags & ~EntityFlags.Locked);
                
                contract = default;
                return false;
            }
        }

        /// <summary>
        /// Gets if the composable object identity's composition is present
        /// </summary>
        /// <param name="entity">The object identity to ensure a certain composition to be present</param>
        /// <returns>True if the composition fulfills the contract, false otherwise</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ValidateContract(EntityId entity)
        {
            return Shard.TryGetShard(entity.ShardId, out Shard? shard) &&
                   (shard?.TryGetComponent(out Component<T1>? c1) ?? false) && c1!.TryGet(entity, out _) &&
                   shard.TryGetComponent(out Component<T2>? c2) && c2!.TryGet(entity, out _) &&
                   shard.TryGetComponent(out Component<T3>? c3) && c3!.TryGet(entity, out _) &&
                   shard.TryGetComponent(out Component<T4>? c4) && c4!.TryGet(entity, out _);
        }
    }
    /// <summary>
    /// Represents a composable object identity that ensures a certain composition to be present
    /// </summary>
    /// <typeparam name="T1">A pure data structure defining specific properties</typeparam>
    /// <typeparam name="T2">A pure data structure defining specific properties</typeparam>
    /// <typeparam name="T3">A pure data structure defining specific properties</typeparam>
    /// <typeparam name="T4">A pure data structure defining specific properties</typeparam>
    /// <typeparam name="T5">A pure data structure defining specific properties</typeparam>
    #if EXPORT_HAMPER_CORE_COMPOSABLE
    public
    #else
    internal
    #endif
    struct Contract<T1, T2, T3, T4, T5> : IDisposable
        where T1 : struct
        where T2 : struct
        where T3 : struct
        where T4 : struct
        where T5 : struct
    {
        private readonly EntityId entity;
        
        private ContractProperty property1;
        /// <summary>
        /// Gets the component data bound to this property
        /// </summary>
        /// <exception cref="AccessViolationException">Thrown if the contract was unable to access the component data for this property</exception>
        public ref T1 Property1
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                if (property1.Access(entity, out Ref<T1> propertyData))
                {
                    return ref propertyData.Value;
                }
                else throw new AccessViolationException();
            }
        }

        private ContractProperty property2;
        /// <summary>
        /// Gets the component data bound to this property
        /// </summary>
        /// <exception cref="AccessViolationException">Thrown if the contract was unable to access the component data for this property</exception>
        public ref T2 Property2
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                if (property2.Access(entity, out Ref<T2> propertyData))
                {
                    return ref propertyData.Value;
                }
                else throw new AccessViolationException();
            }
        }
        
        private ContractProperty property3;
        /// <summary>
        /// Gets the component data bound to this property
        /// </summary>
        /// <exception cref="AccessViolationException">Thrown if the contract was unable to access the component data for this property</exception>
        public ref T3 Property3
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                if (property3.Access(entity, out Ref<T3> propertyData))
                {
                    return ref propertyData.Value;
                }
                else throw new AccessViolationException();
            }
        }
        
        private ContractProperty property4;
        /// <summary>
        /// Gets the component data bound to this property
        /// </summary>
        /// <exception cref="AccessViolationException">Thrown if the contract was unable to access the component data for this property</exception>
        public ref T4 Property4
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                if (property4.Access(entity, out Ref<T4> propertyData))
                {
                    return ref propertyData.Value;
                }
                else throw new AccessViolationException();
            }
        }
        
        private ContractProperty property5;
        /// <summary>
        /// Gets the component data bound to this property
        /// </summary>
        /// <exception cref="AccessViolationException">Thrown if the contract was unable to access the component data for this property</exception>
        public ref T5 Property5
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                if (property5.Access(entity, out Ref<T5> propertyData))
                {
                    return ref propertyData.Value;
                }
                else throw new AccessViolationException();
            }
        }
        
        /// <summary>
        /// Gets if the composable object identity's composition is present
        /// </summary>
        public bool IsValid
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return Shard.TryGetShard(entity.ShardId, out Shard? shard) &&
                       (shard?.TryGetComponent(out Component<T1>? c1) ?? false) && c1!.TryGet(entity, out _) &&
                       shard.TryGetComponent(out Component<T2>? c2) && c2!.TryGet(entity, out _) &&
                       shard.TryGetComponent(out Component<T3>? c3) && c3!.TryGet(entity, out _) &&
                       shard.TryGetComponent(out Component<T4>? c4) && c4!.TryGet(entity, out _) &&
                       shard.TryGetComponent(out Component<T5>? c5) && c5!.TryGet(entity, out _);

            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        Contract(EntityId entity)
        {
            this.entity = entity;
            this.property1 = new ContractProperty();
            this.property2 = new ContractProperty();
            this.property3 = new ContractProperty();
            this.property4 = new ContractProperty();
            this.property5 = new ContractProperty();
        }
        
        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Dispose()
        {
            entity.TrySetComponentFlags<T1>(entity.Flags & ~EntityFlags.Locked);
            entity.TrySetComponentFlags<T2>(entity.Flags & ~EntityFlags.Locked);
            entity.TrySetComponentFlags<T3>(entity.Flags & ~EntityFlags.Locked);
            entity.TrySetComponentFlags<T4>(entity.Flags & ~EntityFlags.Locked);
            entity.TrySetComponentFlags<T5>(entity.Flags & ~EntityFlags.Locked);
        }
        
        /// <summary>
        /// Tries to get the component data bound to this property
        /// </summary>
        /// <param name="propertyData">The component data bound to this property</param>
        /// <returns>True if component data was successfully received, false otherwise</returns>
        public bool TryGetProperty(out Ref<T1> propertyData)
        {
            return property1.Access(entity, out propertyData);
        }
        /// <summary>
        /// Tries to get the component data bound to this property
        /// </summary>
        /// <param name="propertyData">The component data bound to this property</param>
        /// <returns>True if component data was successfully received, false otherwise</returns>
        public bool TryGetProperty(out Ref<T2> propertyData)
        {
            return property2.Access(entity, out propertyData);
        }
        /// <summary>
        /// Tries to get the component data bound to this property
        /// </summary>
        /// <param name="propertyData">The component data bound to this property</param>
        /// <returns>True if component data was successfully received, false otherwise</returns>
        public bool TryGetProperty(out Ref<T3> propertyData)
        {
            return property3.Access(entity, out propertyData);
        }
        /// <summary>
        /// Tries to get the component data bound to this property
        /// </summary>
        /// <param name="propertyData">The component data bound to this property</param>
        /// <returns>True if component data was successfully received, false otherwise</returns>
        public bool TryGetProperty(out Ref<T4> propertyData)
        {
            return property4.Access(entity, out propertyData);
        }
        /// <summary>
        /// Tries to get the component data bound to this property
        /// </summary>
        /// <param name="propertyData">The component data bound to this property</param>
        /// <returns>True if component data was successfully received, false otherwise</returns>
        public bool TryGetProperty(out Ref<T5> propertyData)
        {
            return property5.Access(entity, out propertyData);
        }

        /// <summary>
        /// Creates a new contract for the provided object identity
        /// </summary>
        /// <param name="entity">The object identity to ensure a certain composition to be present</param>
        /// <param name="contract">The created composable object identity</param>
        /// <returns>True if the contract was successfully created, false otherwise</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryContractCreate(EntityId entity, out Contract<T1, T2, T3, T4, T5> contract)
        {
            if (entity.TrySetComponentFlags<T1>(entity.Flags & EntityFlags.Locked) &&
                entity.TrySetComponentFlags<T2>(entity.Flags & EntityFlags.Locked) &&
                entity.TrySetComponentFlags<T3>(entity.Flags & EntityFlags.Locked) &&
                entity.TrySetComponentFlags<T4>(entity.Flags & EntityFlags.Locked) &&
                entity.TrySetComponentFlags<T5>(entity.Flags & EntityFlags.Locked))
            {
                contract = new Contract<T1, T2, T3, T4, T5>(entity);
                return true;
            }
            else
            {
                entity.TrySetComponentFlags<T1>(entity.Flags & ~EntityFlags.Locked);
                entity.TrySetComponentFlags<T2>(entity.Flags & ~EntityFlags.Locked);
                entity.TrySetComponentFlags<T3>(entity.Flags & ~EntityFlags.Locked);
                entity.TrySetComponentFlags<T4>(entity.Flags & ~EntityFlags.Locked);
                entity.TrySetComponentFlags<T5>(entity.Flags & ~EntityFlags.Locked);
                
                contract = default;
                return false;
            }
        }

        /// <summary>
        /// Gets if the composable object identity's composition is present
        /// </summary>
        /// <param name="entity">The object identity to ensure a certain composition to be present</param>
        /// <returns>True if the composition fulfills the contract, false otherwise</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ValidateContract(EntityId entity)
        {
            return Shard.TryGetShard(entity.ShardId, out Shard? shard) &&
                   (shard?.TryGetComponent(out Component<T1>? c1) ?? false) && c1!.TryGet(entity, out _) &&
                   shard.TryGetComponent(out Component<T2>? c2) && c2!.TryGet(entity, out _) &&
                   shard.TryGetComponent(out Component<T3>? c3) && c3!.TryGet(entity, out _) &&
                   shard.TryGetComponent(out Component<T4>? c4) && c4!.TryGet(entity, out _) &&
                   shard.TryGetComponent(out Component<T5>? c5) && c5!.TryGet(entity, out _);
        }
    }
    /// <summary>
    /// Represents a composable object identity that ensures a certain composition to be present
    /// </summary>
    /// <typeparam name="T1">A pure data structure defining specific properties</typeparam>
    /// <typeparam name="T2">A pure data structure defining specific properties</typeparam>
    /// <typeparam name="T3">A pure data structure defining specific properties</typeparam>
    /// <typeparam name="T4">A pure data structure defining specific properties</typeparam>
    /// <typeparam name="T5">A pure data structure defining specific properties</typeparam>
    /// <typeparam name="T6">A pure data structure defining specific properties</typeparam>
    #if EXPORT_HAMPER_CORE_COMPOSABLE
    public
    #else
    internal
    #endif
    struct Contract<T1, T2, T3, T4, T5, T6> : IDisposable
        where T1 : struct
        where T2 : struct
        where T3 : struct
        where T4 : struct
        where T5 : struct
        where T6 : struct
    {
        private readonly EntityId entity;
        
        private ContractProperty property1;
        /// <summary>
        /// Gets the component data bound to this property
        /// </summary>
        /// <exception cref="AccessViolationException">Thrown if the contract was unable to access the component data for this property</exception>
        public ref T1 Property1
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                if (property1.Access(entity, out Ref<T1> propertyData))
                {
                    return ref propertyData.Value;
                }
                else throw new AccessViolationException();
            }
        }

        private ContractProperty property2;
        /// <summary>
        /// Gets the component data bound to this property
        /// </summary>
        /// <exception cref="AccessViolationException">Thrown if the contract was unable to access the component data for this property</exception>
        public ref T2 Property2
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                if (property2.Access(entity, out Ref<T2> propertyData))
                {
                    return ref propertyData.Value;
                }
                else throw new AccessViolationException();
            }
        }
        
        private ContractProperty property3;
        /// <summary>
        /// Gets the component data bound to this property
        /// </summary>
        /// <exception cref="AccessViolationException">Thrown if the contract was unable to access the component data for this property</exception>
        public ref T3 Property3
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                if (property3.Access(entity, out Ref<T3> propertyData))
                {
                    return ref propertyData.Value;
                }
                else throw new AccessViolationException();
            }
        }
        
        private ContractProperty property4;
        /// <summary>
        /// Gets the component data bound to this property
        /// </summary>
        /// <exception cref="AccessViolationException">Thrown if the contract was unable to access the component data for this property</exception>
        public ref T4 Property4
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                if (property4.Access(entity, out Ref<T4> propertyData))
                {
                    return ref propertyData.Value;
                }
                else throw new AccessViolationException();
            }
        }
        
        private ContractProperty property5;
        /// <summary>
        /// Gets the component data bound to this property
        /// </summary>
        /// <exception cref="AccessViolationException">Thrown if the contract was unable to access the component data for this property</exception>
        public ref T5 Property5
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                if (property5.Access(entity, out Ref<T5> propertyData))
                {
                    return ref propertyData.Value;
                }
                else throw new AccessViolationException();
            }
        }
        
        private ContractProperty property6;
        /// <summary>
        /// Gets the component data bound to this property
        /// </summary>
        /// <exception cref="AccessViolationException">Thrown if the contract was unable to access the component data for this property</exception>
        public ref T6 Property6
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                if (property6.Access(entity, out Ref<T6> propertyData))
                {
                    return ref propertyData.Value;
                }
                else throw new AccessViolationException();
            }
        }
        
        /// <summary>
        /// Gets if the composable object identity's composition is present
        /// </summary>
        public bool IsValid
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return Shard.TryGetShard(entity.ShardId, out Shard? shard) &&
                       (shard?.TryGetComponent(out Component<T1>? c1) ?? false) && c1!.TryGet(entity, out _) &&
                       shard.TryGetComponent(out Component<T2>? c2) && c2!.TryGet(entity, out _) &&
                       shard.TryGetComponent(out Component<T3>? c3) && c3!.TryGet(entity, out _) &&
                       shard.TryGetComponent(out Component<T4>? c4) && c4!.TryGet(entity, out _) &&
                       shard.TryGetComponent(out Component<T5>? c5) && c5!.TryGet(entity, out _) &&
                       shard.TryGetComponent(out Component<T6>? c6) && c6!.TryGet(entity, out _);
                       
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        Contract(EntityId entity)
        {
            this.entity = entity;
            this.property1 = new ContractProperty();
            this.property2 = new ContractProperty();
            this.property3 = new ContractProperty();
            this.property4 = new ContractProperty();
            this.property5 = new ContractProperty();
            this.property6 = new ContractProperty();
        }
        
        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Dispose()
        {
            entity.TrySetComponentFlags<T1>(entity.Flags & ~EntityFlags.Locked);
            entity.TrySetComponentFlags<T2>(entity.Flags & ~EntityFlags.Locked);
            entity.TrySetComponentFlags<T3>(entity.Flags & ~EntityFlags.Locked);
            entity.TrySetComponentFlags<T4>(entity.Flags & ~EntityFlags.Locked);
            entity.TrySetComponentFlags<T5>(entity.Flags & ~EntityFlags.Locked);
            entity.TrySetComponentFlags<T6>(entity.Flags & ~EntityFlags.Locked);
        }
        
        /// <summary>
        /// Tries to get the component data bound to this property
        /// </summary>
        /// <param name="propertyData">The component data bound to this property</param>
        /// <returns>True if component data was successfully received, false otherwise</returns>
        public bool TryGetProperty(out Ref<T1> propertyData)
        {
            return property1.Access(entity, out propertyData);
        }
        /// <summary>
        /// Tries to get the component data bound to this property
        /// </summary>
        /// <param name="propertyData">The component data bound to this property</param>
        /// <returns>True if component data was successfully received, false otherwise</returns>
        public bool TryGetProperty(out Ref<T2> propertyData)
        {
            return property2.Access(entity, out propertyData);
        }
        /// <summary>
        /// Tries to get the component data bound to this property
        /// </summary>
        /// <param name="propertyData">The component data bound to this property</param>
        /// <returns>True if component data was successfully received, false otherwise</returns>
        public bool TryGetProperty(out Ref<T3> propertyData)
        {
            return property3.Access(entity, out propertyData);
        }
        /// <summary>
        /// Tries to get the component data bound to this property
        /// </summary>
        /// <param name="propertyData">The component data bound to this property</param>
        /// <returns>True if component data was successfully received, false otherwise</returns>
        public bool TryGetProperty(out Ref<T4> propertyData)
        {
            return property4.Access(entity, out propertyData);
        }
        /// <summary>
        /// Tries to get the component data bound to this property
        /// </summary>
        /// <param name="propertyData">The component data bound to this property</param>
        /// <returns>True if component data was successfully received, false otherwise</returns>
        public bool TryGetProperty(out Ref<T5> propertyData)
        {
            return property5.Access(entity, out propertyData);
        }
        /// <summary>
        /// Tries to get the component data bound to this property
        /// </summary>
        /// <param name="propertyData">The component data bound to this property</param>
        /// <returns>True if component data was successfully received, false otherwise</returns>
        public bool TryGetProperty(out Ref<T6> propertyData)
        {
            return property6.Access(entity, out propertyData);
        }

        /// <summary>
        /// Creates a new contract for the provided object identity
        /// </summary>
        /// <param name="entity">The object identity to ensure a certain composition to be present</param>
        /// <param name="contract">The created composable object identity</param>
        /// <returns>True if the contract was successfully created, false otherwise</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryContractCreate(EntityId entity, out Contract<T1, T2, T3, T4, T5, T6> contract)
        {
            if (entity.TrySetComponentFlags<T1>(entity.Flags & EntityFlags.Locked) &&
                entity.TrySetComponentFlags<T2>(entity.Flags & EntityFlags.Locked) &&
                entity.TrySetComponentFlags<T3>(entity.Flags & EntityFlags.Locked) &&
                entity.TrySetComponentFlags<T4>(entity.Flags & EntityFlags.Locked) &&
                entity.TrySetComponentFlags<T5>(entity.Flags & EntityFlags.Locked) &&
                entity.TrySetComponentFlags<T6>(entity.Flags & EntityFlags.Locked))
            {
                contract = new Contract<T1, T2, T3, T4, T5, T6>(entity);
                return true;
            }
            else
            {
                entity.TrySetComponentFlags<T1>(entity.Flags & ~EntityFlags.Locked);
                entity.TrySetComponentFlags<T2>(entity.Flags & ~EntityFlags.Locked);
                entity.TrySetComponentFlags<T3>(entity.Flags & ~EntityFlags.Locked);
                entity.TrySetComponentFlags<T4>(entity.Flags & ~EntityFlags.Locked);
                entity.TrySetComponentFlags<T5>(entity.Flags & ~EntityFlags.Locked);
                entity.TrySetComponentFlags<T6>(entity.Flags & ~EntityFlags.Locked);
                
                contract = default;
                return false;
            }
        }

        /// <summary>
        /// Gets if the composable object identity's composition is present
        /// </summary>
        /// <param name="entity">The object identity to ensure a certain composition to be present</param>
        /// <returns>True if the composition fulfills the contract, false otherwise</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ValidateContract(EntityId entity)
        {
            return Shard.TryGetShard(entity.ShardId, out Shard? shard) &&
                   (shard?.TryGetComponent(out Component<T1>? c1) ?? false) && c1!.TryGet(entity, out _) &&
                   shard.TryGetComponent(out Component<T2>? c2) && c2!.TryGet(entity, out _) &&
                   shard.TryGetComponent(out Component<T3>? c3) && c3!.TryGet(entity, out _) &&
                   shard.TryGetComponent(out Component<T4>? c4) && c4!.TryGet(entity, out _) &&
                   shard.TryGetComponent(out Component<T5>? c5) && c5!.TryGet(entity, out _) &&
                   shard.TryGetComponent(out Component<T6>? c6) && c6!.TryGet(entity, out _);
        }
    }
}