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
    static partial class SynchronizationBarrier
    {
        /// <summary>
        /// A policy that manages begin and end of a shared operation or block
        /// </summary>
        public readonly struct SharedOperation : IRefScopePolicy<UInt32>
        {
            /// <summary>
            /// Signals the beginning of a new shared operation or block
            /// </summary>
            /// <param name="parameter">A fixed 32-bit value to use as synchronization bits</param>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void Acquire(ref UInt32 parameter)
            {
                BeginSharedOperation(ref parameter);
            }

            /// <summary>
            /// Signals the end of a shared operation or block
            /// </summary>
            /// <param name="parameter">A fixed 32-bit value to use as synchronization bits</param>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void Dispose(ref UInt32 parameter)
            {
                EndSharedOperation(ref parameter);
            }
        }
        
        /// <summary>
        /// A policy that manages begin and end of an exclusive operation or block
        /// </summary>
        public readonly struct ExclusiveOperation : IRefScopePolicy<UInt32>
        {
            /// <summary>
            /// Signals the beginning of an exclusive operation or block. Shared operations are synchronized before an exclusive
            /// operation or block begins and any other operations are suspended until this operation or block ends
            /// </summary>
            /// <param name="parameter">A fixed 32-bit value to use as synchronization bits</param>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void Acquire(ref UInt32 parameter)
            {
                BeginExclusiveOperation(ref parameter);
            }

            /// <summary>
            /// Signals the end of an exclusive operation or block
            /// </summary>
            /// <param name="parameter">A fixed 32-bit value to use as synchronization bits</param>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void Dispose(ref UInt32 parameter)
            {
                EndExclusiveOperation(ref parameter);
            }
        }
    }
}