// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;
using Soe.Unicode;

namespace Soe.Json
{
    #if EXPORT_HAMPER_CORE_JSON
    public
    #else
    internal
    #endif
    partial class JsonReader<Source>
    {
        enum ComparisonState : byte
        {
            Failure = Byte.MaxValue,
            Next = Failure - 1
        }

        /// <summary>
        /// StringLiteral = '\"' ('\\"' | ~'\"')* '\"';
        /// </summary>
        bool GetStringLiteral()
        {
            for (; !EndOfStream;)
            {
                switch (StreamBuffer.Peek())
                {
                    #region Escape Sequence
                    case (UInt32)Scripting.Punctuation.ReverseSolidus:
                        {
                            Char32 c = StreamBuffer.Peek(1);
                            switch (c)
                            {
                                #region QuotationMark
                                case (UInt32)Scripting.Punctuation.QuotationMark:
                                    {
                                        StreamBuffer.MoveNext();
                                        buffer.Append(StreamBuffer.GetNext());
                                    }
                                    break;
                                #endregion

                                #region ReverseSolidus
                                case (UInt32)Scripting.Punctuation.ReverseSolidus:
                                    {
                                        StreamBuffer.MoveNext();
                                        buffer.Append(StreamBuffer.GetNext());
                                    }
                                    break;
                                #endregion

                                #region UCN
                                case (UInt32)Scripting.LowercaseLetter.U:
                                    {
                                        StreamBuffer.MoveNext();
                                        StreamBuffer.MoveNext();
                                        if (!GetUniversalCharacterName())
                                            return false;
                                    }
                                    break;
                                #endregion

                                #region Control
                                default:
                                    {
                                        switch (c)
                                        {
                                            case (UInt32)Scripting.LowercaseLetter.B:
                                            case (UInt32)Scripting.LowercaseLetter.T:
                                            case (UInt32)Scripting.LowercaseLetter.N:
                                            case (UInt32)Scripting.LowercaseLetter.F:
                                            case (UInt32)Scripting.LowercaseLetter.R:
                                                {
                                                    StreamBuffer.MoveNext();
                                                    StreamBuffer.MoveNext();
                                                    switch (c)
                                                    {
                                                        #region Backspace
                                                        case (UInt32)Scripting.LowercaseLetter.B:
                                                            {
                                                                buffer.Append(BasicLatin.Control.Backspace);
                                                            }
                                                            break;
                                                        #endregion

                                                        #region CharacterTabulation
                                                        case (UInt32)Scripting.LowercaseLetter.T:
                                                            {
                                                                buffer.Append(Scripting.WhiteSpace.CharacterTabulation);
                                                            }
                                                            break;
                                                        #endregion

                                                        #region LineFeed
                                                        case (UInt32)Scripting.LowercaseLetter.N:
                                                            {
                                                                buffer.Append(Scripting.LineBreak.LineFeed);
                                                            }
                                                            break;
                                                        #endregion

                                                        #region FormFeed
                                                        case (UInt32)Scripting.LowercaseLetter.F:
                                                            {
                                                                buffer.Append(BasicLatin.Control.FormFeed);
                                                            }
                                                            break;
                                                        #endregion

                                                        #region CarriageReturn
                                                        case (UInt32)Scripting.LowercaseLetter.R:
                                                            {
                                                                buffer.Append(Scripting.LineBreak.CarriageReturn);
                                                            }
                                                            break;
                                                        #endregion
                                                    }
                                                }
                                                break;
                                            default: return false;
                                        }
                                    }
                                    break;
                                #endregion
                            }
                        }
                        break;
                    #endregion

                    #region QuotationMark
                    case (UInt32)Scripting.Punctuation.QuotationMark:
                        {
                            StreamBuffer.MoveNext();
                        }
                        return true;
                    #endregion

                    #region Control
                    default:
                        {
                            if (Current.IsControl())
                            {
                                return false;
                            }
                            else buffer.Append(StreamBuffer.GetNext());
                        }
                        break;
                    #endregion
                }
            }
            return false;
        }

