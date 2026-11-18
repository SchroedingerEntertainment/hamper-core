// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;
using Soe.Utility.Math;

namespace Soe.Crypto
{
    /// <summary>
    /// SECG curve over a 256 bit prime field
    /// </summary>
    #if EXPORT_HAMPER_CORE_CRYPTO
	public
	#else
	internal
	#endif
    sealed class Secp256r1 : Curve<Point256>
    {
        private static readonly Secp256r1 instance;
        /// <summary>
        /// Gets the global instance of this curve
        /// </summary>
        public static Secp256r1 Instance
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return instance; }
        }

        static Secp256r1()
        {
            instance = new Secp256r1();
        }
        private Secp256r1()
            : base(4)
        {
            P[0] = 0xFFFFFFFFFFFFFFFFul;
            P[1] = 0x00000000FFFFFFFFul;
            P[2] = 0x0000000000000000ul;
            P[3] = 0xFFFFFFFF00000001ul;

            B[0] = 0x3BCE3C3E27D2604Bul;
            B[1] = 0x651D06B0CC53B0F6ul;
            B[2] = 0xB3EBBD55769886BCul;
            B[3] = 0x5AC635D8AA3A93E7ul;
            
            g.X[0] = 0xF4A13945D898C296ul;
            g.X[1] = 0x77037D812DEB33A0ul;
            g.X[2] = 0xF8BCE6E563A440F2ul;
            g.X[3] = 0x6B17D1F2E12C4247ul;
            g.Y[0] = 0xCBB6406837BF51F5ul;
            g.Y[1] = 0x2BCE33576B315ECEul;
            g.Y[2] = 0x8EE7EB4A7C0F9E16ul;
            g.Y[3] = 0x4FE342E2FE1A7F9Bul;

            N[0] = 0xF3B9CAC2FC632551ul;
            N[1] = 0xBCE6FAADA7179E84ul;
            N[2] = 0xFFFFFFFFFFFFFFFFul;
            N[3] = 0xFFFFFFFF00000000ul;
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
        /// http://www.nsa.gov/ia/_files/nist-routines.pdf
        /// </summary>
        void ModMultFast(Span<UInt64> a, Span<UInt64> b)
        {
            Span<UInt64> tmp = stackalloc UInt64[Words];

            /* t */
            BigMath.Assign(a, b, Words);

            /* s1 */
            tmp[0] = 0;
            tmp[1] = b[5] & 0xffffffff00000000ul;
            tmp[2] = b[6];
            tmp[3] = b[7];
            var carry = (Int64)BigMath.LeftShift(tmp, tmp, 1, Words);
            carry += (Int64)BigMath.Add(a, a, tmp, Words);

            /* s2 */
            tmp[1] = b[6] << 32;
            tmp[2] = (b[6] >> 32) | (b[7] << 32);
            tmp[3] = b[7] >> 32;
            carry += (Int64)BigMath.LeftShift(tmp, tmp, 1, Words);
            carry += (Int64)BigMath.Add(a, a, tmp, Words);

            /* s3 */
            tmp[0] = b[4];
            tmp[1] = b[5] & 0xffffffff;
            tmp[2] = 0;
            tmp[3] = b[7];
            carry += (Int64)BigMath.Add(a, a, tmp, Words);

            /* s4 */
            tmp[0] = (b[4] >> 32) | (b[5] << 32);
            tmp[1] = (b[5] >> 32) | (b[6] & 0xffffffff00000000ul);
            tmp[2] = b[7];
            tmp[3] = (b[6] >> 32) | (b[4] << 32);
            carry += (Int64)BigMath.Add(a, a, tmp, Words);

            /* d1 */
            tmp[0] = (b[5] >> 32) | (b[6] << 32);
            tmp[1] = (b[6] >> 32);
            tmp[2] = 0;
            tmp[3] = (b[4] & 0xffffffff) | (b[5] << 32);
            carry -= (Int64)BigMath.Sub(a, a, tmp, Words);

            /* d2 */
            tmp[0] = b[6];
            tmp[1] = b[7];
            tmp[2] = 0;
            tmp[3] = (b[4] >> 32) | (b[5] & 0xffffffff00000000ul);
            carry -= (Int64)BigMath.Sub(a, a, tmp, Words);

            /* d3 */
            tmp[0] = (b[6] >> 32) | (b[7] << 32);
            tmp[1] = (b[7] >> 32) | (b[4] << 32);
            tmp[2] = (b[4] >> 32) | (b[5] << 32);
            tmp[3] = (b[6] << 32);
            carry -= (Int64)BigMath.Sub(a, a, tmp, Words);

            /* d4 */
            tmp[0] = b[7];
            tmp[1] = b[4] & 0xffffffff00000000ul;
            tmp[2] = b[5];
            tmp[3] = b[6] & 0xffffffff00000000ul;
            carry -= (Int64)BigMath.Sub(a, a, tmp, Words);

            if (carry < 0)
            {
                do
                {
                    carry += (Int64)BigMath.Add(a, a, P, Words);
                }
                while (carry < 0);
            }
            else
            {
                while (carry != 0 || BigMath.Compare(P, a, Words) != 1)
                {
                    carry -= (Int64)BigMath.Sub(a, a, P, Words);
                }
            }
        }
    }
}
