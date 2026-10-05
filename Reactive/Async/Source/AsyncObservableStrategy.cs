// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;

namespace Soe.Reactive
{
    /// <summary>
    /// A management object controlling the behavior of a data source
    /// </summary>
    /// <typeparam name="T">The object that provides notification information</typeparam>
    /// <typeparam name="DispatchingStrategy">An object managing how and when data is propagated</typeparam>
    /// <typeparam name="SubscriptionStrategy">An object managing how subscribers are handled</typeparam>
    #if EXPORT_HAMPER_CORE_REACTIVE
    public
    #else
    internal
    #endif
    struct AsyncObservableStrategy<T, DispatchingStrategy,  SubscriptionStrategy>
        where DispatchingStrategy : struct, IAsyncDispatchingStrategy<T>
        where SubscriptionStrategy : struct, IAsyncSubscriptionStrategy<T>
    {
        /// <summary>
        /// An object managing how subscribers are handled
        /// </summary>
        public SubscriptionStrategy Subscriptions;
        /// <summary>
        /// An object managing how and when data is propagated
        /// </summary>
        public DispatchingStrategy Dispatcher;

        /// <summary>
        /// Initializes a new instance of this object
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public AsyncObservableStrategy()
        {
            this.Subscriptions = default;
            this.Dispatcher = default;
        }
    }
    
    /// <summary>
    /// A management object controlling the conditional behavior of a data source
    /// </summary>
    /// <typeparam name="TKey">The object that provides information about the target subscriptions</typeparam>
    /// <typeparam name="TValue">The object that provides notification information</typeparam>
    /// <typeparam name="DispatchingStrategy">An object managing how and when data is propagated</typeparam>
    /// <typeparam name="SubscriptionStrategy">An object managing how subscribers are handled</typeparam>
    #if EXPORT_HAMPER_CORE_REACTIVE
    public
    #else
    internal
    #endif
    struct AsyncObservableStrategy<TKey, TValue, DispatchingStrategy,  SubscriptionStrategy>
        where DispatchingStrategy : struct, IAsyncDispatchingStrategy<TValue>
        where SubscriptionStrategy : struct, IAsyncSubscriptionStrategy<TKey, TValue>
    {
        /// <summary>
        /// An object managing how subscribers are handled
        /// </summary>
        public SubscriptionStrategy Subscriptions;
        /// <summary>
        /// An object managing how and when data is propagated
        /// </summary>
        public DispatchingStrategy Dispatcher;

        /// <summary>
        /// Initializes a new instance of this object
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public AsyncObservableStrategy()
        {
            this.Subscriptions = default;
            this.Dispatcher = default;
        }
    }
}