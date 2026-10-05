// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

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
        /// Resolves a potential dependency and tells the task scheduler how to operate on this task
        /// </summary>
        /// <param name="order">The current order bits to probe</param>
        /// <param name="flags">A collection of flags returned to the scheduler by previous operations</param>
        /// <returns>A flag bits to tell the task scheduler how to operate on this task</returns>
        static abstract PolicyResolutionFlags Resolve(AccessType order, PolicyResolutionFlags flags);
    }
}