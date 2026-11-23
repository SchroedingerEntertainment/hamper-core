// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

namespace Soe.Storage
{
    /// <summary>
    /// Handles an iteration context over a certain <see cref="StorageIterator"/>
    /// </summary>
    #if EXPORT_HAMPER_CORE_STORAGE
    public
    #else
    internal
    #endif
    interface IContextHandler
    {
        /// <summary>
        /// Gets the element in the collection at the current position of the iterator
        /// </summary>
        /// <param name="context">An optional context object</param>
        /// <param name="parameter">The reference to an optional memory location</param>
        /// <returns>The element in the collection at the current position of the iterator</returns>
        ReadOnlySpan<char> GetCurrent(object? context, UInt64 parameter);

        /// <summary>
        /// Disposes the current context
        /// </summary>
        /// <param name="context">An optional context object</param>
        /// <param name="parameter">The reference to an optional memory location</param>
        void Dispose(object? context, ref UInt64 parameter);
        
        /// <summary>
        /// Advances the iterator to the next element of the collection
        /// </summary>
        /// <param name="context">An optional context object</param>
        /// <param name="parameter">The reference to an optional memory location</param>
        /// <returns>True if the iterator was successfully advanced to the next element, false otherwise</returns>
        bool MoveNext(object? context, ref UInt64 parameter);

        /// <summary>
        /// Sets the iterator to its initial position, which is before the first element in the collection
        /// </summary>
        /// <param name="context">An optional context object</param>
        /// <param name="parameter">The reference to an optional memory location</param>
        void Reset(object? context, ref UInt64 parameter);
    }
}