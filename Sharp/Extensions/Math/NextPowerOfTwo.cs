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
        /// Processes next power of two greater than or equal to this value
        /// </summary>
        public static Int16 NextPowerOfTwo(this Int16 i)
        {
            if (i < 0) return 0;
            --i;

            i |= (Int16)(i >> 1);
            i |= (Int16)(i >> 2);
            i |= (Int16)(i >> 4);
            i |= (Int16)(i >> 8);

            return (Int16)(i + 1);
        }
        /// <summary>
        /// Processes next power of two greater than or equal to this value
        /// </summary>
        public static UInt16 NextPowerOfTwo(this UInt16 i)
        {
            --i;

            i |= (UInt16)(i >> 1);
            i |= (UInt16)(i >> 2);
            i |= (UInt16)(i >> 4);
            i |= (UInt16)(i >> 8);

            return (UInt16)(i + 1);
        }

        /// <summary>
        /// Processes next power of two greater than or equal to this value
        /// </summary>
        public static Int32 NextPowerOfTwo(this Int32 i)
        {
            if (i < 0) return 0;
            --i;

            i |= (i >> 1);
            i |= (i >> 2);
            i |= (i >> 4);
            i |= (i >> 8);
            i |= (i >> 16);

            return (i + 1);
        }
        /// <summary>
        /// Processes next power of two greater than or equal to this value
        /// </summary>
        public static UInt32 NextPowerOfTwo(this UInt32 i)
        {
            --i;

            i |= (i >> 1);
            i |= (i >> 2);
            i |= (i >> 4);
            i |= (i >> 8);
            i |= (i >> 16);

            return (i + 1);
        }

        /// <summary>
        /// Processes next power of two greater than or equal to this value
        /// </summary>
        public static Int64 NextPowerOfTwo(this Int64 i)
        {
            if (i < 0) return 0;
            --i;

            i |= (i >> 1);
            i |= (i >> 2);
            i |= (i >> 4);
            i |= (i >> 8);
            i |= (i >> 16);

            return (i + 1);
        }
        /// <summary>
        /// Processes next power of two greater than or equal to this value
        /// </summary>
        public static UInt64 NextPowerOfTwo(this UInt64 i)
        {
            --i;

            i |= (i >> 1);
            i |= (i >> 2);
            i |= (i >> 4);
            i |= (i >> 8);
            i |= (i >> 16);

            return (i + 1);
        }
        /// <summary>
        /// Processes next power of two greater than or equal to this value
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int32 NextPowerOfTwo(this float f)
        {
            return NextPowerOfTwo((Int32)Math.Ceiling(f));
        }
        /// <summary>
        /// Processes next power of two greater than or equal to this value
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int64 NextPowerOfTwo(this double d)
        {
            return NextPowerOfTwo((Int64)Math.Ceiling(d));
        }
    }
}
