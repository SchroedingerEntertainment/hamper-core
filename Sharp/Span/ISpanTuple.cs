// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

namespace System
{
    /// <summary>
    /// Represents an ephemeral tuple with a one or more components
    /// </summary>
    #if EXPORT_HAMPER_CORE_SHARP
    public
    #else
    internal
    #endif
    interface ISpanTuple
    {
        /// <summary>
        /// Gets the number of elements in this tuple
        /// </summary>
        int Count
        {
            get;
        }

        /// <summary>
        /// Gets the total length of the memory managed by elements in this tuple
        /// </summary>
        int Length
        {
            get;
        }
    }
}