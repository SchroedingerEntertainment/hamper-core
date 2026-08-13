// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System;
using System.Runtime.CompilerServices;

namespace Soe.Reactive
{
    #if EXPORT_HAMPER_CORE_REACTIVE
    public
    #else
    internal
    #endif
    static partial class IObservableExtension
    {
        #region IObservable<T>
        /// <summary>
        /// Creates a breakout node on an existing data source providing its own dispatching and subsciber strategies
        /// </summary>
        /// <param name="observable">The object that is to receive notifications</param>
        /// <param name="strategy">A management object controlling the behavior of the breakout node</param>
        /// <param name="disposable">A reference to an interface that allows observers to stop receiving notifications before
        /// the provider has finished sending them</param>
        /// <typeparam name="T">The object that provides notification information</typeparam>
        /// <typeparam name="DispatchingStrategy">An object managing how and when data is propagated</typeparam>
        /// <typeparam name="SubscriptionStrategy">An object managing how subscribers are handled</typeparam>
        /// <returns>The newly created breakout node</returns>
        /// <exception cref="ChannelConnectionFailedException">An exception thrown when connecting to the data source failed</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Observable<T, DispatchingStrategy, SubscriptionStrategy> Scope<T, DispatchingStrategy, SubscriptionStrategy>(this IObservable<T> observable, ObservableStrategy<T, DispatchingStrategy, SubscriptionStrategy> strategy, out IDisposable disposable)
            where DispatchingStrategy : struct, IDispatchingStrategy<T>
            where SubscriptionStrategy : struct, ISubscriptionStrategy<T>
        {
            var result =  new Observable<T, DispatchingStrategy, SubscriptionStrategy>();
            disposable = observable.Subscribe(result);

            if (ReferenceEquals(disposable, Disposable.Empty))
            {
                throw new ChannelConnectionFailedException(observable, result);
            }
            else return result;
        }
        
        /// <summary>
        /// Creates a breakout node on an existing data source providing its own dispatching and subsciber strategies
        /// </summary>
        /// <param name="observable">The object that is to receive notifications</param>
        /// <param name="strategy">A management object controlling the behavior of the breakout node</param>
        /// <typeparam name="T">The object that provides notification information</typeparam>
        /// <typeparam name="DispatchingStrategy">An object managing how and when data is propagated</typeparam>
        /// <typeparam name="SubscriptionStrategy">An object managing how subscribers are handled</typeparam>
        /// <returns>The newly created breakout node</returns>
        /// <exception cref="ChannelConnectionFailedException">An exception thrown when connecting to the data source failed</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Observable<T, DispatchingStrategy, SubscriptionStrategy> Scope<T, DispatchingStrategy, SubscriptionStrategy>(this IObservable<T> observable, ObservableStrategy<T, DispatchingStrategy, SubscriptionStrategy> strategy)
            where DispatchingStrategy : struct, IDispatchingStrategy<T>
            where SubscriptionStrategy : struct, ISubscriptionStrategy<T>
        {
            return Scope(observable, strategy, out _);
        }
        
        /// <summary>
        /// Creates a breakout node on an existing data source providing its own dispatching and subsciber strategies
        /// </summary>
        /// <param name="observable">The object that is to receive notifications</param>
        /// <param name="dispatcher">An object managing how and when data is propagated</param>
        /// <param name="subscribers">An object managing how subscribers are handled</param>
        /// <param name="disposable">A reference to an interface that allows observers to stop receiving notifications before
        /// the provider has finished sending them</param>
        /// <typeparam name="T">The object that provides notification information</typeparam>
        /// <typeparam name="DispatchingStrategy">An object managing how and when data is propagated</typeparam>
        /// <typeparam name="SubscriptionStrategy">An object managing how subscribers are handled</typeparam>
        /// <returns>The newly created breakout node</returns>
        /// <exception cref="ChannelConnectionFailedException">An exception thrown when connecting to the data source failed</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Observable<T, DispatchingStrategy, SubscriptionStrategy> Scope<T, DispatchingStrategy, SubscriptionStrategy>(this IObservable<T> observable, DispatchingStrategy dispatcher, SubscriptionStrategy subscribers, out IDisposable disposable)
            where DispatchingStrategy : struct, IDispatchingStrategy<T>
            where SubscriptionStrategy : struct, ISubscriptionStrategy<T>
        {
            return Scope<T, DispatchingStrategy, SubscriptionStrategy>(observable, default, out disposable);
        }
        
