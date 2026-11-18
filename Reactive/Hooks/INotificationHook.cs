// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

namespace Soe.Reactive
{
    /// <summary>
    /// The notification target of an interception point
    /// </summary>
    /// <typeparam name="T">The object that provides notification information to this <see cref="INotificationHook{T}"/></typeparam>
    #if EXPORT_HAMPER_CORE_REACTIVE
    public
    #else
    internal
    #endif
    interface INotificationHook<T>
    {
        /// <summary>
        /// Gets this observer's priority
        /// </summary>
        int Priority
        {
            get;
        }
        
        /// <summary>
        /// Provides the underlying observer with notification data
        /// </summary>
        /// <param name="value">Provides the observer with new data</param>
        /// <returns>The notification result controlling the dispatcher</returns>
        NotificationResult Notify(in T value);
    }
}