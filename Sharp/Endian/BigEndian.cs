// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;
using System.IO;

namespace System
{
    /// <summary>
    /// Handles data in big-endian byte order
    /// </summary>
    #if EXPORT_HAMPER_CORE_SHARP
    public
    #else
    internal
    #endif
    static class BigEndian
    {
        /// <summary>
        /// Stores the given integer into an array in big endian byte order
        /// </summary>
        /// <param name="data">An array to write the value to</param>
        /// <param name="value">An integer to store</param>
        /// <param name="offset">An offset at which the integer should start from zero</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Encode(Span<byte> data, Int16 value, int offset = 0)
        {
            data[offset] = (byte)((value >> 8) & 0xFF);
            data[offset + 1] = (byte)(value & 0xFF);
        }
        /// <summary>
        /// Stores the given integer into a stream in big endian byte order
        /// </summary>
        /// <param name="data">A stream to write the value to</param>
        /// <param name="value">An integer to store</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Encode(Stream data, Int16 value)
        {
            data.WriteByte((byte)((value >> 8) & 0xFF));
            data.WriteByte((byte)(value & 0xFF));
        }
        /// <summary>
        /// Stores the given integer into an array in big endian byte order
        /// </summary>
        /// <param name="data">An array to write the value to</param>
        /// <param name="value">An integer to store</param>
        /// <param name="offset">An offset at which the integer should start from zero</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Encode(Span<byte> data, UInt16 value, int offset = 0)
        {
            data[offset] = (byte)((value >> 8) & 0xFF);
            data[offset + 1] = (byte)(value & 0xFF);
        }
        /// <summary>
        /// Stores the given integer into a stream in big endian byte order
        /// </summary>
        /// <param name="data">A stream to write the value to</param>
        /// <param name="value">An integer to store</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Encode(Stream data, UInt16 value)
        {
            data.WriteByte((byte)((value >> 8) & 0xFF));
            data.WriteByte((byte)(value & 0xFF));
        }
        /// <summary>
        /// Stores the given integer into an array in big endian byte order
        /// </summary>
        /// <param name="data">An array to write the value to</param>
        /// <param name="value">An integer to store</param>
        /// <param name="offset">An offset at which the integer should start from zero</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Encode(Span<byte> data, Int32 value, int offset = 0)
        {
            data[offset] = (byte)((value >> 24) & 0xFF);
            data[offset + 1] = (byte)((value >> 16) & 0xFF);
            data[offset + 2] = (byte)((value >> 8) & 0xFF);
            data[offset + 3] = (byte)(value & 0xFF);
        }
        /// <summary>
        /// Stores the given integer into a stream in big endian byte order
        /// </summary>
        /// <param name="data">A stream to write the value to</param>
        /// <param name="value">An integer to store</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Encode(Stream data, Int32 value)
        {
            data.WriteByte((byte)((value >> 24) & 0xFF));
            data.WriteByte((byte)((value >> 16) & 0xFF));
            data.WriteByte((byte)((value >> 8) & 0xFF));
            data.WriteByte((byte)(value & 0xFF));
        }
        /// <summary>
        /// Stores the given integer into an array in big endian byte order
        /// </summary>
        /// <param name="data">An array to write the value to</param>
        /// <param name="value">An integer to store</param>
        /// <param name="offset">An offset at which the integer should start from zero</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Encode(Span<byte> data, UInt32 value, int offset = 0)
        {
            data[offset] = (byte)((value >> 24) & 0xFF);
            data[offset + 1] = (byte)((value >> 16) & 0xFF);
            data[offset + 2] = (byte)((value >> 8) & 0xFF);
            data[offset + 3] = (byte)(value & 0xFF);
        }
        /// <summary>
        /// Stores the given integer into a stream in big endian byte order
        /// </summary>
        /// <param name="data">A stream to write the value to</param>
        /// <param name="value">An integer to store</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Encode(Stream data, UInt32 value)
        {
            data.WriteByte((byte)((value >> 24) & 0xFF));
            data.WriteByte((byte)((value >> 16) & 0xFF));
            data.WriteByte((byte)((value >> 8) & 0xFF));
            data.WriteByte((byte)(value & 0xFF));
        }
        /// <summary>
        /// Stores the given integer into an array in big endian byte order
        /// </summary>
        /// <param name="data">An array to write the value to</param>
        /// <param name="value">An integer to store</param>
        /// <param name="offset">An offset at which the integer should start from zero</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Encode(Span<byte> data, Int64 value, int offset = 0)
        {
            Encode(data, (UInt32)(value >> 32), offset);
            Encode(data, (UInt32)(value & 0xFFFFFFFF), offset + 4);
        }
        /// <summary>
        /// Stores the given integer into a stream in big endian byte order
        /// </summary>
        /// <param name="data">A stream to write the value to</param>
        /// <param name="value">An integer to store</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Encode(Stream data, Int64 value)
        {
            Encode(data, (UInt32)(value >> 32));
            Encode(data, (UInt32)(value & 0xFFFFFFFF));
        }
        /// <summary>
        /// Stores the given integer into an array in big endian byte order
        /// </summary>
        /// <param name="data">An array to write the value to</param>
        /// <param name="value">An integer to store</param>
        /// <param name="offset">An offset at which the integer should start from zero</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Encode(Span<byte> data, UInt64 value, int offset = 0)
        {
            Encode(data, (UInt32)(value >> 32), offset);
            Encode(data, (UInt32)(value & 0xFFFFFFFF), offset + 4);
        }
        /// <summary>
        /// Stores the given integer into a stream in big endian byte order
        /// </summary>
        /// <param name="data">A stream to write the value to</param>
        /// <param name="value">An integer to store</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Encode(Stream data, UInt64 value)
        {
            Encode(data, (UInt32)(value >> 32));
            Encode(data, (UInt32)(value & 0xFFFFFFFF));
        }