        /// <summary>
        /// Creates a breakout node on an existing data source providing its own dispatching and subsciber strategies
        /// </summary>
        /// <param name="observable">The object that is to receive notifications</param>
        /// <param name="dispatcher">An object managing how and when data is propagated</param>
        /// <param name="subscribers">An object managing how subscribers are handled</param>
        /// <typeparam name="T">The object that provides notification information</typeparam>
        /// <typeparam name="DispatchingStrategy">An object managing how and when data is propagated</typeparam>
        /// <typeparam name="SubscriptionStrategy">An object managing how subscribers are handled</typeparam>
        /// <returns>The newly created breakout node</returns>
        /// <exception cref="ChannelConnectionFailedException">An exception thrown when connecting to the data source failed</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Observable<T, DispatchingStrategy, SubscriptionStrategy> Scope<T, DispatchingStrategy, SubscriptionStrategy>(this IObservable<T> observable, DispatchingStrategy dispatcher, SubscriptionStrategy subscribers)
            where DispatchingStrategy : struct, IDispatchingStrategy<T>
            where SubscriptionStrategy : struct, ISubscriptionStrategy<T>
        {
            return Scope<T, DispatchingStrategy, SubscriptionStrategy>(observable, default, out _);
        }
        #endregion
        
        #region IObservable<TKey, TValue>
        /// <summary>
        /// Creates a breakout node on an existing data source providing its own dispatching and subsciber strategies
        /// </summary>
        /// <param name="observable">The object that is to receive notifications</param>
        /// <param name="subscriptionKey">The object that provides subscription information about the observer</param>
        /// <param name="strategy">A management object controlling the behavior of the breakout node</param>
        /// <param name="disposable">A reference to an interface that allows observers to stop receiving notifications before
        /// the provider has finished sending them</param>
        /// <typeparam name="TValue">The object that provides notification information</typeparam>
        /// <typeparam name="DispatchingStrategy">An object managing how and when data is propagated</typeparam>
        /// <typeparam name="SubscriptionStrategy">An object managing how subscribers are handled</typeparam>
        /// <returns>The newly created breakout node</returns>
        /// <typeparam name="TKey">The object that provides information about the target subscriptions</typeparam>
        /// <exception cref="ChannelConnectionFailedException">An exception thrown when connecting to the data source failed</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Observable<TValue, DispatchingStrategy, SubscriptionStrategy> Scope<TKey, TValue, DispatchingStrategy, SubscriptionStrategy>(this IObservable<TKey, TValue> observable, TKey subscriptionKey, ObservableStrategy<TValue, DispatchingStrategy, SubscriptionStrategy> strategy, out IDisposable disposable)
            where DispatchingStrategy : struct, IDispatchingStrategy<TValue>
            where SubscriptionStrategy : struct, ISubscriptionStrategy<TValue>
        {
            var result =  new Observable<TValue, DispatchingStrategy, SubscriptionStrategy>();
            disposable = observable.Subscribe(subscriptionKey, result);

            if (ReferenceEquals(disposable, Disposable.Empty))
            {
                throw new ChannelConnectionFailedException(observable, result);
            }
            else return result;
        }
        
