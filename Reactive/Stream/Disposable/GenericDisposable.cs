// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;

namespace Soe.Reactive
{
    /// <summary>
    /// Represents a generic disposable resource that can be checked for its current state
    /// </summary>
    #if EXPORT_HAMPER_CORE_REACTIVE
    public
    #else
    internal
    #endif
    class Disposable<T, Target, Handler> : Disposable
        where T : class
        where Target : class
        where Handler : struct, IDisposeHandler<Target, T>
    {
        private readonly T instance;
        private readonly Target target;

        /// <summary>
        /// Initializes a new disposable instance on an object of type <typeparamref name="T"/>
        /// </summary>
        /// <param name="target">The target object to perform the operation on</param>
        /// <param name="instance">The instance to interact with the <typeparamref name="Handler"/></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Disposable(Target target, T instance)
        {
            this.instance = instance;
            this.target = target;
        }
        
        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected override void DisposeInternal()
        {
            Handler handler = default;
            handler.Invoke(target, instance);
        }
    }
    
    /// <summary>
    /// Represents a generic disposable resource that can be checked for its current state
    /// </summary>
    #if EXPORT_HAMPER_CORE_REACTIVE
    public
    #else
    internal
    #endif
    class Disposable<TKey, TValue, Target, Handler> : Disposable
        where TValue : class
        where Target : class
        where Handler : struct, IDisposeHandler<Target, TKey, TValue>
    {
        private readonly TKey key;
        private readonly TValue instance;
        private readonly Target target;

        /// <summary>
        /// Initializes a new disposable instance on an object of type <typeparamref name="TValue"/>
        /// </summary>
        /// <param name="target">The target object to perform the operation on</param>
        /// <param name="key">The object that provides information about the observer</param>
        /// <param name="instance">The instance to interact with the <typeparamref name="Handler"/></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Disposable(Target target, TKey key, TValue instance)
        {
            this.instance = instance;
            this.key = key;
            this.target = target;
        }
        
        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected override void DisposeInternal()
        {
            Handler handler = default;
            handler.Invoke(target, key, instance);
        }
    }
}