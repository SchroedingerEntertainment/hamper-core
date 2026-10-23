// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;

namespace System
{
    #if EXPORT_HAMPER_CORE_SHARP
    public
    #else
    internal
    #endif
    static partial class MathExtension
    {
        // ReSharper disable InvalidXmlDocComment
        
        /// <summary>
        /// Returns the smallest integral value greater than or equal to the division by the divisor 
        /// </summary>
        /// <param name="divisor">The number by which this value is to be divided</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int32 RoundUpDiv(this Int32 numerator, Int32 divisor)
        {
            return ((numerator + divisor - 1) / divisor);
        }
        /// <summary>
        /// Returns the smallest integral value greater than or equal to the division by the divisor 
        /// </summary>
        /// <param name="divisor">The number by which this value is to be divided</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt32 RoundUpDiv(this UInt32 numerator, UInt32 divisor)
        {
            return ((numerator + divisor - 1) / divisor);
        }
        
        /// <summary>
        /// Returns the smallest integral value greater than or equal to the division by the divisor 
        /// </summary>
        /// <param name="divisor">The number by which this value is to be divided</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int64 RoundUpDiv(this Int64 numerator, Int64 divisor)
        {
            return ((numerator + divisor - 1) / divisor);
        }
        /// <summary>
        /// Returns the smallest integral value greater than or equal to the division by the divisor 
        /// </summary>
        /// <param name="divisor">The number by which this value is to be divided</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt64 RoundUpDiv(this UInt64 numerator, UInt64 divisor)
        {
            return ((numerator + divisor - 1) / divisor);
        }
    }
}
