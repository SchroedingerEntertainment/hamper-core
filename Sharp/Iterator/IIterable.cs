// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

namespace System
{
    /// <summary>
    /// Exposes an iterator, which supports iteration over a collection of elements of type <typeparamref name="T"/>
    /// </summary>
    /// <typeparam name="T">The element type of the collection</typeparam>
    /// <typeparam name="Strategy">A strategy controlling the behavior of the iterator</typeparam>
    #if EXPORT_HAMPER_CORE_SHARP
    public
    #else
    internal
    #endif
    interface IIterable<T, Strategy>
        where Strategy : struct, IIteratorStrategy<T>
    {
        /// <summary>
        /// Returns an iterator that iterates through a collection
        /// </summary>
        /// <returns>The iterator handling the underlying collection</returns>
        /// <remarks>The method must be named GetEnumerator to be recognized by foreach</remarks>
        Iterator<T, Strategy> GetEnumerator();
    }
}