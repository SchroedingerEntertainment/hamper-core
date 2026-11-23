// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Distributed under the Schroedinger Entertainment EULA (See EULA.md for details

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Soe.Unicode
{
    /// <summary>
    /// A Unicode compliant single character value
    /// </summary>
    #if EXPORT_HAMPER_CORE_UNICODE
    public
    #else
    internal
    #endif
    partial struct Char32
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Char32(Scripting.LineBreak value)
        {
            return new Char32((UInt32)value);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Scripting.LineBreak(Char32 c)
        {
            return (Scripting.LineBreak)c.Value;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Char32(Scripting.WhiteSpace value)
        {
            return new Char32((UInt32)value);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Scripting.WhiteSpace(Char32 c)
        {
            return (Scripting.WhiteSpace)c.Value;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Char32(Scripting.UppercaseLetter value)
        {
            return new Char32((UInt32)value);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Scripting.UppercaseLetter(Char32 c)
        {
            return (Scripting.UppercaseLetter)c.Value;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Char32(Scripting.LowercaseLetter value)
        {
            return new Char32((UInt32)value);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Scripting.LowercaseLetter(Char32 c)
        {
            return (Scripting.LowercaseLetter)c.Value;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Char32(Scripting.SectionMarker value)
        {
            return new Char32((UInt32)value);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Scripting.SectionMarker(Char32 c)
        {
            return (Scripting.SectionMarker)c.Value;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Char32(Scripting.Punctuation value)
        {
            return new Char32((UInt32)value);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Scripting.Punctuation(Char32 c)
        {
            return (Scripting.Punctuation)c.Value;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Char32(Scripting.Digit value)
        {
            return new Char32((UInt32)value);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Scripting.Digit(Char32 c)
        {
            return (Scripting.Digit)c.Value;
        }
    }
}