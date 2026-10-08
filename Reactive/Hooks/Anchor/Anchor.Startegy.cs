// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;
using Soe.Collections.Embedded;

namespace Soe.Reactive
{
    #if EXPORT_HAMPER_CORE_REACTIVE
    public
    #else
    internal
    #endif
    partial struct Anchor<T>
    {
        /// <summary>
        /// Performs dispatching on a priority sorted list of observers
        /// </summary>
        /// <typeparam name="ArrayBuffer">The array accessor to use as the internal buffer</typeparam>
        public struct DefaultStrategy<ArrayBuffer> : INotificationStrategy<T>
            where ArrayBuffer : struct, IArrayAccessor<EmbeddedSortedList<INotificationHook<T>>.ListNode>
        {
            private EmbeddedSortedList<INotificationHook<T>, ArrayBuffer> subscribers;

            /// <inheritdoc/>
            public int Count
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get { return subscribers.Count; }
            }
            
            /// <inheritdoc/>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool Add(INotificationHook<T> observer)
            {
                subscribers.Add(observer);
                return true;
            }

            /// <inheritdoc/>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void Dispose()
            {
                subscribers.Dispose();
            }
            
            /// <inheritdoc/>
            public void Notify(in T value)
            {
                if(subscribers.Count > 0)
                {
                    foreach (EmbeddedSortedList<INotificationHook<T>>.ListNode hook in subscribers)
                    {
                        switch (hook.Value.Notify(value))
                        {
                            case NotificationResult.Error: throw new InvalidOperationException();
                            case NotificationResult.Abort: return;
                        }
                    }
                }
            }

            /// <inheritdoc/>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void Remove(INotificationHook<T> observer)
            {
                subscribers.Remove(observer);
            }
        }
    }
}