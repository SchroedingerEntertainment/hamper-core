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
    abstract partial class TrackedTokenReader<TokenType, Source>
    {
        /// <summary>
        /// An internal text range vector
        /// </summary>
        public interface ITextData
        {
            /// <summary>
            /// Returns the current caret position in the stream
            /// </summary>
            TextPointer Caret { get; }

            /// <summary>
            /// Calculates the current caret position based on carret and the contents of the underlaying
            /// character buffer up to the current position
            /// </summary>
            TextPointer GetCurrentCaretPosition();
        }

        /// <inheritdoc/>
        TextPointer ITextData.GetCurrentCaretPosition()
        {
            int index = 0;
            int offset = 0;
            int line = caret.Line;
            int column = caret.Column;
            for (int length = buffer.Count; index < length; index++)
            {
                if (Position >= buffer[index])
                {
                    offset = buffer[index];
                    column = 0;
                    line++;
                }
                else break;
            }
            buffer.RemoveRange(0, index);
            return new TextPointer(line, column + (Position - offset));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected override Char32 Fetch()
        {
            Char32 c = base.Fetch();
            switch (c)
            {
                case (UInt32)Scripting.LineBreak.LineFeed:
                    {
                        buffer.Add(StreamBuffer.Length - 1);
                    }
                    break;
                case (UInt32)Scripting.LineBreak.CarriageReturn: if (Fetch() != Scripting.LineBreak.LineFeed)
                    {
                        goto case (UInt32)Scripting.LineBreak.LineFeed;
                    }
                    else break;
            }
            return c;
        }
    }
}
