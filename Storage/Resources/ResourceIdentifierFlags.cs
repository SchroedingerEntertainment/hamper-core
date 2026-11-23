// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

namespace Soe.Storage
{
    /// <summary>
    /// Defines the properties on a unified resource identifier
    /// </summary>
    [Flags]
    #if EXPORT_HAMPER_CORE_STORAGE
    public
    #else
    internal
    #endif
    enum IdentifierFlags : byte
    {
        /// <summary>
        /// The default flags
        /// </summary>
        Default = 0,
        /// <summary>
        /// The resource is located on a UNC path (e.g. \\server)
        /// </summary>
        IsUnc = 0x1,
        /// <summary>
        /// The resource is coupled to a URI (e.g. file:/// or https://)
        /// </summary>
        IsUri = 0x2,
        /// <summary>
        /// The resource is relative to a certain root
        /// </summary>
        IsRelative = 0x4
    }
}