// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;

namespace Soe.Crypto
{
    #if EXPORT_HAMPER_CORE_CRYPTO
	public
	#else
	internal
	#endif
    static partial class SpanExtension
    {
	    // ReSharper disable InvalidXmlDocComment
	    
	    /// <summary>
	    /// Creates a new secure token from the provided key
	    /// </summary>
	    /// <param name="key">The key to generate a secure token from</param>
	    /// <returns></returns>
	    [MethodImpl(MethodImplOptions.AggressiveInlining)]
	    public static Token CreateToken(this ReadOnlySpan<byte> data, ReadOnlySpan<byte> key)
	    {
		    return Token.Create(key, data);
	    }
	    /// <summary>
	    /// Creates a new secure token from the provided key
	    /// </summary>
	    /// <param name="key">The key to generate a secure token from</param>
	    /// <returns></returns>
	    [MethodImpl(MethodImplOptions.AggressiveInlining)]
	    public static Token CreateToken(this Span<byte> data, ReadOnlySpan<byte> key)
	    {
		    return Token.Create(key, data);
	    }
	    
	    // ReSharper restore InvalidXmlDocComment
    }
}