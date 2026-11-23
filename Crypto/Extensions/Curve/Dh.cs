// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;
using System.Security.Cryptography;
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
        /// Creates a shared secret from two elliptic curve Cryptography keys
        /// </summary>
        /// <param name="curve">The curve in which order the key should exist</param>
        /// <param name="publicKey">A foreign Elliptic Curve Cryptography public key</param>
        /// <param name="selfPrivateKey">The local Elliptic Curve Cryptography private key</param>
        /// <param name="shared">An array of 64 bit segments to store the secret</param>
        /// <returns>An indicator flag to determine if the result is odd or even</returns>
        public static byte GetSharedKey<CurvePoint>(this Curve<CurvePoint> curve, in CurvePoint publicKey, Span<UInt64> selfPrivateKey, Span<UInt64> shared)
            where CurvePoint : struct, ICurvePoint<CurvePoint>
        {
            CurvePoint tmp = CurvePoint.Create(curve.Words);

            EccMath.Mult(curve, ref tmp, publicKey, selfPrivateKey);
            BigMath.Assign(shared, tmp.X, curve.Words);

            if (EccMath.IsZero(tmp))
            {
                return 0;
            }
            else return (byte)(2 + (tmp.Y[0] & 0x01));
        }

        /// <summary>
        /// Creates a private secret key from two Elliptic Curve Cryptography keys. This
        /// key could be used to sign data using Elliptic Curve Cryptography Data Signing Algorithm
        /// </summary>
        /// <param name="curve">The curve in which order the key should exist</param>
        /// <param name="publicKey">A foreign Elliptic Curve Cryptography public key</param>
        /// <param name="selfPrivateKey">The local Elliptic Curve Cryptography private key</param>
        /// <param name="sharedPrivateKey">An array of 64 bit segments to store the key</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void GetSharedPrivateKey<CurvePoint>(this Curve<CurvePoint> curve, in CurvePoint publicKey, Span<UInt64> selfPrivateKey, Span<UInt64> sharedPrivateKey)
            where CurvePoint : struct, ICurvePoint<CurvePoint>
        {
            Span<UInt64> c = stackalloc UInt64[curve.Words];
            GetScalarFromSecret(curve, publicKey, selfPrivateKey, c);

            BigMath.ModAdd(sharedPrivateKey, selfPrivateKey, c, curve.N, curve.Words);
        }

        /// <summary>
        /// Creates a public secret key from two Elliptic Curve Cryptography keys. This
        /// key could be used to verify data using Elliptic Curve Cryptography Data Signing Algorithm
        /// </summary>
        /// <param name="curve">The curve in which order the key should exist</param>
        /// <param name="publicKey">A foreign Elliptic Curve Cryptography public key</param>
        /// <param name="selfPrivateKey">The local Elliptic Curve Cryptography private key</param>
        /// <param name="sharedPubKey">The resulting public key</param>
        public static void GetSharedPublicKey<CurvePoint>(this Curve<CurvePoint> curve, in CurvePoint publicKey, Span<UInt64> selfPrivateKey, ref CurvePoint sharedPubKey)
            where CurvePoint : struct, ICurvePoint<CurvePoint>
        {
            Span<UInt64> c = stackalloc UInt64[curve.Words];
            
            GetScalarFromSecret(curve, publicKey, selfPrivateKey, c);

            CurvePoint tmp = CurvePoint.Create(curve.Words);
            EccMath.Mult(curve, ref tmp, curve.G, c);
            EccMath.Add(curve, ref sharedPubKey, publicKey, tmp);
        }

        private static void GetScalarFromSecret<CurvePoint>(Curve<CurvePoint> curve, in CurvePoint publicKey, Span<UInt64> privateKey, Span<UInt64> scalar)
            where CurvePoint : struct, ICurvePoint<CurvePoint>
        {
            CurvePoint tmp = CurvePoint.Create(curve.Words);
            EccMath.Mult(curve, ref tmp, publicKey, privateKey);

            Span<byte> hash = stackalloc byte[curve.Bytes + 1];
            EccMath.Compress(tmp, hash);

            Span<UInt64> tc = stackalloc UInt64[curve.Words];
            Span<byte> tt = stackalloc byte[32];
            SHA256.HashData(hash, tt);

            int length = Math.Min(curve.Words, 4);
            for (int i = 0; i < length; i++)
            {
                tc[i] = BitConverter.ToUInt64(tt.Slice(i * 8));
            }

            BigMath.AssignZero(scalar, curve.Words);
            BigMath.Assign(scalar, tc, curve.Words);
        }
    }
}