        /// <summary>
        /// Creates a breakout node on an existing data source providing its own dispatching and subsciber strategies
        /// </summary>
        /// <param name="observable">The object that is to receive notifications</param>
        /// <param name="subscriptionKey">The object that provides subscription information about the observer</param>
        /// <param name="strategy">A management object controlling the behavior of the breakout node</param>
        /// <typeparam name="TValue">The object that provides notification information</typeparam>
        /// <typeparam name="DispatchingStrategy">An object managing how and when data is propagated</typeparam>
        /// <typeparam name="SubscriptionStrategy">An object managing how subscribers are handled</typeparam>
        /// <returns>The newly created breakout node</returns>
        /// <typeparam name="TKey">The object that provides information about the target subscriptions</typeparam>
        /// <exception cref="ChannelConnectionFailedException">An exception thrown when connecting to the data source failed</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Observable<TValue, DispatchingStrategy, SubscriptionStrategy> Scope<TKey, TValue, DispatchingStrategy, SubscriptionStrategy>(this IObservable<TKey, TValue> observable, TKey subscriptionKey, ObservableStrategy<TValue, DispatchingStrategy, SubscriptionStrategy> strategy)
            where DispatchingStrategy : struct, IDispatchingStrategy<TValue>
            where SubscriptionStrategy : struct, ISubscriptionStrategy<TValue>
        {
            return Scope(observable, subscriptionKey, strategy, out _);
        }
        
        /// <summary>
        /// Creates a breakout node on an existing data source providing its own dispatching and subsciber strategies
        /// </summary>
        /// <param name="observable">The object that is to receive notifications</param>
        /// <param name="subscriptionKey">The object that provides subscription information about the observer</param>
        /// <param name="dispatcher">An object managing how and when data is propagated</param>
        /// <param name="subscribers">An object managing how subscribers are handled</param>
        /// <param name="disposable">A reference to an interface that allows observers to stop receiving notifications before
        /// the provider has finished sending them</param>
        /// <typeparam name="TValue">The object that provides notification information</typeparam>
        /// <typeparam name="DispatchingStrategy">An object managing how and when data is propagated</typeparam>
        /// <typeparam name="SubscriptionStrategy">An object managing how subscribers are handled</typeparam>
        /// <returns>The newly created breakout node</returns>
        /// <typeparam name="TKey">The object that provides information about the target subscriptions</typeparam>
        /// <exception cref="ChannelConnectionFailedException">An exception thrown when connecting to the data source failed</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Observable<TValue, DispatchingStrategy, SubscriptionStrategy> Scope<TKey, TValue, DispatchingStrategy, SubscriptionStrategy>(this IObservable<TKey, TValue> observable, TKey subscriptionKey, DispatchingStrategy dispatcher, SubscriptionStrategy subscribers, out IDisposable disposable)
            where DispatchingStrategy : struct, IDispatchingStrategy<TValue>
            where SubscriptionStrategy : struct, ISubscriptionStrategy<TValue>
        {
            return Scope<TKey, TValue, DispatchingStrategy, SubscriptionStrategy>(observable, subscriptionKey, default, out disposable);
        }
        
        /// <summary>
        /// Creates a breakout node on an existing data source providing its own dispatching and subsciber strategies
        /// </summary>
        /// <param name="observable">The object that is to receive notifications</param>
        /// <param name="subscriptionKey">The object that provides subscription information about the observer</param>
        /// <param name="dispatcher">An object managing how and when data is propagated</param>
        /// <param name="subscribers">An object managing how subscribers are handled</param>
        /// <typeparam name="TValue">The object that provides notification information</typeparam>
        /// <typeparam name="DispatchingStrategy">An object managing how and when data is propagated</typeparam>
        /// <typeparam name="SubscriptionStrategy">An object managing how subscribers are handled</typeparam>
        /// <returns>The newly created breakout node</returns>
        /// <typeparam name="TKey">The object that provides information about the target subscriptions</typeparam>
        /// <exception cref="ChannelConnectionFailedException">An exception thrown when connecting to the data source failed</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Observable<TValue, DispatchingStrategy, SubscriptionStrategy> Scope<TKey, TValue, DispatchingStrategy, SubscriptionStrategy>(this IObservable<TKey, TValue> observable, TKey subscriptionKey, DispatchingStrategy dispatcher, SubscriptionStrategy subscribers)
            where DispatchingStrategy : struct, IDispatchingStrategy<TValue>
            where SubscriptionStrategy : struct, ISubscriptionStrategy<TValue>
        {
            return Scope<TKey, TValue, DispatchingStrategy, SubscriptionStrategy>(observable, subscriptionKey, default, out _);
        }
        #endregion
    }
}