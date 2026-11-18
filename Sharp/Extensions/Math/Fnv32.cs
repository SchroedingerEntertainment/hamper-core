// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;
using System.IO;

namespace System
{
    #if EXPORT_HAMPER_CORE_SHARP
    public
    #else
    internal
    #endif
    static partial class HashExtensions
    {
        // ReSharper disable InvalidXmlDocComment
        
        /// <summary>
        /// Returns a 32-bit hash value from this value
        /// </summary>
        /// <param name="offsetBasis">An optional 32-bit hash value to concatenate</param>
        /// <returns>The resulting 32-bit hash value</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt32 Fnv32(this UInt16 i, UInt32 offsetBasis = HashUtility.FnvOffsetBias)
        {
            offsetBasis ^= i;
            offsetBasis *= HashUtility.FnvPrime;

            return offsetBasis;
        }
        /// <summary>
        /// Returns a 32-bit hash value from this value
        /// </summary>
        /// <param name="offsetBasis">An optional 32-bit hash value to concatenate</param>
        /// <returns>The resulting 32-bit hash value</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt32 Fnv32(this Int16 i, UInt32 offsetBasis = HashUtility.FnvOffsetBias)
        {
            offsetBasis ^= (UInt16)i;
            offsetBasis *= HashUtility.FnvPrime;

            return offsetBasis;
        }
        /// <summary>
        /// Returns a 32-bit hash value from this value
        /// </summary>
        /// <param name="offsetBasis">An optional 32-bit hash value to concatenate</param>
        /// <returns>The resulting 32-bit hash value</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt32 Fnv32(this UInt32 i, UInt32 offsetBasis = HashUtility.FnvOffsetBias)
        {
            offsetBasis ^= i;
            offsetBasis *= HashUtility.FnvPrime;

            return offsetBasis;
        }
        /// <summary>
        /// Returns a 32-bit hash value from this value
        /// </summary>
        /// <param name="offsetBasis">An optional 32-bit hash value to concatenate</param>
        /// <returns>The resulting 32-bit hash value</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt32 Fnv32(this Int32 i, UInt32 offsetBasis = HashUtility.FnvOffsetBias)
        {
            offsetBasis ^= (UInt32)i;
            offsetBasis *= HashUtility.FnvPrime;

            return offsetBasis;
        }
        /// <summary>
        /// Returns a 32-bit hash value from this value
        /// </summary>
        /// <param name="offsetBasis">An optional 32-bit hash value to concatenate</param>
        /// <returns>The resulting 32-bit hash value</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt32 Fnv32(this UInt64 i, UInt32 offsetBasis = HashUtility.FnvOffsetBias)
        {
            offsetBasis ^= (UInt32)(i & UInt32.MaxValue);
            offsetBasis *= HashUtility.FnvPrime;

            offsetBasis ^= (UInt32)(i >> 32);
            offsetBasis *= HashUtility.FnvPrime;
            
            return offsetBasis;
        }
        /// <summary>
        /// Returns a 32-bit hash value from this value
        /// </summary>
        /// <param name="offsetBasis">An optional 32-bit hash value to concatenate</param>
        /// <returns>The resulting 32-bit hash value</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt32 Fnv32(this Int64 i, UInt32 offsetBasis = HashUtility.FnvOffsetBias)
        {
            offsetBasis ^= (UInt32)(i & UInt32.MaxValue);
            offsetBasis *= HashUtility.FnvPrime;

            offsetBasis ^= (UInt32)(i >> 32);
            offsetBasis *= HashUtility.FnvPrime;
            
            return offsetBasis;
        }
        
        /// <summary>
        /// Returns a 32-bit hash value from this string
        /// </summary>
        /// <param name="offsetBasis">An optional 32-bit hash value to concatenate</param>
        /// <returns>The resulting 32-bit hash value</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt32 Fnv32(this string str, UInt32 offsetBasis = HashUtility.FnvOffsetBias)
        {
            for (int i = 0; i < str.Length; i++)
            {
                offsetBasis ^= str[i];
                offsetBasis *= HashUtility.FnvPrime;
            }
            return offsetBasis;
        }
        /// <summary>
        /// Returns a 32-bit hash value from this string
        /// </summary>
        /// <param name="offsetBasis">An optional 32-bit hash value to concatenate</param>
        /// <returns>The resulting 32-bit hash value</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt32 Fnv32(this ReadOnlySpan<byte> bytes, UInt32 offsetBasis = HashUtility.FnvOffsetBias)
        {
            for (int i = 0; i < bytes.Length; i++)
            {
                offsetBasis ^= bytes[i];
                offsetBasis *= HashUtility.FnvPrime;
            }
            return offsetBasis;
        }
        /// <summary>
        /// Returns a 32-bit hash value from this stream
        /// </summary>
        /// <param name="size">The size of bytes to process</param>
        /// <param name="offsetBasis">An optional 32 bit hash value to concatenate</param>
        /// <returns>The resulting 32-bit hash value</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt32 Fnv32(this Stream stream, int size, UInt32 offsetBasis = HashUtility.FnvOffsetBias)
        {
            for (; size > 0; size--)
            {
                offsetBasis ^= (byte)stream.ReadByte();
                offsetBasis *= HashUtility.FnvPrime;
            }
            return offsetBasis;
        }
        
        // ReSharper restore InvalidXmlDocComment
    }
}
