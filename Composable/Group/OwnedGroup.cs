// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;
using Soe.Threading;

namespace Soe.Composable
{
    /// <summary>
    /// Aligns multiple component pools so that iterating over specific component combinations runs at maximum speed
    /// </summary>
    /// <typeparam name="T1">A component type</typeparam>
    /// <typeparam name="T2">A component type</typeparam>
    #if EXPORT_HAMPER_CORE_COMPOSITION
    public
    #else
    internal
    #endif
    class OwnedGroup<T1, T2> : IComponentGroup
        where T1 : struct
        where T2 : struct
    {
        private Component<T1>? component1;
        private Component<T2>? component2;

        private int count;
        /// <inheritdoc/>
        public int Count
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return count; }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private OwnedGroup()
        {
            this.count = 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        bool AddDependencies<T>(AccessManager.DependencyTreeResolver resolver, UInt32 uniqueId, Component<T> instance)
            where T : struct
        {
            switch (resolver.GetAccess(uniqueId))
            {
                case AccessType.Mutable: return resolver.AddConditional<Component<T>, MutablePolicy>(resolver.GetUniqueId(instance), instance);
                default: return false;
            }
        }
        
        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlySpan<EntityId> AsReadOnlySpan()
        {
            // Requires mutable access when scheduled
            AccessManager.ThrowOnLessAccessible<Component<T1>>(component1!, AccessType.Immutable);
            
            return component1!.AsReadOnlySpan()
                .Slice(0, count);
        }
        
        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Dispose()
        {
            component1?.ReleaseGroup(this);
            component2?.ReleaseGroup(this);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void MoveIntoGroup<ComponentT1, ComponentT2>(EntityId entity, Component<ComponentT1> c1, ref int index, Component<ComponentT2> c2)
            where ComponentT1 : struct
            where ComponentT2 : struct
        {
            int other = c2.IndexOf(entity);
            if (index >= 0 && other >= 0)
            {
                MoveComponent(c1, ref index);
                MoveComponent(c2, ref other);
                
                count++;
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void MoveOutOfGroup<ComponentT1, ComponentT2>(EntityId entity, Component<ComponentT1> c1, ref int index, Component<ComponentT2> c2)
            where ComponentT1 : struct
            where ComponentT2 : struct
        {
            if(index < count - 1)
            {
                c1.Swap(index, count - 1);
                if (c2.IndexOf(entity) == index)
                {
                    c2.Swap(index, count - 1);
                }
                else throw new InvalidOperationException(nameof(Component<ComponentT2>));
            }
            index = --count;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void MoveComponent<T>(Component<T> component, ref int index)
            where T : struct
        {
            if (index != count)
            {
                if (component.Swap(index, count))
                {
                    index = count;
                }
                else throw new InvalidOperationException();
            }
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGetEntity(EntityId entity, out int index)
        {
            index = component1?.IndexOf(entity) ?? -1;
            return (index >= 0 && index < count);
        }

        #region IComponentGroup Members
        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void IComponentGroup.ComponentAdded<T>(EntityId entity, ref int index)
        {
            if (typeof(T) == typeof(T1))
            {
                MoveIntoGroup(entity, component1!, ref index, component2!);
            }
            else if (typeof(T) == typeof(T2))
            {
                MoveIntoGroup(entity, component2!, ref index, component1!);
            }
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void IComponentGroup.ComponentRemoved<T>(EntityId entity, ref int index)
        {
            if (index >= 0 && index <= count)
            {
                if (typeof(T) == typeof(T1))
                {
                    MoveOutOfGroup(entity, component1!, ref index, component2!);
                }
                else if (typeof(T) == typeof(T2))
                {
                    MoveOutOfGroup(entity, component2!, ref index, component1!);
                }
            }
        }
        
        bool IComponentGroup.OnRequest<T>(AccessManager.DependencyTreeResolver resolver, UInt32 uniqueId) 
            where T : struct
        {
            if (typeof(T) == typeof(T1))
            {
                return AddDependencies(resolver, uniqueId, component2!);
            }
            else if (typeof(T) == typeof(T2))
            {
                return AddDependencies(resolver, uniqueId, component1!);
            }
            else
            {
                return AddDependencies(resolver, uniqueId, component1!) |
                       AddDependencies(resolver, uniqueId, component2!);
            }
        }
        #endregion
        
        /// <summary>
        /// Initializes a new group instance based on the components provided
        /// </summary>
        /// <param name="component1">A component to be managed by this group</param>
        /// <param name="component2">A component to be managed by this group</param>
        /// <returns>The newly created group instance</returns>
        /// <exception cref="ArgumentException">Thrown if the group tries to override an already existing group</exception>
        /// <exception cref="AccessViolationException">Thrown if component access is performed across different shards</exception>
        public static OwnedGroup<T1, T2> Initialize(Component<T1> component1, Component<T2> component2)
        {
            if (component1.ShardId == component2.ShardId)
            {
                OwnedGroup<T1, T2> result = new OwnedGroup<T1, T2>();
                if (!component1.AttachGroup(result))
                {
                    throw new ArgumentException(nameof(component1));
                }
                else if (!component2.AttachGroup(result))
                {
                    component2.ReleaseGroup(result);
                    throw new ArgumentException(nameof(component2));
                }
                else
                {
                    result.component1 = component1;
                    result.component2 = component2;
                    return result;
                }
            }
            else throw new AccessViolationException();
        }
    }
    /// <summary>
    /// Aligns multiple component pools so that iterating over specific component combinations runs at maximum speed
    /// </summary>
    /// <typeparam name="T1">A component type</typeparam>
    /// <typeparam name="T2">A component type</typeparam>
    /// <typeparam name="T3">A component type</typeparam>
    #if EXPORT_HAMPER_CORE_COMPOSITION
    public
    #else
    internal
    #endif
    class OwnedGroup<T1, T2, T3> : IComponentGroup
        where T1 : struct
        where T2 : struct
        where T3 : struct
    {
        private Component<T1>? component1;
        private Component<T2>? component2;
        private Component<T3>? component3;
        
        private int count;
        /// <inheritdoc/>
        public int Count
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return count; }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private OwnedGroup()
        {
            this.count = 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        bool AddDependencies<T>(AccessManager.DependencyTreeResolver resolver, UInt32 uniqueId, Component<T> instance)
            where T : struct
        {
            switch (resolver.GetAccess(uniqueId))
            {
                case AccessType.Mutable: return resolver.AddConditional<Component<T>, MutablePolicy>(resolver.GetUniqueId(instance), instance);
                default: return false;
            }
        }
        
        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlySpan<EntityId> AsReadOnlySpan()
        {
            // Requires mutable access when scheduled
            AccessManager.ThrowOnLessAccessible<Component<T1>>(component1!, AccessType.Immutable);
            
            return component1!.AsReadOnlySpan()
                .Slice(0, count);
        }
        
        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Dispose()
        {
            component1?.ReleaseGroup(this);
            component2?.ReleaseGroup(this);
            component3?.ReleaseGroup(this);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void MoveIntoGroup<ComponentT1, ComponentT2, ComponentT3>(EntityId entity, Component<ComponentT1> c1, ref int index, Component<ComponentT2> c2, Component<ComponentT3> c3)
            where ComponentT1 : struct
            where ComponentT2 : struct
            where ComponentT3 : struct
        {
            int i2 = c2.IndexOf(entity);
            int i3 = c3.IndexOf(entity);
            if (index >= 0 && i2 >= 0 && i3 >= 0)
            {
                MoveComponent(c1, ref index);
                MoveComponent(c2, ref i2);
                MoveComponent(c3, ref i3);
                
                count++;
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void MoveOutOfGroup<ComponentT1, ComponentT2, ComponentT3>(EntityId entity, Component<ComponentT1> c1, ref int index, Component<ComponentT2> c2, Component<ComponentT3> c3)
            where ComponentT1 : struct
            where ComponentT2 : struct
            where ComponentT3 : struct
        {
            if(index < count - 1)
            {
                c1.Swap(index, count - 1);
                if (c2.IndexOf(entity) == index)
                {
                    c2.Swap(index, count - 1);
                }
                else throw new InvalidOperationException(nameof(Component<ComponentT2>));

                if (c3.IndexOf(entity) == index)
                {
                    c3.Swap(index, count - 1);
                }
                else throw new InvalidOperationException(nameof(Component<ComponentT3>));
            }
            index = --count;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void MoveComponent<T>(Component<T> component, ref int index)
            where T : struct
        {
            if (index != count)
            {
                if (component.Swap(index, count))
                {
                    index = count;
                }
                else throw new InvalidOperationException();
            }
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGetEntity(EntityId entity, out int index)
        {
            index = component1?.IndexOf(entity) ?? -1;
            return (index >= 0 && index < count);
        }

        #region IComponentGroup Members
        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void IComponentGroup.ComponentAdded<T>(EntityId entity, ref int index)
        {
            if (typeof(T) == typeof(T1))
            {
                MoveIntoGroup(entity, component1!, ref index, component2!, component3!);
            }
            else if (typeof(T) == typeof(T2))
            {
                MoveIntoGroup(entity, component2!, ref index, component1!, component3!);
            }
            else if (typeof(T) == typeof(T3))
            {
                MoveIntoGroup(entity, component3!, ref index, component1!, component2!);
            }
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void IComponentGroup.ComponentRemoved<T>(EntityId entity, ref int index)
        {
            if (index >= 0 && index <= count)
            {
                if (typeof(T) == typeof(T1))
                {
                    MoveOutOfGroup(entity, component1!, ref index, component2!, component3!);
                }
                else if (typeof(T) == typeof(T2))
                {
                    MoveOutOfGroup(entity, component2!, ref index, component1!, component3!);
                }
                else if (typeof(T) == typeof(T3))
                {
                    MoveOutOfGroup(entity, component3!, ref index, component1!, component2!);
                }
            }
        }
        
        bool IComponentGroup.OnRequest<T>(AccessManager.DependencyTreeResolver resolver, UInt32 uniqueId) 
            where T : struct
        {
            if (typeof(T) == typeof(T1))
            {
                return AddDependencies(resolver, uniqueId, component2!) |
                       AddDependencies(resolver, uniqueId, component3!);
            }
            else if (typeof(T) == typeof(T2))
            {
                return AddDependencies(resolver, uniqueId, component1!) |
                       AddDependencies(resolver, uniqueId, component3!);
            }
            else  if (typeof(T) == typeof(T3))
            {
                return AddDependencies(resolver, uniqueId, component1!) |
                       AddDependencies(resolver, uniqueId, component2!);
            }
            else
            {
                return AddDependencies(resolver, uniqueId, component1!) |
                       AddDependencies(resolver, uniqueId, component2!) |
                       AddDependencies(resolver, uniqueId, component3!);
            }
        }
        #endregion
        
        /// <summary>
        /// Initializes a new group instance based on the components provided
        /// </summary>
        /// <param name="component1">A component to be managed by this group</param>
        /// <param name="component2">A component to be managed by this group</param>
        /// <param name="component3">A component to be managed by this group</param>
        /// <returns>The newly created group instance</returns>
        /// <exception cref="ArgumentException">Thrown if the group tries to override an already existing group</exception>
        /// <exception cref="AccessViolationException">Thrown if component access is performed across different shards</exception>
        public static OwnedGroup<T1, T2, T3> Initialize(Component<T1> component1, Component<T2> component2, Component<T3> component3)
        {
            if (component1.ShardId == component2.ShardId)
            {
                OwnedGroup<T1, T2, T3> result = new OwnedGroup<T1, T2, T3>();
                if (!component1.AttachGroup(result))
                {
                    throw new ArgumentException(nameof(component1));
                }
                else if (!component2.AttachGroup(result))
                {
                    component2.ReleaseGroup(result);
                    throw new ArgumentException(nameof(component2));
                }
                else if (!component3.AttachGroup(result))
                {
                    component3.ReleaseGroup(result);
                    throw new ArgumentException(nameof(component3));
                }
                else
                {
                    result.component1 = component1;
                    result.component2 = component2;
                    result.component3 = component3;
                    return result;
                }
            }
            else throw new AccessViolationException();
        }
    }
    /// <summary>
    /// Aligns multiple component pools so that iterating over specific component combinations runs at maximum speed
    /// </summary>
    /// <typeparam name="T1">A component type</typeparam>
    /// <typeparam name="T2">A component type</typeparam>
    /// <typeparam name="T3">A component type</typeparam>
    /// <typeparam name="T4">A component type</typeparam>
    #if EXPORT_HAMPER_CORE_COMPOSITION
    public
    #else
    internal
    #endif
    class OwnedGroup<T1, T2, T3, T4> : IComponentGroup
        where T1 : struct
        where T2 : struct
        where T3 : struct
        where T4 : struct
    {
        private Component<T1>? component1;
        private Component<T2>? component2;
        private Component<T3>? component3;
        private Component<T4>? component4;
        
        private int count;
        /// <inheritdoc/>
        public int Count
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return count; }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private OwnedGroup()
        {
            this.count = 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        bool AddDependencies<T>(AccessManager.DependencyTreeResolver resolver, UInt32 uniqueId, Component<T> instance)
            where T : struct
        {
            switch (resolver.GetAccess(uniqueId))
            {
                case AccessType.Mutable: return resolver.AddConditional<Component<T>, MutablePolicy>(resolver.GetUniqueId(instance), instance);
                default: return false;
            }
        }
        
        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlySpan<EntityId> AsReadOnlySpan()
        {
            // Requires mutable access when scheduled
            AccessManager.ThrowOnLessAccessible<Component<T1>>(component1!, AccessType.Immutable);
            
            return component1!.AsReadOnlySpan()
                .Slice(0, count);
        }
        
        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Dispose()
        {
            component1?.ReleaseGroup(this);
            component2?.ReleaseGroup(this);
            component3?.ReleaseGroup(this);
            component4?.ReleaseGroup(this);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void MoveIntoGroup<ComponentT1, ComponentT2, ComponentT3, ComponentT4>(EntityId entity, Component<ComponentT1> c1, ref int index, Component<ComponentT2> c2, Component<ComponentT3> c3, Component<ComponentT4> c4)
            where ComponentT1 : struct
            where ComponentT2 : struct
            where ComponentT3 : struct
            where ComponentT4 : struct
        {
            int i2 = c2.IndexOf(entity);
            int i3 = c3.IndexOf(entity);
            int i4 = c4.IndexOf(entity);
            if (index >= 0 && i2 >= 0 && i3 >= 0 && i4 >= 0)
            {
                MoveComponent(c1, ref index);
                MoveComponent(c2, ref i2);
                MoveComponent(c3, ref i3);
                MoveComponent(c4, ref i4);
                
                count++;
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void MoveOutOfGroup<ComponentT1, ComponentT2, ComponentT3, ComponentT4>(EntityId entity, Component<ComponentT1> c1, ref int index, Component<ComponentT2> c2, Component<ComponentT3> c3, Component<ComponentT4> c4)
            where ComponentT1 : struct
            where ComponentT2 : struct
            where ComponentT3 : struct
            where ComponentT4 : struct
        {
            if(index < count - 1)
            {
                c1.Swap(index, count - 1);
                if (c2.IndexOf(entity) == index)
                {
                    c2.Swap(index, count - 1);
                }
                else throw new InvalidOperationException(nameof(Component<ComponentT2>));

                if (c3.IndexOf(entity) == index)
                {
                    c3.Swap(index, count - 1);
                }
                else throw new InvalidOperationException(nameof(Component<ComponentT3>));

                if (c4.IndexOf(entity) == index)
                {
                    c4.Swap(index, count - 1);
                }
                else throw new InvalidOperationException(nameof(Component<ComponentT4>));
            }
            index = --count;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void MoveComponent<T>(Component<T> component, ref int index)
            where T : struct
        {
            if (index != count)
            {
                if (component.Swap(index, count))
                {
                    index = count;
                }
                else throw new InvalidOperationException();
            }
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGetEntity(EntityId entity, out int index)
        {
            index = component1?.IndexOf(entity) ?? -1;
            return (index >= 0 && index < count);
        }

        #region IComponentGroup Members
        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void IComponentGroup.ComponentAdded<T>(EntityId entity, ref int index)
        {
            if (typeof(T) == typeof(T1))
            {
                MoveIntoGroup(entity, component1!, ref index, component2!, component3!, component4!);
            }
            else if (typeof(T) == typeof(T2))
            {
                MoveIntoGroup(entity, component2!, ref index, component1!, component3!, component4!);
            }
            else if (typeof(T) == typeof(T3))
            {
                MoveIntoGroup(entity, component3!, ref index, component1!, component2!, component4!);
            }
            else if (typeof(T) == typeof(T4))
            {
                MoveIntoGroup(entity, component4!, ref index, component1!, component2!, component3!);
            }
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void IComponentGroup.ComponentRemoved<T>(EntityId entity, ref int index)
        {
            if (index >= 0 && index <= count)
            {
                if (typeof(T) == typeof(T1))
                {
                    MoveOutOfGroup(entity, component1!, ref index, component2!, component3!, component4!);
                }
                else if (typeof(T) == typeof(T2))
                {
                    MoveOutOfGroup(entity, component2!, ref index, component1!, component3!, component4!);
                }
                else if (typeof(T) == typeof(T3))
                {
                    MoveOutOfGroup(entity, component3!, ref index, component1!, component2!, component4!);
                }
                else if (typeof(T) == typeof(T4))
                {
                    MoveOutOfGroup(entity, component4!, ref index, component1!, component2!, component3!);
                }
            }
        }
        
        bool IComponentGroup.OnRequest<T>(AccessManager.DependencyTreeResolver resolver, UInt32 uniqueId) 
            where T : struct
        {
            if (typeof(T) == typeof(T1))
            {
                return AddDependencies(resolver, uniqueId, component2!) |
                       AddDependencies(resolver, uniqueId, component3!) |
                       AddDependencies(resolver, uniqueId, component4!);
            }
            else if (typeof(T) == typeof(T2))
            {
                return AddDependencies(resolver, uniqueId, component1!) |
                       AddDependencies(resolver, uniqueId, component3!) |
                       AddDependencies(resolver, uniqueId, component4!);
            }
            else  if (typeof(T) == typeof(T3))
            {
                return AddDependencies(resolver, uniqueId, component1!) |
                       AddDependencies(resolver, uniqueId, component2!) |
                       AddDependencies(resolver, uniqueId, component4!);
            }
            else  if (typeof(T) == typeof(T4))
            {
                return AddDependencies(resolver, uniqueId, component1!) |
                       AddDependencies(resolver, uniqueId, component2!) |
                       AddDependencies(resolver, uniqueId, component3!);
            }
            else
            {
                return AddDependencies(resolver, uniqueId, component1!) |
                       AddDependencies(resolver, uniqueId, component2!) |
                       AddDependencies(resolver, uniqueId, component3!) |
                       AddDependencies(resolver, uniqueId, component4!);
            }
        }
        #endregion
        
        /// <summary>
        /// Initializes a new group instance based on the components provided
        /// </summary>
        /// <param name="component1">A component to be managed by this group</param>
        /// <param name="component2">A component to be managed by this group</param>
        /// <param name="component3">A component to be managed by this group</param>
        /// <param name="component4">A component to be managed by this group</param>
        /// <returns>The newly created group instance</returns>
        /// <exception cref="ArgumentException">Thrown if the group tries to override an already existing group</exception>
        /// <exception cref="AccessViolationException">Thrown if component access is performed across different shards</exception>
        public static OwnedGroup<T1, T2, T3, T4> Initialize(Component<T1> component1, Component<T2> component2, Component<T3> component3, Component<T4> component4)
        {
            if (component1.ShardId == component2.ShardId)
            {
                OwnedGroup<T1, T2, T3, T4> result = new OwnedGroup<T1, T2, T3, T4>();
                if (!component1.AttachGroup(result))
                {
                    throw new ArgumentException(nameof(component1));
                }
                else if (!component2.AttachGroup(result))
                {
                    component2.ReleaseGroup(result);
                    throw new ArgumentException(nameof(component2));
                }
                else if (!component3.AttachGroup(result))
                {
                    component3.ReleaseGroup(result);
                    throw new ArgumentException(nameof(component3));
                }
                else if (!component4.AttachGroup(result))
                {
                    component4.ReleaseGroup(result);
                    throw new ArgumentException(nameof(component4));
                }
                else
                {
                    result.component1 = component1;
                    result.component2 = component2;
                    result.component3 = component3;
                    result.component4 = component4;
                    return result;
                }
            }
            else throw new AccessViolationException();
        }
    }
    /// <summary>
    /// Aligns multiple component pools so that iterating over specific component combinations runs at maximum speed
    /// </summary>
    /// <typeparam name="T1">A component type</typeparam>
    /// <typeparam name="T2">A component type</typeparam>
    /// <typeparam name="T3">A component type</typeparam>
    /// <typeparam name="T4">A component type</typeparam>
    /// <typeparam name="T5">A component type</typeparam>
    #if EXPORT_HAMPER_CORE_COMPOSITION
    public
    #else
    internal
    #endif
    class OwnedGroup<T1, T2, T3, T4, T5> : IComponentGroup
        where T1 : struct
        where T2 : struct
        where T3 : struct
        where T4 : struct
        where T5 : struct
    {
        private Component<T1>? component1;
        private Component<T2>? component2;
        private Component<T3>? component3;
        private Component<T4>? component4;
        private Component<T5>? component5;
        
        private int count;
        /// <inheritdoc/>
        public int Count
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return count; }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private OwnedGroup()
        {
            this.count = 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        bool AddDependencies<T>(AccessManager.DependencyTreeResolver resolver, UInt32 uniqueId, Component<T> instance)
            where T : struct
        {
            switch (resolver.GetAccess(uniqueId))
            {
                case AccessType.Mutable: return resolver.AddConditional<Component<T>, MutablePolicy>(resolver.GetUniqueId(instance), instance);
                default: return false;
            }
        }
        
        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlySpan<EntityId> AsReadOnlySpan()
        {
            // Requires mutable access when scheduled
            AccessManager.ThrowOnLessAccessible<Component<T1>>(component1!, AccessType.Immutable);
            
            return component1!.AsReadOnlySpan()
                .Slice(0, count);
        }
        
        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Dispose()
        {
            component1?.ReleaseGroup(this);
            component2?.ReleaseGroup(this);
            component3?.ReleaseGroup(this);
            component4?.ReleaseGroup(this);
            component5?.ReleaseGroup(this);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void MoveIntoGroup<ComponentT1, ComponentT2, ComponentT3, ComponentT4, ComponentT5>(EntityId entity, Component<ComponentT1> c1, ref int index, Component<ComponentT2> c2, Component<ComponentT3> c3, Component<ComponentT4> c4, Component<ComponentT5> c5)
            where ComponentT1 : struct
            where ComponentT2 : struct
            where ComponentT3 : struct
            where ComponentT4 : struct
            where ComponentT5 : struct
        {
            int i2 = c2.IndexOf(entity);
            int i3 = c3.IndexOf(entity);
            int i4 = c4.IndexOf(entity);
            int i5 = c5.IndexOf(entity);
            if (index >= 0 && i2 >= 0 && i3 >= 0 && i4 >= 0 && i5 >= 0)
            {
                MoveComponent(c1, ref index);
                MoveComponent(c2, ref i2);
                MoveComponent(c3, ref i3);
                MoveComponent(c4, ref i4);
                MoveComponent(c5, ref i5);
                
                count++;
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void MoveOutOfGroup<ComponentT1, ComponentT2, ComponentT3, ComponentT4, ComponentT5>(EntityId entity, Component<ComponentT1> c1, ref int index, Component<ComponentT2> c2, Component<ComponentT3> c3, Component<ComponentT4> c4, Component<ComponentT5> c5)
            where ComponentT1 : struct
            where ComponentT2 : struct
            where ComponentT3 : struct
            where ComponentT4 : struct
            where ComponentT5 : struct
        {
            if(index < count - 1)
            {
                c1.Swap(index, count - 1);
                if (c2.IndexOf(entity) == index)
                {
                    c2.Swap(index, count - 1);
                }
                else throw new InvalidOperationException(nameof(Component<ComponentT2>));

                if (c3.IndexOf(entity) == index)
                {
                    c3.Swap(index, count - 1);
                }
                else throw new InvalidOperationException(nameof(Component<ComponentT3>));

                if (c4.IndexOf(entity) == index)
                {
                    c4.Swap(index, count - 1);
                }
                else throw new InvalidOperationException(nameof(Component<ComponentT4>));

                if (c5.IndexOf(entity) == index)
                {
                    c5.Swap(index, count - 1);
                }
                else throw new InvalidOperationException(nameof(Component<ComponentT5>));
            }
            index = --count;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void MoveComponent<T>(Component<T> component, ref int index)
            where T : struct
        {
            if (index != count)
            {
                if (component.Swap(index, count))
                {
                    index = count;
                }
                else throw new InvalidOperationException();
            }
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGetEntity(EntityId entity, out int index)
        {
            index = component1?.IndexOf(entity) ?? -1;
            return (index >= 0 && index < count);
        }

        #region IComponentGroup Members
        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void IComponentGroup.ComponentAdded<T>(EntityId entity, ref int index)
        {
            if (typeof(T) == typeof(T1))
            {
                MoveIntoGroup(entity, component1!, ref index, component2!, component3!, component4!, component5!);
            }
            else if (typeof(T) == typeof(T2))
            {
                MoveIntoGroup(entity, component2!, ref index, component1!, component3!, component4!, component5!);
            }
            else if (typeof(T) == typeof(T3))
            {
                MoveIntoGroup(entity, component3!, ref index, component1!, component2!, component4!, component5!);
            }
            else if (typeof(T) == typeof(T4))
            {
                MoveIntoGroup(entity, component4!, ref index, component1!, component2!, component3!, component5!);
            }
            else if (typeof(T) == typeof(T5))
            {
                MoveIntoGroup(entity, component5!, ref index, component1!, component2!, component3!, component4!);
            }
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void IComponentGroup.ComponentRemoved<T>(EntityId entity, ref int index)
        {
            if (index >= 0 && index <= count)
            {
                if (typeof(T) == typeof(T1))
                {
                    MoveOutOfGroup(entity, component1!, ref index, component2!, component3!, component4!, component5!);
                }
                else if (typeof(T) == typeof(T2))
                {
                    MoveOutOfGroup(entity, component2!, ref index, component1!, component3!, component4!, component5!);
                }
                else if (typeof(T) == typeof(T3))
                {
                    MoveOutOfGroup(entity, component3!, ref index, component1!, component2!, component4!, component5!);
                }
                else if (typeof(T) == typeof(T4))
                {
                    MoveOutOfGroup(entity, component4!, ref index, component1!, component2!, component3!, component5!);
                }
                else if (typeof(T) == typeof(T5))
                {
                    MoveOutOfGroup(entity, component5!, ref index, component1!, component2!, component3!, component4!);
                }
            }
        }
        
        bool IComponentGroup.OnRequest<T>(AccessManager.DependencyTreeResolver resolver, UInt32 uniqueId) 
            where T : struct
        {
            if (typeof(T) == typeof(T1))
            {
                return AddDependencies(resolver, uniqueId, component2!) |
                       AddDependencies(resolver, uniqueId, component3!) |
                       AddDependencies(resolver, uniqueId, component4!) |
                       AddDependencies(resolver, uniqueId, component5!);
            }
            else if (typeof(T) == typeof(T2))
            {
                return AddDependencies(resolver, uniqueId, component1!) |
                       AddDependencies(resolver, uniqueId, component3!) |
                       AddDependencies(resolver, uniqueId, component4!) |
                       AddDependencies(resolver, uniqueId, component5!);
            }
            else  if (typeof(T) == typeof(T3))
            {
                return AddDependencies(resolver, uniqueId, component1!) |
                       AddDependencies(resolver, uniqueId, component2!) |
                       AddDependencies(resolver, uniqueId, component4!) |
                       AddDependencies(resolver, uniqueId, component5!);
            }
            else  if (typeof(T) == typeof(T4))
            {
                return AddDependencies(resolver, uniqueId, component1!) |
                       AddDependencies(resolver, uniqueId, component2!) |
                       AddDependencies(resolver, uniqueId, component3!) |
                       AddDependencies(resolver, uniqueId, component5!);
            }
            else  if (typeof(T) == typeof(T5))
            {
                return AddDependencies(resolver, uniqueId, component1!) |
                       AddDependencies(resolver, uniqueId, component2!) |
                       AddDependencies(resolver, uniqueId, component3!) |
                       AddDependencies(resolver, uniqueId, component4!);
            }
            else
            {
                return AddDependencies(resolver, uniqueId, component1!) |
                       AddDependencies(resolver, uniqueId, component2!) |
                       AddDependencies(resolver, uniqueId, component3!) |
                       AddDependencies(resolver, uniqueId, component4!) |
                       AddDependencies(resolver, uniqueId, component5!);
            }
        }
        #endregion
        
        /// <summary>
        /// Initializes a new group instance based on the components provided
        /// </summary>
        /// <param name="component1">A component to be managed by this group</param>
        /// <param name="component2">A component to be managed by this group</param>
        /// <param name="component3">A component to be managed by this group</param>
        /// <param name="component4">A component to be managed by this group</param>
        /// <param name="component5">A component to be managed by this group</param>
        /// <returns>The newly created group instance</returns>
        /// <exception cref="ArgumentException">Thrown if the group tries to override an already existing group</exception>
        /// <exception cref="AccessViolationException">Thrown if component access is performed across different shards</exception>
        public static OwnedGroup<T1, T2, T3, T4, T5> Initialize(Component<T1> component1, Component<T2> component2, Component<T3> component3, Component<T4> component4, Component<T5> component5)
        {
            if (component1.ShardId == component2.ShardId)
            {
                OwnedGroup<T1, T2, T3, T4, T5> result = new OwnedGroup<T1, T2, T3, T4, T5>();
                if (!component1.AttachGroup(result))
                {
                    throw new ArgumentException(nameof(component1));
                }
                else if (!component2.AttachGroup(result))
                {
                    component2.ReleaseGroup(result);
                    throw new ArgumentException(nameof(component2));
                }
                else if (!component3.AttachGroup(result))
                {
                    component3.ReleaseGroup(result);
                    throw new ArgumentException(nameof(component3));
                }
                else if (!component4.AttachGroup(result))
                {
                    component4.ReleaseGroup(result);
                    throw new ArgumentException(nameof(component4));
                }
                else if (!component5.AttachGroup(result))
                {
                    component5.ReleaseGroup(result);
                    throw new ArgumentException(nameof(component5));
                }
                else
                {
                    result.component1 = component1;
                    result.component2 = component2;
                    result.component3 = component3;
                    result.component4 = component4;
                    result.component5 = component5;
                    return result;
                }
            }
            else throw new AccessViolationException();
        }
    }
    /// <summary>
    /// Aligns multiple component pools so that iterating over specific component combinations runs at maximum speed
    /// </summary>
    /// <typeparam name="T1">A component type</typeparam>
    /// <typeparam name="T2">A component type</typeparam>
    /// <typeparam name="T3">A component type</typeparam>
    /// <typeparam name="T4">A component type</typeparam>
    /// <typeparam name="T5">A component type</typeparam>
    /// <typeparam name="T6">A component type</typeparam>
    #if EXPORT_HAMPER_CORE_COMPOSITION
    public
    #else
    internal
    #endif
    class OwnedGroup<T1, T2, T3, T4, T5, T6> : IComponentGroup
        where T1 : struct
        where T2 : struct
        where T3 : struct
        where T4 : struct
        where T5 : struct
        where T6 : struct
    {
        private Component<T1>? component1;
        private Component<T2>? component2;
        private Component<T3>? component3;
        private Component<T4>? component4;
        private Component<T5>? component5;
        private Component<T6>? component6;
        
        private int count;
        /// <inheritdoc/>
        public int Count
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return count; }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private OwnedGroup()
        {
            this.count = 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        bool AddDependencies<T>(AccessManager.DependencyTreeResolver resolver, UInt32 uniqueId, Component<T> instance)
            where T : struct
        {
            switch (resolver.GetAccess(uniqueId))
            {
                case AccessType.Mutable: return resolver.AddConditional<Component<T>, MutablePolicy>(resolver.GetUniqueId(instance), instance);
                default: return false;
            }
        }
        
        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlySpan<EntityId> AsReadOnlySpan()
        {
            // Requires mutable access when scheduled
            AccessManager.ThrowOnLessAccessible<Component<T1>>(component1!, AccessType.Immutable);
            
            return component1!.AsReadOnlySpan()
                .Slice(0, count);
        }
        
        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Dispose()
        {
            component1?.ReleaseGroup(this);
            component2?.ReleaseGroup(this);
            component3?.ReleaseGroup(this);
            component4?.ReleaseGroup(this);
            component5?.ReleaseGroup(this);
            component6?.ReleaseGroup(this);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void MoveIntoGroup<ComponentT1, ComponentT2, ComponentT3, ComponentT4, ComponentT5, ComponentT6>(EntityId entity, Component<ComponentT1> c1, ref int index, Component<ComponentT2> c2, Component<ComponentT3> c3, Component<ComponentT4> c4, Component<ComponentT5> c5, Component<ComponentT6> c6)
            where ComponentT1 : struct
            where ComponentT2 : struct
            where ComponentT3 : struct
            where ComponentT4 : struct
            where ComponentT5 : struct
            where ComponentT6 : struct
        {
            int i2 = c2.IndexOf(entity);
            int i3 = c3.IndexOf(entity);
            int i4 = c4.IndexOf(entity);
            int i5 = c5.IndexOf(entity);
            int i6 = c6.IndexOf(entity);
            if (index >= 0 && i2 >= 0 && i3 >= 0 && i4 >= 0 && i5 >= 0 && i6 >= 0)
            {
                MoveComponent(c1, ref index);
                MoveComponent(c2, ref i2);
                MoveComponent(c3, ref i3);
                MoveComponent(c4, ref i4);
                MoveComponent(c5, ref i5);
                MoveComponent(c6, ref i6);
                
                count++;
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void MoveOutOfGroup<ComponentT1, ComponentT2, ComponentT3, ComponentT4, ComponentT5, ComponentT6>(EntityId entity, Component<ComponentT1> c1, ref int index, Component<ComponentT2> c2, Component<ComponentT3> c3, Component<ComponentT4> c4, Component<ComponentT5> c5, Component<ComponentT6> c6)
            where ComponentT1 : struct
            where ComponentT2 : struct
            where ComponentT3 : struct
            where ComponentT4 : struct
            where ComponentT5 : struct
            where ComponentT6 : struct
        {
            if(index < count - 1)
            {
                c1.Swap(index, count - 1);
                if (c2.IndexOf(entity) == index)
                {
                    c2.Swap(index, count - 1);
                }
                else throw new InvalidOperationException(nameof(Component<ComponentT2>));

                if (c3.IndexOf(entity) == index)
                {
                    c3.Swap(index, count - 1);
                }
                else throw new InvalidOperationException(nameof(Component<ComponentT3>));

                if (c4.IndexOf(entity) == index)
                {
                    c4.Swap(index, count - 1);
                }
                else throw new InvalidOperationException(nameof(Component<ComponentT4>));

                if (c5.IndexOf(entity) == index)
                {
                    c5.Swap(index, count - 1);
                }
                else throw new InvalidOperationException(nameof(Component<ComponentT5>));

                if (c6.IndexOf(entity) == index)
                {
                    c6.Swap(index, count - 1);
                }
                else throw new InvalidOperationException(nameof(Component<ComponentT6>));
            }
            index = --count;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void MoveComponent<T>(Component<T> component, ref int index)
            where T : struct
        {
            if (index != count)
            {
                if (component.Swap(index, count))
                {
                    index = count;
                }
                else throw new InvalidOperationException();
            }
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGetEntity(EntityId entity, out int index)
        {
            index = component1?.IndexOf(entity) ?? -1;
            return (index >= 0 && index < count);
        }

        #region IComponentGroup Members
        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void IComponentGroup.ComponentAdded<T>(EntityId entity, ref int index)
        {
            if (typeof(T) == typeof(T1))
            {
                MoveIntoGroup(entity, component1!, ref index, component2!, component3!, component4!, component5!, component6!);
            }
            else if (typeof(T) == typeof(T2))
            {
                MoveIntoGroup(entity, component2!, ref index, component1!, component3!, component4!, component5!, component6!);
            }
            else if (typeof(T) == typeof(T3))
            {
                MoveIntoGroup(entity, component3!, ref index, component1!, component2!, component4!, component5!, component6!);
            }
            else if (typeof(T) == typeof(T4))
            {
                MoveIntoGroup(entity, component4!, ref index, component1!, component2!, component3!, component5!, component6!);
            }
            else if (typeof(T) == typeof(T5))
            {
                MoveIntoGroup(entity, component5!, ref index, component1!, component2!, component3!, component4!, component6!);
            }
            else if (typeof(T) == typeof(T6))
            {
                MoveIntoGroup(entity, component6!, ref index, component1!, component2!, component3!, component4!, component5!);
            }
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void IComponentGroup.ComponentRemoved<T>(EntityId entity, ref int index)
        {
            if (index >= 0 && index <= count)
            {
                if (typeof(T) == typeof(T1))
                {
                    MoveOutOfGroup(entity, component1!, ref index, component2!, component3!, component4!, component5!, component6!);
                }
                else if (typeof(T) == typeof(T2))
                {
                    MoveOutOfGroup(entity, component2!, ref index, component1!, component3!, component4!, component5!, component6!);
                }
                else if (typeof(T) == typeof(T3))
                {
                    MoveOutOfGroup(entity, component3!, ref index, component1!, component2!, component4!, component5!, component6!);
                }
                else if (typeof(T) == typeof(T4))
                {
                    MoveOutOfGroup(entity, component4!, ref index, component1!, component2!, component3!, component5!, component6!);
                }
                else if (typeof(T) == typeof(T5))
                {
                    MoveOutOfGroup(entity, component5!, ref index, component1!, component2!, component3!, component4!, component6!);
                }
                else if (typeof(T) == typeof(T6))
                {
                    MoveOutOfGroup(entity, component6!, ref index, component1!, component2!, component3!, component4!, component5!);
                }
            }
        }
        
        bool IComponentGroup.OnRequest<T>(AccessManager.DependencyTreeResolver resolver, UInt32 uniqueId) 
            where T : struct
        {
            if (typeof(T) == typeof(T1))
            {
                return AddDependencies(resolver, uniqueId, component2!) |
                       AddDependencies(resolver, uniqueId, component3!) |
                       AddDependencies(resolver, uniqueId, component4!) |
                       AddDependencies(resolver, uniqueId, component5!) |
                       AddDependencies(resolver, uniqueId, component6!);
            }
            else if (typeof(T) == typeof(T2))
            {
                return AddDependencies(resolver, uniqueId, component1!) |
                       AddDependencies(resolver, uniqueId, component3!) |
                       AddDependencies(resolver, uniqueId, component4!) |
                       AddDependencies(resolver, uniqueId, component5!) |
                       AddDependencies(resolver, uniqueId, component6!);
            }
            else  if (typeof(T) == typeof(T3))
            {
                return AddDependencies(resolver, uniqueId, component1!) |
                       AddDependencies(resolver, uniqueId, component2!) |
                       AddDependencies(resolver, uniqueId, component4!) |
                       AddDependencies(resolver, uniqueId, component5!) |
                       AddDependencies(resolver, uniqueId, component6!);
            }
            else  if (typeof(T) == typeof(T4))
            {
                return AddDependencies(resolver, uniqueId, component1!) |
                       AddDependencies(resolver, uniqueId, component2!) |
                       AddDependencies(resolver, uniqueId, component3!) |
                       AddDependencies(resolver, uniqueId, component5!) |
                       AddDependencies(resolver, uniqueId, component6!);
            }
            else  if (typeof(T) == typeof(T5))
            {
                return AddDependencies(resolver, uniqueId, component1!) |
                       AddDependencies(resolver, uniqueId, component2!) |
                       AddDependencies(resolver, uniqueId, component3!) |
                       AddDependencies(resolver, uniqueId, component4!) |
                       AddDependencies(resolver, uniqueId, component6!);
            }
            else if (typeof(T) == typeof(T6))
            {
                return AddDependencies(resolver, uniqueId, component1!) |
                       AddDependencies(resolver, uniqueId, component2!) |
                       AddDependencies(resolver, uniqueId, component3!) |
                       AddDependencies(resolver, uniqueId, component4!) |
                       AddDependencies(resolver, uniqueId, component5!);
            }
            else
            {
                return AddDependencies(resolver, uniqueId, component1!) |
                       AddDependencies(resolver, uniqueId, component2!) |
                       AddDependencies(resolver, uniqueId, component3!) |
                       AddDependencies(resolver, uniqueId, component4!) |
                       AddDependencies(resolver, uniqueId, component5!) |
                       AddDependencies(resolver, uniqueId, component6!);
            }
        }
        #endregion
        
        /// <summary>
        /// Initializes a new group instance based on the components provided
        /// </summary>
        /// <param name="component1">A component to be managed by this group</param>
        /// <param name="component2">A component to be managed by this group</param>
        /// <param name="component3">A component to be managed by this group</param>
        /// <param name="component4">A component to be managed by this group</param>
        /// <param name="component5">A component to be managed by this group</param>
        /// <param name="component6">A component to be managed by this group</param>
        /// <returns>The newly created group instance</returns>
        /// <exception cref="ArgumentException">Thrown if the group tries to override an already existing group</exception>
        /// <exception cref="AccessViolationException">Thrown if component access is performed across different shards</exception>
        public static OwnedGroup<T1, T2, T3, T4, T5, T6> Initialize(Component<T1> component1, Component<T2> component2, Component<T3> component3, Component<T4> component4, Component<T5> component5, Component<T6> component6)
        {
            if (component1.ShardId == component2.ShardId)
            {
                OwnedGroup<T1, T2, T3, T4, T5, T6> result = new OwnedGroup<T1, T2, T3, T4, T5, T6>();
                if (!component1.AttachGroup(result))
                {
                    throw new ArgumentException(nameof(component1));
                }
                else if (!component2.AttachGroup(result))
                {
                    component2.ReleaseGroup(result);
                    throw new ArgumentException(nameof(component2));
                }
                else if (!component3.AttachGroup(result))
                {
                    component3.ReleaseGroup(result);
                    throw new ArgumentException(nameof(component3));
                }
                else if (!component4.AttachGroup(result))
                {
                    component4.ReleaseGroup(result);
                    throw new ArgumentException(nameof(component4));
                }
                else if (!component5.AttachGroup(result))
                {
                    component5.ReleaseGroup(result);
                    throw new ArgumentException(nameof(component5));
                }
                else if (!component6.AttachGroup(result))
                {
                    component6.ReleaseGroup(result);
                    throw new ArgumentException(nameof(component6));
                }
                else
                {
                    result.component1 = component1;
                    result.component2 = component2;
                    result.component3 = component3;
                    result.component4 = component4;
                    result.component5 = component5;
                    result.component6 = component6;
                    return result;
                }
            }
            else throw new AccessViolationException();
        }
    }
}