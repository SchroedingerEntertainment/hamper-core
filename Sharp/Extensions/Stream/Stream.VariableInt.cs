// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

namespace System.IO
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
        /// Stores the given integer into this stream
        /// </summary>
        /// <param name="value">An integer to store</param>
        public static void EncodeVariableInt(this Stream data, UInt32 value)
        {
            do
            {
                byte lower7bits = (byte)(value & 0x7f);
                value >>= 7;

                if (value > 0)
                {
                    lower7bits |= 128;
                }
                data.WriteByte(lower7bits);
            }
            while (value > 0);
        }
        /// <summary>
        /// Reads the given integer from this stream
        /// </summary>
        /// <param name="offset">An offset at which the integer should start from zero</param>
        /// <returns>The integer value stored in this stream</returns>
        public static UInt32 ToVariableInt(this Stream data)
        {
            UInt32 value = 0;
            for (int i = 0, shift = 0; ; i++)
            {
                byte lower7bits = (byte)data.ReadByte();
                value |= (UInt32)((lower7bits & 0x7f) << shift);
                shift += 7;

                if ((lower7bits & 128) == 0)
                    break;
            }
            return value;
        }
        
        // ReSharper restore InvalidXmlDocComment
    }
}