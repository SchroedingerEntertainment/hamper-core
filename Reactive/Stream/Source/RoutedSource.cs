// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System;
using System.Runtime.CompilerServices;

namespace Soe.Reactive
{
    /// <summary>
    /// Represents the conditional entry point of an observable data stream
    /// </summary>
    /// <typeparam name="TKey">The object that provides information about the target subscriptions</typeparam>
    /// <typeparam name="TValue">The object that provides notification information</typeparam>
    /// <typeparam name="DispatchingStrategy">An object managing how and when data is propagated</typeparam>
    /// <typeparam name="SubscriptionStrategy">An object managing how subscribers are handled</typeparam>
    /// <remarks>The notification information is provided conditionally based on the underlying strategy and <typeparamref name="TKey"/></remarks>
    #if EXPORT_HAMPER_CORE_REACTIVE
    public
    #else
    internal
    #endif
    abstract class Source<TKey, TValue, DispatchingStrategy, SubscriptionStrategy> : IObservable<TKey, TValue>
        where DispatchingStrategy : struct, IDispatchingStrategy<TValue>
        where SubscriptionStrategy : struct, ISubscriptionStrategy<TKey, TValue>
    {
        private readonly struct DisposeHandler : IDisposeHandler<Source<TKey, TValue, DispatchingStrategy, SubscriptionStrategy>, TKey, IObserver<TValue>>
        {
            /// <inheritdoc/>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void Invoke(Source<TKey, TValue, DispatchingStrategy, SubscriptionStrategy> target, in TKey key, IObserver<TValue> instance)
            {
                target.strategy.Subscriptions.Remove(key, instance);
            }
        }
        
        /// <summary>
        /// The overall managing strategy 
        /// </summary>
        protected ObservableStrategy<TKey, TValue, DispatchingStrategy, SubscriptionStrategy> strategy;
        
        /// <summary>
        /// Initializes a new instance of the data source
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected Source()
        {
            this.strategy = default;
        }

        /// <summary>
        /// Gets the management object of this data source
        /// </summary>
        /// <returns>An empty copy of the management object</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ObservableStrategy<TKey, TValue, DispatchingStrategy, SubscriptionStrategy> GetStrategy()
        {
            return default;
        }

        /// <summary>
        /// Conditionally provides the underlying observers with notification data
        /// </summary>
        /// <param name="key">Provides information about the target observers</param>
        /// <param name="value">Provides the observer with new data</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected virtual void OnNext(in TKey key, in TValue value)
        {
            if (strategy.Subscriptions.Count > 0)
            {
                strategy.Subscriptions.OnNext(strategy.Dispatcher, key, value);
            }
        }
        
        /// <summary>
        /// Notifies the underlying observers that the provider has experienced an error condition
        /// </summary>
        /// <param name="error">An object that provides additional information about the error</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected virtual void OnError(Exception error) 
        {
            if (strategy.Subscriptions.Count > 0)
            {
                strategy.Subscriptions.OnError(strategy.Dispatcher, error);
            }
        }

        /// <summary>
        /// Notifies the underlying observers that the provider has finished sending push-based notifications
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected virtual void OnCompleted()
        {
            if (strategy.Subscriptions.Count > 0)
            {
                strategy.Subscriptions.OnCompleted(strategy.Dispatcher);
            }
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual IDisposable Subscribe(in TKey key, IObserver<TValue> observer)
        {
            if (strategy.Subscriptions.Add(key, observer))
            {
                return new Disposable<TKey, IObserver<TValue>, Source<TKey, TValue, DispatchingStrategy, SubscriptionStrategy>, DisposeHandler>(this, key, observer);
            }
            else return Disposable.Empty;
        }
    }
}