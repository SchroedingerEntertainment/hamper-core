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
    static partial class Char32Extension
    {
        /// <summary>
        /// Determines if this character is part of BasicLatin.Control
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsControl(this Char32 c)
        {
            return (c >= BasicLatin.Control.Null && c <= BasicLatin.Control.InformationSeparatorOne) || (c == BasicLatin.Control.Delete);
        }
    }
}
