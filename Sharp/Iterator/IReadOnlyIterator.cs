// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

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
    interface IReadOnlyIterator<out T>
        where T : allows ref struct
    {
        /// <summary>
        /// Gets the element in the collection at the current position of this iterator
        /// </summary>
        T Current
        {
            get;
        }

        /// <summary>
        /// Advances the iterator to the next element of the collection
        /// </summary>
        /// <returns>True if the iterator was successfully advanced to the next element, false otherwise</returns>
        bool MoveNext();

        /// <summary>
        /// Sets the iterator to its initial position
        /// </summary>
        void Reset();
    }
}