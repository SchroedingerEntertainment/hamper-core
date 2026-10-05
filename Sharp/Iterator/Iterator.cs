// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;

namespace System
{
    /// <summary>
    /// Handles iteration over a collection of elements of type <typeparamref name="T"/>
    /// </summary>
    /// <typeparam name="T">The element type of the collection</typeparam>
    #if EXPORT_HAMPER_CORE_SHARP
    public
    #else
    internal
    #endif
    static class Iterator<T>
    {
        /// <summary>
        /// Iterates through a collection from the first to the last element
        /// </summary>
        public struct DefaultStrategy : IIteratorStrategy<T>
        {
            /// <inheritdoc/>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static int Begin(Span<T> source)
            {
                return -1;
            }

            /// <inheritdoc/>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool MoveNext(Span<T> source, ref int index)
            {
                return (++index < source.Length);
            }
        }
    }
    /// <summary>
    /// Handles iteration over a collection of elements of type <typeparamref name="T"/>
    /// </summary>
    /// <typeparam name="T">The element type of the collection</typeparam>
    /// <typeparam name="Strategy">A strategy controlling the behavior of the iterator</typeparam>
    /// <param name="source">The collection this iterator instance handles</param>
    [method: MethodImpl(MethodImplOptions.AggressiveInlining)]
    #if EXPORT_HAMPER_CORE_SHARP
    public
    #else
    internal
    #endif
    ref struct Iterator<T, Strategy>(Span<T> source)
        where Strategy : struct, IIteratorStrategy<T>
    {
        private readonly Span<T> source = source;
        private int index = Strategy.Begin(source);
        
        /// <summary>
        /// Gets a reference to the element in the collection at the current position of this iterator
        /// </summary>
        public ref T Current
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return ref source[index]; }
        }

        /// <summary>
        /// Advances the iterator to the next element of the collection
        /// </summary>
        /// <returns>True if the iterator was successfully advanced to the next element, false otherwise</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool MoveNext()
        {
            return Strategy.MoveNext(source, ref index);
        }

        /// <summary>
        /// Sets the iterator to its initial position
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Reset()
        {
            index = Strategy.Begin(source);
        }
    }
}