        /// <summary>
        /// UniversalCharacterName = '\u' HEX_DIGIT HEX_DIGIT HEX_DIGIT HEX_DIGIT;
        /// </summary>
        bool GetUniversalCharacterName()
        {
            Char32 result = 0;
            for (int i = 0; i < 4; i++)
            {
                Char32 c = StreamBuffer.Peek();
                switch (c)
                {
                    case (UInt32)Scripting.Digit.Zero:
                    case (UInt32)Scripting.Digit.One:
                    case (UInt32)Scripting.Digit.Two:
                    case (UInt32)Scripting.Digit.Three:
                    case (UInt32)Scripting.Digit.Four:
                    case (UInt32)Scripting.Digit.Five:
                    case (UInt32)Scripting.Digit.Six:
                    case (UInt32)Scripting.Digit.Seven:
                    case (UInt32)Scripting.Digit.Eight:
                    case (UInt32)Scripting.Digit.Nine:
                        {
                            result = (UInt32)((result << 4) + (c - Scripting.Digit.Zero));
                        }
                        break;
                    case (UInt32)Scripting.UppercaseLetter.A:
                    case (UInt32)Scripting.UppercaseLetter.B:
                    case (UInt32)Scripting.UppercaseLetter.C:
                    case (UInt32)Scripting.UppercaseLetter.D:
                    case (UInt32)Scripting.UppercaseLetter.E:
                    case (UInt32)Scripting.UppercaseLetter.F:
                        {
                            result += (UInt32)((result << 4) + (c - Scripting.UppercaseLetter.A));
                        }
                        break;
                    case (UInt32)Scripting.LowercaseLetter.A:
                    case (UInt32)Scripting.LowercaseLetter.B:
                    case (UInt32)Scripting.LowercaseLetter.C:
                    case (UInt32)Scripting.LowercaseLetter.D:
                    case (UInt32)Scripting.LowercaseLetter.E:
                    case (UInt32)Scripting.LowercaseLetter.F:
                        {
                            result += (UInt32)((result << 4) + (c - Scripting.LowercaseLetter.A));
                        }
                        break;
                    default: return false;
                }
                Position++;
            }
            buffer.Append(result);
            return true;
        }

        /// <summary>
        /// Numeric = (('-')?['0', '9']) ((('E' | 'e') ('+' | '-')) | ['0','9'] '.')*
        /// </summary>
        bool GetNumeric()
        {
            buffer.Append(StreamBuffer.GetNext());
            for (bool onlyDigitsAllowed = false; !EndOfStream;)
            {
                switch (StreamBuffer.Peek())
                {
                    #region DecimalPunctuation
                    case (UInt32)Scripting.Punctuation.Dot:
                        {
                            if (!onlyDigitsAllowed && IsNumericChar(StreamBuffer.Peek(1)))
                            {
                                buffer.Append(StreamBuffer.GetNext());
                                onlyDigitsAllowed = true;
                            }
                            else return false;
                        }
                        break;
                    #endregion

                    #region Exponent
                    case (UInt32)Scripting.LowercaseLetter.E:
                    case (UInt32)Scripting.UppercaseLetter.E:
                        {
                            Char32 c = StreamBuffer.Peek(1);
                            if (!onlyDigitsAllowed && (IsNumericChar(c) || c == Scripting.Punctuation.PlusSign || c == Scripting.Punctuation.HyphenMinus))
                            {
                                if (c == Scripting.Punctuation.PlusSign || c == Scripting.Punctuation.HyphenMinus)
                                {
                                    if (IsNumericChar(StreamBuffer.Peek(2)))
                                    {
                                        buffer.Append(StreamBuffer.GetNext());
                                        buffer.Append(StreamBuffer.GetNext());
                                    }
                                    else return false;
                                }
                                else buffer.Append(StreamBuffer.GetNext());
                                onlyDigitsAllowed = true;
                            }
                            else return false;
                        }
                        break;
                    #endregion

                    #region Digits
                    default:
                        {
                            if (!IsNumericChar(Current))
                            {
                                return true;
                            }
                            else buffer.Append(StreamBuffer.GetNext());
                        }
                        break;
                    #endregion
                }
            }
            return true;
        }

