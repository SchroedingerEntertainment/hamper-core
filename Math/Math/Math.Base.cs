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
	    /// Provides preservation of overflowing arithmetic operations
	    /// </summary>
		private struct ArithmeticBuffer
		{
			public UInt64 LowPart;
			public UInt64 HighPart;
		}

		/// <summary>
		/// Determines the number of digits used
		/// </summary>
		public static int GetDigits(Span<UInt64> a, int size)
		{
			int result = (size - 1);
			while(result >= 0 && a[result] == 0)
			{
				result--;
			}
			return (result + 1);
		}
        /// <summary>
        /// Determines the number of digits used
        /// </summary>
        public static int GetDigits(Span<UInt64> a, int index, int size)
        {
            int result = (size - 1);
            while (result >= 0 && a[result + index] == 0)
            {
	            result--;
            }
            return (result + 1);
        }

		/// <summary>
		/// Assigns sum of b and c to a
		/// </summary>
		public static UInt64 Add(Span<UInt64> a, Span<UInt64> b, Span<UInt64> c, int size)
		{
			UInt64 carry = 0;
			for(int i = 0; i < size; i++)
			{
				UInt64 tmp = (b[i] + c[i] + carry);
				if(tmp != b[i])
				{
					carry = ((tmp < b[i]) ? 1u : 0u);
				}
				a[i] = tmp;
			}
			return carry;
		}
        /// <summary>
        /// Assigns sum of b and c to a
        /// </summary>
        public static UInt64 Add(Span<UInt64> a, int aIndex, Span<UInt64> b, int bIndex, Span<UInt64> c, int cIndex, int size)
        {
            UInt64 carry = 0;
            for (int i = 0; i < size; i++)
            {
                UInt64 tmp = (b[i + bIndex] + c[i + cIndex] + carry);
                if (tmp != b[i + bIndex])
                {
	                carry = ((tmp < b[i + bIndex]) ? 1u : 0u);
                }
                a[i + aIndex] = tmp;
            }
            return carry;
        }

        /// <summary>
        /// Assigns difference of b and c to a
        /// </summary>
        public static UInt64 Sub(Span<UInt64> a, Span<UInt64> b, Span<UInt64> c, int size)
        {
            UInt64 carry = 0;
            for (int i = 0; i < size; i++)
            {
                UInt64 tmp = b[i] - c[i] - carry;
                if (tmp != b[i])
                {
	                carry = ((tmp > b[i]) ? 1u : 0u);
                }
                a[i] = tmp;
            }
            return carry;
        }
		/// <summary>
		/// Assigns difference of b and c to a
		/// </summary>
        public static UInt64 Sub(Span<UInt64> a, int aIndex, Span<UInt64> b, int bIndex, Span<UInt64> c, int cIndex, int size)
		{
			UInt64 carry = 0;
			for(int i = 0; i < size; i++)
			{
                UInt64 tmp = b[i + bIndex] - c[i + cIndex] - carry;
                if (tmp != b[i + bIndex])
                {
	                carry = ((tmp > b[i + bIndex]) ? 1u : 0u);
                }
                a[i + aIndex] = tmp;
			}
			return carry;
		}

        /// <summary>
		/// Assigns product of b and c to a. 
		/// </summary>
		/// <remarks>a must be of double word size</remarks>
		public static void Mult(Span<UInt64> a, Span<UInt64> b, Span<UInt64> c, int size)
		{
			ArithmeticBuffer result = new ArithmeticBuffer();
			for(int i = 0; i < ((size * 2) - 1); i++)
			{
				UInt64 carry = 0;
				for(int j = (i < size ? 0 : (i + 1) - size); j <= i && j < size; j++)
				{
					ArithmeticBuffer tmp = BitwiseMult(b[j], c[i - j]);
					result = BitwiseAdd(ref result, ref tmp);
					carry += ((result.HighPart < tmp.HighPart) ? 1u : 0u);
				}
				a[i] = result.LowPart;
				result.LowPart = result.HighPart;
				result.HighPart = carry;
			}
			a[((size * 2) - 1)] = result.LowPart;
		}

		/// <summary>
		/// Assigns division of c and d to a and remainder to b
		/// </summary>
		public static void Div(Span<UInt64> a, Span<UInt64> b, Span<UInt64> c, Span<UInt64> d, int size)
		{
			Span<UInt64> one = stackalloc UInt64[size];
            one[0] = 1;

			int divBits = GetBits(c, size);

            AssignZero(a, size);
            AssignZero(b, size);
			for(; divBits > 0; divBits--)
			{
                LeftShift(a, a, 1, size);
                LeftShift(b, b, 1, size);

                if (GetBitLong(c, divBits - 1, size))
                    Add(b, b, one, size);

                if (Compare(b, d, size) >= 0)
				{
                    Sub(b, b, d, size);
                    Add(a, a, one, size);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		static ArithmeticBuffer BitwiseAdd(ref ArithmeticBuffer a, ref ArithmeticBuffer b)
		{
			UInt64 lowPart = (a.LowPart + b.LowPart);
			return new ArithmeticBuffer()
			{
				LowPart = lowPart,
				HighPart = (a.HighPart + b.HighPart + ((lowPart < a.LowPart) ? 1u : 0u))
			};
		}
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		static ArithmeticBuffer BitwiseMult(UInt64 a, UInt64 b)
		{
			UInt64 a0 = (a & UInt32.MaxValue);
			UInt64 a1 = (a >> 32);
			UInt64 b0 = (b & UInt32.MaxValue);
			UInt64 b1 = (b >> 32);
			UInt64 t0 = (a0 * b0);
			UInt64 t1 = (a0 * b1);
			UInt64 t2 = (a1 * b0) + (t0 >> 32) + t1;
			UInt64 t3 = (a1 * b1) + ((t2 < t1) ? 0x100000000ul : 0u);

			return new ArithmeticBuffer()
			{
				LowPart = ((t0 & UInt32.MaxValue) | (t2 << 32)),
				HighPart = (t3 + (t2 >> 32))
			};
		}
	}
}
