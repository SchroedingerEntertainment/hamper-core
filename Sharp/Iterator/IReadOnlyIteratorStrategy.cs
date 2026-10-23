// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

namespace System
{
    /// <summary>
    /// Manages the behavior of an exposed iterator
    /// </summary>
    /// <typeparam name="T">The element type of the collection</typeparam>
    #if EXPORT_HAMPER_CORE_SHARP
    public
    #else
    internal
    #endif
    interface IReadOnlyIteratorStrategy<T>
    {
        /// <summary>
        /// Resets the iterator to its initial position
        /// </summary>
        /// <param name="source">The collection to determine the initial position for</param>
        /// <returns>The index pointing to the initial position if the iterator</returns>
        static abstract int Begin(ReadOnlySpan<T> source);
        
        /// <summary>
        /// Advances the iterator to the next element of the collection
        /// </summary>
        /// <param name="source">The collection to advance the iterator on</param>
        /// <param name="index">The current position of the iterator</param>
        /// <returns>True if the iterator was successfully advanced to the next element, false otherwise</returns>
        static abstract bool MoveNext(ReadOnlySpan<T> source, ref int index);
    }
}