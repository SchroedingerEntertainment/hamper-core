// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;

namespace Soe.Threading
{
    /// <summary>
    /// Requests immutable access to an object instance
    /// </summary>
    /// <remarks>This policy allows parallel immutable access</remarks>
    #if EXPORT_HAMPER_CORE_THREADING
    public
    #else
    internal
    #endif
    readonly struct ImmutablePolicy : IAccessPolicy
    {
        /// <inheritdoc/>
        public static AccessType Order
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return AccessType.Immutable; }
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static PolicyResolutionFlags Resolve(AccessType order, PolicyResolutionFlags flags)
        {
            if (order < AccessType.Immutable)
            {
                return (PolicyResolutionFlags.Wait | PolicyResolutionFlags.Barrier);
            }
            else return PolicyResolutionFlags.None;
        }
    }
}