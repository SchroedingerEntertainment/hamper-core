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
    static partial class StringExtension
    {
        // ReSharper disable InvalidXmlDocComment
        
        /// <summary>
        /// Counts the number of occurrences of a given character into a target String instance
        /// </summary>
        /// <param name="c">The character to count the number of occurrences</param>
        /// <returns>The number of occurrences found</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Count(this string s, char c)
        {
            int count = 0;
            for (int i = s.Length-1; i >= 0; i--)
            {
                if (s[i] == c)
                    count++;
            }
            return count;
        }
        /// <summary>
        /// Counts the number of occurrences of a given character predicate into a target String instance
        /// </summary>
        /// <param name="predicate">The character predicate to count the number of occurrences</param>
        /// <returns>The number of occurrences found</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Count(this string s, Predicate<char> predicate)
        {
            int count = 0;
            for (int i = s.Length - 1; i >= 0; i--)
            {
                if (predicate(s[i]))
                    count++;
            }
            return count;
        }
    }
}
