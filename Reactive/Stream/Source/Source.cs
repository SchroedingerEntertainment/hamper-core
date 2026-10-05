// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System;
using System.Runtime.CompilerServices;

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
    abstract class Source<T, DispatchingStrategy, SubscriptionStrategy> : System.IObservable<T>
        where DispatchingStrategy : struct, IDispatchingStrategy<T>
        where SubscriptionStrategy : struct, ISubscriptionStrategy<T>
    {
        private readonly struct DisposeHandler : IDisposeHandler<Source<T, DispatchingStrategy, SubscriptionStrategy>, IObserver<T>>
        {
            /// <inheritdoc/>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void Invoke(Source<T, DispatchingStrategy, SubscriptionStrategy> target, IObserver<T> instance)
            {
                target.strategy.Subscriptions.Remove(instance);
            }
        }
        
        /// <summary>
        /// The overall managing strategy 
        /// </summary>
        protected ObservableStrategy<T, DispatchingStrategy, SubscriptionStrategy> strategy;
        
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
        public ObservableStrategy<T, DispatchingStrategy, SubscriptionStrategy> GetStrategy()
        {
            return default;
        }
        
        /// <summary>
        /// Provides the underlying observers with notification data
        /// </summary>
        /// <param name="value">Provides the observer with new data</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected virtual void OnNext(in T value)
        {
            if (strategy.Subscriptions.Count > 0)
            {
                strategy.Subscriptions.OnNext(strategy.Dispatcher, value);
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
        public virtual IDisposable Subscribe(IObserver<T> observer)
        {
            if (strategy.Subscriptions.Add(observer))
            {
                return new Disposable<IObserver<T>, Source<T, DispatchingStrategy, SubscriptionStrategy>, DisposeHandler>(this, observer);
            }
            else return Disposable.Empty;
        }
    }
}