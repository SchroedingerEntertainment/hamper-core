// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;

namespace Soe.Reactive
{
    /// <summary>
    /// Provides access to an objects interception point
    /// </summary>
    /// <typeparam name="T">The object that provides notification information to an <see cref="INotificationHook{T}"/></typeparam>
    /// <typeparam name="TAnchor">The type of the interception point</typeparam>
    /// <param name="anchor">The interception point instance to provide access to</param>
    [method: MethodImpl(MethodImplOptions.AggressiveInlining)]
    #if EXPORT_HAMPER_CORE_REACTIVE
    public
    #else
    internal
    #endif
    readonly ref struct AnchorHandle<T, TAnchor>(ref TAnchor anchor)
        where TAnchor : struct, IAnchor<T>
    {
        private readonly ref TAnchor anchor = ref anchor;
        
        /// <summary>
        /// Gets the amount of observers managed by the interception point
        /// </summary>
        public int Count
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return anchor.Count; }
        }

        /// <summary>
        /// Tries to add a new observer to this interception point
        /// </summary>
        /// <param name="observer">An object to receive notifications</param>
        /// <returns>True if the observer was added successfully, false otherwise</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Add(INotificationHook<T> observer)
        {
            return anchor.Add(observer);
        }

        /// <summary>
        /// Removes an existing observer from this interception point
        /// </summary>
        /// <param name="observer">An object to receive notifications</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void Remove(INotificationHook<T> observer)
        {
            anchor.Remove(observer);
        }
    }
}