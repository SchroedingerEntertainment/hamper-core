// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;
using Soe.Utility.Math;

namespace Soe.Crypto
{
    /// <summary>
    /// SECG curve over a 128 bit prime field
    /// </summary>
    #if EXPORT_HAMPER_CORE_CRYPTO
	public
	#else
	internal
	#endif
    sealed class Secp128r1 : Curve<Point128>
    {
        private static readonly Secp128r1 instance;
        /// <summary>
        /// Gets the global instance of this curve
        /// </summary>
        public static Secp128r1 Instance
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return instance; }
        }

        static Secp128r1()
        {
            instance = new Secp128r1();
        }
        private Secp128r1()
            : base(2)
        {
            P[0] = 0xFFFFFFFFFFFFFFFFul;
            P[1] = 0xFFFFFFFDFFFFFFFFul;

            B[0] = 0xD824993C2CEE5ED3ul;
            B[1] = 0xE87579C11079F43Dul;

            g.X[0] = 0x0C28607CA52C5B86ul;
            g.X[1] = 0x161FF7528B899B2Dul;
            g.Y[0] = 0xC02DA292DDED7A83ul;
            g.Y[1] = 0xCF5AC8395BAFEB13ul;

            N[0] = 0x75A30D1B9038A115ul;
            N[1] = 0xFFFFFFFE00000000ul;
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void ModMult(Span<UInt64> a, Span<UInt64> b, Span<UInt64> c)
        {
            Span<UInt64> tmp = stackalloc UInt64[Words * 2];
            
            BigMath.Mult(tmp, b, c, Words);
            ModMultFast(a, tmp);
        }

        /// <summary>
        /// http://www.isys.uni-klu.ac.at/PDF/2001-0126-MT.pdf
        /// </summary>
        void ModMultFast(Span<UInt64> a, Span<UInt64> b)
        {
            Span<UInt64> tmp = stackalloc UInt64[Words];

            BigMath.Assign(a, b, Words);
            tmp[0] = b[2];
            tmp[1] = (b[3] & 0x1FFFFFFFFul) | (b[2] << 33);
            UInt64 carry = BigMath.Add(a, a, tmp, Words);

            tmp[0] = (b[2] >> 31) | (b[3] << 33);
            tmp[1] = (b[3] >> 31) | ((b[2] & 0xFFFFFFFF80000000ul) << 2);
            carry += BigMath.Add(a, a, tmp, Words);

            tmp[0] = (b[2] >> 62) | (b[3] << 2);
            tmp[1] = (b[3] >> 62) | ((b[2] & 0xC000000000000000ul) >> 29) | (b[3] << 35);
            carry += BigMath.Add(a, a, tmp, Words);

            tmp[0] = (b[3] >> 29);
            tmp[1] = ((b[3] & 0xFFFFFFFFE0000000ul) << 4);
            carry += BigMath.Add(a, a, tmp, Words);

            tmp[0] = (b[3] >> 60);
            tmp[1] = (b[3] & 0xFFFFFFFE00000000ul);
            carry += BigMath.Add(a, a, tmp, Words);

            tmp[0] = 0;
            tmp[1] = ((b[3] & 0xF000000000000000ul) >> 27);
            carry += BigMath.Add(a, a, tmp, Words);

            while (carry != 0 || BigMath.Compare(P, a, Words) != 1)
            {
                carry -= BigMath.Sub(a, a, P, Words);
            }
        }
    }
}
