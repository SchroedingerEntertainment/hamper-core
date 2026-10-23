// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

namespace Soe.Threading
{
    /// <summary>
    /// Allows this type to conditionally modify permission processing
    /// </summary>
    #if EXPORT_HAMPER_CORE_THREADING
    public
    #else
    internal
    #endif
    interface IBorrowAnchor
    {
        /// <summary>
        /// Notifies this type instance about a permission request
        /// </summary>
        /// <param name="resolver">The type resolver instance of this request</param>
        /// <param name="uniqueId">The unique object id for this instance</param>
        /// <returns>True if the request needs revaluation, false otherwise</returns>
        bool OnNext(AccessManager.DependencyTreeResolver resolver, UInt32 uniqueId);
    }
}