// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Text;

namespace Soe.Parsing
{
    /// <summary>
    /// A parser fragment
    /// </summary>
    #if EXPORT_HAMPER_CORE_PARSING
    public
    #else
    internal
    #endif
    interface IParserFragment<Source>
        where Source : ITextSource
    {
        /// <summary>
        /// Gets the key of this fragment
        /// </summary>
        TextSource<Source> Key
        {
            get;
        }
        
        /// <summary>
        /// Gets the value of this fragment
        /// </summary>
        TextSource<Source> Value
        {
            get;
        }
        
        /// <summary>
        /// Determines if this fragment's value is of a certain type
        /// </summary>
        /// <param name="type">The value type to check</param>
        /// <returns>True if this fragment's value is of the requested type, false otherwise</returns>
        bool IsOfType(ValueType type);
    }
}