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
        /// Reads the given integer from this stream in platform byte order
        /// </summary>
        /// <param name="flag">The desired endian byte order</param>
        /// <returns>The integer value stored in this stream</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int16 ToInt16(this Stream data, EndiannessFlags flag)
        {
            if (flag == EndiannessFlags.BigEndian)
            {
                return BigEndian.ToInt16(data);
            }
            else return LittleEndian.ToInt16(data);
        }
        /// <summary>
        /// Reads the given integer from this stream in platform byte order
        /// </summary>
        /// <param name="flag">The desired endian byte order</param>
        /// <returns>The integer value stored in this stream</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt16 ToUInt16(this Stream data, EndiannessFlags flag)
        {
            if (flag == EndiannessFlags.BigEndian)
            {
                return BigEndian.ToUInt16(data);
            }
            else return LittleEndian.ToUInt16(data);
        }
        /// <summary>
        /// Reads the given integer from this stream in platform byte order
        /// </summary>
        /// <param name="flag">The desired endian byte order</param>
        /// <returns>The integer value stored in this stream</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int32 ToInt32(this Stream data, EndiannessFlags flag)
        {
            if (flag == EndiannessFlags.BigEndian)
            {
                return BigEndian.ToInt32(data);
            }
            else return LittleEndian.ToInt32(data);
        }
        /// <summary>
        /// Reads the given integer from this stream in platform byte order
        /// </summary>
        /// <param name="flag">The desired endian byte order</param>
        /// <returns>The integer value stored in this stream</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt32 ToUInt32(this Stream data, EndiannessFlags flag)
        {
            if (flag == EndiannessFlags.BigEndian)
            {
                return BigEndian.ToUInt32(data);
            }
            else return LittleEndian.ToUInt32(data);
        }
        /// <summary>
        /// Reads the given integer from this stream in platform byte order
        /// </summary>
        /// <param name="flag">The desired endian byte order</param>
        /// <returns>The integer value stored in this stream</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int64 ToInt64(this Stream data, EndiannessFlags flag)
        {
            if (flag == EndiannessFlags.BigEndian)
            {
                return BigEndian.ToInt64(data);
            }
            else return LittleEndian.ToInt64(data);
        }
        /// <summary>
        /// Reads the given integer from this stream in platform byte order
        /// </summary>
        /// <param name="flag">The desired endian byte order</param>
        /// <returns>The integer value stored in this stream</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt64 ToUInt64(this Stream data, EndiannessFlags flag)
        {
            if (flag == EndiannessFlags.BigEndian)
            {
                return BigEndian.ToUInt64(data);
            }
            else return LittleEndian.ToUInt64(data);
        }
        
        // ReSharper restore InvalidXmlDocComment
    }
}