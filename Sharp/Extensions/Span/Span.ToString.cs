// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;

namespace System
{
    #if EXPORT_HAMPER_CORE_SHARP
    public
    #else
    internal
    #endif
    static partial class EndianExtension
    {
        // ReSharper disable InvalidXmlDocComment

        /// <summary>
        /// Returns the string representation of this object
        /// </summary>
        /// <returns>The string representation of this object</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string ToString(this UniteSpan<char> span)
        {
            return string.Concat(span.Left, span.Right);
        }
        /// <summary>
        /// Returns the string representation of this object
        /// </summary>
        /// <returns>The string representation of this object</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string ToString(this SpanSequence<char> span)
        {
            return string.Concat(span.Left, span.Right.Left, span.Right.Right);
        }
        /// <summary>
        /// Returns the string representation of this object
        /// </summary>
        /// <returns>The string representation of this object</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string ToString(this ReadOnlyUniteSpan<char> span)
        {
            return string.Concat(span.Left, span.Right);
        }
        /// <summary>
        /// Returns the string representation of this object
        /// </summary>
        /// <returns>The string representation of this object</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string ToString(this ReadOnlySpanSequence<char> span)
        {
            return string.Concat(span.Left, span.Right.Left, span.Right.Right);
        }
        
        // ReSharper restore InvalidXmlDocComment
    }
}