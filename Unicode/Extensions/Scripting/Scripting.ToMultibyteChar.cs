// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Distributed under the Schroedinger Entertainment EULA (See EULA.md for details

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Soe.Unicode
{
    #if EXPORT_HAMPER_CORE_UNICODE
    public
    #else
    internal
    #endif
    static partial class ScriptingExtension
    {
        /// <summary>
        /// Converts this value to a multibyte char
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string ToMultibyteChar(this Scripting.LineBreak value)
        {
            return Char.ConvertFromUtf32((int)value);
        }

        /// <summary>
        /// Converts this value to a multibyte char
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string ToMultibyteChar(this Scripting.WhiteSpace value)
        {
            return Char.ConvertFromUtf32((int)value);
        }

        /// <summary>
        /// Converts this value to a multibyte char
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string ToMultibyteChar(this Scripting.UppercaseLetter value)
        {
            return Char.ConvertFromUtf32((int)value);
        }

        /// <summary>
        /// Converts this value to a multibyte char
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string ToMultibyteChar(this Scripting.LowercaseLetter value)
        {
            return Char.ConvertFromUtf32((int)value);
        }

        /// <summary>
        /// Converts this value to a multibyte char
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string ToMultibyteChar(this Scripting.SectionMarker value)
        {
            return Char.ConvertFromUtf32((int)value);
        }

        /// <summary>
        /// Converts this value to a multibyte char
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string ToMultibyteChar(this Scripting.Punctuation value)
        {
            return Char.ConvertFromUtf32((int)value);
        }

        /// <summary>
        /// Converts this value to a multibyte char
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string ToMultibyteChar(this Scripting.Digit value)
        {
            return Char.ConvertFromUtf32((int)value);
        }
    }
}