        /// <summary>
        /// TrueConstant = 'true';
        /// </summary>
        bool GetTrueConstant()
        {
            ComparisonState compState = 0;
            for (; ; )
            {
                switch (compState)
                {
                    case (ComparisonState)0:
                        {
                            if (StreamBuffer.Peek() == Scripting.LowercaseLetter.R) goto case ComparisonState.Next;
                            else goto case ComparisonState.Failure;
                        }
                    case (ComparisonState)1:
                        {
                            if (StreamBuffer.Peek() == Scripting.LowercaseLetter.U) goto case ComparisonState.Next;
                            else goto case ComparisonState.Failure;
                        }
                    case (ComparisonState)2:
                        {
                            if (StreamBuffer.Peek() == Scripting.LowercaseLetter.E) goto case ComparisonState.Next;
                            else goto case ComparisonState.Failure;
                        }
                    case (ComparisonState)3:
                        {
                            if (!IsIdentifierChar(StreamBuffer.Peek())) return true;
                            else goto case ComparisonState.Failure;
                        }
                    default:
                    case ComparisonState.Failure: return false;
                    case ComparisonState.Next: compState++; break;
                }
                Position++;
            }
        }

        /// <summary>
        /// FalseConstant = 'false';
        /// </summary>
        bool GetFalseConstant()
        {
            ComparisonState compState = 0;
            for (; ; )
            {
                switch (compState)
                {
                    case (ComparisonState)0:
                        {
                            if (StreamBuffer.Peek() == Scripting.LowercaseLetter.A) goto case ComparisonState.Next;
                            else goto case ComparisonState.Failure;
                        }
                    case (ComparisonState)1:
                        {
                            if (StreamBuffer.Peek() == Scripting.LowercaseLetter.L) goto case ComparisonState.Next;
                            else goto case ComparisonState.Failure;
                        }
                    case (ComparisonState)2:
                        {
                            if (StreamBuffer.Peek() == Scripting.LowercaseLetter.S) goto case ComparisonState.Next;
                            else goto case ComparisonState.Failure;
                        }
                    case (ComparisonState)3:
                        {
                            if (StreamBuffer.Peek() == Scripting.LowercaseLetter.E) goto case ComparisonState.Next;
                            else goto case ComparisonState.Failure;
                        }
                    case (ComparisonState)4:
                        {
                            if (!IsIdentifierChar(StreamBuffer.Peek())) return true;
                            else goto case ComparisonState.Failure;
                        }
                    default:
                    case ComparisonState.Failure: return false;
                    case ComparisonState.Next: compState++; break;
                }
                Position++;
            }
        }

        /// <summary>
        /// NullConstant = 'null';
        /// </summary>
        bool GetNullConstant()
        {
            ComparisonState compState = 0;
            for (; ; )
            {
                switch (compState)
                {
                    case (ComparisonState)0:
                        {
                            if (StreamBuffer.Peek() == Scripting.LowercaseLetter.U) goto case ComparisonState.Next;
                            else goto case ComparisonState.Failure;
                        }
                    case (ComparisonState)2:
                    case (ComparisonState)1:
                        {
                            if (StreamBuffer.Peek() == Scripting.LowercaseLetter.L) goto case ComparisonState.Next;
                            else goto case ComparisonState.Failure;
                        }
                    case (ComparisonState)3:
                        {
                            if (!IsIdentifierChar(StreamBuffer.Peek())) return true;
                            else goto case ComparisonState.Failure;
                        }
                    default:
                    case ComparisonState.Failure: return false;
                    case ComparisonState.Next: compState++; break;
                }
                Position++;
            }
        }

        /// <summary>
        /// Determines if a character is a valid identifier character to this tokenizer
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool IsIdentifierChar(Char32 c)
        {
            return ((c >= Scripting.LowercaseLetter.Begin && c <= Scripting.LowercaseLetter.End) || (c >= Scripting.UppercaseLetter.Begin && c <= Scripting.UppercaseLetter.End));
        }

        /// <summary>
        /// Determines if a character is a valid numeric to this tokenizer
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool IsNumericChar(Char32 c)
        {
            return (c >= Scripting.Digit.Begin && c <= Scripting.Digit.End);
        }
    }
}
