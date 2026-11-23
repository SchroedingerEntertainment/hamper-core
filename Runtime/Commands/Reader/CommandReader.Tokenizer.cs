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
        protected override bool TryRead(out CommandToken token)
        {
            bool isValue = Trim();

        Process:
            switch (StreamBuffer.GetNext())
            {
                #region Quoted Literal
                case (UInt32)Scripting.Punctuation.Apostrophe:
                case (UInt32)Scripting.Punctuation.QuotationMark:
                    {
                        buffer.Clear();

                        Position--;
                        GetStringLiteral(StreamBuffer.GetNext());
                        token = CommandToken.ArgumentValue;

                        StreamBuffer.GetNext();
                    }
                    return true;
                #endregion

                case (UInt32)Scripting.Punctuation.HyphenMinus: switch (StreamBuffer.Peek())
                    {
                        case (UInt32)Scripting.Punctuation.HyphenMinus:
                            {
                                StreamBuffer.GetNext();
                                Clear();

                                #region -- PlainText
                                if (EndOfStream || IsSpaceCharacter(StreamBuffer.Peek()))
                                {
                                    StreamBuffer.GetNext();
                                    Clear();

                                    buffer.Clear();
                                    while (!EndOfStream)
                                    {
                                        buffer.Append(StreamBuffer.GetNext());
                                    }
                                    token = CommandToken.Verb;
                                    return true;
                                }
                                #endregion

                                #region --ArgumentName
                                else
                                {
                                    buffer.Clear();
                                    if (GetStringLiteral())
                                    {
                                        token = CommandToken.ArgumentName;
                                        return true;
                                    }
                                    else goto Process;
                                }
                                #endregion
                            }

                        #region -ArgumentName
                        default:
                            {
                                buffer.Clear();
                                if (GetStringLiteral())
                                {
                                    token = (options.HasFlag(CommandReaderOptions.AllowCompound) && buffer.Length > 1 ? CommandToken.CompoundArgument : CommandToken.ArgumentName);
                                    return true;
                                }
                                else goto Process;
                            }
                        #endregion
                    }

                #region /ArgumentName
                case (UInt32)Scripting.Punctuation.Solidus: if(options.FlagSet(CommandReaderOptions.AllowInvariant) && !isValue)
                    {
                        buffer.Clear();
                        if (GetStringLiteral())
                        {
                            token = CommandToken.ArgumentName;
                            return true;
                        }
                        else goto Process;
                    }
                    else goto default;
                #endregion

                #region Literal
                default:
                    {
                        buffer.Clear();

                        Position--;
                        GetStringLiteral(Scripting.WhiteSpace.Space);
                        token = (isValue) ? CommandToken.ArgumentValue : CommandToken.FloatingValue;
                    }
                    return true;
                #endregion

                #region EOF
                case Char32.Invalid:
                    {
                        token = CommandToken.EndOfStream;
                        return false;
                    }
                #endregion
            }
        }
        bool Trim()
        {
            bool isValue = false;
            for (; ; ) switch (StreamBuffer.Peek())
            {
                #region Separator
                case (UInt32)Scripting.Punctuation.EqualsSign:
                case (UInt32)Scripting.Punctuation.Colon:
                    {
                        Position++;
                        isValue = true;
                    }
                    break;
                #endregion

                #region WhiteSpace
                default:
                    {
                        if (IsSpaceCharacter(Current))
                        {
                            Position++;
                            isValue = false;
                        }
                        else return isValue;
                    }
                    break;
                #endregion
            }
        }
    }
}
