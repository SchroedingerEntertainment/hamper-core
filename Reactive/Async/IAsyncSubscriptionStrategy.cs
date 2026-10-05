// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System;
using System.Threading.Tasks;

namespace Soe.Reactive
{
    /// <summary>
    /// A management object controlling how the data source handles subscribers
    /// </summary>
    /// <typeparam name="T">The object that provides notification information to observers</typeparam>
    #if EXPORT_HAMPER_CORE_REACTIVE
    public
    #else
    internal
    #endif
    interface IAsyncSubscriptionStrategy<T>
    {
        /// <summary>
        /// Gets the amount of observers managed by this object
        /// </summary>
        int Count
        {
            get;
        }
        
        /// <summary>
        /// Tries to add a new observer to this object
        /// </summary>
        /// <param name="observer">An object to receive notifications</param>
        /// <returns>True if the observer was added successfully, false otherwise</returns>
        bool Add(IAsyncObserver<T> observer);
        
        /// <summary>
        /// Provides the underlying observers with notification data asynchronously
        /// </summary>
        /// <param name="dispatcher">An object managing how and when data is propagated</param>
        /// <param name="value">Provides the observer with new data</param>
        /// <typeparam name="Dispatcher">A specifically typed management object controlling data propagation</typeparam>
        /// <returns>A task that represents the completion of the notification</returns>
        Task OnNextAsync<Dispatcher>(in Dispatcher dispatcher, in T value)
            where Dispatcher : IAsyncDispatchingStrategy<T>;
        
        /// <summary>
        /// Notifies the underlying observers that the provider has experienced an error condition asynchronously
        /// </summary>
        /// <param name="dispatcher">An object managing how and when data is propagated</param>
        /// <param name="error">An object that provides additional information about the error</param>
        /// <typeparam name="Dispatcher">A specifically typed management object controlling data propagation</typeparam>
        /// <returns>A task that represents the completion of the notification</returns>
        Task OnErrorAsync<Dispatcher>(in Dispatcher dispatcher, in Exception error)
            where Dispatcher : IAsyncDispatchingStrategy<T>;
        
        /// <summary>
        /// Notifies the underlying observers that the provider has finished sending push-based notifications asynchronously
        /// </summary>
        /// <param name="dispatcher">An object managing how and when data is propagated</param>
        /// <typeparam name="Dispatcher">A specifically typed management object controlling data propagation</typeparam>
        /// <returns>A task that represents the completion of the notification</returns>
        Task OnCompletedAsync<Dispatcher>(in Dispatcher dispatcher)
            where Dispatcher : IAsyncDispatchingStrategy<T>;
        
        /// <summary>
        /// Removes an existing observer from this object
        /// </summary>
        /// <param name="observer">An object to receive notifications</param>
        void Remove(IAsyncObserver<T> observer);
    }
    
    /// <summary>
    /// A management object controlling how the data source handles subscribers
    /// </summary>
    /// <typeparam name="TKey">The object that provides information about the target subscriptions</typeparam>
    /// <typeparam name="TValue">The object that provides notification information to observers</typeparam>
    #if EXPORT_HAMPER_CORE_REACTIVE
    public
    #else
    internal
    #endif
    interface IAsyncSubscriptionStrategy<TKey, TValue>
    {
        /// <summary>
        /// Gets the amount of observers managed by this object
        /// </summary>
        int Count
        {
            get;
        }
        
        /// <summary>
        /// Tries to add a new observer to this object
        /// </summary>
        /// <param name="key">The object that provides information about the observer</param>
        /// <param name="observer">An object to receive notifications</param>
        /// <returns>True if the observer was added successfully, false otherwise</returns>
        bool Add(TKey key, IAsyncObserver<TValue> observer);

        /// <summary>
        /// Conditionally provides the underlying observers with notification data asynchronously
        /// </summary>
        /// <param name="dispatcher">An object managing how and when data is propagated</param>
        /// <param name="key">Provides information about the target observers</param>
        /// <param name="value">Provides the observer with new data</param>
        /// <typeparam name="Dispatcher">A specifically typed management object controlling data propagation</typeparam>
        /// <returns>A task that represents the completion of the notification</returns>
        Task OnNextAsync<Dispatcher>(in Dispatcher dispatcher, in TKey key, in TValue value)
            where Dispatcher : IAsyncDispatchingStrategy<TValue>;
        
        /// <summary>
        /// Notifies the underlying observers that the provider has experienced an error condition asynchronously
        /// </summary>
        /// <param name="dispatcher">An object managing how and when data is propagated</param>
        /// <param name="error">An object that provides additional information about the error</param>
        /// <typeparam name="Dispatcher">A specifically typed management object controlling data propagation</typeparam>
        /// <returns>A task that represents the completion of the notification</returns>
        Task OnErrorAsync<Dispatcher>(in Dispatcher dispatcher, in Exception error)
            where Dispatcher : IAsyncDispatchingStrategy<TValue>;
        
        /// <summary>
        /// Notifies the underlying observers that the provider has finished sending push-based notifications asynchronously
        /// </summary>
        /// <param name="dispatcher">An object managing how and when data is propagated</param>
        /// <typeparam name="Dispatcher">A specifically typed management object controlling data propagation</typeparam>
        /// <returns>A task that represents the completion of the notification</returns>
        Task OnCompletedAsync<Dispatcher>(in Dispatcher dispatcher)
            where Dispatcher : IAsyncDispatchingStrategy<TValue>;
        
        /// <summary>
        /// Removes an existing observer from this object
        /// </summary>
        /// <param name="key">The object that provides information about the observer</param>
        /// <param name="observer">An object to receive notifications</param>
        void Remove(TKey key, IAsyncObserver<TValue> observer);
    }
}