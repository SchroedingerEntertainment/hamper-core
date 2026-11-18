// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;
using Soe.Utility.Math;

namespace Soe.Crypto
{
    /// <summary>
    /// Elliptic Curve Cryptography Math Utilities
    /// </summary>
    #if EXPORT_HAMPER_CORE_CRYPTO
	public
	#else
	internal
	#endif
    static class EccMath
    {
		/// <summary>
		/// Determines if a is zero
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsZero<CurvePoint>(in CurvePoint a)
			where CurvePoint : struct, ICurvePoint<CurvePoint>
		{
			return (BigMath.IsZero(a.X, a.SegmentCount) && 
				    BigMath.IsZero(a.Y, a.SegmentCount));
		}

		/// <summary>
		/// Assigns product of b and c modulo P to a
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void ModMult<CurvePoint>(Curve<CurvePoint> curve, Span<UInt64> a, Span<UInt64> b, Span<UInt64> c)
			where CurvePoint : struct, ICurvePoint<CurvePoint>
		{
			curve.ModMult(a, b, c);
		}
		/// <summary>
		/// Assigns sqrt of b modulo P to a
		/// </summary>
		public static void ModSqrt<CurvePoint>(Curve<CurvePoint> curve, Span<UInt64> a, Span<UInt64> b)
			where CurvePoint : struct, ICurvePoint<CurvePoint>
		{
			Span<UInt64> tmp = stackalloc UInt64[curve.Words];
            tmp[0] = 1;
            Span<UInt64> result = stackalloc UInt64[curve.Words];
            result[0] = 1;

			BigMath.Add(tmp, curve.P, tmp, curve.Words);
			for(int i = (BigMath.GetBits(tmp, curve.Words) - 1); i > 1; i--)
			{
				ModMult(curve, result, result, result);
				if(BigMath.GetBitLong(tmp, i, curve.Words))
				{
					ModMult(curve, result, result, b);
				}
			}
			BigMath.Assign(a, result, curve.Words);
		}

		/// <summary>
		/// Encodes a into its binary representation
		/// </summary>
		public static void Encode(Span<byte> bytes, Span<UInt64> a, int words)
		{
			for(int i = 0; i < words; i++)
			{
				int index = (8 * (words - 1 - i));
				bytes[0 + index] = (byte)(a[i] >> 56);
				bytes[1 + index] = (byte)(a[i] >> 48);
				bytes[2 + index] = (byte)(a[i] >> 40);
				bytes[3 + index] = (byte)(a[i] >> 32);
				bytes[4 + index] = (byte)(a[i] >> 24);
				bytes[5 + index] = (byte)(a[i] >> 16);
				bytes[6 + index] = (byte)(a[i] >> 8);
				bytes[7 + index] = (byte)(a[i]);
			}
		}
        /// <summary>
		/// Encodes a into its binary representation
		/// </summary>
        public static void Encode(Span<byte> bytes, int bIndex, Span<UInt64> a, int aIndex, int words)
        {
            for (int i = 0; i < words; i++)
            {
                int index = (8 * (words - 1 - i)) + bIndex;
                bytes[0 + index] = (byte)(a[i + aIndex] >> 56);
                bytes[1 + index] = (byte)(a[i + aIndex] >> 48);
                bytes[2 + index] = (byte)(a[i + aIndex] >> 40);
                bytes[3 + index] = (byte)(a[i + aIndex] >> 32);
                bytes[4 + index] = (byte)(a[i + aIndex] >> 24);
                bytes[5 + index] = (byte)(a[i + aIndex] >> 16);
                bytes[6 + index] = (byte)(a[i + aIndex] >> 8);
                bytes[7 + index] = (byte)(a[i + aIndex]);
            }
        }
        /// <summary>
        /// Decodes a from its binary representation
        /// </summary>
        public static void Decode(Span<UInt64> a, Span<byte> bytes, int words)
        {
            for (int i = 0; i < words; i++)
            {
                int index = (8 * (words - 1 - i));
                a[i] = ((UInt64)bytes[0 + index] << 56) | ((UInt64)bytes[1 + index] << 48) | ((UInt64)bytes[2 + index] << 40) | ((UInt64)bytes[3 + index] << 32) |
                       ((UInt64)bytes[4 + index] << 24) | ((UInt64)bytes[5 + index] << 16) | ((UInt64)bytes[6 + index] << 8) | bytes[7 + index];
            }
        }
		/// <summary>
		/// Decodes a from its binary representation
		/// </summary>
		public static void Decode(Span<UInt64> a, int aIndex, Span<byte> bytes, int bIndex, int words)
		{
			for(int i = 0; i < words; i++)
			{
                int index = (8 * (words - 1 - i)) + bIndex;
                a[i + aIndex] = ((UInt64)bytes[0 + index] << 56) | ((UInt64)bytes[1 + index] << 48) | ((UInt64)bytes[2 + index] << 40) | ((UInt64)bytes[3 + index] << 32) |
					            ((UInt64)bytes[4 + index] << 24) | ((UInt64)bytes[5 + index] << 16) | ((UInt64)bytes[6 + index] << 8)  |  bytes[7 + index];
			}
		}

