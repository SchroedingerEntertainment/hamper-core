// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System;

namespace Soe.Reactive
{
    /// <summary>
    /// A management object controlling how and when the data source provides notification information to observers
    /// </summary>
    /// <typeparam name="T">The object that provides notification information to observers</typeparam>
    #if EXPORT_HAMPER_CORE_REACTIVE
    public
    #else
    internal
    #endif
    interface IDispatchingStrategy<T>
    {
        /// <summary>
        /// Provides the underlying observers with notification data
        /// </summary>
        /// <param name="observers">The instance list of observers to provide notification data to</param>
        /// <param name="value">Provides the observer with new data</param>
        void OnNext(Span<IObserver<T>?> observers, in T value);
        
        /// <summary>
        /// Notifies the underlying observers that the provider has experienced an error condition
        /// </summary>
        /// <param name="observers">The instance list of observers to provide error data to</param>
        /// <param name="error">An object that provides additional information about the error</param>
        void OnError(Span<IObserver<T>?> observers, in Exception error);
        
        /// <summary>
        /// Notifies the underlying observers that the provider has finished sending push-based notifications
        /// </summary>
        /// <param name="observers">The instance list of observers to notify</param>
        void OnCompleted(Span<IObserver<T>?> observers);
    }
}