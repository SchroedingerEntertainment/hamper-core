// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;
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
        /// Creates a new elliptic curve cryptography data signing private key from a given seed
        /// </summary>
        /// <param name="curve">The curve in which order the key should exist</param>
        /// <param name="privateKey">An array of 64 bit segments to store the key</param>
        /// <param name="seed">The seed to use as key basic</param>
        public static bool GetPrivateKey<CurvePoint>(this Curve<CurvePoint> curve, Span<UInt64> privateKey, Span<byte> seed)
            where CurvePoint : struct, ICurvePoint<CurvePoint>
        {
            for (int i = 0; i < curve.Words; i++)
            {
                privateKey[i] = BitConverter.ToUInt64(seed.Slice(i * 8));
            }
            if (BigMath.IsZero(privateKey, curve.Words))
            {
                return false;
            }
            else if (BigMath.Compare(curve.N, privateKey, curve.Words) != 1)
            {
                BigMath.Sub(privateKey, privateKey, curve.N, curve.Words);
            }
            return (!BigMath.IsZero(privateKey, curve.Words));
        }
        /// <summary>
        /// Creates a new elliptic curve cryptography data signing private key
        /// </summary>
        /// <param name="curve">The curve in which order the key should exist</param>
        /// <param name="privateKey">An array of 64 bit segments to store the key</param>
        public static void GetPrivateKey<CurvePoint>(this Curve<CurvePoint> curve, Span<UInt64> privateKey)
            where CurvePoint : struct, ICurvePoint<CurvePoint>
        {
            do
            {
                RandomBytes(privateKey, curve.Words);
                if (BigMath.IsZero(privateKey, curve.Words))
                {
                    continue;
                }
                if (BigMath.Compare(curve.N, privateKey, curve.Words) != 1)
                {
                    BigMath.Sub(privateKey, privateKey, curve.N, curve.Words);
                }
            }
            while (BigMath.IsZero(privateKey, curve.Words));
        }

        /// <summary>
        /// Creates the corresponding elliptic curve cryptography data signing public key from a given private key
        /// </summary>
        /// <param name="curve">The curve in which order the key should exist</param>
        /// <param name="privateKey">The key to generate it's public counterpart for</param>
        /// <param name="publicKey">The resulting public key</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void GetPublicKey<CurvePoint>(this Curve<CurvePoint> curve, Span<UInt64> privateKey, ref CurvePoint publicKey)
            where CurvePoint : struct, ICurvePoint<CurvePoint>
        {
            EccMath.Mult(curve, ref publicKey, curve.G, privateKey);
        }

        static void RandomBytes(Span<UInt64> digits, int words)
        {
            Span<byte> tmp = stackalloc byte[words * 8];
            Random.Shared.NextBytes(tmp);

            for (int i = 0; i < words; i++)
            {
                digits[i] = BitConverter.ToUInt64(tmp.Slice(i * 8));
            }
        }
    }
}