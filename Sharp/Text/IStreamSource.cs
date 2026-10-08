// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

namespace System.Text
{
    /// <summary>
    /// Implements a text reader that reads characters from the underlying source in a particular encoding
    /// </summary>
    #if EXPORT_HAMPER_CORE_SHARP
    public
    #else
    internal
    #endif
    interface IStreamSource : IDisposable
    {
        /// <summary>
        /// Gets the character encoding set 
        /// </summary>
        Encoding CurrentEncoding
        {
            get;
        }

        /// <summary>
        /// Gets or sets the position within the current stream if supported
        /// </summary>
        /// <exception cref="NotSupportedException">Thrown when the stream does not support seeking</exception>
        public long Position
        {
            get;
            set;
        }

        /// <summary>
        /// Gets a value that indicates whether the current stream position is at the
        /// end of the stream
        /// </summary>
        bool EndOfStream
        {
            get;
        }

        /// <summary>
        /// Returns the next available character but does not consume it
        /// </summary>
        /// <returns>
        /// An integer representing the next character to be read, or -1 if there are no characters 
        /// to be read or if the stream does not support seeking
        /// </returns>
        int Peek();

        /// <summary>
        /// Reads the next character from the input stream and advances the character position by one character
        /// </summary>
        /// <returns>
        /// The next character from the input stream represented as an Int32 object, or -1 if no
        /// more characters are available
        /// </returns>
        int Read();
    }
}
