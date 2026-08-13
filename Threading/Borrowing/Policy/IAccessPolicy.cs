// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System;

namespace Soe.Threading
{
    /// <summary>
    /// A policy managing how access to the underlying object instance is scheduled
    /// </summary>
    #if EXPORT_HAMPER_CORE_THREADING
    public
    #else
    internal
    #endif
    interface IAccessPolicy
    {
        /// <summary>
        /// Gets a number related to the current access order of this policy
        /// </summary>
        static abstract AccessType Order
        {
            get;
        }

        /// <summary>
        /// Determines if the provided order ID is in conflict with this policy 
        /// </summary>
        /// <param name="order">An order ID to compare</param>
        /// <returns>True if access conflicts with the provided order ID, false otherwise</returns>
        static abstract bool IsConflicting(AccessType order);
    }
}