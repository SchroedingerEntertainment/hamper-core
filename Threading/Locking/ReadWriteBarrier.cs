// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Soe.Threading
{
    /// <summary>
    /// Provides synchronization for data that is frequently read, rarely written
    /// </summary>
    /// <remarks>The barrier allows readers to proceed completely lock-free by using an optimistic retry loop</remarks>
    #if EXPORT_HAMPER_CORE_THREADING
    public
    #else
    internal
    #endif
    static partial class ReadWriteBarrier
    {
        /// <summary>
        /// Starts a lock-free read operation
        /// </summary>
        /// <param name="lockVariable">A fixed 32-bit value to use as synchronization bits</param>
        /// <returns>The current version of the data counter</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt32 BeginRead(ref UInt32 lockVariable)
        {
            return Volatile.Read(ref lockVariable);
        }

        /// <summary>
        /// Ends a lock-free read operation and verifies the data counter
        /// </summary>
        /// <param name="lockVariable">A fixed 32-bit value to use as synchronization bits</param>
        /// <param name="version">The current version of the data counter</param>
        /// <returns>True if the data counter equals version, false otherwise</returns>
        /// <remarks>A reader checks the data counter before and after reading. If the number is odd or if the
        /// number changed during the read, the reader must retry the entire operation</remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool EndRead(ref UInt32 lockVariable, UInt32 version)
        {
            UInt32 oldLock = Volatile.Read(ref lockVariable);
            return ((oldLock & 1) == 0 && oldLock == version);
        }
        
        /// <summary>
        /// Starts a synchronous write operation and increases the data counter
        /// </summary>
        /// <param name="lockVariable">A fixed 32-bit value to use as synchronization bits</param>
        public static void BeginWrite(ref UInt32 lockVariable)
        {
            SpinWait wait = new SpinWait();
            for (; ; )
            {
                UInt32 oldLok = Volatile.Read(ref lockVariable);
                if ((oldLok & 1) == 0 && Interlocked.CompareExchange(ref lockVariable, oldLok + 1, oldLok) == oldLok)
                {
                    return;
                }
                else wait.SpinOnce();
            }
        }

        /// <summary>
        /// Ends a synchronous write operation and increases the data counter
        /// </summary>
        /// <param name="lockVariable">A fixed 32-bit value to use as synchronization bits</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void EndWrite(ref UInt32 lockVariable)
        {
            Interlocked.Increment(ref lockVariable);
        }
    }
}