// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;
using Soe.Utility.Math;

namespace Soe.Utility
{
    #if EXPORT_HAMPER_CORE_MATH
    public
    #else
    internal
    #endif
    static partial class BigNumberExtension
    {
        // ReSharper disable InvalidXmlDocComment
        
        /// <summary>
        /// Compares two long values for equality
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int32 Compare<BigNumber>(this BigNumber a, BigNumber b)
            where BigNumber : struct, IBigNumber
        {
            return BigMath.Compare(a.AsSpan(), b.AsSpan(), BigNumber.SegmentCount);
        }
        /// <summary>
        /// Compares two long values for equality
        /// </summary>
        /// <param name="size">The size in segments to compare</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int32 Compare<BigNumber>(this BigNumber a, BigNumber b, int size)
            where BigNumber : struct, IBigNumber
        {
            return BigMath.Compare(a.AsSpan(), b.AsSpan(), size);
        }
        /// <summary>
        /// Compares two long values for equality
        /// </summary>
        /// <param name="size">The size in segments to compare</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int32 Compare<BigNumber>(this BigNumber a, int aIndex, BigNumber b, int bIndex, int size)
            where BigNumber : struct, IBigNumber
        {
            return BigMath.Compare(a.AsSpan(), aIndex, b.AsSpan(), bIndex, size);
        }
        
        /// <summary>
        /// Determines if a is zero
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsZero<BigNumber>(this BigNumber a)
            where BigNumber : struct, IBigNumber
        {
            return BigMath.IsZero(a.AsSpan(), BigNumber.SegmentCount);
        }
        /// <summary>
        /// Determines if a is zero
        /// </summary>
        /// <param name="size">The size in segments to compare</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsZero<BigNumber>(this BigNumber a, int size)
            where BigNumber : struct, IBigNumber
        {
            return BigMath.IsZero(a.AsSpan(), size);
        }
        /// <summary>
        /// Determines if a is zero
        /// </summary>
        /// <param name="size">The size in segments to compare</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsZero<BigNumber>(this BigNumber a, int index, int size)
            where BigNumber : struct, IBigNumber
        {
            return BigMath.IsZero(a.AsSpan(), index, size);
        }

        /// <summary>
        /// Determines if a is even
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsEven<BigNumber>(this BigNumber a)
            where BigNumber : struct, IBigNumber
        {
            return  BigMath.IsEven(a.AsSpan());
        }
        
        // ReSharper restore InvalidXmlDocComment
    }
}