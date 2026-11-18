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
        /// Returns a 16-bit hash value from this string
        /// </summary>
        /// <param name="offsetBasis">An optional 32-bit hash value to concatenate</param>
        /// <returns>The resulting 16-bit hash value</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt16 Fnv16(this string str, UInt32 offsetBasis = HashUtility.FnvOffsetBias)
        {
            offsetBasis = str.Fnv32(offsetBasis);
            offsetBasis ^= (offsetBasis >> 16);

            return (UInt16)(offsetBasis & UInt16.MaxValue);
        }
        /// <summary>
        /// Returns a 16-bit hash value from this string
        /// </summary>
        /// <param name="offsetBasis">An optional 32-bit hash value to concatenate</param>
        /// <returns>The resulting 16-bit hash value</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt16 Fnv16(this ReadOnlySpan<byte> bytes, UInt32 offsetBasis = HashUtility.FnvOffsetBias)
        {
            offsetBasis = bytes.Fnv32(offsetBasis);
            offsetBasis ^= (offsetBasis >> 16);

            return (UInt16)(offsetBasis & UInt16.MaxValue);
        }
        /// <summary>
        /// Returns a 16-bit hash value from this stream
        /// </summary>
        /// <param name="size">The size of bytes to process</param>
        /// <param name="offsetBasis">An optional 32-bit hash value to concatenate</param>
        /// <returns>The resulting 16-bit hash value</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt16 Fnv16(this Stream stream, int size, UInt32 offsetBasis = HashUtility.FnvOffsetBias)
        {
            offsetBasis = stream.Fnv32(size, offsetBasis);
            offsetBasis ^= (offsetBasis >> 16);

            return (UInt16)(offsetBasis & UInt16.MaxValue);
        }
        
        // ReSharper restore InvalidXmlDocComment
    }
}
