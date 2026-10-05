// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System;

namespace Soe.Collections.Inline
{
    /// <summary>
    /// An array of pre-allocated elements of type <typeparamref name="T"/> on the stack
    /// </summary>
    /// <typeparam name="T">A reference type to be stored</typeparam>
    #if EXPORT_HAMPER_CORE_COLLECTIONS_INLINE
    public
    #else
    internal
    #endif
    interface IFixedArray<T>
        where T : class?
    {
        /// <summary>
        /// Creates a new span over the elements in the array
        /// </summary>
        /// <returns>The span created</returns>
        Span<T> AsSpan();
        
        /// <summary>
        /// Searches for the specified object and returns the index of its first occurrence in the array
        /// </summary>
        /// <param name="item">The object to locate in the array</param>
        /// <returns>The index of the first occurrence of value in array, if found; otherwise, the lower bound of the array minus 1</returns>
        int IndexOf(T item);
    }
}