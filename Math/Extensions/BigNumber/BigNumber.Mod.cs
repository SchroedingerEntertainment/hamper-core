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
		/// Assigns sum of b and c modulo mod to a
		/// </summary>
		/// <returns>a</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static BigNumber ModAdd<BigNumber>(this BigNumber b, BigNumber c, BigNumber mod)
			where BigNumber : struct, IBigNumber
		{
			BigNumber result = default;
			
			BigMath.ModAdd(result.AsSpan(), b.AsSpan(), c.AsSpan(), mod.AsSpan(), BigNumber.SegmentCount);
			return result;
		}

		/// <summary>
		/// Assigns difference of b and c modulo mod to a
		/// </summary>
		/// <returns>a</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static BigNumber ModSub<BigNumber>(this BigNumber b, BigNumber c, BigNumber mod)
			where BigNumber : struct, IBigNumber
		{
			BigNumber result = default;
			
			BigMath.ModSub(result.AsSpan(), b.AsSpan(), c.AsSpan(), mod.AsSpan(), BigNumber.SegmentCount);
			return result;
		}

		/// <summary>
		/// Assigns product of b and c modulo mod to a
		/// </summary>
		/// <returns>a</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static BigNumber ModMult<BigNumber>(this BigNumber b, BigNumber c, BigNumber mod)
			where BigNumber : struct, IBigNumber
		{
			BigNumber result = default;
			
			BigMath.ModMult(result.AsSpan(), b.AsSpan(), c.AsSpan(), mod.AsSpan(), BigNumber.SegmentCount);
			return result;
		}

		/// <summary>
		/// Assigns inverse of b modulo mod to a if existing
		/// </summary>
		/// <returns>a</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static BigNumber ModInv<BigNumber>(this BigNumber b, BigNumber mod)
			where BigNumber : struct, IBigNumber
		{
			BigNumber result = default;
			
			BigMath.ModInv(result.AsSpan(), b.AsSpan(), mod.AsSpan(), BigNumber.SegmentCount);
			return result;
		}
		
        // ReSharper restore InvalidXmlDocComment
    }
}