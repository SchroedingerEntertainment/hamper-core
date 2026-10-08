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
    static partial class EndianExtension
    {
        // ReSharper disable InvalidXmlDocComment
        
        /// <summary>
        /// Stores the given integer into this array in certain endian byte order
        /// </summary>
        /// <param name="flag">The desired endian byte order</param>
        /// <param name="value">An integer to store</param>
        /// <param name="offset">An offset at which the integer should start from zero</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Encode(this Span<byte> data, EndiannessFlags flag, Int16 value, int offset = 0)
        {
            if (flag == EndiannessFlags.BigEndian)
            {
                BigEndian.Encode(data, value, offset);
            }
            else LittleEndian.Encode(data, value, offset);
        }
        /// <summary>
        /// Stores the given integer into this array in certain endian byte order
        /// </summary>
        /// <param name="flag">The desired endian byte order</param>
        /// <param name="value">An integer to store</param>
        /// <param name="offset">An offset at which the integer should start from zero</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Encode(this Span<byte> data, EndiannessFlags flag, UInt16 value, int offset = 0)
        {
            if (flag == EndiannessFlags.BigEndian)
            {
                BigEndian.Encode(data, value, offset);
            }
            else LittleEndian.Encode(data, value, offset);
        }
        /// <summary>
        /// Stores the given integer into this array in certain endian byte order
        /// </summary>
        /// <param name="flag">The desired endian byte order</param>
        /// <param name="value">An integer to store</param>
        /// <param name="offset">An offset at which the integer should start from zero</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Encode(this Span<byte> data, EndiannessFlags flag, Int32 value, int offset = 0)
        {
            if (flag == EndiannessFlags.BigEndian)
            {
                BigEndian.Encode(data, value, offset);
            }
            else LittleEndian.Encode(data, value, offset);
        }
        /// <summary>
        /// Stores the given integer into this array in certain endian byte order
        /// </summary>
        /// <param name="flag">The desired endian byte order</param>
        /// <param name="value">An integer to store</param>
        /// <param name="offset">An offset at which the integer should start from zero</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Encode(this Span<byte> data, EndiannessFlags flag, UInt32 value, int offset = 0)
        {
            if (flag == EndiannessFlags.BigEndian)
            {
                BigEndian.Encode(data, value, offset);
            }
            else LittleEndian.Encode(data, value, offset);
        }
        /// <summary>
        /// Stores the given integer into this array in certain endian byte order
        /// </summary>
        /// <param name="flag">The desired endian byte order</param>
        /// <param name="value">An integer to store</param>
        /// <param name="offset">An offset at which the integer should start from zero</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Encode(this Span<byte> data, EndiannessFlags flag, Int64 value, int offset = 0)
        {
            if (flag == EndiannessFlags.BigEndian)
            {
                BigEndian.Encode(data, value, offset);
            }
            else LittleEndian.Encode(data, value, offset);
        }
        /// <summary>
        /// Stores the given integer into this array in certain endian byte order
        /// </summary>
        /// <param name="flag">The desired endian byte order</param>
        /// <param name="value">An integer to store</param>
        /// <param name="offset">An offset at which the integer should start from zero</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Encode(this Span<byte> data, EndiannessFlags flag, UInt64 value, int offset = 0)
        {
            if (flag == EndiannessFlags.BigEndian)
            {
                BigEndian.Encode(data, value, offset);
            }
            else LittleEndian.Encode(data, value, offset);
        }
        
        // ReSharper restore InvalidXmlDocComment
    }
}
