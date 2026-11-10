// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;

namespace System
{
    /// <summary>
    /// Handles iteration over a collection of read-only elements of type <typeparamref name="T"/>
    /// </summary>
    /// <typeparam name="T">The element type of the collection</typeparam>
    #if EXPORT_HAMPER_CORE_SHARP
    public
    #else
    internal
    #endif
    static class ReadOnlyIterator<T>
    {
        /// <summary>
        /// Iterates through a collection from the first to the last element
        /// </summary>
        public struct DefaultStrategy : IReadOnlyIteratorStrategy<T>
        {
            /// <inheritdoc/>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static int Begin(ReadOnlySpan<T> source)
            {
                return -1;
            }

            /// <inheritdoc/>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool MoveNext(ReadOnlySpan<T> source, ref int index)
            {
                return (++index < source.Length);
            }
        }
    }
    /// <summary>
    /// Handles iteration over a collection of read-only elements of type <typeparamref name="T"/>
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
    ref struct ReadOnlyIterator<T, Strategy>(ReadOnlySpan<T> source) : IReadOnlyIterator<T>
        where Strategy : struct, IReadOnlyIteratorStrategy<T>
    {
        private readonly ReadOnlySpan<T> source = source;
        private int index = Strategy.Begin(source);
        
        /// <summary>
        /// Gets the element in the collection at the current position of this iterator
        /// </summary>
        public T Current
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return source[index]; }
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
    /// <summary>
    /// Handles iteration over a collection of read-only elements of type <typeparamref name="T"/>
    /// </summary>
    /// <typeparam name="T">The element type of the collection</typeparam>
    /// <typeparam name="Parameter">An additional parameter used by the strategy</typeparam>
    /// <typeparam name="Strategy">A strategy controlling the behavior of the iterator</typeparam>
    #if EXPORT_HAMPER_CORE_SHARP
    public
    #else
    internal
    #endif
    ref struct ReadOnlyIterator<T, Parameter, Strategy> : IReadOnlyIterator<T>
        where Parameter : struct
        where Strategy : struct, IReadOnlyIteratorStrategy<T, Parameter>
    {
        private readonly ReadOnlySpan<T> source;
        private Parameter parameter;
        private int index;
        
        /// <summary>
        /// Gets the element in the collection at the current position of this iterator
        /// </summary>
        public T Current
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return source[index]; }
        }

        /// <summary>
        /// Initializes this iterator with the provided source
        /// </summary>
        /// <param name="source">The collection this iterator instance handles</param>
        public ReadOnlyIterator(ReadOnlySpan<T> source)
        {
            this.source = source;
            this.parameter = default;
            this.index = Strategy.Begin(source, ref parameter);
        }
        
        /// <summary>
        /// Advances the iterator to the next element of the collection
        /// </summary>
        /// <returns>True if the iterator was successfully advanced to the next element, false otherwise</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool MoveNext()
        {
            return Strategy.MoveNext(source, ref parameter, ref index);
        }

        /// <summary>
        /// Sets the iterator to its initial position
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Reset()
        {
            index = Strategy.Begin(source, ref parameter);
        }
    }
}