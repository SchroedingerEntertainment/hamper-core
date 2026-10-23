// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

namespace Soe.Threading
{
    /// <summary>
    /// An enum of order IDs used by <see cref="IAccessPolicy"/>
    /// </summary>
    #if EXPORT_HAMPER_CORE_THREADING
    public
    #else
    internal
    #endif
    enum AccessType : byte
    {
        /// <summary>
        /// The underlying object is fully mutable and data it's can be changed
        /// </summary>
        Mutable = 0,
        /// <summary>
        /// The underlying object is immutable and can be read or changed in non-lethal ways
        /// </summary>
        Immutable = 1,
        
        /// <summary>
        /// An error type, leads to an exception if used
        /// </summary>
        Reserved = 0x80
    }
}