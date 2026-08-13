// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;

namespace Soe.Threading
{
    /// <summary>
    /// Requests fully mutable access to an object instance
    /// </summary>
    /// <remarks>This policy allows exclusive access only</remarks>
    #if EXPORT_HAMPER_CORE_THREADING
    public
    #else
    internal
    #endif
    readonly struct MutablePolicy : IAccessPolicy
    {
        /// <inheritdoc/>
        public static AccessType Order
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return AccessType.Mutable; }
        }
        
        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsConflicting(AccessType order)
        {
            return true;
        }
    }
}