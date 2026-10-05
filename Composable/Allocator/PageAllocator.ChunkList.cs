// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Soe.Composable
{
    #if EXPORT_HAMPER_CORE_COMPOSABLE
    public
    #else
    internal
    #endif
    partial class PageAllocator
    {
        /// <summary>
        /// An array of fixed pre-allocated <seealso cref="Chunk"/> on the stack, expanding into the heap if needed
        /// </summary>
        [InlineArray(4)]
        struct ChunkList
        {
            private const int DefaultCapacity = 4;

            private Chunk element0;

            /// <summary>
            /// Creates a new span over the elements in the array
            /// </summary>
            /// <returns>The span created</returns>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Span<Chunk> AsSpan()
            {
                if (element0.Handler is Chunk[] array)
                {
                    return array.AsSpan();
                }
                else return AsSpanInternal();
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            Span<Chunk> AsSpanInternal()
            {
                return MemoryMarshal.CreateSpan(ref element0, DefaultCapacity);
            }

            /// <summary>
            /// Changes the number of elements to the provided size
            /// </summary>
            /// <param name="size">The new size of the array</param>
            /// <returns>True if the collection was resized, false otherwise</returns>
            public bool Resize(int size)
            {
                // Size can't be less than the inline portion
                size = Math.Max(size, DefaultCapacity);

                Span<Chunk> span = AsSpan();
                if (size > span.Length)
                {
                    // Grow
                    Chunk[] newArray = new Chunk[size];
                    span.CopyTo(newArray);

                    if (span.Length == DefaultCapacity)
                    {
                        // Resets the inline elements to prevent false GC references
                        span.Clear();
                    }

                    element0.Handler = newArray;
                }
                else if (size < span.Length)
                {
                    // Shrink
                    if (size > DefaultCapacity)
                    {
                        Chunk[] newArray = new Chunk[size];
                        span.Slice(0, size).CopyTo(newArray);

                        element0.Handler = newArray;
                    }
                    else span.Slice(0, size).CopyTo(AsSpanInternal());
                }
                else return false;

                return true;
            }
        }
    }
}