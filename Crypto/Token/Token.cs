// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace Soe.Crypto
{
    /// <summary>
    /// Represents a secure token
    /// </summary>
    [InlineArray(32)]
    #if EXPORT_HAMPER_CORE_CRYPTO
	public
	#else
	internal
	#endif
    partial struct Token : IEquatable<Token>
    {
        private const int Capacity = 32;
        private byte element0;
        
        /// <summary>
        /// Gets the underlying binary representation of this token
        /// </summary>
        /// <returns>The underlying binary representation of this token</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Span<byte> AsSpan()
        {
	        return MemoryMarshal.CreateSpan(ref element0, Capacity);
        }
        
        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Token other)
        {
	        return CryptographicOperations.FixedTimeEquals(AsSpan(), other.AsSpan());
        }
        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object? obj)
        {
	        return (obj is Token other && Equals(other));
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode()
        {
	        return AsSpan().GetHashCode();
        }
    }
}