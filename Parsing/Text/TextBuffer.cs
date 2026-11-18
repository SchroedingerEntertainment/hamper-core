// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;
using System.Text;
using Soe.Unicode;

namespace Soe.Parsing
{
    /// <summary>
    /// Represents a mutable range of characters
    /// </summary>
    /// <param name="buffer">The buffer instance used internally</param>
    [method: MethodImpl(MethodImplOptions.AggressiveInlining)]
    #if EXPORT_HAMPER_CORE_PARSING
    public
    #else
    internal
    #endif
    struct TextBuffer(StringBuilder buffer) : ITextSource
    {
        /// <summary>
        /// Gets the internal buffer instance
        /// </summary>
        public StringBuilder Buffer
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return buffer; }
        }

        private int offset;
        /// <summary>
        /// Gets or sets the buffer offset
        /// </summary>
        public int Offset
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return offset; }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set { offset = value; }
        }
        
        /// <summary>
        /// Gets or sets the length of this builder.
        /// </summary>
        public int Length
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return buffer.Length; }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set { buffer.Length = value; }
        }

        /// <summary>
        /// Gets or sets the UTF-16 character at the given index
        /// </summary>
        /// <param name="index">The index to get the character at</param>
        /// <remarks>The index can be negative to access preserved chars</remarks>
        public char this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return buffer[index + offset]; }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set{ buffer[index + offset] = value; }
        }
        
        /// <summary>
        /// Appends the string representation of a specified <see cref="Char"/> object to this instance
        /// </summary>
        /// <param name="value">The UTF-16-encoded code unit to append</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Append(char value)
        {
            buffer.Append(value);
        }
        /// <summary>
        /// Appends the string representation of a specified <see cref="Char32"/> object to this instance
        /// </summary>
        /// <param name="value">The UTF-32-encoded code unit to append</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Append(Char32 value)
        {
            buffer.AppendChar(value);
        }
        /// <summary>
        /// Appends the string representation to this instance
        /// </summary>
        /// <param name="value">The string to append</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Append(string value)
        {
            buffer.Append(value);
        }
        /// <summary>
        /// Appends a specified number of copies of the string representation of a Unicode character to this instance
        /// </summary>
        /// <param name="value">The character to append</param>
        /// <param name="repeatCount">The number of times to append</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Append(char value, int repeatCount)
        {
            buffer.Append(value, repeatCount);
        }
        /// <summary>
        /// Appends a specified number of copies of the string representation of a Unicode character to this instance
        /// </summary>
        /// <param name="value">The character to append</param>
        /// <param name="repeatCount">The number of times to append</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Append(Char32 value, int repeatCount)
        {
            for (int i = 0; i < repeatCount; i++)
            {
                buffer.AppendChar(value);
            }
        }

        /// <summary>
        /// Removes all characters from the current buffer
        /// </summary>
        /// <param name="reset">True if the buffer should be cleared including the offset, false otherwise</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Clear(bool reset = false)
        {
            if (reset)
            {
                buffer.Clear();
                offset = 0;
            }
            else Length = offset;
        }

        /// <summary>
        /// Sets the buffer offset to the current length
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Preserve()
        {
            offset = Length;
        }
        
        /// <summary>
        /// Removes the specified range of characters from this instance
        /// </summary>
        /// <param name="startIndex">The zero-based position in this instance where removal begins</param>
        /// <param name="length">The number of characters to remove</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Remove(int startIndex, int length)
        {
            buffer.Remove(startIndex + offset, length);
        }

        /// <summary>
        /// Replaces all occurrences of a specified character in this instance with another specified character
        /// </summary>
        /// <param name="oldChar">The character to replace</param>
        /// <param name="newChar">The character that replaces <paramref name="oldChar"/></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Replace(char oldChar, char newChar)
        {
            buffer.Replace(oldChar, newChar, offset, Length - offset);
        }
        
        /// <summary>
        /// Converts the value of the internal buffer to a <see cref="String"/>
        /// </summary>
        /// <returns>A string whose value is the same as this instance</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString()
        {
            return buffer.ToString(offset, Length - offset);
        }

        /// <summary>
        /// Converts the value of the internal buffer to a <see cref="String"/>
        /// </summary>
        /// <param name="startIndex">The starting position of the substring in this instance</param>
        /// <param name="length">The length of the substring.</param>
        /// <returns>A string whose value is the same as the specified substring of this instance</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string ToString(int startIndex, int length)
        {
            return buffer.ToString(startIndex + offset, length - offset);
        }
        
        #region ICharacterSource Members
        /// <inheritdoc/>
        char ITextSource.this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return buffer[index]; }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set{ buffer[index] = value; }
        }
        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void ITextSource.CopyTo(int sourceIndex, Span<char> destination, int count)
        {
            buffer.CopyTo(sourceIndex, destination, count);
        }
        /// <inheritdoc/>
        bool ITextSource.Equals(ReadOnlySpan<char> value, int startIndex, int length)
        {
            if (startIndex < 0 || length < 0 || startIndex > Length - length)
            {
                return false;
            }
            else if (value.Length != length)
            {
                return false;
            }
            else
            {
                for (int i = 0; i < length; i++)
                {
                    if (buffer[startIndex + i] != value[i])
                        return false;
                }
                return true;
            }
        }
        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        string ITextSource.ToString(int startIndex, int length)
        {
            return buffer.ToString(startIndex, length);
        }
        #endregion
    }
}