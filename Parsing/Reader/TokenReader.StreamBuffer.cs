// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;
using Soe.Unicode;

namespace Soe.Parsing
{
    #if EXPORT_HAMPER_CORE_PARSING
    public
    #else
    internal
    #endif
    abstract partial class TokenReader<TokenType, Source>
    {
        /// <summary>
        /// An internal Unicode character buffer
        /// </summary>
        public interface IStreamBuffer
        {
            /// <summary>
            /// Gets or sets the position within the current character buffer
            /// </summary>
            int Position { get; set; }

            /// <summary>
            /// Gets the amount of items in the current character buffer
            /// </summary>
            int Length { get; }

            /// <summary>
            /// Gets or sets the cached character at the specified index
            /// </summary>
            /// <param name="index">The zero-based index of the item to get or set</param>
            Char32 this[int index] { get; set; }

            /// <summary>
            /// Moves the stream pointer to next available position if possible
            /// </summary>
            /// <returns>True if the pointer was moved, false otherwise</returns>
            bool MoveNext();

            /// <summary>
            /// Returns an ahead character if available but does not consume it
            /// </summary>
            /// <param name="offset">An offset from current position</param>
            Char32 Peek(int offset);
            /// <summary>
            /// Returns the next available character but does not consume it
            /// </summary>
            Char32 Peek();

            /// <summary>
            /// Reads the next character from the input stream and advances it's position by one
            /// </summary>
            Char32 GetNext();
        }

        /// <inheritdoc/>
        int IStreamBuffer.Position
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return position; }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                if (value < 0)
                {
                    position = (position + value).Clamp(0, buffer.Count);
                }
                else position = value.Clamp(0, buffer.Count);
            }
        }
        
        /// <inheritdoc/>
        int IStreamBuffer.Length
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return buffer.Count; }
         }
        
        /// <inheritdoc/>
        Char32 IStreamBuffer.this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return buffer[index]; }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set { buffer[index] = (int)value; }
        }

        /// <inheritdoc/>
        bool IStreamBuffer.MoveNext()
        {
            if (position == buffer.Count)
            {
                if (!source.EndOfStream)
                {
                    buffer.Add(source.Read());
                }
                else return false;
            }
            Position++;
            return true;
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        Char32 IStreamBuffer.Peek(int offset)
        {
            while (position + offset >= buffer.Count)
            {
                Fetch();
            }
            return buffer[position + offset];
        }
        
        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        Char32 IStreamBuffer.Peek()
        {
            if (position == buffer.Count)
            {
                return Fetch();
            }
            else return buffer[position];
        }

        /// <summary>
        /// Caches the next character from the input stream and advances it's position by one
        /// </summary>
        protected virtual Char32 Fetch()
        {
            if (!source.EndOfStream)
            {
                int c = source.Read();
                if (char.IsHighSurrogate((char)c))
                {
                    // Resolve UTF-16 Unicode character
                    
                    int low = source.Read();
                    if (low >= 0 && char.IsLowSurrogate((char)low))
                    {
                        c = char.ConvertToUtf32((char)c, (char)low);
                    }
                    else throw new InvalidDataException();
                }
                buffer.Add(c);

                return c;
            }
            else return Char32.Invalid;
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        Char32 IStreamBuffer.GetNext()
        {
            Char32 result = StreamBuffer.Peek();
            Position++;

            return result;
        }
    }
}