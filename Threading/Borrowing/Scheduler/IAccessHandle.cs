// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

namespace Soe.Threading
{
    /// <summary>
    /// An abstract access handle managed by the corresponding <see cref="AccessManager"/>
    /// </summary>
    #if EXPORT_HAMPER_CORE_THREADING
    public
    #else
    internal
    #endif
    interface IAccessHandle
    {
        /// <summary>
        /// Gets the desired access type for the given object instance
        /// </summary>
        /// <param name="uniqueId">The unique object id for the requested instance</param>
        /// <returns>The desired access type for the given object instance if present, Empty otherwise</returns>
        public AccessType GetAccess(UInt32 uniqueId);
    }
}