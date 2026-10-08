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
    partial struct EmbeddedSortedList<T>
    {
        /// <summary>
        /// Iterates through this container from the first to the last valid element
        /// </summary>
        public struct IteratorStrategy : IIteratorStrategy<ListNode, bool>
        {
            /// <inheritdoc/>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static int Begin(Span<ListNode> source, ref bool parameter)
            {
                for (int i = 0; i < source.Length; i++)
                {
                    if (source[i].IsValid && source[i].Previous == InvalidIndex)
                    {
                        parameter = true;
                        return i;
                    }
                }
                return InvalidIndex;
            }

            /// <inheritdoc/>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool MoveNext(Span<ListNode> source, ref bool parameter, ref int index)
            {
                if(!parameter)
                {
                    if (index != InvalidIndex)
                    {
                        index = source[index].Next;
                    }
                    return (index != InvalidIndex);
                }
                else
                {
                    parameter = false;
                    return true;
                }
            }
        }
        
        /// <summary>
        /// Iterates through this container from the first to the last valid element
        /// </summary>
        public struct ReadOnlyIteratorStrategy : IReadOnlyIteratorStrategy<ListNode, bool>
        {
            /// <inheritdoc/>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static int Begin(ReadOnlySpan<ListNode> source, ref bool parameter)
            {
                for (int i = 0; i < source.Length; i++)
                {
                    if (source[i].IsValid && source[i].Previous == InvalidIndex)
                    {
                        parameter = true;
                        return i;
                    }
                }
                return InvalidIndex;
            }

            /// <inheritdoc/>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool MoveNext(ReadOnlySpan<ListNode> source, ref bool parameter, ref int index)
            {
                if(!parameter)
                {
                    if (index != InvalidIndex)
                    {
                        index = source[index].Next;
                    }
                    return (index != InvalidIndex);
                }
                else
                {
                    parameter = false;
                    return true;
                }
            }
        }
    }
}
