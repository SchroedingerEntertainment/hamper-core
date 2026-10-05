// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;

namespace Soe.Threading
{
    #if EXPORT_HAMPER_CORE_THREADING
    public
    #else
    internal
    #endif
    partial struct ConcurrentBuffer<T>
    {
        /// <summary>
        /// A policy that manages begin and end of a shared operation or block
        /// </summary>
        public readonly struct SharedOperation : IRefScopePolicy<ConcurrentBuffer<T>>
        {
            /// <summary>
            /// Signals the beginning of a new shared operation or block
            /// </summary>
            /// <param name="parameter">A concurrent container instance</param>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void Acquire(ref ConcurrentBuffer<T> parameter)
            {
                SynchronizationBarrier.BeginSharedOperation(ref parameter.lockVariable);
            }

            /// <summary>
            /// Signals the end of a shared operation or block
            /// </summary>
            /// <param name="parameter">A concurrent container instance</param>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void Dispose(ref ConcurrentBuffer<T> parameter)
            {
                SynchronizationBarrier.EndSharedOperation(ref parameter.lockVariable);
            }
        }
        
        /// <summary>
        /// A policy that manages begin and end of an exclusive operation or block
        /// </summary>
        public readonly struct ExclusiveOperation : IRefScopePolicy<ConcurrentBuffer<T>>
        {
            /// <summary>
            /// Signals the beginning of an exclusive operation or block. Shared operations are synchronized before an exclusive
            /// operation or block begins and any other operations are suspended until this operation or block ends
            /// </summary>
            /// <param name="parameter">A concurrent container instance</param>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void Acquire(ref ConcurrentBuffer<T> parameter)
            {
                SynchronizationBarrier.BeginExclusiveOperation(ref parameter.lockVariable);
            }

            /// <summary>
            /// Signals the end of an exclusive operation or block
            /// </summary>
            /// <param name="parameter">A concurrent container instance</param>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void Dispose(ref ConcurrentBuffer<T> parameter)
            {
                SynchronizationBarrier.EndExclusiveOperation(ref parameter.lockVariable);
            }
        }
    }
}