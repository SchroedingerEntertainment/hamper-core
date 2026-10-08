// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Distributed under the Schroedinger Entertainment EULA (See EULA.md for details

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Soe.Unicode
{
    #if EXPORT_HAMPER_CORE_UNICODE
    public
    #else
    internal
    #endif
    static partial class StringBuilderExtension
    {
        /// <summary>
        /// Appends the string representation of a specified 32-bit Unicode character to this instance
        /// </summary>
        /// <param name="c">The 32-bit Unicode character to append</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AppendChar(this StringBuilder builder, Char32 c)
        {
            builder.Append(c.ToMultibyteChar());
        }
    }
}
