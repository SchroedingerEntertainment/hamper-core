// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;

namespace System.IO
{
    #if EXPORT_HAMPER_CORE_SHARP
    public
    #else
    internal
    #endif
    static partial class EndianExtension
    {
        // ReSharper disable InvalidXmlDocComment
        
        /// <summary>
        /// Stores the given integer into this stream in certain endian byte order
        /// </summary>
        /// <param name="flag">The desired endian byte order</param>
        /// <param name="value">An integer to store</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Encode(this Stream data, EndiannessFlags flag, Int16 value)
        {
            if (flag == EndiannessFlags.BigEndian)
            {
                BigEndian.Encode(data, value);
            }
            else LittleEndian.Encode(data, value);
        }
        /// <summary>
        /// Stores the given integer into this stream in certain endian byte order
        /// </summary>
        /// <param name="flag">The desired endian byte order</param>
        /// <param name="value">An integer to store</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Encode(this Stream data, EndiannessFlags flag, UInt16 value)
        {
            if (flag == EndiannessFlags.BigEndian)
            {
                BigEndian.Encode(data, value);
            }
            else LittleEndian.Encode(data, value);
        }
        /// <summary>
        /// Stores the given integer into this stream in certain endian byte order
        /// </summary>
        /// <param name="flag">The desired endian byte order</param>
        /// <param name="value">An integer to store</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Encode(this Stream data, EndiannessFlags flag, Int32 value)
        {
            if (flag == EndiannessFlags.BigEndian)
            {
                BigEndian.Encode(data, value);
            }
            else LittleEndian.Encode(data, value);
        }
        /// <summary>
        /// Stores the given integer into this stream in certain endian byte order
        /// </summary>
        /// <param name="flag">The desired endian byte order</param>
        /// <param name="value">An integer to store</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Encode(this Stream data, EndiannessFlags flag, UInt32 value)
        {
            if (flag == EndiannessFlags.BigEndian)
            {
                BigEndian.Encode(data, value);
            }
            else LittleEndian.Encode(data, value);
        }
        /// <summary>
        /// Stores the given integer into this stream in certain endian byte order
        /// </summary>
        /// <param name="flag">The desired endian byte order</param>
        /// <param name="value">An integer to store</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Encode(this Stream data, EndiannessFlags flag, Int64 value)
        {
            if (flag == EndiannessFlags.BigEndian)
            {
                BigEndian.Encode(data, value);
            }
            else LittleEndian.Encode(data, value);
        }
        /// <summary>
        /// Stores the given integer into this stream in certain endian byte order
        /// </summary>
        /// <param name="flag">The desired endian byte order</param>
        /// <param name="value">An integer to store</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Encode(this Stream data, EndiannessFlags flag, UInt64 value)
        {
            if (flag == EndiannessFlags.BigEndian)
            {
                BigEndian.Encode(data, value);
            }
            else LittleEndian.Encode(data, value);
        }
        
        // ReSharper restore InvalidXmlDocComment
    }
}
