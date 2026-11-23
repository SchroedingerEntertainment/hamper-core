// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Text;

namespace Soe.Json
{
    #if EXPORT_HAMPER_CORE_JSON
    public
    #else
    internal
    #endif
    static partial class JsonReaderExtension
    {
        /// <summary>
        /// Skips over the nodes contained in current object
        /// </summary>
        /// <returns>True if all tokens were read properly and the object was closed, false otherwise</returns>
        public static bool SkipObject<Source>(this JsonReader<Source> reader)
            where Source : struct, IStreamSource
        {
            int layer = 1;
            for (; layer > 0 && reader.Read();)
            {
                switch (reader.Fragment.Type)
                {
                    case JsonToken.BeginObject: layer++;
                        break;
                    case JsonToken.EndObject: layer--;
                        break;
                }
            }
            return (layer == 0);
        }
    }
}
