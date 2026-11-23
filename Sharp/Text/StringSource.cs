// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;

namespace System.Text
{
    /// <summary>
    /// A wrapper class embedding a string into a streamable source
    /// </summary>
    #if EXPORT_HAMPER_CORE_PARSING
    public
    #else
    internal
    #endif
    struct StringSource : IStreamSource
    {
        private readonly string? source;
        private readonly int length;
        
        int position;
        /// <inheritdoc/>
        public long Position
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return position; }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set { position = (int)value.Clamp(0, length); }
        }

        /// <inheritdoc/>
        public Encoding CurrentEncoding
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return Encoding.Unicode; }
        }
        
        /// <inheritdoc/>
        public bool EndOfStream
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return (position >= length); }
        }

        /// <summary>
        /// Initializes a new wrapper that reads from the specified string
        /// </summary>
        /// <param name="identifier">The string to which the wrapper is initialized from</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public StringSource(string? identifier) 
        {
            this.source = identifier;
            this.length = (identifier?.Length ?? 0);
        }
        
        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Dispose()
        { }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int Peek() 
        {
            if (!EndOfStream)
            {
                return source![position];
            }
            else return -1;
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int Read()
        {
            if (!EndOfStream)
            {
                return source![position++];
            }
            else return -1;
        }
    }
}
