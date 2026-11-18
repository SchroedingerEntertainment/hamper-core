// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

namespace System
{
    #if EXPORT_HAMPER_CORE_SHARP
    public
    #else
    internal
    #endif
    static partial class HashUtility
    {
        /// <summary>
        /// FNVb Prime Number
        /// </summary>
        public const UInt32 FnvPrime = 16777619u;
        /// <summary>
        /// FNVb Initial Value
        /// </summary>
        public const UInt32 FnvOffsetBias = 2166136261u;
    }
}
