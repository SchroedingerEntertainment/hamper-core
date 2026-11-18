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
		private const UInt64 ModInvSwapMask = 0x8000000000000000ul;

		/// <summary>
		/// Assigns sum of b and c modulo mod to a
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void ModAdd(Span<UInt64> a, Span<UInt64> b, Span<UInt64> c, Span<UInt64> mod, int size)
		{
            UInt64 carry = Add(a, b, c, size);
            if (carry != 0 || Compare(a, mod, size) >= 0)
            {
	            Sub(a, a, mod, size);
            }
		}

		/// <summary>
		/// Assigns difference of b and c modulo mod to a
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void ModSub(Span<UInt64> a, Span<UInt64> b, Span<UInt64> c, Span<UInt64> mod, int size)
		{
            UInt64 carry = Sub(a, b, c, size);
            if (carry != 0)
            {
	            Add(a, a, mod, size);
            }
		}

		/// <summary>
		/// Assigns product of b and c modulo mod to a
		/// </summary>
		public static void ModMult(Span<UInt64> a, Span<UInt64> b, Span<UInt64> c, Span<UInt64> mod, int size)
		{
			Span<UInt64> prod = stackalloc UInt64[size * 2];
			Span<UInt64> modProd = stackalloc UInt64[size * 2];
            Mult(prod, b, c, size);

            int prodBits = GetBits(prod, size, size);
            int modBits = GetBits(mod, size);

			if(prodBits != 0) prodBits += size * 64;
            else prodBits = GetBits(prod, size);
			if(prodBits < modBits)
			{
                Assign(a, prod, size);
				return;
			}

            AssignZero(modProd, size);
            AssignZero(modProd, size, size);
			int digitShift = (prodBits - modBits) / 64;
			int bitShift = (prodBits - modBits) % 64;

            if (bitShift != 0)
            {
	            modProd[digitShift + size] = LeftShift(modProd, digitShift, mod, 0, bitShift, size);
            }
            else
            {
	            Assign(modProd, digitShift, mod, 0, size);
            }
            AssignZero(a, size);
			a[0] = 1;

            while (prodBits > size * 64 || Compare(modProd, mod, size) >= 0)
			{
                int equality = Compare(modProd, size, prod, size, size);
                if (equality < 0 || (equality == 0 && Compare(modProd, prod, size) <= 0))
				{
                    if (Sub(prod, prod, modProd, size) != 0)
                    {
	                    Sub(prod, size, prod, size, a, 0, size);
                    }
                    Sub(prod, size, prod, size, modProd, size, size);
				}
				UInt64 carry = (modProd[size] & 1) << 63;
                RightShift(modProd, size, modProd, size, 1, size);
                RightShift(modProd, modProd, 1, size);
				modProd[size - 1] |= carry;
				prodBits--;
			}

            Assign(a, prod, size);
		}

		/// <summary>
        /// Assigns inverse of b modulo mod to a if existing
		/// </summary>
		public static void ModInv(Span<UInt64> a, Span<UInt64> b, Span<UInt64> mod, int size)
		{
			Span<UInt64> tmp = stackalloc UInt64[size];
			Span<UInt64> m = stackalloc UInt64[size];
			Span<UInt64> x = stackalloc UInt64[size];
			Span<UInt64> y = stackalloc UInt64[size];
			
			if(IsZero(b, size))
			{
				AssignZero(a, size);
				return;
			}

			Assign(tmp, b, size);
			Assign(m, mod, size);
			AssignZero(x, size);
			AssignZero(y, size);
			x[0] = 1;
    
			int equality;
			while((equality = Compare(tmp, m, size)) != 0)
			{
				UInt64 carry = 0;
				if(IsEven(tmp))
				{
					RightShift(tmp, tmp, 1, size);
					if(!IsEven(x))
					{
						carry = Add(x, x, mod, size);
					}
					RightShift(x, x, 1, size);

					if(carry != 0)
					{
						x[size - 1] |= ModInvSwapMask;
					}
				}
				else if(IsEven(m))
				{
					RightShift(m, m, 1, size);
					if(!IsEven(y))
					{
						carry = Add(y, y, mod, size);
					}
					RightShift(y, y, 1, size);

					if(carry != 0)
					{
						y[size - 1] |= ModInvSwapMask;
					}
				}
				else if(equality > 0)
				{
                    Sub(tmp, tmp, m, size);
                    RightShift(tmp, tmp, 1, size);

                    if (Compare(x, y, size) < 0)
                    {
	                    Add(x, x, mod, size);
                    }

                    Sub(x, x, y, size);
                    if (!IsEven(x))
                    {
	                    carry = Add(x, x, mod, size);
                    }
                    RightShift(x, x, 1, size);

					if(carry != 0)
					{
						x[size - 1] |= ModInvSwapMask;
					}
				}
				else
				{
                    Sub(m, m, tmp, size);
                    RightShift(m, m, 1, size);

                    if (Compare(y, x, size) < 0)
                    {
	                    Add(y, y, mod, size);
                    }

                    Sub(y, y, x, size);
                    if (!IsEven(y))
                    {
	                    carry = Add(y, y, mod, size);
                    }
                    RightShift(y, y, 1, size);

					if(carry != 0)
					{
						y[size - 1] |= ModInvSwapMask;
					}
				}
			}
            Assign(a, x, size);
		}
    }
}
