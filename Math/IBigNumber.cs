// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

namespace Soe.Utility
{
    /// <summary>
    /// Represents a large number of at least 128-bit
    /// </summary>
    #if EXPORT_HAMPER_CORE_MATH
    public
    #else
    internal
    #endif
    interface IBigNumber
    {
        /// <summary>
        /// Gets the amount of data segments this integer is based on
        /// </summary>
        static abstract int SegmentCount
        {
            get;
        }
        
        /// <summary>
        /// Gets the underlying data segments of this integer
        /// </summary>
        /// <returns>A collection of data segments belonging to this integer</returns>
        Span<UInt64> AsSpan();
    }
}