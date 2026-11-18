// Copyright (C) 2017 Schroedinger Entertainment
// Distributed under the Schroedinger Entertainment EULA (See EULA.md for details)

namespace Soe.Json
{
    /// <summary>
    /// Defines valid JSON tokens
    /// </summary>
    #if EXPORT_HAMPER_CORE_JSON
    public
    #else
    internal
    #endif
    enum JsonToken : byte
    {
        Invalid = 0,
        Whitespace = 1,
        
        Comma = 3,
        Colon = 4,
        
        BeginObject = 7,
        EndObject = 8,

        BeginArray = 12,
        EndArray = 13,
        
        Null = 15,
        True = 16,
        False = 17,
        Numeric = 18,
        String = 19,

        Boolean,
    }
}
