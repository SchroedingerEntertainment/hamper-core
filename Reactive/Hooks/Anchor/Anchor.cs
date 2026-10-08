// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;

namespace Soe.Reactive
{
    /// <summary>
    /// Provides an interception point in method execution 
    /// </summary>
    /// <typeparam name="T">The object that provides notification information to an <see cref="INotificationHook{T}"/></typeparam>
    /// <typeparam name="NotificationStrategy">An object managing the dispatch of notification events</typeparam>
    [method: MethodImpl(MethodImplOptions.AggressiveInlining)]
    #if EXPORT_HAMPER_CORE_REACTIVE
    public
    #else
    internal
    #endif
    partial struct Anchor<T, NotificationStrategy>() : IAnchor<T>, IDisposable
        where NotificationStrategy : struct, INotificationStrategy<T>
    {
        private NotificationStrategy notifications = new();

        /// <inheritdoc/>
        public int Count
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return notifications.Count; }
        }
        
        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Dispose()
        {
            notifications.Dispose();
        }
        
        /// <summary>
        /// Provides the underlying observers with notification data
        /// </summary>
        /// <param name="value">Provides the observer with new data</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Notify(in T value)
        {
            notifications.Notify(value);
        }
        
        #region IAnchor Members
        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Add(INotificationHook<T> observer)
        {
            return notifications.Add(observer);
        }
        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Remove(INotificationHook<T> observer)
        {
            notifications.Remove(observer);
        }
        #endregion
    }
}