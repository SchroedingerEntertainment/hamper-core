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
    static partial class EndianExtension
    {
        // ReSharper disable InvalidXmlDocComment
        
        /// <summary>
        /// Reads the given integer from this array in platform byte order
        /// </summary>
        /// <param name="flag">The desired endian byte order</param>
        /// <param name="offset">An offset at which the integer should start from zero</param>
        /// <returns>The integer value stored in this array</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int16 ToInt16(this Span<byte> data, EndiannessFlags flag, int offset = 0)
        {
            if (flag == EndiannessFlags.BigEndian)
            {
                return BigEndian.ToInt16(data, offset);
            }
            else return LittleEndian.ToInt16(data, offset);
        }
        /// <summary>
        /// Reads the given integer from this array in platform byte order
        /// </summary>
        /// <param name="flag">The desired endian byte order</param>
        /// <param name="offset">An offset at which the integer should start from zero</param>
        /// <returns>The integer value stored in this array</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt16 ToUInt16(this Span<byte> data, EndiannessFlags flag, int offset = 0)
        {
            if (flag == EndiannessFlags.BigEndian)
            {
                return BigEndian.ToUInt16(data, offset);
            }
            else return LittleEndian.ToUInt16(data, offset);
        }
        /// <summary>
        /// Reads the given integer from this array in platform byte order
        /// </summary>
        /// <param name="flag">The desired endian byte order</param>
        /// <param name="offset">An offset at which the integer should start from zero</param>
        /// <returns>The integer value stored in this array</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int32 ToInt32(this Span<byte> data, EndiannessFlags flag, int offset = 0)
        {
            if (flag == EndiannessFlags.BigEndian)
            {
                return BigEndian.ToInt32(data, offset);
            }
            else return LittleEndian.ToInt32(data, offset);
        }
        /// <summary>
        /// Reads the given integer from this array in platform byte order
        /// </summary>
        /// <param name="flag">The desired endian byte order</param>
        /// <param name="offset">An offset at which the integer should start from zero</param>
        /// <returns>The integer value stored in this array</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt32 ToUInt32(this Span<byte> data, EndiannessFlags flag, int offset = 0)
        {
            if (flag == EndiannessFlags.BigEndian)
            {
                return BigEndian.ToUInt32(data, offset);
            }
            else return LittleEndian.ToUInt32(data, offset);
        }
        /// <summary>
        /// Reads the given integer from this array in platform byte order
        /// </summary>
        /// <param name="flag">The desired endian byte order</param>
        /// <param name="offset">An offset at which the integer should start from zero</param>
        /// <returns>The integer value stored in this array</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int64 ToInt64(this Span<byte> data, EndiannessFlags flag, int offset = 0)
        {
            if (flag == EndiannessFlags.BigEndian)
            {
                return BigEndian.ToInt64(data, offset);
            }
            else return LittleEndian.ToInt64(data, offset);
        }
        /// <summary>
        /// Reads the given integer from this array in platform byte order
        /// </summary>
        /// <param name="flag">The desired endian byte order</param>
        /// <param name="offset">An offset at which the integer should start from zero</param>
        /// <returns>The integer value stored in this array</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt64 ToUInt64(this Span<byte> data, EndiannessFlags flag, int offset = 0)
        {
            if (flag == EndiannessFlags.BigEndian)
            {
                return BigEndian.ToUInt64(data, offset);
            }
            else return LittleEndian.ToUInt64(data, offset);
        }
        
        // ReSharper restore InvalidXmlDocComment
    }
}