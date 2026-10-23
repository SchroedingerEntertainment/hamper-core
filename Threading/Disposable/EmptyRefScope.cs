// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;

namespace Soe.Threading
{
    /// <summary>
    /// Represents an empty policy
    /// </summary>
    /// <remarks>Can be used to conditionally distinguish between a real and proxy scope</remarks>
    #if EXPORT_HAMPER_CORE_THREADING
    public
    #else
    internal
    #endif
    readonly struct EmptyRefScope<T> : IRefScopePolicy<T>
    {
        /// <summary>
        /// Does nothing
        /// </summary>
        /// <param name="parameter">Unused</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Acquire(ref T parameter)
        { }

        /// <summary>
        /// Does nothing
        /// </summary>
        /// <param name="parameter">Unused</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Dispose(ref T parameter)
        { }
    }
}