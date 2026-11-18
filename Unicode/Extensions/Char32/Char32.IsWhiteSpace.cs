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
        /// Determines if this character is part of Scripting.WhiteSpace
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsWhiteSpace(this Char32 c)
        {
            switch (c)
            {
                case (UInt32)Scripting.WhiteSpace.CharacterTabulation:
                case (UInt32)Scripting.WhiteSpace.Space:
                case (UInt32)Scripting.WhiteSpace.NoBreakSpace:
                case (UInt32)Scripting.WhiteSpace.OghamSpaceMark:
                case (UInt32)Scripting.WhiteSpace.EnQuad:
                case (UInt32)Scripting.WhiteSpace.EmQuad:
                case (UInt32)Scripting.WhiteSpace.EnSpace:
                case (UInt32)Scripting.WhiteSpace.EmSpace:
                case (UInt32)Scripting.WhiteSpace.ThreePerEmSpace:
                case (UInt32)Scripting.WhiteSpace.FourPerEmSpace:
                case (UInt32)Scripting.WhiteSpace.SixPerEmSpace:
                case (UInt32)Scripting.WhiteSpace.FigureSpace:
                case (UInt32)Scripting.WhiteSpace.PunctuationSpace:
                case (UInt32)Scripting.WhiteSpace.ThinSpace:
                case (UInt32)Scripting.WhiteSpace.HairSpace:
                case (UInt32)Scripting.WhiteSpace.NarrowNoBreakSpace:
                case (UInt32)Scripting.WhiteSpace.MediumMathematicalSpace:
                case (UInt32)Scripting.WhiteSpace.IdeographicSpace:
                    return true;

                default:
                    return false;
            }
        }
    }
}
