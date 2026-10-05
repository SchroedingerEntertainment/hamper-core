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
    readonly struct EmptyScope<T> : IScopePolicy<T>
    {
        /// <summary>
        /// Does nothing
        /// </summary>
        /// <param name="parameter">Unused</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Initialize(in T parameter)
        { }

        /// <summary>
        /// Does nothing
        /// </summary>
        /// <param name="parameter">Unused</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Dispose(in T parameter)
        { }
    }
}