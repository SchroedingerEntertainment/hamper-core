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
		/// Assigns sum of b and c to a
		/// </summary>
		/// <returns>a and overflow</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ValueTuple<BigNumber, UInt64> Add<BigNumber>(this BigNumber b, BigNumber c)
	        where BigNumber : struct, IBigNumber
        {
	        BigNumber result = default;
	        return new ValueTuple<BigNumber, ulong>(result, BigMath.Add(result.AsSpan(), b.AsSpan(), c.AsSpan(), BigNumber.SegmentCount));
        }

        /// <summary>
        /// Assigns difference of b and c to a
        /// </summary>
        /// <returns>a and overflow</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ValueTuple<BigNumber, UInt64> Sub<BigNumber>(this BigNumber b, BigNumber c)
	        where BigNumber : struct, IBigNumber
        {
	        BigNumber result = default;
	        return new ValueTuple<BigNumber, ulong>(result, BigMath.Sub(result.AsSpan(), b.AsSpan(), c.AsSpan(), BigNumber.SegmentCount));
        }

        /// <summary>
		/// Assigns product of c and d to a and overflow to b
		/// </summary>
		/// <returns>a and b</returns>
        public static ValueTuple<BigNumber, BigNumber> Mult<BigNumber>(this BigNumber c, BigNumber d)
	        where BigNumber : struct, IBigNumber
		{
			Span<UInt64> tmp = stackalloc UInt64[BigNumber.SegmentCount * 2];
			BigNumber result = default;
			BigNumber reminder = default;
			
			BigMath.Mult(tmp, c.AsSpan(), d.AsSpan(), BigNumber.SegmentCount);
			
			tmp.Slice(0, BigNumber.SegmentCount).
				CopyTo(result.AsSpan());
			tmp.Slice(BigNumber.SegmentCount).
				CopyTo(reminder.AsSpan());
			
			return new ValueTuple<BigNumber, BigNumber>(result, reminder);
		}

		/// <summary>
		/// Assigns division of c and d to a and remainder to b
		/// </summary>
		/// <returns>a and b</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ValueTuple<BigNumber, BigNumber> Div<BigNumber>(this BigNumber c, BigNumber d)
			where BigNumber : struct, IBigNumber
		{
			BigNumber result = default;
			BigNumber reminder = default;

			BigMath.Div(result.AsSpan(), reminder.AsSpan(), c.AsSpan(), d.AsSpan(), BigNumber.SegmentCount);
			return new ValueTuple<BigNumber, BigNumber>(result, reminder);
		}
        
        // ReSharper restore InvalidXmlDocComment
    }
}