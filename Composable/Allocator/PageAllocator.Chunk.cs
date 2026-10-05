// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System;
using System.Runtime.CompilerServices;

namespace Soe.Composable
{
    #if EXPORT_HAMPER_CORE_COMPOSABLE
    public
    #else
    internal
    #endif
    partial class PageAllocator
    {
        /// <summary>
        /// Represents a page with a free list
        /// </summary>
        struct Chunk
        {
            public object? Handler;
            public UInt64 FreeList;

            /// <summary>
            /// Determines if this page exists
            /// </summary>
            public bool IsEmpty
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get { return (Volatile.Read(ref Handler) == null); }
            }
        }
    }
}