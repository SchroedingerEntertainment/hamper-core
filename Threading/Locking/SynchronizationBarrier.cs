// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Soe
{
    /// <summary>
    /// Provides synchronization between a single exclusive operation and shared operations via fast locking mechanisms
    /// </summary>
    #if EXPORT_HAMPER_CORE_THREADING
    public
    #else
    internal
    #endif
    static class SynchronizationBarrier
    {
        private const UInt32 SharedBits = 0x7FFFFFFF;
        private const UInt32 ExclusiveBit = 0x80000000;
        
        /// <summary>
        /// Signals the beginning of a new shared operation or block
        /// </summary>
        /// <param name="lockVariable">A fixed 32-bit value to use as synchronization bits</param>
        /// <param name="sharedBits">A bit-mask defining the shared operation bits. Default is 0x7FFFFFFF</param>
        /// <param name="exclusiveBit">A bit-mask defining the exclusive operation bit. Default is 0x80000000</param>
        public static void BeginSharedOperation(ref UInt32 lockVariable, UInt32 sharedBits = SharedBits, UInt32 exclusiveBit = ExclusiveBit)
        {
            SpinWait wait = new SpinWait();
            for (; ; )
            {
                while ((Volatile.Read(ref lockVariable) & exclusiveBit) != 0)
                {
                    wait.SpinOnce();
                }

                UInt32 oldLock = (Volatile.Read(ref lockVariable) & sharedBits);
                UInt32 newLock = oldLock + 1;

                if (Interlocked.CompareExchange(ref lockVariable, newLock, oldLock) == oldLock)
                {
                    return;
                }
            }
        }

        /// <summary>
        /// Signals the end of a shared operation or block
        /// </summary>
        /// <param name="lockVariable">A fixed 32-bit value to use as synchronization bits</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void EndSharedOperation(ref UInt32 lockVariable)
        {
            Interlocked.Decrement(ref lockVariable);
        }
        
        /// <summary>
        /// Signals the beginning of an exclusive operation or block. Shared operations are synchronized before an exclusive
        /// operation or block begins and any other operations are suspended until this operation or block ends
        /// </summary>
        /// <param name="lockVariable">A fixed 32-bit value to use as synchronization bits</param>
        /// <param name="sharedBits">A bit-mask defining the shared operation bits. Default is 0x7FFFFFFF</param>
        /// <param name="exclusiveBit">A bit-mask defining the exclusive operation bit. Default is 0x80000000</param>
        public static void BeginExclusiveOperation(ref UInt32 lockVariable, UInt32 sharedBits = SharedBits, UInt32 exclusiveBit = ExclusiveBit)
        {
            SpinWait wait = new SpinWait();
            for (; ; )
            {
                while ((Volatile.Read(ref lockVariable) & exclusiveBit) != 0)
                {
                    wait.SpinOnce();
                }

                UInt32 oldLock = (Volatile.Read(ref lockVariable) & sharedBits);
                UInt32 newLock = (oldLock | exclusiveBit);

                if (Interlocked.CompareExchange(ref lockVariable, newLock, oldLock) == oldLock)
                {
                    while ((lockVariable & sharedBits) != 0)
                    {
                        wait.SpinOnce();
                    }
                    return;
                }
            }
        }
        
        /// <summary>
        /// Signals the end of an exclusive operation or block
        /// </summary>
        /// <param name="lockVariable">A fixed 32-bit value to use as synchronization bits</param>
        /// <param name="sharedBits">A bit-mask defining the shared operation bits. Default is 0x7FFFFFFF</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void EndExclusiveOperation(ref UInt32 lockVariable, UInt32 sharedBits = SharedBits)
        {
            Interlocked.Exchange(ref lockVariable, lockVariable & sharedBits);
        }
    }
}
