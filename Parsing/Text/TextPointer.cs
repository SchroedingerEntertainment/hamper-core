// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;

namespace Soe.Parsing
{
    /// <summary>
    /// A line and column based text indexer
    /// </summary>
    [method: MethodImpl(MethodImplOptions.AggressiveInlining)]
    #if EXPORT_HAMPER_CORE_PARSING
    public
    #else
    internal
    #endif
    readonly struct TextPointer(int line, int column)
    {
        /// <summary>
        /// The initial index in a text stream
        /// </summary>
        public static readonly TextPointer Initial = new(1, 1);

        /// <summary>
        /// Current line of the text stream
        /// </summary>
        public int Line
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return line; }
        }
        
        /// <summary>
        /// Current column of the text stream
        /// </summary>
        public int Column
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return column; }
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString()
        {
            return string.Concat(line, ":", column);
        }
    }
}
