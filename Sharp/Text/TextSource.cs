// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;

namespace System.Text
{
    /// <summary>
    /// Represents a mutable slice of a character buffer
    /// </summary>
    /// <param name="source">An object instance implementing <see cref="ITextSource"/></param>
    /// <param name="startIndex start index of the slice of this source</param>
    /// <param name="length">The length of the slice</param>
    [method: MethodImpl(MethodImplOptions.AggressiveInlining)]
    #if EXPORT_HAMPER_CORE_SHARP
    public
    #else
    internal
    #endif
    readonly ref struct TextSource<Source>(ref Source source, int startIndex, int length)
        where Source : ITextSource
    {
        private readonly ref Source source = ref source;

        /// <summary>
        /// Gets the length of this slice
        /// </summary>
        public int Length
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return length; }
        }
        
        /// <summary>
        /// Gets or sets the UTF-16 character at the given index
        /// </summary>
        /// <param name="index">The index to get the character at</param>
        /// <exception cref="IndexOutOfRangeException">Thrown when the index is smaller or larger than this slice</exception>
        public char this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                if(index >= 0 && index < Length)
                {
                    return source[startIndex + index];
                }
                else throw new IndexOutOfRangeException();
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                if(index >= 0 && index < Length)
                {
                    source[startIndex + index] = value;
                }
                else throw new IndexOutOfRangeException();
            }
        }

        /// <summary>
        /// Returns a value indicating whether the characters in this instance are equal to the characters
        /// in a specified read-only character span
        /// </summary>
        /// <param name="value">The character span to compare with the current instance</param>
        /// <returns>True if the characters in this slice and span are the same, false otherwise</returns>
        public bool Equals(ReadOnlySpan<char> value)
        {
            return source.Equals(value, startIndex, length);
        }

        /// <summary>
        /// Copies the characters from this slice to a destination <see cref="Char"/> span
        /// </summary>
        /// <param name="destination">The writable span where characters will be copied</param>
        /// <param name="count">The number of characters to be copied</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void CopyTo(Span<char> destination, int count)
        {
            source.CopyTo(startIndex, destination, count);
        }

        /// <summary>
        /// Converts the value of the internal slice to a <see cref="String"/>
        /// </summary>
        /// <returns>A string whose value is the same as the specified substring of this instance</returns>
        public override string ToString()
        {
            return source.ToString(startIndex, length);
        }
    }
}