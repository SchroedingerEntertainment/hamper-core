// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;

namespace Soe.Utility.Math
{
    #if EXPORT_HAMPER_CORE_MATH
    public
    #else
    internal
    #endif
    static partial class BigMath
    {
        /// <summary>
        /// Determines the highest bit set
        /// </summary>
        public static int GetBits(Span<UInt64> a, int size)
        {
            int digits = GetDigits(a, size);
            if (digits == 0)
            {
	            return 0;
            }

            int result = 0;
            UInt64 digit = a[digits - 1];
            for (; digit != 0; result++)
            {
	            digit >>= 1;
            }
            return ((digits - 1) * 64 + result);
        }
		/// <summary>
		/// Determines the highest bit set
		/// </summary>
		public static int GetBits(Span<UInt64> a, int index, int size)
		{
			int digits = GetDigits(a, size);
			if(digits == 0)
			{
				return 0;
			}

			int result = 0;
            UInt64 digit = a[digits - 1 + index];
			for(; digit != 0; result++)
			{
				digit >>= 1;
			}
			return ((digits - 1) * 64 + result);
		}

		/// <summary>
		/// Gets the long bit at index
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool GetBitLong(Span<UInt64> a, int index, int size)
		{
			return ((a[index / 64] & ((UInt64)1 << (index % 64))) != 0);
		}

        /// <summary>
		/// Assigns b to a shifted left n bits
		/// </summary>
		public static UInt64 LeftShift(Span<UInt64> a, Span<UInt64> b, int n, int size)
		{
			UInt64 carry = 0;
			for(int i = 0; i < size; i++)
			{
				UInt64 tmp = b[i];
				a[i] = ((tmp << n) | carry);
				carry = (tmp >> (64 - n));
			}
			return carry;
		}
        /// <summary>
        /// Assigns b to a shifted left n bits
        /// </summary>
        public static UInt64 LeftShift(Span<UInt64> a, int aIndex, Span<UInt64> b, int bIndex, int n, int size)
        {
            UInt64 carry = 0;
            for (int i = 0; i < size; i++)
            {
                UInt64 tmp = b[i + bIndex];
                a[i + aIndex] = ((tmp << n) | carry);
                carry = (tmp >> (64 - n));
            }
            return carry;
        }

        /// <summary>
        /// Assigns b to a shifted right n bits
        /// </summary>
        public static UInt64 RightShift(Span<UInt64> a, Span<UInt64> b, int n, int size)
        {
            UInt64 carry = 0;
            for (int i = (size - 1); i >= 0; i--)
            {
                UInt64 tmp = b[i];
                a[i] = ((tmp >> n) | carry);
                carry = (tmp << (64 - n));
            }
            return carry;
        }
		/// <summary>
		/// Assigns b to a shifted right n bits
		/// </summary>
        public static UInt64 RightShift(Span<UInt64> a, int aIndex, Span<UInt64> b, int bIndex, int n, int size)
		{
			UInt64 carry = 0;
			for(int i = (size - 1); i >= 0; i--)
			{
                UInt64 tmp = b[i + bIndex];
                a[i + aIndex] = ((tmp >> n) | carry);
				carry = (tmp << (64 - n));
			}
			return carry;
		}
    }
}
