// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using Soe.Unicode;

namespace Soe.Json
{
    /// <summary>
    /// A stream writer specialized to turn JSON tokens into text
    /// </summary>
    #if EXPORT_HAMPER_CORE_JSON
    public
    #else
    internal
    #endif
    struct JsonWriter : IDisposable
    {
        const int IndentationLevel = 4;
        
        private readonly StreamWriter writer;
        private readonly bool formatted;

        int indentation;
        bool beginBlock;

        /// <summary>
        /// Gets the current character encoding used to write JSON values
        /// </summary>
        public Encoding Encoding
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return writer.Encoding; }
        }
        
        /// <summary>
        /// Creates a new JSON writer from the given stream
        /// </summary>
        /// <param name="writer">The stream writer used to process the data</param>
        /// <param name="formatted">
        /// Determines if additional formatting should be applied to create human-readable text
        /// </param>
        public JsonWriter(StreamWriter writer, bool formatted)
        {
            this.writer = writer;
            this.formatted = formatted;
        }
        
        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Dispose()
        {
            writer.Flush();
            writer.Dispose();
        }

        /// <summary>
        /// Writes the next property token to the output stream
        /// </summary>
        /// /// <remarks>
        /// Must be one of (Numeric, String)
        /// </remarks>
        public void Write(JsonToken token, string name, string value)
        {
            switch (token)
            {
                #region Numeric
                case JsonToken.Numeric:
                    {
                        Format();
                        Format(name);

                        writer.Write(value);

                        beginBlock = false;
                    }
                    break;
                #endregion

                #region String
                case JsonToken.String:
                    {
                        Format();
                        Format(name);

                        writer.Write(string.Concat("\"", value, "\""));

                        beginBlock = false;
                    }
                    break;
                #endregion

                #region Error
                default:
                    throw new ArgumentOutOfRangeException("token");
                #endregion
            }
        }
        /// <summary>
        /// Writes the next token to the output stream
        /// </summary>
        /// /// <remarks>
        /// Must be one of (Numeric, String) for a pure property or (BeginObject, EndObject,
        /// BeginArray, EndArray, Null, True, False) for a named property
        /// </remarks>
        public void Write(JsonToken token, string valueOrName)
        {
            switch (token)
            {
                #region BeginObject
                case JsonToken.BeginObject:
                    {
                        Format();
                        Format(valueOrName);

                        writer.Write(Scripting.SectionMarker.LeftCurlyBracket.ToMultibyteChar());

                        indentation += IndentationLevel;
                        beginBlock = true;
                    }
                    break;
                #endregion
                    
                #region BeginArray
                case JsonToken.BeginArray:
                    {
                        Format();
                        Format(valueOrName);

                        writer.Write(Scripting.SectionMarker.LeftSquareBracket.ToMultibyteChar());

                        indentation += IndentationLevel;
                        beginBlock = true;
                    }
                    break;
                #endregion

                #region Null
                case JsonToken.Null:
                #endregion

                #region Boolean
                case JsonToken.True:
                case JsonToken.False:
                    {
                        Format();
                        Format(valueOrName);

                        writer.Write(token.ToString().ToLowerInvariant());

                        beginBlock = false;
                    }
                    break;
                #endregion
                    
                #region Numeric
                case JsonToken.Numeric:
                    {
                        Format();

                        writer.Write(valueOrName);

                        beginBlock = false;
                    }
                    break;
                #endregion

                #region String
                case JsonToken.String:
                    {
                        Format();

                        writer.Write(string.Concat("\"", valueOrName, "\""));

                        beginBlock = false;
                    }
                    break;
                #endregion

                #region Error
                default:
                    throw new ArgumentOutOfRangeException("token");
                #endregion
            }
        }
        /// <summary>
        /// Writes the next token to the output stream
        /// </summary>
        /// /// <remarks>
        /// Must be one of (BeginObject, EndObject, BeginArray, EndArray, Null, True, False)
        /// </remarks>
        public void Write(JsonToken token)
        {
            switch (token)
            {
                #region BeginObject
                case JsonToken.BeginObject:
                    {
                        Format();

                        writer.Write(Scripting.SectionMarker.LeftCurlyBracket.ToMultibyteChar());

                        indentation += IndentationLevel;
                        beginBlock = true;
                    }
                    break;
                #endregion
                    
                #region EndObject
                case JsonToken.EndObject:
                    {
                        indentation -= IndentationLevel;
                        if (formatted)
                        {
                            writer.WriteLine(string.Empty);
                            SetIndentation();
                        }

                        writer.Write(Scripting.SectionMarker.RightCurlyBracket.ToMultibyteChar());

                        beginBlock = false;
                    }
                    break;
                #endregion

                #region BeginArray
                case JsonToken.BeginArray:
                    {
                        Format();

                        writer.Write(Scripting.SectionMarker.LeftSquareBracket.ToMultibyteChar());

                        indentation += IndentationLevel;
                        beginBlock = true;
                    }
                    break;
                #endregion

                #region EndArray
                case JsonToken.EndArray:
                    {
                        indentation -= IndentationLevel;
                        if (formatted)
                        {
                            writer.WriteLine(string.Empty);
                            SetIndentation();
                        }

                        writer.Write(Scripting.SectionMarker.RightSquareBracket.ToMultibyteChar());

                        beginBlock = false;
                    }
                    break;
                #endregion

                #region Null
                case JsonToken.Null:
                #endregion

                #region Boolean
                case JsonToken.True:
                case JsonToken.False:
                    {
                        Format();

                        writer.Write(token.ToString().ToLowerInvariant());

                        beginBlock = false;
                    }
                    break;
                #endregion

                #region Error
                default:
                    throw new ArgumentOutOfRangeException("token");
                #endregion
            }
        }

        /// <summary>
        /// Writes the next property token to the output stream
        /// </summary>
        /// /// <remarks>
        /// Must be one of (Numeric, String)
        /// </remarks>
        public JsonToken BeginWrite(JsonToken token, string name)
        {
            switch (token)
            {
                #region Numeric
                case JsonToken.Numeric:
                    {
                        Format();
                        Format(name);
                        Flush();
                    }
                    return token;
                #endregion

                #region String
                case JsonToken.String:
                    {
                        Format();
                        Format(name);

                        writer.Write('"');

                        Flush();
                    }
                    return token;
                #endregion

                #region Error
                default:
                    throw new ArgumentOutOfRangeException("token");
                #endregion
            }
        }
        /// <summary>
        /// Writes the next token to the output stream
        /// </summary>
        /// /// <remarks>
        /// Must be one of (Numeric, String)
        /// </remarks>
        public JsonToken BeginWrite(JsonToken token)
        {
            switch (token)
            {
                #region Numeric
                case JsonToken.Numeric:
                    {
                        Format();
                        Flush();
                    }
                    return token;
                #endregion

                #region String
                case JsonToken.String:
                    {
                        Format();

                        writer.Write('"');

                        Flush();
                    }
                    return token;
                #endregion

                #region Error
                default:
                    throw new ArgumentOutOfRangeException("token");
                #endregion
            }
        }

        /// <summary>
        /// Closes the previously opened token on the output stream
        /// </summary>
        /// /// <remarks>
        /// Must be one of (Numeric, String)
        /// </remarks>
        public void EndWrite(JsonToken token)
        {
            switch (token)
            {
                #region Numeric
                case JsonToken.Numeric:
                    {
                        beginBlock = false;
                    }
                    break;
                #endregion

                #region String
                case JsonToken.String:
                    {
                        writer.Write('"');
                        beginBlock = false;
                    }
                    break;
                #endregion

                #region Error
                default:
                    throw new ArgumentOutOfRangeException("token");
                #endregion
            }
        }

        /// <summary>
        /// Flushes the already written content to the underlaying stream
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Flush()
        {
            writer.Flush();
        }

        void Format()
        {
            if (formatted && indentation > 0)
            {
                if (!beginBlock)
                {
                    writer.Write(Scripting.Punctuation.Comma.ToMultibyteChar());
                }
                writer.WriteLine(string.Empty);
                SetIndentation();
            }
            else if (indentation > 0 && !beginBlock)
            {
                writer.Write(Scripting.Punctuation.Comma.ToMultibyteChar());
            }
        }
        void Format(string name)
        {
            writer.Write(string.Concat("\"", name, "\""));
            writer.Write(Scripting.Punctuation.Colon.ToMultibyteChar());
            if (formatted)
            {
                writer.Write(Scripting.WhiteSpace.Space.ToMultibyteChar());
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void SetIndentation()
        {
            for (int i = 0; i < indentation; i++)
                writer.Write(Scripting.WhiteSpace.Space.ToMultibyteChar());
        }
    }
}
