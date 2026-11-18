// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;
using System.Text;

namespace Soe.Runtime
{
    /// <summary>
    /// A wrapper class embedding a parameter array into a streamable source
    /// </summary>
    #if EXPORT_HAMPER_CORE_RUNTIME_COMMANDS
    public
    #else
    internal
    #endif
    struct ParamSource : IStreamSource
    {
        private const char Whitespace = ' ';
        
        private readonly string[] source;
        private readonly int length;
        
        int position;
        /// <inheritdoc/>
        public long Position
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return position; }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set { position = (int)value.Clamp(0, (length * source.Length)); }
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
            get { return (position >= (length * source.Length)); }
        }

        /// <summary>
        /// Initializes a new wrapper that reads from the specified parameter array
        /// </summary>
        /// <param name="args">The string array to which the wrapper is initialized from</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ParamSource(string[] args) 
        {
            this.source = args;
            for (int i = 0, len = args.Length; i < len; i++)
            {
                this.length = Math.Max(length, args[i].Length + 1);
            }
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
                ref string block = ref source[position / length];
                int index = position % length;

                if (index < block.Length)
                {
                    return block[index];
                }
                else return Whitespace;
            }
            else return -1;
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int Read()
        {
            if (!EndOfStream)
            {
                ref string block = ref source[position / length];
                int index = position % length;

                if (index < block.Length)
                {
                    position++;
                    return block[index];
                }
                else
                {
                    position += length - index;
                    return Whitespace;
                }
            }
            else return -1;
        }
    }
}
