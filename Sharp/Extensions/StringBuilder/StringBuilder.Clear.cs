// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;

namespace System.Text
{
    #if EXPORT_HAMPER_CORE_SHARP
    public
    #else
    internal
    #endif
    static partial class StringBuilderExtension
    {
        // ReSharper disable InvalidXmlDocComment
        
        /// <summary>
        /// Replaces all characters in the current StringBuilder instance with the provided value
        /// </summary>
        /// <param name="value">The string to replace all characters in the current StringBuilder instance</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Clear(this StringBuilder builder, string value)
        {
            builder.Clear();
            builder.Append(value);
        }
    }
}
