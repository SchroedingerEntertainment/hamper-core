// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Soe.Threading
{
    /// <summary>
    /// Provides synchronization to single exclusive operations via spin locking mechanism
    /// </summary>
    #if EXPORT_HAMPER_CORE_THREADING
    public
    #else
    internal
    #endif
    static partial class FastSpinLock
    {
        private const UInt32 LockBit = 0x1;
        
        /// <summary>
        /// Signals the beginning of an exclusive operation or block
        /// </summary>
        /// <param name="lockVariable">A fixed 32-bit value to use as synchronization bits</param>
        /// <param name="lockBit">A bit-mask defining the exclusive operation bit. Default is 0x1</param>
        public static void Lock(ref UInt32 lockVariable, UInt32 lockBit = LockBit)
        {
            SpinWait wait = new SpinWait();
            for (; ; )
            {
                UInt32 oldLock = (Volatile.Read(ref lockVariable) & ~lockBit);
                UInt32 newLock = (oldLock | lockBit);

                if (Interlocked.CompareExchange(ref lockVariable, newLock, oldLock) == oldLock)
                {
                    return;
                }
                else wait.SpinOnce();
            }
        }

        /// <summary>
        /// Tries to signal the beginning of an exclusive operation or block
        /// </summary>
        /// <param name="lockVariable">A fixed 32-bit value to use as synchronization bits</param>
        /// <param name="lockBit">A bit-mask defining the exclusive operation bit. Default is 0x1</param>
        /// <returns>True if the lock was successfully acquired, false otherwise</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryLock(ref UInt32 lockVariable, UInt32 lockBit = LockBit)
        {
            UInt32 oldLock = (Volatile.Read(ref lockVariable) & ~lockBit);
            UInt32 newLock = (oldLock | lockBit);

            return (Interlocked.CompareExchange(ref lockVariable, newLock, oldLock) == oldLock);
        }

        /// <summary>
        /// Signals the end of an exclusive operation or block
        /// </summary>
        /// <param name="lockVariable">A fixed 32-bit value to use as synchronization bits</param>
        /// <param name="lockBit">A bit-mask defining the exclusive operation bit. Default is 0x1</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Release(ref UInt32 lockVariable, UInt32 lockBit = LockBit)
        {
            Interlocked.Exchange(ref lockVariable, lockVariable & ~lockBit);
        }
    }
}