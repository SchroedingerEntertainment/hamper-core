// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

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
    abstract class AsyncSource<TKey, TValue, DispatchingStrategy, SubscriptionStrategy> : IAsyncObservable<TKey, TValue>
        where DispatchingStrategy : struct, IAsyncDispatchingStrategy<TValue>
        where SubscriptionStrategy : struct, IAsyncSubscriptionStrategy<TKey, TValue>
    {
        private readonly struct DisposeHandler : IDisposeHandler<AsyncSource<TKey, TValue, DispatchingStrategy, SubscriptionStrategy>, TKey, IAsyncObserver<TValue>>
        {
            /// <inheritdoc/>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void Invoke(AsyncSource<TKey, TValue, DispatchingStrategy, SubscriptionStrategy> target, in TKey key, IAsyncObserver<TValue> instance)
            {
                target.strategy.Subscriptions.Remove(key, instance);
            }
        }
        
        /// <summary>
        /// The overall managing strategy 
        /// </summary>
        protected AsyncObservableStrategy<TKey, TValue, DispatchingStrategy, SubscriptionStrategy> strategy;
        
        /// <summary>
        /// Initializes a new instance of the data source
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected AsyncSource()
        {
            this.strategy = default;
        }

        /// <summary>
        /// Gets the management object of this data source
        /// </summary>
        /// <returns>An empty copy of the management object</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public AsyncObservableStrategy<TKey, TValue, DispatchingStrategy, SubscriptionStrategy> GetStrategy()
        {
            return default;
        }

        /// <summary>
        /// Conditionally provides the underlying observers with notification data
        /// </summary>
        /// <param name="key">Provides information about the target observers</param>
        /// <param name="value">Provides the observer with new data</param>
        /// <returns>A task that represents the completion of the notification</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected virtual Task OnNextAsync(in TKey key, in TValue value)
        {
            if (strategy.Subscriptions.Count > 0)
            {
                return strategy.Subscriptions.OnNextAsync(strategy.Dispatcher, key, value);
            }
            else return Task.CompletedTask;
        }
        
        /// <summary>
        /// Notifies the underlying observers that the provider has experienced an error condition
        /// </summary>
        /// <param name="error">An object that provides additional information about the error</param>
        /// <returns>A task that represents the completion of the notification</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected virtual Task OnErrorAsync(Exception error) 
        {
            if (strategy.Subscriptions.Count > 0)
            {
                return strategy.Subscriptions.OnErrorAsync(strategy.Dispatcher, error);
            }
            else return Task.CompletedTask;
        }

        /// <summary>
        /// Notifies the underlying observers that the provider has finished sending push-based notifications
        /// </summary>
        /// <returns>A task that represents the completion of the notification</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected virtual Task OnCompletedAsync()
        {
            if (strategy.Subscriptions.Count > 0)
            {
                return strategy.Subscriptions.OnCompletedAsync(strategy.Dispatcher);
            }
            else return Task.CompletedTask;
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual IDisposable Subscribe(in TKey key, IAsyncObserver<TValue> observer)
        {
            if (strategy.Subscriptions.Add(key, observer))
            {
                return new Disposable<TKey, IAsyncObserver<TValue>, AsyncSource<TKey, TValue, DispatchingStrategy, SubscriptionStrategy>, DisposeHandler>(this, key, observer);
            }
            else return Disposable.Empty;
        }
    }
}