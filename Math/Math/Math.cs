// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;

namespace Soe.Utility.Math
{
    /// <summary>
    /// Big Integer Math Utilities
    /// </summary>
    #if EXPORT_HAMPER_CORE_MATH
    public
    #else
    internal
    #endif
    static partial class BigMath
    {
        /// <summary>
		/// Sets the value of a to zero
		/// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AssignZero(Span<UInt64> a, int size)
        {
            for (int i = 0; i < size; i++)
            {
                a[i] = 0;
            }
        }
        /// <summary>
        /// Sets the value of a to zero
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AssignZero(Span<UInt64> a, int index, int size)
        {
            for (int i = 0; i < size; i++)
            {
                a[i + index] = 0;
            }
        }

        /// <summary>
        /// Assigns b to a
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Assign(Span<UInt64> a, Span<UInt64> b, int size)
        {
            for (int i = 0; i < size; i++)
            {
                a[i] = b[i];
            }
        }
        /// <summary>
        /// Assigns b to a
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Assign(Span<UInt64> a, int aIndex, Span<UInt64> b, int bIndex, int size)
        {
            for (int i = 0; i < size; i++)
            {
                a[i + aIndex] = b[i + bIndex];
            }
        }

        /// <summary>
        /// Determines if a is zero
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsZero(Span<UInt64> a, int size)
        {
            for (int i = 0; i < size; i++)
            {
                if (a[i] != 0)
                {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// Determines if a is zero
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsZero(Span<UInt64> a, int index, int size)
        {
            for (int i = 0; i < size; i++)
            {
                if (a[i + index] != 0)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Determines if a is even
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsEven(Span<UInt64> a)
        {
            return ((a[0] & 1) == 0);
        }

        /// <summary>
        /// Compares two long values for equality
        /// </summary>
        public static Int32 Compare(Span<UInt64> a, Span<UInt64> b, int size)
        {
            for (int i = (size - 1); i >= 0; i--)
            {
                if (a[i] > b[i])
                {
                    return 1;
                }
                else if (a[i] < b[i])
                {
                    return -1;
                }
            }
            return 0;
        }
        /// <summary>
        /// Compares two long values for equality
        /// </summary>
        public static Int32 Compare(Span<UInt64> a, int aIndex, Span<UInt64> b, int bIndex, int size)
        {
            for (int i = (size - 1); i >= 0; i--)
            {
                if (a[i + aIndex] > b[i + bIndex])
                {
                    return 1;
                }
                else if (a[i + aIndex] < b[i + bIndex])
                {
                    return -1;
                }
            }
            return 0;
        }
    }
}