// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Soe.Utility
{
    /// <summary>
    /// Represents a large number of 128-bit
    /// </summary>
    [InlineArray(2)]
    #if EXPORT_HAMPER_CORE_MATH
    public
    #else
    internal
    #endif
    struct UInt128 : IBigNumber
    {
        /// <summary>
        /// A large number of 128 bit, initialized to zero
        /// </summary>
        public static readonly UInt128 Zero = 0;
        /// <summary>
        /// A large number of 128 bit, initialized to one
        /// </summary>
        public static readonly UInt128 One = 1;
        
        private const int Capacity = 2;
        private UInt64 element0;

        /// <inheritdoc/>
        public static int SegmentCount
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return Capacity; }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator UInt128(UInt64 value)
        {
            return new UInt128{ element0 = value };
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt128 operator ++(UInt128 value)
        {
            return value.Add(One).Item1;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt128 operator --(UInt128 value)
        {
            return value.Sub(One).Item1;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt128 operator +(UInt128 left, UInt128 right)
        {
            return left.Add(right).Item1;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt128 operator -(UInt128 left, UInt128 right)
        {
            return left.Sub(right).Item1;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt128 operator *(UInt128 left, UInt128 right)
        {
            return left.Mult(right).Item1;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt128 operator /(UInt128 left, UInt128 right)
        {
            return left.Div(right).Item1;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt128 operator %(UInt128 left, UInt128 right)
        {
            return left.Div(right).Item2;
        }
        
        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Span<UInt64> AsSpan()
        {
            return MemoryMarshal.CreateSpan(ref element0, Capacity);
        }
    }
}