// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

namespace System
{
    /// <summary>
    /// Provides enumerated values to use to control expression pattern matching
    /// </summary>
    #if EXPORT_HAMPER_CORE_SHARP
    public
    #else
    internal
    #endif
    enum MatchingFlags
    {
        Default = 0,
        IgnoreCase,
        Invariant
    }
}
