// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

namespace System
{
    #if EXPORT_HAMPER_CORE_SHARP
    public
    #else
    internal
    #endif
    static partial class EndianExtension
    {
        // ReSharper disable InvalidXmlDocComment
        
        /// <summary>
        /// Stores the given integer into this array. The array must
        /// at least have 5 bytes left to encode
        /// </summary>
        /// <param name="value">An integer to store</param>
        /// <param name="offset">An offset at which the integer should start from zero</param>
        /// <returns>The amount of bytes written</returns>
        public static int EncodeVariableInt(this Span<byte> data, UInt32 value, int offset = 0)
        {
            int i = offset;
            do
            {
                byte lower7bits = (byte)(value & 0x7f);
                value >>= 7;

                if (value > 0)
                {
                    lower7bits |= 128;
                }
                data[i] = lower7bits;
                i++;
            }
            while (value > 0);
            return (i - offset);
        }

        /// <summary>
        /// Reads the given integer from this array
        /// </summary>
        /// <param name="offset">An offset at which the integer should start from zero</param>
        /// <returns>The integer value stored in this array</returns>
        public static UInt32 ToVariableInt(this Span<byte> data, int offset = 0)
        {
            UInt32 value = 0;
            for (int i = offset, shift = 0; ; i++)
            {
                value |= (UInt32)((data[i] & 0x7f) << shift);
                shift += 7;

                if ((data[i] & 128) == 0)
                    break;
            }
            return value;
        }
        
        // ReSharper restore InvalidXmlDocComment
    }
}
