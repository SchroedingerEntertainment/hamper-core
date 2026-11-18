// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;

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
    sealed class HookComparer<T> : IComparer<INotificationHook<T>>
    {
        private static readonly HookComparer<T> defaultComparer;
        /// <summary>
        /// Gets the default comparer
        /// </summary>
        public static HookComparer<T> Default
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return defaultComparer; }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static HookComparer()
        {
            defaultComparer = new HookComparer<T>();
        }
        
        /// <inheritdoc/>
        public int Compare(INotificationHook<T>? x, INotificationHook<T>? y)
        {
            if (ReferenceEquals(x, y))
            {
                return 0;
            }
            else if (y is null)
            {
                return 1;
            }
            else if (x is null)
            {
                return -1;
            }
            else return x.Priority.CompareTo(y.Priority);
        }
    }
}