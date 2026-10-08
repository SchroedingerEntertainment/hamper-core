// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;
using Soe.Utility.Math;

namespace Soe.Crypto
{
    /// <summary>
    /// Elliptic Curve Descriptor in Weierstrass Form
    /// </summary>
    #if EXPORT_HAMPER_CORE_CRYPTO
	public
	#else
	internal
	#endif
    abstract class Curve<CurvePoint>
        where CurvePoint : struct, ICurvePoint<CurvePoint>
    {
        private readonly int words;
        /// <summary>
        /// Gets the amount of 64 bit segments per value in this curve
        /// </summary>
        public int Words
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return words; }
        }
        
        /// <summary>
        /// Gets the size in bytes per value in this curve
        /// </summary>
        public int Bytes
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return (words * 8); }
        }

        private UInt64[] p;
        /// <summary>
        /// This curves modulus
        /// </summary>
        public UInt64[] P
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return p; }
        }
        
        private UInt64[] b;
		/// <summary>
		/// The curve parameter
		/// </summary>
        public UInt64[] B
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return b; }
        }

        protected CurvePoint g;
		/// <summary>
		/// A base point describing the curve
		/// </summary>
        public CurvePoint G
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return g; }
        }

        UInt64[] n;
		/// <summary>
		/// Order of point G. The number of points on the cuvre that could
		/// be generated through multiplication of G
		/// </summary>
        public UInt64[] N
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return n; }
        }

        UInt64[] h;
        /// <summary>
		/// The curves cofactor
		/// </summary>
        public UInt64[] H
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return h; }
        }

        /// <summary>
        /// Declares a new curve descriptor
        /// </summary>
        /// <param name="size">Size in 64 bit segments per value</param>
        public Curve(int size)
        {
            this.words = size;
            this.p = new UInt64[size];
            this.b = new UInt64[size];
            this.g = CurvePoint.Create(size);
            this.n = new UInt64[size];
            this.h = new UInt64[size];
        }

        /// <summary>
        /// Assigns product of b and c modulo P to a
		/// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void ModMult(Span<UInt64> a, Span<UInt64> b, Span<UInt64> c)
        {
            BigMath.ModMult(a, b, c, P, Words);
        }
    }
}
