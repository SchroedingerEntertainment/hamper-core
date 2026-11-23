// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

namespace Soe.Parsing
{
    /// <summary>
    /// Represents type information about the value of a certain fragment
    /// </summary>
    #if EXPORT_HAMPER_CORE_PARSING
    public
    #else
    internal
    #endif
    enum ValueType : int
    {
        /// <summary>
        /// The type is either true or false
        /// </summary>
        Boolean,
        /// <summary>
        /// The type can be converted into a numeric value
        /// </summary>
        Numeric,
        /// <summary>
        /// The type is a text
        /// </summary>
        String
    }
}