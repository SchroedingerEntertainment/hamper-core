// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using Soe.Parsing;
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
        protected override bool TryRead(out JsonToken token)
        {
            Trim();

            TextPointer start = Advance();
            switch (StreamBuffer.GetNext())
            {
                #region StringLiteral = '\"' ('\\"' | ~'\"')* '\"';
                case (UInt32)Scripting.Punctuation.QuotationMark:
                    {
                        buffer.Clear();
                        if (GetStringLiteral())
                        {
                            match = new TextRange(start, TextData.GetCurrentCaretPosition());
                            token = JsonToken.String;
                            return true;
                        }
                    }
                    goto default;
                #endregion

                #region Numeric = (('-')?['0', '9']) ((('E' | 'e') ('+' | '-')) | ['0','9'] '.')*;
                case (UInt32)Scripting.Punctuation.HyphenMinus:
                    {
                        if (IsNumericChar(StreamBuffer.Peek()))
                            goto case (UInt32)Scripting.Digit.Zero;
                    }
                    goto default;
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
                        Position = -1;
                        buffer.Clear();
                        if (GetNumeric())
                        {
                            match = new TextRange(start, TextData.GetCurrentCaretPosition());
                            token = JsonToken.Numeric;
                            return true;
                        }
                    }
                    goto default;
                #endregion

                #region Boolean = 'true' | 'false';
                case (UInt32)Scripting.LowercaseLetter.T:
                    {
                        if (GetTrueConstant())
                        {
                            match = new TextRange(start, 4);
                            token = JsonToken.True;
                            return true;
                        }
                    }
                    goto default;
                case (UInt32)Scripting.LowercaseLetter.F:
                    {
                        if (GetFalseConstant())
                        {
                            match = new TextRange(start, 5);
                            token = JsonToken.False;
                            return true;
                        }
                    }
                    goto default;
                #endregion

                #region Null = 'null';
                case (UInt32)Scripting.LowercaseLetter.N:
                    {
                        if (GetNullConstant())
                        {
                            match = new TextRange(start, 4);
                            token = JsonToken.Null;
                            return true;
                        }
                    }
                    goto default;
                #endregion

                #region BeginObject {
                case (UInt32)Scripting.SectionMarker.LeftCurlyBracket:
                    {
                        match = new TextRange(start, 1);
                        token = JsonToken.BeginObject;
                    }
                    return true;
                #endregion

                #region EndObject }
                case (UInt32)Scripting.SectionMarker.RightCurlyBracket:
                    {
                        match = new TextRange(start, 1);
                        token = JsonToken.EndObject;
                    }
                    return true;
                #endregion

                #region BeginArray [
                case (UInt32)Scripting.SectionMarker.LeftSquareBracket:
                    {
                        match = new TextRange(start, 1);
                        token = JsonToken.BeginArray;
                    }
                    return true;
                #endregion

                #region EndArray ]
                case (UInt32)Scripting.SectionMarker.RightSquareBracket:
                    {
                        match = new TextRange(start, 1);
                        token = JsonToken.EndArray;
                    }
                    return true;
                #endregion

                #region Colon (:)
                case (UInt32)Scripting.Punctuation.Colon:
                    {
                        match = new TextRange(start, 1);
                        token = JsonToken.Colon;
                    }
                    return true;
                #endregion

                #region Comma (,)
                case (UInt32)Scripting.Punctuation.Comma:
                    {
                        match = new TextRange(start, 1);
                        token = JsonToken.Comma;
                    }
                    return true;
                #endregion

                default:
                    {
                        match = new TextRange(start, 0);
                        token = JsonToken.Invalid;
                    }
                    return false;
            }
        }
        void Trim()
        {
            for (; ; ) switch (StreamBuffer.Peek())
            {
                #region LineBreak
                case (UInt32)Scripting.LineBreak.CarriageReturn:
                    {
                        if (StreamBuffer.Peek(1) == Scripting.LineBreak.LineFeed)
                        {
                            StreamBuffer[Position] = Scripting.WhiteSpace.Space;
                            continue;
                        }
                    }
                    goto default;
                #endregion

                #region WhiteSpace
                default:
                    {
                        if (Current.IsWhiteSpace() || Current.IsLineBreak())
                        {
                            Position++;
                        }
                        else return;
                    }
                    break;
                #endregion
            }
        }
    }
}
