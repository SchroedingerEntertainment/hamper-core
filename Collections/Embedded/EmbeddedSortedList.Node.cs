// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;

namespace Soe.Collections.Embedded
{
    #if EXPORT_HAMPER_CORE_COLLECTIONS_EMBEDDED
    public
    #else
    internal
    #endif
    partial struct EmbeddedSortedList<T>
    {
        /// <summary>
        /// An element for this <see cref="EmbeddedSortedList{T}"/>
        /// </summary>
        public struct ListNode
        {
            private readonly T value;
            /// <summary>
            /// Gets the value of this element
            /// </summary>
            public T Value
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get { return value; }
            }

            private int previous;
            /// <summary>
            /// Gets the predecessor of this element
            /// </summary>
            public int Previous
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get { return previous; }
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                internal set { previous = value; }
            }
            
            private int next;
            /// <summary>
            /// Gets the successor of this element
            /// </summary>
            public int Next
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get { return next; }
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                internal set { next = value; }
            }
            
            private readonly bool isValid;
            /// <summary>
            /// Gets if the element is initialized
            /// </summary>
            public bool IsValid
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get { return isValid; }
            }
            
            /// <summary>
            /// Initializes this element to default
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public ListNode()
            {
                this.value = default!;
                this.next = InvalidIndex;
                this.previous = InvalidIndex;
                this.isValid = false;
            }
            /// <summary>
            /// Initializes this element with the provided value
            /// </summary>
            /// <param name="value">The value of this element</param>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public ListNode(in T value)
            {
                this.value = value;
                this.next = InvalidIndex;
                this.previous = InvalidIndex;
                this.isValid = true;
            }
            
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static implicit operator T(in ListNode listNode)
            {
                return listNode.Value;
            }
        }
    }
}
