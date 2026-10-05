// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System;
using System.Runtime.CompilerServices;

namespace Soe.Threading
{
    #if EXPORT_HAMPER_CORE_THREADING
    public
    #else
    internal
    #endif
    static partial class ReadWriteBarrier
    {
        /// <summary>
        /// A policy that manages begin and end of a synchronous write operation
        /// </summary>
        public readonly struct WriteOperation : IRefScopePolicy<UInt32>
        {
            /// <summary>
            /// Starts a synchronous write operation and increases the data counter
            /// </summary>
            /// <param name="parameter">A fixed 32-bit value to use as synchronization bits</param>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void Acquire(ref UInt32 parameter)
            {
                BeginWrite(ref parameter);
            }

            /// <summary>
            /// Ends a synchronous write operation and increases the data counter
            /// </summary>
            /// <param name="parameter">A fixed 32-bit value to use as synchronization bits</param>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void Dispose(ref UInt32 parameter)
            {
                EndWrite(ref parameter);
            }
        }
    }
}