// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Distributed under the Schroedinger Entertainment EULA (See EULA.md for details

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
        /// Determines if this character is part of Scripting.LineBreak
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsLineBreak(this Char32 c)
        {
            switch (c)
            {
                case (UInt32)Scripting.LineBreak.LineFeed:
                case (UInt32)Scripting.LineBreak.LineTabulation:
                case (UInt32)Scripting.LineBreak.FormFeed:
                case (UInt32)Scripting.LineBreak.CarriageReturn:
                case (UInt32)Scripting.LineBreak.NextLine:
                case (UInt32)Scripting.LineBreak.LineSeparator:
                case (UInt32)Scripting.LineBreak.ParagraphSeparator:
                    return true;

                default:
                    return false;
            }
        }
    }
}
