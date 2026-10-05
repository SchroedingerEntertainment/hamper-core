// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Soe.Reactive
{
    /// <summary>
    /// Represents the entry point of an observable data stream
    /// </summary>
    /// <typeparam name="T">The object that provides notification information</typeparam>
    /// <typeparam name="DispatchingStrategy">An object managing how and when data is propagated</typeparam>
    /// <typeparam name="SubscriptionStrategy">An object managing how subscribers are handled</typeparam>
    #if EXPORT_HAMPER_CORE_REACTIVE
    public
    #else
    internal
    #endif
    abstract class AsyncSource<T, DispatchingStrategy, SubscriptionStrategy> : IAsyncObservable<T>
        where DispatchingStrategy : struct, IAsyncDispatchingStrategy<T>
        where SubscriptionStrategy : struct, IAsyncSubscriptionStrategy<T>
    {
        private readonly struct DisposeHandler : IDisposeHandler<AsyncSource<T, DispatchingStrategy, SubscriptionStrategy>, IAsyncObserver<T>>
        {
            /// <inheritdoc/>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void Invoke(AsyncSource<T, DispatchingStrategy, SubscriptionStrategy> target, IAsyncObserver<T> instance)
            {
                target.strategy.Subscriptions.Remove(instance);
            }
        }
        
        /// <summary>
        /// The overall managing strategy 
        /// </summary>
        protected AsyncObservableStrategy<T, DispatchingStrategy, SubscriptionStrategy> strategy;
        
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
        public AsyncObservableStrategy<T, DispatchingStrategy, SubscriptionStrategy> GetStrategy()
        {
            return default;
        }
        
        /// <summary>
        /// Provides the underlying observers with notification data
        /// </summary>
        /// <param name="value">Provides the observer with new data</param>
        /// <returns>A task that represents the completion of the notification</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected virtual Task OnNextAsync(in T value)
        {
            if (strategy.Subscriptions.Count > 0)
            {
                return strategy.Subscriptions.OnNextAsync(strategy.Dispatcher, value);
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
        public virtual IDisposable Subscribe(IAsyncObserver<T> observer)
        {
            if (strategy.Subscriptions.Add(observer))
            {
                return new Disposable<IAsyncObserver<T>, AsyncSource<T, DispatchingStrategy, SubscriptionStrategy>, DisposeHandler>(this, observer);
            }
            else return Disposable.Empty;
        }
    }
}