        /// <summary>
        /// Reads the given integer from an array in platform byte order
        /// </summary>
        /// <param name="data">An array to read the value from</param>
        /// <param name="offset">An offset at which the integer should start from zero</param>
        /// <returns>The integer value stored in the array</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int16 ToInt16(ReadOnlySpan<byte> data, int offset = 0)
        {
            return (Int16)(data[offset] << 8 | data[offset + 1]);
        }
        /// <summary>
        /// Reads the given integer from a stream in platform byte order
        /// </summary>
        /// <param name="data">A stream to read the value from</param>
        /// <returns>The integer value stored in the stream</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int16 ToInt16(Stream data)
        {
            return (Int16)(data.ReadByte() << 8 | data.ReadByte());
        }
        /// <summary>
        /// Reads the given integer from an array in platform byte order
        /// </summary>
        /// <param name="data">An array to read the value from</param>
        /// <param name="offset">An offset at which the integer should start from zero</param>
        /// <returns>The integer value stored in the array</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt16 ToUInt16(ReadOnlySpan<byte> data, int offset = 0)
        {
            return (UInt16)(data[offset] << 8 | data[offset + 1]);
        }
        /// <summary>
        /// Reads the given integer from a stream in platform byte order
        /// </summary>
        /// <param name="data">A stream to read the value from</param>
        /// <returns>The integer value stored in the stream</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt16 ToUInt16(Stream data)
        {
            return (UInt16)(data.ReadByte() << 8 | data.ReadByte());
        }
        /// <summary>
        /// Reads the given integer from an array in platform byte order
        /// </summary>
        /// <param name="data">An array to read the value from</param>
        /// <param name="offset">An offset at which the integer should start from zero</param>
        /// <returns>The integer value stored in the array</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int32 ToInt32(ReadOnlySpan<byte> data, int offset = 0)
        {
            Int32 result = data[offset];
            result = (result << 8) | data[offset + 1];
            result = (result << 8) | data[offset + 2];
            result = (result << 8) | data[offset + 3];
            return result;
        }
        /// <summary>
        /// Reads the given integer from a stream in platform byte order
        /// </summary>
        /// <param name="data">A stream to read the value from</param>
        /// <returns>The integer value stored in the stream</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int32 ToInt32(Stream data)
        {
            Int32 result = data.ReadByte();
            result = (result << 8) | data.ReadByte();
            result = (result << 8) | data.ReadByte();
            result = (result << 8) | data.ReadByte();
            return result;
        }
        /// <summary>
        /// Reads the given integer from an array in platform byte order
        /// </summary>
        /// <param name="data">An array to read the value from</param>
        /// <param name="offset">An offset at which the integer should start from zero</param>
        /// <returns>The integer value stored in the array</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt32 ToUInt32(ReadOnlySpan<byte> data, int offset = 0)
        {
            UInt32 result = data[offset];
            result = (result << 8) | data[offset + 1];
            result = (result << 8) | data[offset + 2];
            result = (result << 8) | data[offset + 3];
            return result;
        }
        /// <summary>
        /// Reads the given integer from a stream in platform byte order
        /// </summary>
        /// <param name="data">A stream to read the value from</param>
        /// <returns>The integer value stored in the stream</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt32 ToUInt32(Stream data)
        {
            UInt32 result = (UInt32)data.ReadByte();
            result = (result << 8) | (UInt32)data.ReadByte();
            result = (result << 8) | (UInt32)data.ReadByte();
            result = (result << 8) | (UInt32)data.ReadByte();
            return result;
        }
        /// <summary>
        /// Reads the given integer from an array in platform byte order
        /// </summary>
        /// <param name="data">An array to read the value from</param>
        /// <param name="offset">An offset at which the integer should start from zero</param>
        /// <returns>The integer value stored in the array</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int64 ToInt64(ReadOnlySpan<byte> data, int offset = 0)
        {
            UInt32 hiPart = ToUInt32(data, offset);
            UInt32 loPart = ToUInt32(data, offset + 4);
            return (((Int64)hiPart) << 32) | loPart;
        }
        /// <summary>
        /// Reads the given integer from a stream in platform byte order
        /// </summary>
        /// <param name="data">A stream to read the value from</param>
        /// <returns>The integer value stored in the stream</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int64 ToInt64(Stream data)
        {
            UInt32 hiPart = ToUInt32(data);
            UInt32 loPart = ToUInt32(data);
            return (((Int64)hiPart) << 32) | loPart;
        }
        /// <summary>
        /// Reads the given integer from an array in platform byte order
        /// </summary>
        /// <param name="data">An array to read the value from</param>
        /// <param name="offset">An offset at which the integer should start from zero</param>
        /// <returns>The integer value stored in the array</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt64 ToUInt64(ReadOnlySpan<byte> data, int offset = 0)
        {
            UInt32 hiPart = ToUInt32(data, offset);
            UInt32 loPart = ToUInt32(data, offset + 4);
            return (((UInt64)hiPart) << 32) | loPart;
        }
        /// <summary>
        /// Reads the given integer from a stream in platform byte order
        /// </summary>
        /// <param name="data">A stream to read the value from</param>
        /// <returns>The integer value stored in the stream</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt64 ToUInt64(Stream data)
        {
            UInt32 hiPart = ToUInt32(data);
            UInt32 loPart = ToUInt32(data);
            return (((UInt64)hiPart) << 32) | loPart;
        }
    }
}
