// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

namespace Soe.Storage
{
    /// <summary>
    /// Defines normalization settings for a unified resource identifier
    /// </summary>
    [Flags]
    #if EXPORT_HAMPER_CORE_STORAGE
    public
    #else
    internal
    #endif
    enum NormalizationFlags : byte
    {
        /// <summary>
        /// The default flags
        /// </summary>
        Default = 0,
        /// <summary>
        /// Replaces all occurrences of a path separator char with the operating system specific equivalent
        /// </summary>
        SystemSeparatorChar = 0x1,
        /// <summary>
        /// Builds an absolute path from the provided one
        /// </summary>
        AbsolutePath = 0x2
    }
}