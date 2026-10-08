// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;
using System.Text;
using Soe.Collections.Embedded;
using Soe.Unicode;

namespace Soe.Parsing
{
    /// <summary>
    /// A stream reader specialized to process text into tokens of certain grammar
    /// </summary>
    #if EXPORT_HAMPER_CORE_PARSING
    public
    #else
    internal
    #endif
    abstract partial class TokenReader<TokenType, Source> : TokenReader<TokenType, Source>.IStreamBuffer, IReadOnlyIterable<int, ReadOnlyIterator<int>.DefaultStrategy>, IReadOnlySequence<int>, IDisposable 
        where TokenType : struct, IComparable, IConvertible
        where Source : struct, IStreamSource
    {
        private EmbeddedList<int, PoolArray<int>> buffer;
        
        private Source source;
        /// <summary>
        /// Gets the current source parsed
        /// </summary>
        public Source StreamSource
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return source; }
        }

        private int position;
        /// <summary>
        /// Gets the position within the current character buffer
        /// </summary>
        public int Position
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return position; }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            protected set { StreamBuffer.Position = value; }
        }

        /// <summary>
        /// Gets the current character encoding used to parse tokens
        /// </summary>
        public Encoding Encoding
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return source.CurrentEncoding; }
        }

        /// <summary>
        /// Gets a value that indicates whether the current stream position is at the
        /// end of the stream and there are no tokens left to process
        /// </summary>
        public bool EndOfStream
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return source.EndOfStream && (position == buffer.Count); }
        }

        /// <summary>
        /// The character considered as currently pointed to
        /// </summary>
        protected Char32 Current
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                if (position == buffer.Count)
                {
                    return Char32.Invalid;
                }
                else return buffer[position];
            }
        }

        /// <summary>
        /// Returns the underlying Unicode buffer
        /// </summary>
        protected IStreamBuffer StreamBuffer
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return this; }
        }

        /// <summary>
        /// Creates a new token reader from the provided stream
        /// </summary>
        /// <param name="source">The streaming source used to process the data</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TokenReader(Source source)
        {
            this.buffer = new EmbeddedList<int, PoolArray<int>>();
            this.source = source;
        }

        // ReSharper disable ParameterHidesMember
        
        /// <summary>
        /// Reinitialize the token reader from the provided stream
        /// </summary>
        /// <param name="source">The streaming source used to process the data</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Initialize(Source source)
        {
            this.source = source;
            this.buffer.Clear();
            this.position = 0;
        }
        
        // ReSharper restore ParameterHidesMember
        
        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlySpan<int> AsReadOnlySpan()
        {
            return buffer.AsSpan();
        }
        
        /// <summary>
        /// Trims the internal buffer up to the current position
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void Clear()
        {
            buffer.RemoveRange(0, position);
            position = 0;
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Dispose()
        {
            buffer.Dispose();
            source.Dispose();
        }
        
        /// <summary>
        /// Reads the next token from the input stream and advances it's position by one
        /// </summary>
        /// <returns>The token type read from the stream</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected virtual TokenType Read()
        {
            if (TryRead(out TokenType result))
            {
                Clear();
            }
            return result;
        }

        /// <summary>
        /// Reads the next token from the input stream if possible
        /// </summary>
        /// <param name="token">The token type read from the stream</param>
        /// <returns>True if a token was successfully read, false otherwise</returns>
        protected abstract bool TryRead(out TokenType token);

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlyIterator<int, ReadOnlyIterator<int>.DefaultStrategy> GetEnumerator()
        {
            return new ReadOnlyIterator<int, ReadOnlyIterator<int>.DefaultStrategy>(AsReadOnlySpan());
        }
    }
}
