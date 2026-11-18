// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;
using System.IO;

namespace System.Text
{
    #if EXPORT_HAMPER_CORE_SHARP
    public
    #else
    internal
    #endif
    static partial class StreamExtension
    {
        // ReSharper disable InvalidXmlDocComment
        
        /// <summary>
        /// Tries to determine this stream's encoding by analyzing its byte order mark (BOM)
        /// </summary>
        /// <param name="buffer">The buffer used to read up to the first 4 bytes of the stream</param>
        /// <param name="bomLength">The detected length of the byte order mark</param>
        /// <param name="fallbackEncoding">A fallback encoding to return if detection fails</param>
        /// <returns>The detected encoding if a byte order mark is present, defaults to UTF-8 otherwise</returns>
        public static Encoding DetectEncoding(this Stream stream, Span<byte> prefixBuffer, out int bomLength, Encoding? fallbackEncoding = null)
        {
            int prefixLength = 0;
            while (prefixLength < prefixBuffer.Length)
            {
                int value = stream.ReadByte();
                if (value == -1)
                {
                    break;
                }

                prefixBuffer[prefixLength++] = (byte)value;
                if (IsCompleteBom(prefixBuffer, prefixLength))
                {
                    break;
                }
                if (!IsPossibleBomPrefix(prefixBuffer, prefixLength))
                {
                    break;
                }
            }
            
            Encoding detectedEncoding;
            if (prefixLength >= 4 &&
                prefixBuffer[0] == 0x00 &&
                prefixBuffer[1] == 0x00 &&
                prefixBuffer[2] == 0xFE &&
                prefixBuffer[3] == 0xFF)
            {
                detectedEncoding = new UTF32Encoding(true, true);
                bomLength = 4;
            }
            else if (prefixLength >= 4 &&
                     prefixBuffer[0] == 0xFF &&
                     prefixBuffer[1] == 0xFE &&
                     prefixBuffer[2] == 0x00 &&
                     prefixBuffer[3] == 0x00)
            {
                detectedEncoding = new UTF32Encoding(false, true);
                bomLength = 4;
            }
            else if (prefixLength >= 3 &&
                     prefixBuffer[0] == 0xEF &&
                     prefixBuffer[1] == 0xBB &&
                     prefixBuffer[2] == 0xBF)
            {
                detectedEncoding = new UTF8Encoding(true);
                bomLength = 3;
            }
            else if (prefixLength >= 2 &&
                     prefixBuffer[0] == 0xFE &&
                     prefixBuffer[1] == 0xFF)
            {
                detectedEncoding = new UnicodeEncoding(true, true);
                bomLength = 2;
            }
            else if (prefixLength >= 2 &&
                     prefixBuffer[0] == 0xFF &&
                     prefixBuffer[1] == 0xFE)
            {
                detectedEncoding = new UnicodeEncoding(false, true);
                bomLength = 2;
            }
            else
            {
                detectedEncoding = fallbackEncoding ?? Encoding.UTF8;
                bomLength = 0;
            }
            if (stream.CanSeek)
            {
                stream.Position = bomLength;
            }
            return detectedEncoding;
        }
        /// <summary>
        /// Tries to determine this stream's encoding by analyzing its byte order mark (BOM)
        /// </summary>
        /// <param name="bomLength">The detected length of the byte order mark</param>
        /// <param name="fallbackEncoding">A fallback encoding to return if detection fails</param>
        /// <returns>The detected encoding if a byte order mark is present, defaults to UTF-8 otherwise</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Encoding DetectEncoding(this Stream stream, out int bomLength, Encoding? fallbackEncoding = null)
        {
            Span<byte> buffer = stackalloc byte[4];
            return DetectEncoding(stream, buffer, out bomLength, fallbackEncoding);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool IsCompleteBom(Span<byte> prefixBuffer, int prefixLength)
        {
            return (prefixLength >= 3 && prefixBuffer[0] == 0xEF && prefixBuffer[1] == 0xBB && prefixBuffer[2] == 0xBF) ||
                   (prefixLength >= 2 && prefixBuffer[0] == 0xFE && prefixBuffer[1] == 0xFF) ||
                   (prefixLength >= 4 && prefixBuffer[0] == 0x00 && prefixBuffer[1] == 0x00 && prefixBuffer[2] == 0xFE && prefixBuffer[3] == 0xFF) ||
                   (prefixLength >= 4 && prefixBuffer[0] == 0xFF && prefixBuffer[1] == 0xFE && prefixBuffer[2] == 0x00 && prefixBuffer[3] == 0x00);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool IsPossibleBomPrefix(Span<byte> prefixBuffer, int prefixLength)
        {
            return (prefixLength == 1 && (prefixBuffer[0] == 0xEF || prefixBuffer[0] == 0xFE || prefixBuffer[0] == 0xFF || prefixBuffer[0] == 0x00)) ||
                   (prefixLength == 2 && ((prefixBuffer[0] == 0xEF && prefixBuffer[1] == 0xBB) || (prefixBuffer[0] == 0xFF && prefixBuffer[1] == 0xFE) || (prefixBuffer[0] == 0x00 && prefixBuffer[1] == 0x00))) ||
                   (prefixLength == 3 && ((prefixBuffer[0] == 0xFF && prefixBuffer[1] == 0xFE && prefixBuffer[2] == 0x00) || (prefixBuffer[0] == 0x00 && prefixBuffer[1] == 0x00 && prefixBuffer[2] == 0xFE)));
        }
        
        // ReSharper restore InvalidXmlDocComment
    }
}