		/// <summary>
		/// Converts a point into jacobian coords 
		/// (X, Y) = (X * Z^2, Y * Z^3)
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Add<CurvePoint>(Curve<CurvePoint> curve, Span<UInt64> x, Span<UInt64> y, Span<UInt64> z)
			where CurvePoint : struct, ICurvePoint<CurvePoint>
		{
			UInt64[] tmp = new UInt64[curve.Words];
			ModMult(curve, tmp, z, z);
			ModMult(curve, x, x, tmp);
			ModMult(curve, tmp, tmp, z);
			ModMult(curve, y, y, tmp);
		}
		/// <summary>
		/// Adds a <typeparamref name="CurvePoint"/> to another <typeparamref name="CurvePoint"/> sharing the same z coord
		/// </summary>
		public static void Add<CurvePoint>(Curve<CurvePoint> curve, ref CurvePoint a, in CurvePoint b, in CurvePoint c)
			where CurvePoint : struct, ICurvePoint<CurvePoint>
		{
			Span<UInt64> tX1 = stackalloc UInt64[curve.Words];
			Span<UInt64> tY1 = stackalloc UInt64[curve.Words];
			Span<UInt64> tX2 = stackalloc UInt64[curve.Words];
			Span<UInt64> tY2 = stackalloc UInt64[curve.Words];

			BigMath.Assign(tX1, b.X, curve.Words);
			BigMath.Assign(tY1, b.Y, curve.Words);
			BigMath.Assign(tX2, c.X, curve.Words);
			BigMath.Assign(tY2, c.Y, curve.Words);
			Add(curve, tX1, tY1, tX2, tY2);

			Span<UInt64> z = stackalloc UInt64[curve.Words];
			
			ModMult(curve, z, b.X, tY1);
			BigMath.ModInv(z, z, curve.P, curve.Words);
			ModMult(curve, z, z, tX1);
			ModMult(curve, z, z, b.Y);
			Add(curve, tX2, tY2, z);

			BigMath.Assign(a.X, tX2, curve.Words);
			BigMath.Assign(a.Y, tY2, curve.Words);
		}
		/// <summary>
		/// Adds a point to another point via jacobian transform
        /// (X2, Y2) = (X1, Y1) + (X2, Y2)
		/// (X1, Y1) = (X1', Y1')
		/// </summary>
		public static void Add<CurvePoint>(Curve<CurvePoint> curve, Span<UInt64> x1, Span<UInt64> y1, Span<UInt64> x2, Span<UInt64> y2)
			where CurvePoint : struct, ICurvePoint<CurvePoint>
		{
			Span<UInt64> tmp = stackalloc UInt64[curve.Words];
			
			BigMath.ModSub(tmp, x2, x1, curve.P, curve.Words);
			ModMult(curve, tmp, tmp, tmp);
			ModMult(curve, x1, x1, tmp);
			ModMult(curve, x2, x2, tmp);
			BigMath.ModSub(y2, y2, y1, curve.P, curve.Words);
			ModMult(curve, tmp, y2, y2);
    
			BigMath.ModSub(tmp, tmp, x1, curve.P, curve.Words);
			BigMath.ModSub(tmp, tmp, x2, curve.P, curve.Words);
			BigMath.ModSub(x2, x2, x1, curve.P, curve.Words);
			ModMult(curve, y1, y1, x2);
			BigMath.ModSub(x2, x1, tmp, curve.P, curve.Words);
			ModMult(curve, y2, y2, x2);
			BigMath.ModSub(y2, y2, y1, curve.P, curve.Words);
			BigMath.Assign(x2, tmp, curve.Words);
		}
		/// <summary>
		/// Adds a point to another point via jacobian transform over c
        /// (X1, Y1) = (X1, Y1) - (X2, Y2)
		/// (X2, Y2) = (X1, Y1) + (X2, Y2)
		/// </summary>
		public static void Add2<CurvePoint>(Curve<CurvePoint> curve, Span<UInt64> x1, Span<UInt64> y1, Span<UInt64> x2, Span<UInt64> y2)
			where CurvePoint : struct, ICurvePoint<CurvePoint>
		{
			Span<UInt64> p1 = stackalloc UInt64[curve.Words];
			Span<UInt64> p2 = stackalloc UInt64[curve.Words];
			Span<UInt64> p3 = stackalloc UInt64[curve.Words];
			
			BigMath.ModSub(p1, x2, x1, curve.P, curve.Words);
			ModMult(curve, p1, p1, p1);
			ModMult(curve, x1, x1, p1);
			ModMult(curve, x2, x2, p1);
			BigMath.ModAdd(p1, y2, y1, curve.P, curve.Words);
			BigMath.ModSub(y2, y2, y1, curve.P, curve.Words);

			BigMath.ModSub(p2, x2, x1, curve.P, curve.Words);
			ModMult(curve, y1, y1, p2);
			BigMath.ModAdd(p2, x1, x2, curve.P, curve.Words);
			ModMult(curve, x2, y2, y2);
			BigMath.ModSub(x2, x2, p2, curve.P, curve.Words);
    
			BigMath.ModSub(p3, x1, x2, curve.P, curve.Words);
			ModMult(curve, y2, y2, p3);
			BigMath.ModSub(y2, y2, y1, curve.P, curve.Words);
    
			ModMult(curve, p3, p1, p1);
			BigMath.ModSub(p3, p3, p2, curve.P, curve.Words);
			BigMath.ModSub(p2, p3, x1, curve.P, curve.Words);
			ModMult(curve, p2, p2, p1);
			BigMath.ModSub(y1, p2, y1, curve.P, curve.Words);
			BigMath.Assign(x1, p3, curve.Words);
		}

