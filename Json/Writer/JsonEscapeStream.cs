// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;
using Soe.Unicode;

namespace Soe.Json
{
    /// <summary>
    /// A streaming JSON string encoder
    /// </summary>
    #if EXPORT_HAMPER_CORE_JSON
    public
    #else
    internal
    #endif
    partial class JsonEscapeStream : Stream
    {
        private static readonly byte[] CharacterBuffer =
        {
            (byte)'\\', (byte)'u', (byte)'\\', (byte)'b', (byte)'\\', (byte)'t',
            (byte)'\\', (byte)'n', (byte)'\\', (byte)'f', (byte)'\\', (byte)'r'
        };
        
        private readonly Stream baseStream;
        
        /// <inheritdoc/>
        public override bool CanRead
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return false; }
        }
        
        bool isOpen;
        /// <inheritdoc/>
        public override bool CanSeek
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return isOpen && baseStream.CanSeek; }
        }

        bool isWritable;
        /// <inheritdoc/>
        public override bool CanWrite
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return isWritable && baseStream.CanWrite; }
        }

        /// <inheritdoc/>
        public override long Length
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return baseStream.Length; }
        }
        
        /// <inheritdoc/>
        public override long Position
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return baseStream.Position; }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set { baseStream.Position = value; }
        }

        /// <summary>
        /// Creates a new streaming JSON string encoder from a provided base stream
        /// </summary>
        public JsonEscapeStream(Stream baseStream)
        {
            this.baseStream = baseStream;
            this.isWritable = true;
            this.isOpen = true;
        }
        
        /// <inheritdoc/>
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    isWritable = false;
                    isOpen = false;
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override long Seek(long offset, SeekOrigin origin)
        {
            return baseStream.Seek(offset, origin);
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void SetLength(long value)
        {
            baseStream.SetLength(value);
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int Read(byte[] buffer, int offset, int count)
        {
            throw new InvalidOperationException();
        }
        
        /// <inheritdoc/>
        public override void Write(byte[] buffer, int offset, int count)
        {
            if (!isWritable)
            {
                throw new InvalidOperationException();
            }
            else if (count < 0 || count > buffer.Length - offset)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }
            else if (offset < 0 || offset >= buffer.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(offset));
            }
            else if (count > 0)
            {
                for (int i = offset, length = (offset + count); i < length; )
                {
                    switch (buffer[i])
                    {
                        #region QUOTATION MARK (")
                        case (byte)BasicLatin.OtherPunctuation.QuotationMark:
                        #endregion

                        #region REVERSE SOLIDUS (\)
                        case (byte)BasicLatin.OtherPunctuation.ReverseSolidus:
                            {
                                baseStream.WriteByte(CharacterBuffer[0]);
                                baseStream.WriteByte(buffer[i++]);
                            }
                            break;
                        #endregion

                        #region BACKSPACE (\b)
                        case (byte)BasicLatin.Control.Backspace:
                            {
                                baseStream.Write(CharacterBuffer, 2, 2);
                                i++;
                            }
                            break;
                        #endregion

                        #region CHARACTER TABULATION (\t)
                        case (byte)BasicLatin.Control.CharacterTabulation:
                            {
                                baseStream.Write(CharacterBuffer, 4, 2);
                                i++;
                            }
                            break;
                        #endregion

                        #region LINE FEED (\n)
                        case (byte)BasicLatin.Control.LineFeed:
                            {
                                baseStream.Write(CharacterBuffer, 6, 2);
                                i++;
                            }
                            break;
                        #endregion

                        #region FORM FEED (\f)
                        case (byte)BasicLatin.Control.FormFeed:
                            {
                                baseStream.Write(CharacterBuffer, 8, 2);
                                i++;
                            }
                            break;
                        #endregion

                        #region CARRIAGE RETURN (\r)
                        case (byte)BasicLatin.Control.CarriageReturn:
                            {
                                baseStream.Write(CharacterBuffer, 10, 2);
                                i++;
                            }
                            break;
                        #endregion

                        default: switch (buffer[i])
                            {
                                #region DELETE
                                case (byte)BasicLatin.Control.Delete:
                                    {
                                        EncodeUnicodeEscape(buffer[i++]);
                                    }
                                    break;
                                #endregion

                                #region Anything else
                                default:
                                    {
                                        byte c = buffer[i++];
                                        if (c <= 0x1F)
                                        {
                                            EncodeUnicodeEscape(c);
                                        }
                                        else baseStream.WriteByte(c);
                                    }
                                    break;
                                #endregion
                            }
                            break;
                    }
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void EncodeUnicodeEscape(int i)
        {
            baseStream.Write(CharacterBuffer, 0, 2);
            foreach (char c in i.ToString("X4"))
                baseStream.WriteByte((byte)c);
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Flush()
        {
            baseStream.Flush();
        }
    }
}