// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System;
using System.Threading.Tasks;

namespace Soe.Reactive
{
    /// <summary>
    /// Provides a mechanism for receiving asynchronous push-based notifications
    /// </summary>
    /// <typeparam name="T">The object that provides notification information</typeparam>
    #if EXPORT_HAMPER_CORE_REACTIVE
    public
    #else
    internal
    #endif
    interface IAsyncObserver<in T>
    {
        /// <summary>
        /// Provides the observer with new data asynchronously
        /// </summary>
        /// <param name="value">The current notification information</param>
        /// <returns>A task that represents the completion of the notification</returns>
        Task OnNextAsync(T value);
        
        /// <summary>
        /// Notifies the observer that the provider has experienced an error condition asynchronously
        /// </summary>
        /// <param name="error">An object that provides additional information about the error</param>
        /// <returns>A task that represents the completion of the notification</returns>
        Task OnErrorAsync(Exception error);
        
        /// <summary>
        /// Notifies the observer that the provider has finished sending push-based notifications asynchronously
        /// </summary>
        /// <returns>A task that represents the completion of the notification</returns>
        Task OnCompletedAsync();
    }
}