// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

namespace Soe.Json
{
    #if EXPORT_HAMPER_CORE_JSON
    public
    #else
    internal
    #endif
    partial class JsonReader<Source>
    {
        private static class ErrorCodes
        {
            public const string InvalidJsonRoot = "({0}, {1}): Invalid JSON root element. Expected BeginObject or BeginArray but found {2}";
            public const string InvalidArrayElement = "({0}, {1}): Invalid token '{2}' in JSON array";
            public const string InvalidObjectProperty = "({0}, {1}): Invalid token '{2}' in JSON object";
            public const string IncompleteProperty = "({0}, {1}): Expected a proeprty value";
            public const string UnexpectedItemSeparator = "({0}, {1}): Unexpected item separator before closing token";
        }
    }
}
