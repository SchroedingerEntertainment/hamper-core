// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;

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
        /// Returns a 16-bit hash value from this integer
        /// </summary>
        /// <returns>The resulting 16-bit hash value</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt16 FibonacciHash(this UInt16 i)
        {
            return (UInt16)((i ^ (i >> 8)) * HashUtility.FibonacciConstant16);
        }
        /// <summary>
        /// Returns a 16-bit hash value from this integer
        /// </summary>
        /// <returns>The resulting 16-bit hash value</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int16 FibonacciHash(this Int16 i)
        {
            return (Int16)FibonacciHash((UInt16)i);
        }

        /// <summary>
        /// Returns a 32-bit hash value from this integer
        /// </summary>
        /// <returns>The resulting 32-bit hash value</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt32 FibonacciHash(this UInt32 i)
        {
            return ((i ^ (i >> 16)) * HashUtility.FibonacciConstant32);
        }
        /// <summary>
        /// Returns a 32-bit hash value from this integer
        /// </summary>
        /// <returns>The resulting 32-bit hash value</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int32 FibonacciHash(this Int32 i)
        {
            return (Int32)FibonacciHash((UInt32)i);
        }

        /// <summary>
        /// Returns a 64-bit hash value from this integer
        /// </summary>
        /// <returns>The resulting 64-bit hash value</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt64 FibonacciHash(this UInt64 i)
        {
            return ((i ^ (i >> 32)) * HashUtility.FibonacciConstant64);
        }
        /// <summary>
        /// Returns a 64-bit hash value from this integer
        /// </summary>
        /// <returns>The resulting 64-bit hash value</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int64 FibonacciHash(this Int64 i)
        {
            return (Int64)FibonacciHash((UInt64)i);
        }
        
        // ReSharper restore InvalidXmlDocComment
    }
}