		/// <summary>
		/// Adds a point to itself inplace
		/// </summary>
		public static void Double<CurvePoint>(Curve<CurvePoint> curve, Span<UInt64> x, Span<UInt64> y, Span<UInt64> z)
			where CurvePoint : struct, ICurvePoint<CurvePoint>
		{
			Span<UInt64> p1 = stackalloc UInt64[curve.Words];
			Span<UInt64> p2 = stackalloc UInt64[curve.Words];
    
			if(BigMath.IsZero(z, curve.Words))
			{
				return;
			}
    
			ModMult(curve, p1, y, y);
			ModMult(curve, p2, x, p1);
			ModMult(curve, p1, p1, p1);
			ModMult(curve, y, y, z);
			ModMult(curve, z, z, z);
			BigMath.ModAdd(x, x, z, curve.P, curve.Words);
			BigMath.ModAdd(z, z, z, curve.P, curve.Words);
			BigMath.ModSub(z, x, z, curve.P, curve.Words);
			ModMult(curve, x, x, z);
			BigMath.ModAdd(z, x, x, curve.P, curve.Words);
			BigMath.ModAdd(x, x, z, curve.P, curve.Words);

			if(BigMath.GetBitLong(x, 0, curve.Words))
			{
				UInt64 carry = BigMath.Add(x, x, curve.P, curve.Words);
				BigMath.RightShift(x, x, 1, curve.Words);
				x[curve.Words - 1] |= (carry << 63);
			}
			else BigMath.RightShift(x, x, 1, curve.Words);

			ModMult(curve, z, x, x);
			BigMath.ModSub(z, z, p2, curve.P, curve.Words);
			BigMath.ModSub(z, z, p2, curve.P, curve.Words);
			BigMath.ModSub(p2, p2, z, curve.P, curve.Words);
			ModMult(curve, x, x, p2);
			BigMath.ModSub(p1, x, p1, curve.P, curve.Words);
			BigMath.Assign(x, z, curve.Words);
			BigMath.Assign(z, y, curve.Words);
			BigMath.Assign(y, p1, curve.Words);
		}
		/// <summary>
		/// Assigns a point to another point twice
		/// </summary>
		public static void Double<CurvePoint>(Curve<CurvePoint> curve, Span<UInt64> x1, Span<UInt64> y1, Span<UInt64> x2, Span<UInt64> y2)
			where CurvePoint : struct, ICurvePoint<CurvePoint>
		{
			UInt64[] z = new UInt64[curve.Words];
			BigMath.Assign(x2, x1, curve.Words);
			BigMath.Assign(y2, y1, curve.Words);
    
			BigMath.AssignZero(z, curve.Words);
			z[0] = 1;

			Add(curve, x1, y1, z);
			Double(curve, x1, y1, z);
			Add(curve, x2, y2, z);
		}

