// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;

namespace Soe.Composable
{
    #if EXPORT_HAMPER_CORE_COMPOSABLE
    public
    #else
    internal
    #endif
    abstract partial class SparseMap
    {
        /// <summary>
        /// Represents a container element of the <seealso cref="SparseMap{T}"/>
        /// </summary>
        protected struct SparseElement
        {
            /// <summary>
            /// A pointer to a block in a memory page
            /// </summary>
            public MemoryHandle Handle;
            
            private readonly int slot;
            /// <summary>
            /// The slot index this element was assigned to
            /// </summary>
            public int Slot
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get { return slot; }
            }
            
            /// <summary>
            /// Gets if this element has a memory handle assigned to
            /// </summary>
            public bool IsValid
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get { return Handle != 0; }
            }
            
            /// <summary>
            /// Gets if this element is currently occupied
            /// </summary>
            public bool IsEmpty
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get { return Handle == 0; }
            }

            /// <summary>
            /// Initializes this element with the corresponding slot index and a reserved handle
            /// </summary>
            /// <param name="slot"></param>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public SparseElement(int slot)
            {
                this.Handle = MemoryHandle.Reserved;
                this.slot = slot;
            }
        }
    }
}