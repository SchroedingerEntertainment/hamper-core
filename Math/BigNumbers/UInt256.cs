// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Soe.Utility
{
    /// <summary>
    /// Represents a large number of 256-bit
    /// </summary>
    [InlineArray(4)]
    #if EXPORT_HAMPER_CORE_MATH
    public
    #else
    internal
    #endif
    struct UInt256 : IBigNumber
    {
        /// <summary>
        /// A large number of 256 bit, initialized to zero
        /// </summary>
        public static readonly UInt256 Zero = 0;
        /// <summary>
        /// A large number of 256 bit, initialized to one
        /// </summary>
        public static readonly UInt256 One = 1;
        
        private const int Capacity = 4;
        private UInt64 element0;

        /// <inheritdoc/>
        public static int SegmentCount
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return Capacity; }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator UInt256(UInt64 value)
        {
            return new UInt256{ element0 = value };
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt256 operator ++(UInt256 value)
        {
            return value.Add(One).Item1;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt256 operator --(UInt256 value)
        {
            return value.Sub(One).Item1;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt256 operator +(UInt256 left, UInt256 right)
        {
            return left.Add(right).Item1;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt256 operator -(UInt256 left, UInt256 right)
        {
            return left.Sub(right).Item1;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt256 operator *(UInt256 left, UInt256 right)
        {
            return left.Mult(right).Item1;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt256 operator /(UInt256 left, UInt256 right)
        {
            return left.Div(right).Item1;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt256 operator %(UInt256 left, UInt256 right)
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