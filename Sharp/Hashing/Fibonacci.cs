// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

namespace System
{
    #if EXPORT_HAMPER_CORE_SHARP
    public
    #else
    internal
    #endif
    static partial class HashUtility
    {
        /// <summary>
        /// 2^16 Fibonacci constant
        /// </summary>
        public const UInt16 FibonacciConstant16 = 0x9E37;
        /// <summary>
        /// 2^32 Fibonacci constant
        /// </summary>
        public const UInt32 FibonacciConstant32 = 0x9E3779B9;
        /// <summary>
        /// 2^64 Fibonacci constant
        /// </summary>
        public const UInt64 FibonacciConstant64 = 0x9E3779B97F4A7C15;
    }
}
