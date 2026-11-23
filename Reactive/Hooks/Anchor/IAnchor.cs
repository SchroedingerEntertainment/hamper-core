// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

namespace Soe.Reactive
{
    /// <summary>
    /// A management object controlling how an interception point handles <see cref="INotificationHook{T}"/>s
    /// </summary>
    /// <typeparam name="T">The object that provides notification information to this <see cref="INotificationHook{T}"/></typeparam>
    #if EXPORT_HAMPER_CORE_REACTIVE
    public
    #else
    internal
    #endif
    interface IAnchor<T>
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
        bool Add(INotificationHook<T> observer);
        
        /// <summary>
        /// Removes an existing observer from this object
        /// </summary>
        /// <param name="observer">An object to receive notifications</param>
        void Remove(INotificationHook<T> observer);
    }
}