// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;
using Soe.Collections.Embedded;

namespace Soe.Parsing
{
    /// <summary>
    /// Provides a token buffer to be used in a parser
    /// </summary>
    #if EXPORT_HAMPER_CORE_PARSING
    public
    #else
    internal
    #endif
    struct BuildBuffer<TokenType> : IIterable<TokenType, Iterator<TokenType>.DefaultStrategy>, ISequence<TokenType>, IDisposable
        where TokenType : struct, IComparable, IConvertible
    {
        private EmbeddedList<TokenType, PoolArray<TokenType>> buffer;

        /// <summary>
        /// Returns the amount of tokens currently preserved
        /// </summary>
        public int Count
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return buffer.Count; }
        }

        /// <summary>
        /// Creates a new token buffer
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BuildBuffer()
        {
            this.buffer = new EmbeddedList<TokenType, PoolArray<TokenType>>();
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Span<TokenType> AsSpan()
        {
            return buffer.AsSpan();
        }
        
        /// <summary>
        /// Adds the provided token to the end of this buffer
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add(TokenType value)
        {
            buffer.Add(value);
        }

        /// <summary>
        /// Removes all tokens from the buffer
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Clear()
        {
            buffer.Clear();
        }

        /// <summary>
        /// Discards an amount of tokens from the end of the underlying buffer
        /// </summary>
        /// <param name="count">
        /// The amount of tokens that should be discarded from the end of the underlying buffer
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Discard(int count)
        {
            if (count > 0)
            {
                buffer.RemoveRange(buffer.Count - count, count);
            }
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Dispose()
        {
            buffer.Dispose();
        }
        
        /// <summary>
        /// Adds the provided token to the end of this buffer
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Push(TokenType value)
        {
            Add(value);
        }
        
        /// <summary>
        /// Adds the provided token to this buffer
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Enqueue(TokenType value)
        {
            buffer.Insert(0, value);
        }

        /// <summary>
        /// Modifies current top most token to provided one
        /// </summary>
        public TokenType Replace(TokenType value)
        {
            TokenType ret = (buffer.Count > 0 ? Pop() : default(TokenType));

            Add(value);
            return ret;
        }

        /// <summary>
        /// Returns the top most token without processing it
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TokenType Peek()
        {
            if (buffer.Count != 0)
            {
                return buffer[buffer.Count - 1];
            }
            else throw new InvalidOperationException();
        }

        /// <summary>
        /// Tries to return the top most token if any
        /// </summary>
        public bool TryPeek(out TokenType value)
        {
            if (buffer.Count != 0)
            {
                value = buffer[buffer.Count - 1];
                return true;
            }
            else
            {
                value = default(TokenType);
                return false;
            }
        }

        /// <summary>
        /// Returns the top most token if any
        /// </summary>
        public TokenType Pop()
        {
            if (buffer.Count != 0)
            {
                TokenType ret = buffer[buffer.Count - 1];
                buffer.RemoveAt(buffer.Count - 1);

                return ret;
            }
            else throw new InvalidOperationException();
        }

        /// <summary>
        /// Tries to return the top most token if any
        /// </summary>
        public bool TryPop(out TokenType value)
        {
            if (buffer.Count != 0)
            {
                value = buffer[buffer.Count - 1];
                buffer.RemoveAt(buffer.Count - 1);

                return true;
            }
            else
            {
                value = default(TokenType);
                return false;
            }
        }

        /// <summary>
        /// Returns the top most token if any
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TokenType Dequeue()
        {
            return Pop();
        }

        /// <summary>
        /// Tries to return the top most token if any
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryDequeue(out TokenType value)
        {
            return TryPop(out value);
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Iterator<TokenType, Iterator<TokenType>.DefaultStrategy> GetEnumerator()
        {
            return buffer.GetEnumerator();
        }
    }
}
