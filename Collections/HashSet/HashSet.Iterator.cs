// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;

namespace Soe.Collections.HashSet
{
    #if EXPORT_HAMPER_CORE_COLLECTIONS_HASHSET
    public
    #else
    internal
    #endif
    static partial class HashSet
    {
        /// <summary>
        /// Iterates through this container from the first to the last valid element
        /// </summary>
        public struct IteratorStrategy<T, Container> : IIteratorStrategy<Container>
            where Container : struct, IHashContainer<T>
        {
            /// <inheritdoc/>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static int Begin(Span<Container> source)
            {
                return -1;
            }

            /// <inheritdoc/>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool MoveNext(Span<Container> source, ref int index)
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
        public struct ReadOnlyIteratorStrategy<T, Container> : IReadOnlyIteratorStrategy<Container>
            where Container : struct, IHashContainer<T>
        {
            /// <inheritdoc/>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static int Begin(ReadOnlySpan<Container> source)
            {
                return -1;
            }

            /// <inheritdoc/>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool MoveNext(ReadOnlySpan<Container> source, ref int index)
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