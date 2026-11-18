// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Buffers;
using System.Runtime.CompilerServices;

namespace System.Text
{
    /// <summary>
    /// A wrapper class embedding a string into a streamable source
    /// </summary>
    #if EXPORT_HAMPER_CORE_PARSING
    public
    #else
    internal
    #endif
    struct StreamSource : IStreamSource
    {
        private const int DefaultBufferSize = 1024;
        private const int MinBufferSize = 128;
        
        private Stream? stream;
        private byte[]? byteBuffer;
        private char[]? charBuffer;
        private readonly Encoding encoding;
        private readonly Decoder decoder;
        private readonly bool leaveOpen;
        
        private int byteOffset;
        private int charPosition;
        private int charLength;
        private bool endOfStream;

        /// <inheritdoc/>
        public Encoding CurrentEncoding
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return encoding; }
        }

        /// <inheritdoc/>
        public long Position
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return stream?.Position ?? 0;
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                if(stream != null)
                {
                    stream.Position = value;
                }
                charPosition = 0;
                charLength = 0;
            }
        }

        /// <inheritdoc/>
        public bool EndOfStream
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return (endOfStream && charPosition >= charLength); }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public StreamSource(Stream stream, Encoding? encoding = null, bool detectEncodingFromByteOrderMarks = true, int bufferSize = DefaultBufferSize, bool leaveOpen = false)
        {
            this.byteOffset = 0;
            this.stream = !stream.CanRead ? null : stream;
            this.byteBuffer = ArrayPool<byte>.Shared.Rent(Math.Max(MinBufferSize, bufferSize));
            if (detectEncodingFromByteOrderMarks)
            {
                encoding = stream?.DetectEncoding(byteBuffer!.AsSpan(), out byteOffset, encoding);
            }
            else encoding ??= Encoding.UTF8;
            this.encoding = encoding!;
            this.decoder = encoding!.GetDecoder();
            this.charBuffer = ArrayPool<char>.Shared.Rent(encoding.GetMaxCharCount(byteBuffer.Length));
            this.leaveOpen = leaveOpen;
        }
        
        bool FillBuffer()
        {
            charPosition = 0;
            charLength = 0;
            
            if (!endOfStream && byteBuffer != null && charBuffer != null)
            {
                for(;;)
                {
                    int bytesRead = (stream?.Read(byteBuffer!, byteOffset, byteBuffer!.Length - byteOffset) ?? 0);
                    bool flush = (bytesRead == 0);
                    
                    decoder.Convert(byteBuffer!, byteOffset, bytesRead, charBuffer!, 0, charBuffer.Length, flush, out int bytesUsed, out charLength, out _);
                    byteOffset = bytesRead - bytesUsed;

                    if (flush)
                    {
                        endOfStream = true;
                    }
                    if (charLength > 0)
                    {
                        return true;
                    }
                    else if (endOfStream)
                    {
                        return false;
                    }
                    else if (bytesUsed == 0 && charLength == 0)
                    {
                        throw new IOException();
                    }
                }
            }
            else return false;
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            if (byteBuffer != null)
            {
                ArrayPool<byte>.Shared.Return(byteBuffer);
                byteBuffer = null;
            }
            if (charBuffer != null)
            {
                ArrayPool<char>.Shared.Return(charBuffer);
                charBuffer = null;
            }
            if(!leaveOpen)
            {
                stream?.Dispose();
            }
            stream = null;
        }
        
        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int Peek()
        {
            if (charPosition >= charLength && !FillBuffer())
            {
                return -1;
            } 
            else return charBuffer![charPosition];
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int Read()
        {
            if (charPosition >= charLength && !FillBuffer())
            {
                return -1;
            }
            else return charBuffer![charPosition++];
        }
    }
}