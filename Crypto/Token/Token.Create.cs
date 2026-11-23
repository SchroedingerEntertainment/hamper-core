// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Buffers;
using System.Security.Cryptography;
using System.Text;

namespace Soe.Crypto
{
    #if EXPORT_HAMPER_CORE_CRYPTO
	public
	#else
	internal
	#endif
    partial struct Token
    {
	    /// <summary>
	    /// Creates a new secure token from the provided key and binary data
	    /// </summary>
	    /// <param name="key">The key to generate a secure token from</param>
	    /// <param name="data">The binary data to encode into the token</param>
	    /// <returns>The created secure token</returns>
	    /// <remarks>The key must be at least 32 bytes</remarks>
	    public static Token Create(ReadOnlySpan<byte> key, ReadOnlySpan<byte> data)
	    {
		    Token result = default;
		    
		    HMACSHA256.HashData(key, data, result.AsSpan());
		    return result;
	    }
	    /// <summary>
	    /// Creates a new secure token from a new random key and binary data
	    /// </summary>
	    /// <param name="key">The key to generate a secure token from</param>
	    /// <param name="data">The binary data to encode into the token</param>
	    /// <returns>The created secure token</returns>
	    /// <remarks>The key must be at least 32 bytes</remarks>
	    public static Token Create(Span<byte> key, ReadOnlySpan<byte> data)
	    {
		    Random.Shared.NextBytes(key);
		    return Create((ReadOnlySpan<byte>)key, data);
	    }
	    
	    /// <summary>
	    /// Creates a new secure token from the provided key and data
	    /// </summary>
	    /// <param name="key">The key to generate a secure token from</param>
	    /// <param name="data">The data to encode into the token</param>
	    /// <returns>The created secure token</returns>
	    /// <remarks>The key must be at least 32 bytes</remarks>
	    public static Token CreateFrom(ReadOnlySpan<byte> key, string data)
	    {
		    Span<byte> hash = stackalloc byte[32];
		    byte[] buffer = ArrayPool<byte>.Shared.Rent(Encoding.UTF8.GetMaxByteCount(data.Length));
		    try
		    {
				Span<byte> binaryData = buffer.AsSpan(0, Encoding.UTF8.GetBytes(data, buffer));
				SHA256.HashData(binaryData, hash);
				
				return Create(key, hash);
		    }
		    finally
		    {
			    ArrayPool<byte>.Shared.Return(buffer);
		    }
	    }
	    /// <summary>
	    /// Creates a new secure token from a new random key and binary data
	    /// </summary>
	    /// <param name="key">The key to generate a secure token from</param>
	    /// <param name="data">The binary data to encode into the token</param>
	    /// <returns>The created secure token</returns>
	    /// <remarks>The key must be at least 32 bytes</remarks>
	    public static Token CreateFrom(Span<byte> key, string data)
	    {
		    Random.Shared.NextBytes(key);
		    return CreateFrom((ReadOnlySpan<byte>)key, data);
	    }
    }
}