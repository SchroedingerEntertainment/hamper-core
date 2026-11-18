// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using Soe.Unicode;

namespace Soe.Runtime
{
    #if EXPORT_HAMPER_CORE_RUNTIME_COMMANDS
    public
    #else
    internal
    #endif
    partial class CommandReader<Source>
    {
        /// <summary>
        /// StringLiteral = ~('=' | ':' | '\'' | '"' | WHITESPACE)+;
        /// </summary>
        bool GetStringLiteral()
        {
            int count; for (count = 0; !EndOfStream; count++)
            {
                switch (StreamBuffer.Peek())
                {
                    case (UInt32)Scripting.Punctuation.EqualsSign:
                    case (UInt32)Scripting.Punctuation.Colon:
                    case (UInt32)Scripting.Punctuation.Apostrophe:
                    case (UInt32)Scripting.Punctuation.QuotationMark:
                    EndOfSequence:
                        {
                            return (count > 0);
                        }
                    default: if(Current.IsWhiteSpace() || Current.IsLineBreak())
                        {
                            goto EndOfSequence;
                        }
                        else buffer.Append(StreamBuffer.GetNext());
                        break;
                }
            }
            return (count > 0);
        }
        bool GetStringLiteral(Char32 delimiter)
        {
            int count; for (count = 0; !EndOfStream; count++)
            {
                Char32 c = StreamBuffer.Peek();
                if (c != delimiter)
                {
                    if (delimiter == Scripting.WhiteSpace.Space && (c.IsWhiteSpace() || c.IsLineBreak()))
                    {
                        return (count > 0);
                    }
                    else buffer.Append(StreamBuffer.GetNext());
                }
                else return (count > 0);
            }
            return false;
        }

        private static bool IsSpaceCharacter(Char32 c)
        {
            return c.IsWhiteSpace() || c.IsLineBreak();
        }
    }
}
