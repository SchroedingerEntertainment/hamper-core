// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Diagnostics.Contracts;

namespace System.Text
{
    /// <summary>
    /// Represents a mutable slice of a character buffer
    /// </summary>
    #if EXPORT_HAMPER_CORE_SHARP
    public
    #else
    internal
    #endif
    interface ITextSource
    {
        /// <summary>
        /// Gets or sets the UTF-16 character at the given index
        /// </summary>
        /// <param name="index">The index to get the character at</param>
        char this[int index]
        {
            [Pure]
            get;
            set;
        }

        // ReSharper disable PureAttributeOnVoidMethod
        
        /// <summary>
        /// Copies the characters from this slice to a destination <see cref="Char"/> span
        /// </summary>
        /// <param name="sourceIndex">The starting position in this instance where characters will be copied from.
        /// The index is zero-based.</param>
        /// <param name="destination">The writable span where characters will be copied</param>
        /// <param name="count">The number of characters to be copied</param>
        [Pure]
        void CopyTo(int sourceIndex, Span<char> destination, int count);
        
        // ReSharper restore PureAttributeOnVoidMethod
        
        /// <summary>
        /// Returns a value indicating whether the characters in this instance are equal to the characters
        /// in a specified read-only character span
        /// </summary>
        /// <param name="value">The character span to compare with the current instance</param>
        /// <param name="startIndex">The starting position of the substring in this instance</param>
        /// <param name="length">The length of the substring</param>
        /// <returns>True if the characters in this slice and span are the same, false otherwise</returns>
        [Pure]
        bool Equals(ReadOnlySpan<char> value, int startIndex, int length);
        
        /// <summary>
        /// Converts the value of the internal buffer to a <see cref="String"/>
        /// </summary>
        /// <param name="startIndex">The starting position of the substring in this instance</param>
        /// <param name="length">The length of the substring</param>
        /// <returns>A string whose value is the same as the specified substring of this instance</returns>
        [Pure]
        string ToString(int startIndex, int length);
    }
}