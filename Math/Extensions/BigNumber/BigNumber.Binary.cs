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
        /// Assigns b to a shifted left n bits
        /// </summary>
        /// <returns>a and overflow</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ValueTuple<BigNumber, UInt64> LeftShift<BigNumber>(this BigNumber b, int n)
            where BigNumber : struct, IBigNumber
        {
            BigNumber result = default;
            return new ValueTuple<BigNumber, UInt64>(result, BigMath.LeftShift(result.AsSpan(), b.AsSpan(), n, BigNumber.SegmentCount));
        }

        /// <summary>
        /// Assigns b to a shifted right n bits
        /// </summary>
        /// <returns>a and overflow</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ValueTuple<BigNumber, UInt64> RightShift<BigNumber>(this BigNumber b, int n)
            where BigNumber : struct, IBigNumber
        {
            BigNumber result = default;
            return new ValueTuple<BigNumber, UInt64>(result, BigMath.RightShift(result.AsSpan(), b.AsSpan(), n, BigNumber.SegmentCount));
        }
        
        // ReSharper restore InvalidXmlDocComment
    }
}