		/// <summary>
		/// Assigns product of b and scalar to a
		/// </summary>
		public static void Mult<CurvePoint>(Curve<CurvePoint> curve, ref CurvePoint a, in CurvePoint b, Span<UInt64> scalar)
			where CurvePoint : struct, ICurvePoint<CurvePoint>
		{
			UInt64[][] rX =
			[
				new UInt64[curve.Words],
                new UInt64[curve.Words]
			];
			UInt64[][] rY =
			[
				new UInt64[curve.Words],
                new UInt64[curve.Words]
			];
			Span<UInt64>z = stackalloc UInt64[curve.Words];

			BigMath.Assign(rX[1], b.X, curve.Words);
			BigMath.Assign(rY[1], b.Y, curve.Words);
			Double(curve, rX[1], rY[1], rX[0], rY[0]);

			int nb;
			for(int i = (BigMath.GetBits(scalar, curve.Words) - 2); i > 0; i--)
			{
				nb = ((!BigMath.GetBitLong(scalar, i, curve.Words)) ? 1 : 0);
				Add2(curve, rX[1 - nb], rY[1 - nb], rX[nb], rY[nb]);
				Add(curve, rX[nb], rY[nb], rX[1 - nb], rY[1 - nb]);
			}
			nb = ((!BigMath.GetBitLong(scalar, 0, curve.Words)) ? 1 : 0);
			Add2(curve, rX[1 - nb], rY[1 - nb], rX[nb], rY[nb]);
    
			BigMath.ModSub(z, rX[1], rX[0], curve.P, curve.Words);
			ModMult(curve, z, z, rY[1 - nb]);
			ModMult(curve, z, z, b.X);
			BigMath.ModInv(z, z, curve.P, curve.Words);
			ModMult(curve, z, z, b.Y);
			ModMult(curve, z, z, rX[1 - nb]);
			Add(curve, rX[nb], rY[nb], rX[1 - nb], rY[1 - nb]);
			Add(curve, rX[0], rY[0], z);
			BigMath.Assign(a.X, rX[0], curve.Words);
			BigMath.Assign(a.Y, rY[0], curve.Words);
		}

		/// <summary>
		/// Compresses a CurvePoint into half its size + 1 in bytes
		/// </summary>
		public static void Compress<CurvePoint>(in CurvePoint a, Span<byte> compressed)
			where CurvePoint : struct, ICurvePoint<CurvePoint>
		{
			Encode(compressed, 1, a.X, 0, a.SegmentCount);
		 	compressed[0] = (byte)(2 + (a.Y[0] & 0x01));
		}
		/// <summary>
        /// Decompresses a range of bytes into a CurvePoint
		/// </summary>
		public static bool Decompress<CurvePoint>(Curve<CurvePoint> curve, ref CurvePoint a, Span<byte> compressed)
			where CurvePoint : struct, ICurvePoint<CurvePoint>
		{
			if (compressed[0] == 0x04)
			{
				return false;
			}
            Decode(a.X, 0, compressed, 1, a.SegmentCount);
			Expand(curve, ref a, a.X, compressed[0]);
			
			return true;
		}

		/// <summary>
		/// Expands the given X-coord b into a CurvePoint a
		/// </summary>
		public static void Expand<CurvePoint>(Curve<CurvePoint> curve, ref CurvePoint a, Span<UInt64> b, byte signalBit)
			where CurvePoint : struct, ICurvePoint<CurvePoint>
		{
			Span<UInt64> _3 = stackalloc UInt64[curve.Words];
			_3[0] = 3;

			BigMath.Assign(a.X, b, curve.Words);
			ModMult(curve, a.Y, a.X, a.X);
			BigMath.ModSub(a.Y, a.Y, _3, curve.P, curve.Words);
			ModMult(curve, a.Y, a.Y, a.X);
			BigMath.ModAdd(a.Y, a.Y, curve.B, curve.P, curve.Words);

			ModSqrt(curve, a.Y, a.Y);
			if ((int)(a.Y[0] & 0x01) != (signalBit & 0x01))
			{
				BigMath.Sub(a.Y, curve.P, a.Y, curve.Words);
			}
		}
	}
}
