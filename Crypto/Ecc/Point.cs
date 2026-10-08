// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;
using Soe.Utility;

namespace Soe.Crypto
{
    /// <summary>
    /// Represents a certain point on an elliptic curve
    /// </summary>
    #if EXPORT_HAMPER_CORE_CRYPTO
	public
	#else
	internal
	#endif
    readonly struct Point : ICurvePoint<Point>
    {
        private readonly int size;
        /// <inheritdoc/>
        public int SegmentCount
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return size; }
        }
        
        private readonly UInt64[] x;
        /// <inheritdoc/>
        public Span<UInt64> X
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return x; }
        }

        private readonly UInt64[] y;
        /// <inheritdoc/>
        public Span<UInt64> Y
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return y; }
        }

        /// <inheritdoc/>
        public bool IsValid
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return (size != 0); }
        }

        /// <summary>
        /// Creates a new curve point
        /// </summary>
        /// <param name="size">Size of 64 bit segments per value</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Point(int size)
        {
            this.size = size;
            this.x = new UInt64[size];
            this.y = new UInt64[size];
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point Create(int size)
        {
            return new Point(size);
        }
    }
    
    /// <summary>
    /// Represents a certain point on an elliptic curve over 128 bits
    /// </summary>
    #if EXPORT_HAMPER_CORE_CRYPTO
	public
	#else
	internal
	#endif
    struct Point128 : ICurvePoint<Point128>
    {
        /// <inheritdoc/>
        public int SegmentCount
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return 2; }
        }
        
        private Utility.UInt128 x;
        /// <inheritdoc/>
        public Span<UInt64> X
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return x.AsSpan(); }
        }

        private Utility.UInt128 y;
        /// <inheritdoc/>
        public Span<UInt64> Y
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return y.AsSpan(); }
        }
        
        /// <inheritdoc/>
        public bool IsValid
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return !(x.IsZero() && y.IsZero()); }
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point128 Create(int size)
        {
            return default;
        }
    }
    
    /// <summary>
    /// Represents a certain point on an elliptic curve over 256 bits
    /// </summary>
    #if EXPORT_HAMPER_CORE_CRYPTO
	public
	#else
	internal
	#endif
    struct Point256 : ICurvePoint<Point256>
    {
        /// <inheritdoc/>
        public int SegmentCount
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return 2; }
        }
        
        private Utility.UInt256 x;
        /// <inheritdoc/>
        public Span<UInt64> X
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return x.AsSpan(); }
        }

        private Utility.UInt256 y;
        /// <inheritdoc/>
        public Span<UInt64> Y
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return y.AsSpan(); }
        }
        
        /// <inheritdoc/>
        public bool IsValid
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return !(x.IsZero() && y.IsZero()); }
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point256 Create(int size)
        {
            return default;
        }
    }
}
