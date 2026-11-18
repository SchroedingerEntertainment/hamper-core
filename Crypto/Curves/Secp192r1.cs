// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;
using Soe.Utility.Math;

namespace Soe.Crypto
{
    /// <summary>
    /// SECG curve over a 192 bit prime field
    /// </summary>
    #if EXPORT_HAMPER_CORE_CRYPTO
	public
	#else
	internal
	#endif
    sealed class Secp192r1 : Curve<Point>
    {
	    private static readonly Secp192r1 instance;
	    /// <summary>
	    /// Gets the global instance of this curve
	    /// </summary>
	    public static Secp192r1 Instance
	    {
		    [MethodImpl(MethodImplOptions.AggressiveInlining)]
		    get { return instance; }
	    }

	    static Secp192r1()
	    {
		    instance = new Secp192r1();
	    }
        private Secp192r1()
            : base(3)
        {
            P[0] = 0xFFFFFFFFFFFFFFFFul;
            P[1] = 0xFFFFFFFFFFFFFFFEul;
            P[2] = 0xFFFFFFFFFFFFFFFFul;

            B[0] = 0xFEB8DEECC146B9B1ul;
            B[1] = 0x0FA7E9AB72243049ul;
            B[2] = 0x64210519E59C80E7ul;

            G.X[0] = 0xF4FF0AFD82FF1012ul;
            G.X[1] = 0x7CBF20EB43A18800ul;
            G.X[2] = 0x188DA80EB03090F6ul;

            G.Y[0] = 0x73F977A11E794811ul;
            G.Y[1] = 0x631011ED6B24CDD5ul;
            G.Y[2] = 0x07192B95FFC8DA78ul;

            N[0] = 0x146BC9B1B4D22831ul;
            N[1] = 0xFFFFFFFF99DEF836ul;
            N[2] = 0xFFFFFFFFFFFFFFFFul;
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
			BigMath.Assign(tmp, 0, b, 3, Words);
			var carry = BigMath.Add(a, a, tmp, Words);
    
			tmp[0] = 0;
			tmp[1] = b[3];
			tmp[2] = b[4];
			carry += BigMath.Add(a, a, tmp, Words);
    
			tmp[0] = tmp[1] = b[5];
			tmp[2] = 0;
			carry += BigMath.Add(a, a, tmp, Words);
    
			while(carry != 0 || BigMath.Compare(P, a, Words) != 1)
			{
				carry -= BigMath.Sub(a, a, P, Words);
			}
        }
    }
}
