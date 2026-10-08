// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;

namespace Soe.Parsing
{
    /// <summary>
    /// A line and column based selection range
    /// </summary>
    #if EXPORT_HAMPER_CORE_PARSING
    public
    #else
    internal
    #endif
    readonly struct TextRange
    {
        private readonly TextPointer start;
        /// <summary>
        /// The beginning of the text stream
        /// </summary>
        public TextPointer Start
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return start; }
        }

        private readonly TextPointer end;
        /// <summary>
        /// The end of the text stream
        /// </summary>
        public TextPointer End
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return end; }
        }

        /// <summary>
        /// Creates a new range at the given position
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TextRange(TextPointer start, TextPointer end)
        {
            this.start = start;
            this.end = end;
        }
        /// <summary>
        /// Creates a new range at the given position
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TextRange(TextPointer caret, int length)
            :this(caret, new TextPointer(caret.Line, caret.Column + length))
        { }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString()
        {
            return string.Concat(start.Line, ",", start.Column, ",", end.Line, ",", end.Column);
        }
    }
}
