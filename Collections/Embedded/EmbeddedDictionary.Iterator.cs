// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;

namespace Soe.Collections.Embedded
{
    #if EXPORT_HAMPER_CORE_COLLECTIONS_EMBEDDED
    public
    #else
    internal
    #endif
    partial struct EmbeddedDictionary<TKey, TValue, ArrayBuffer>
    {
        /// <summary>
        /// Iterates through this container from the first to the last valid element
        /// </summary>
        public struct IteratorStrategy : IIteratorStrategy<HashEntry>
        {
            /// <inheritdoc/>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static int Begin(Span<HashEntry> source)
            {
                return -1;
            }

            /// <inheritdoc/>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool MoveNext(Span<HashEntry> source, ref int index)
            {
                for (index++; index < source.Length; index++)
                {
                    if(source[index].IsValid)
                    {
                        break;
                    }
                }
                return (index < source.Length);
            }
        }
        
        /// <summary>
        /// Iterates through this container from the first to the last valid element
        /// </summary>
        public struct ReadOnlyIteratorStrategy : IReadOnlyIteratorStrategy<HashEntry>
        {
            /// <inheritdoc/>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static int Begin(ReadOnlySpan<HashEntry> source)
            {
                return -1;
            }

            /// <inheritdoc/>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool MoveNext(ReadOnlySpan<HashEntry> source, ref int index)
            {
                for (index++; index < source.Length; index++)
                {
                    if(source[index].IsValid)
                    {
                        break;
                    }
                }
                return (index < source.Length);
            }
        }
    }
}