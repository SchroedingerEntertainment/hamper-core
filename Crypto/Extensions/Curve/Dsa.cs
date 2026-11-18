// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using Soe.Utility.Math;

namespace Soe.Crypto
{
    #if EXPORT_HAMPER_CORE_CRYPTO
	public
	#else
	internal
	#endif
    static partial class CurveExtension
    {
        /// <summary>
        /// Computes an elliptic curve cryptography signature pair from a hash value and
        /// the local private key
        /// </summary>
        /// <param name="curve">The curve in which order the key should exist</param>
        /// <param name="privateKey">The local Elliptic Curve Cryptography private key</param>
        /// <param name="hash">A hash value to create signature from</param>
        /// <param name="signature">The resulting signature pair</param>
        public static void Sign<CurvePoint>(this Curve<CurvePoint> curve, Span<UInt64> privateKey, Span<byte> hash, ref CurvePoint signature)
            where CurvePoint : struct, ICurvePoint<CurvePoint>
        {
            Span<UInt64> k = stackalloc UInt64[curve.Words];
            do
            {
                GetPrivateKey(curve, k);
                EccMath.Mult(curve, ref signature, curve.G, k);

                if (BigMath.Compare(curve.N, signature.X, curve.Words) != 1)
                {
                    BigMath.Sub(signature.X, signature.X, curve.N, curve.Words);
                }
            }
            while (BigMath.IsZero(signature.X, curve.Words));

            BigMath.ModMult(signature.Y, signature.X, privateKey, curve.N, curve.Words);

            Span<UInt64> tt = stackalloc UInt64[curve.Words];
            
            int length = Math.Min(curve.Words, hash.Length / 8);
            for (int i = 0; i < length; i++)
            {
                tt[i] = BitConverter.ToUInt64(hash.Slice(i * 8));
            }
            BigMath.ModAdd(signature.Y, tt, signature.Y, curve.N, curve.Words);
            BigMath.ModInv(k, k, curve.N, curve.Words);
            BigMath.ModMult(signature.Y, signature.Y, k, curve.N, curve.Words);
        }

        /// <summary>
        /// Verifies a signature pair to confirm the corresponding elliptic curve cryptography public key
        /// </summary>
        /// <param name="curve">The curve in which order the key should exist</param>
        /// <param name="publicKey">The known corresponding public key</param>
        /// <param name="hash">The hash value the signature was computed from</param>
        /// <param name="signature">The signature pair to verify against</param>
        /// <returns>True if the signature pair matches the corresponding public key, false otherwise</returns>
        public static bool Verify<CurvePoint>(this Curve<CurvePoint> curve, in CurvePoint publicKey, Span<byte> hash, in CurvePoint signature)
            where CurvePoint : struct, ICurvePoint<CurvePoint>
        {
            if (BigMath.IsZero(signature.X, curve.Words) || BigMath.IsZero(signature.Y, curve.Words))
            {
                return false;
            }
            else if (BigMath.Compare(curve.N, signature.X, curve.Words) != 1 || BigMath.Compare(curve.N, signature.Y, curve.Words) != 1)
            {
                return false;
            }

            Span<UInt64> hashValue = stackalloc UInt64[curve.Words];
            
            int length = Math.Min(curve.Words, hash.Length / 8);
            for (int i = 0; i < length; i++)
            {
                hashValue[i] = BitConverter.ToUInt64(hash.Slice(i * 8));
            }

            Span<UInt64> rX = stackalloc UInt64[curve.Words];
            Unpack(curve, in publicKey, signature.X, signature.Y, hashValue, rX);

            return (BigMath.Compare(rX, signature.X, curve.Words) == 0);
        }

        static void Unpack<CurvePoint>(Curve<CurvePoint> curve, in CurvePoint key, Span<UInt64> r, Span<UInt64> s, Span<UInt64> hash, Span<UInt64> rX)
            where CurvePoint : struct, ICurvePoint<CurvePoint>
        {
            Span<UInt64> z = stackalloc UInt64[curve.Words];
            Span<UInt64> u1 = stackalloc UInt64[curve.Words];
            Span<UInt64> u2 = stackalloc UInt64[curve.Words];
            
            BigMath.ModInv(z, s, curve.N, curve.Words);

            BigMath.ModMult(u1, hash, z, curve.N, curve.Words);
            BigMath.ModMult(u2, r, z, curve.N, curve.Words);

            CurvePoint pq = CurvePoint.Create(curve.Words);
            BigMath.Assign(pq.X, key.X, curve.Words);
            BigMath.Assign(pq.Y, key.Y, curve.Words);

            Span<UInt64> tX = stackalloc UInt64[curve.Words];
            Span<UInt64> tY = stackalloc UInt64[curve.Words];
            Span<UInt64> tZ = stackalloc UInt64[curve.Words];
            
            BigMath.Assign(tX, curve.G.X, curve.Words);
            BigMath.Assign(tY, curve.G.Y, curve.Words);
            BigMath.ModSub(z, pq.X, curve.G.X, curve.P, curve.Words);
            EccMath.Add(curve, tX, tY, pq.X, pq.Y);
            BigMath.ModInv(z, z, curve.P, curve.Words);
            EccMath.Add(curve, pq.X, pq.Y, z);

            CurvePoint[] shamirCurvePoints =
            [
                default,
                curve.G,
                key,
                pq
            ];
            int bits = Math.Max(BigMath.GetBits(u1, curve.Words), BigMath.GetBits(u2, curve.Words));

            ref CurvePoint p = ref shamirCurvePoints
            [
                ((BigMath.GetBitLong(u1, bits - 1, curve.Words)) ? 1 : 0) |
                (((BigMath.GetBitLong(u2, bits - 1, curve.Words)) ? 1 : 0) << 1)
            ];

            Span<UInt64> rY = stackalloc UInt64[curve.Words];
            
            BigMath.Assign(rX, p.X, curve.Words);
            BigMath.Assign(rY, p.Y, curve.Words);
            BigMath.AssignZero(z, curve.Words);
            z[0] = 1;

            for (int i = (bits - 2); i >= 0; i--)
            {
                EccMath.Double(curve, rX, rY, z);
                p = ref shamirCurvePoints
                [
                    ((BigMath.GetBitLong(u1, i, curve.Words)) ? 1 : 0) |
                    (((BigMath.GetBitLong(u2, i, curve.Words)) ? 1 : 0) << 1)
                ];
                if (p.IsValid)
                {
                    BigMath.Assign(tX, p.X, curve.Words);
                    BigMath.Assign(tY, p.Y, curve.Words);
                    EccMath.Add(curve, tX, tY, z);
                    BigMath.ModSub(tZ, rX, tX, curve.P, curve.Words);
                    EccMath.Add(curve, tX, tY, rX, rY);
                    EccMath.ModMult(curve, z, z, tZ);
                }
            }
            BigMath.ModInv(z, z, curve.P, curve.Words);
            EccMath.Add(curve, rX, rY, z);

            if (BigMath.Compare(curve.N, rX, curve.Words) != 1)
            {
                BigMath.Sub(rX, rX, curve.N, curve.Words);
            }
        }
    }
}
