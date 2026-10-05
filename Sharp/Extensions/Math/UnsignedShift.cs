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
    static partial class MathExtension
    {
        // ReSharper disable InvalidXmlDocComment
        
        /// <summary>
        /// Shifts this integer to the left without taking the sign bit into account
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int32 UnsignedLeftShift(this Int32 signed, int places)
        {
            unchecked
            {
                UInt32 unsigned = (UInt32)signed;
                return (Int32)(unsigned << places);
            }
        }
        /// <summary>
        /// Shifts this integer to the left without taking the sign bit into account
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int64 UnsignedLeftShift(this Int64 signed, int places)
        {
            unchecked
            {
                UInt64 unsigned = (UInt64)signed;
                return (Int64)(unsigned << places);
            }
        }

        /// <summary>
        /// Shifts this integer to the right without taking the sign bit into account
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int32 UnsignedRightShift(this Int32 signed, int places)
        {
            unchecked
            {
                UInt32 unsigned = (UInt32)signed;
                return (Int32)(unsigned >> places);
            }
        }
        /// <summary>
        /// Shifts this integer to the right without taking the sign bit into account
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int64 UnsignedRightShift(this Int64 signed, int places)
        {
            unchecked
            {
                UInt64 unsigned = (UInt64)signed;
                return (Int64)(unsigned >> places);
            }
        }
    }
}
