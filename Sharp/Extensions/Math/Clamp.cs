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
        /// Clamps this value to fit into min/max range
        /// </summary>
        /// <param name="min">The lower end of the range</param>
        /// <param name="max">The upper end of the range</param>
        /// <returns>The value that is in range</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int16 Clamp(this Int16 value, Int16 min, Int16 max)
        {
            if (value < min) return min;
            else if (value > max) return max;
            else return value;
        }
        /// <summary>
        /// Clamps this value to fit into min/max range
        /// </summary>
        /// <param name="min">The lower end of the range</param>
        /// <param name="max">The upper end of the range</param>
        /// <returns>The value that is in range</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt16 Clamp(this UInt16 value, UInt16 min, UInt16 max)
        {
            if (value < min) return min;
            else if (value > max) return max;
            else return value;
        }
        /// <summary>
        /// Clamps this value to fit into min/max range
        /// </summary>
        /// <param name="min">The lower end of the range</param>
        /// <param name="max">The upper end of the range</param>
        /// <returns>The value that is in range</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int32 Clamp(this Int32 value, Int32 min, Int32 max)
        {
            if (value < min) return min;
            else if (value > max) return max;
            else return value;
        }
        /// <summary>
        /// Clamps this value to fit into min/max range
        /// </summary>
        /// <param name="min">The lower end of the range</param>
        /// <param name="max">The upper end of the range</param>
        /// <returns>The value that is in range</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt32 Clamp(this UInt32 value, UInt32 min, UInt32 max)
        {
            if (value < min) return min;
            else if (value > max) return max;
            else return value;
        }
        /// <summary>
        /// Clamps this value to fit into min/max range
        /// </summary>
        /// <param name="min">The lower end of the range</param>
        /// <param name="max">The upper end of the range</param>
        /// <returns>The value that is in range</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int64 Clamp(this Int64 value, Int64 min, Int64 max)
        {
            if (value < min) return min;
            else if (value > max) return max;
            else return value;
        }
        /// <summary>
        /// Clamps this value to fit into min/max range
        /// </summary>
        /// <param name="min">The lower end of the range</param>
        /// <param name="max">The upper end of the range</param>
        /// <returns>The value that is in range</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt64 Clamp(this UInt64 value, UInt64 min, UInt64 max)
        {
            if (value < min) return min;
            else if (value > max) return max;
            else return value;
        }
        /// <summary>
        /// Clamps this value to fit into min/max range
        /// </summary>
        /// <param name="min">The lower end of the range</param>
        /// <param name="max">The upper end of the range</param>
        /// <returns>The value that is in range</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Clamp(this float value, float min, float max)
        {
            if (value < min) return min;
            else if (value > max) return max;
            else return value;
        }
        /// <summary>
        /// Clamps this value to fit into min/max range
        /// </summary>
        /// <param name="min">The lower end of the range</param>
        /// <param name="max">The upper end of the range</param>
        /// <returns>The value that is in range</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Clamp(this double value, double min, double max)
        {
            if (value < min) return min;
            else if (value > max) return max;
            else return value;
        }
    }
}