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
    static partial class FastSpinLock
    {
        /// <summary>
        /// A policy that manages begin and end of an exclusive operation or block
        /// </summary>
        public readonly struct LockOperation : IRefScopePolicy<UInt32>
        {
            /// <summary>
            /// Signals the beginning of an exclusive operation or block
            /// </summary>
            /// <param name="parameter">A fixed 32-bit value to use as synchronization bits</param>
            public static void Acquire(ref UInt32 parameter)
            {
                Lock(ref parameter);
            }

            /// <summary>
            /// Signals the end of an exclusive operation or block
            /// </summary>
            /// <param name="parameter">A fixed 32-bit value to use as synchronization bits</param>
            public static void Dispose(ref UInt32 parameter)
            {
                Release(ref parameter);
            }
        }
    }
}