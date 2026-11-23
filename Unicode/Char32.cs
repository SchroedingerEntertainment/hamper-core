// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Distributed under the Schroedinger Entertainment EULA (See EULA.md for details

using System;
using System.Runtime.CompilerServices;

namespace Soe.Unicode
{
    /// <summary>
    /// A Unicode compliant single character value
    /// </summary>
    [Serializable]
    #if EXPORT_HAMPER_CORE_UNICODE
    public
    #else
    internal
    #endif
    partial struct Char32 : IComparable, IFormattable, IConvertible, IComparable<Char32>, IEquatable<Char32>
    {
        public const UInt32 Invalid = UInt32.MaxValue;

        readonly UInt32 value;
        /// <summary>
        /// The full 4 byte value of this character
        /// </summary>
        public UInt32 Value
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return value; }
        }

        /// <summary>
        /// Creates a new character from the given value
        /// </summary>
        public Char32(UInt32 value)
        {
            this.value = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Int32(Char32 c)
        {
            return (Int32)c.value;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator UInt32(Char32 c)
        {
            return c.value;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator String(Char32 c)
        {
            return Char.ConvertFromUtf32((int)c.value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Char32(Int32 i)
        {
            return new Char32((UInt32)i);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Char32(UInt32 i)
        {
            return new Char32(i);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int CompareTo(object? obj)
        {
            return value.CompareTo(obj);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int CompareTo(Char32 other)
        {
            return value.CompareTo(other.value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Char32 other)
        {
            return value.Equals(other.value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TypeCode GetTypeCode()
        {
            return value.GetTypeCode();
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool ToBoolean(IFormatProvider? provider)
        {
            return Convert.ToBoolean(value, provider);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public char ToChar(IFormatProvider? provider)
        {
            return Convert.ToChar(value, provider);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public sbyte ToSByte(IFormatProvider? provider)
        {
            return Convert.ToSByte(value, provider);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte ToByte(IFormatProvider? provider)
        {
            return Convert.ToByte(value, provider);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public short ToInt16(IFormatProvider? provider)
        {
            return Convert.ToInt16(value, provider);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ushort ToUInt16(IFormatProvider? provider)
        {
            return Convert.ToUInt16(value, provider);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int ToInt32(IFormatProvider? provider)
        {
            return Convert.ToInt32(value, provider);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public uint ToUInt32(IFormatProvider? provider)
        {
            return Convert.ToUInt32(value, provider);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public long ToInt64(IFormatProvider? provider)
        {
            return Convert.ToInt64(value, provider);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ulong ToUInt64(IFormatProvider? provider)
        {
            return Convert.ToUInt64(value, provider);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float ToSingle(IFormatProvider? provider)
        {
            return Convert.ToSingle(value, provider);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double ToDouble(IFormatProvider? provider)
        {
            return Convert.ToDouble(value, provider);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public decimal ToDecimal(IFormatProvider? provider)
        {
            return Convert.ToDecimal(value, provider);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public DateTime ToDateTime(IFormatProvider? provider)
        {
            return Convert.ToDateTime(value, provider);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string ToString(IFormatProvider? provider)
        {
            return Convert.ToString(value, provider);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public object ToType(Type conversionType, IFormatProvider? provider)
        {
            return Convert.ChangeType(value, conversionType, provider);
        }

        /// <summary>
        /// Converts this character into a multibyte character string
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string ToMultibyteChar()
        {
            return Char.ConvertFromUtf32((int)value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString()
        {
            return ToMultibyteChar();
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string ToString(string? format, IFormatProvider? formatProvider)
        {
            return value.ToString(format, formatProvider);
        }
    }
}