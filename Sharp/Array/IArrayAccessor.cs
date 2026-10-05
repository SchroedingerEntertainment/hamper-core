// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

namespace System
{
    /// <summary>
    /// Represents a collection of elements of type <typeparamref name="T"/>, accessible by their index
    /// </summary>
    /// <typeparam name="T">A reference type to be stored</typeparam>
    #if EXPORT_HAMPER_CORE_SHARP
    public
    #else
    internal
    #endif
    interface IArrayAccessor<T>
        where T : class?
    {
        /// <summary>
        /// Gets the length of the underlying collection
        /// </summary>
        int Length
        {
            get;
        }

        /// <summary>
        /// Gets or sets the element at a certain index
        /// </summary>
        /// <param name="index">The index of the element to access</param>
        ref T this[int index]
        {
            get;
        }
            
        /// <summary>
        /// Creates a new span over the elements in the array
        /// </summary>
        /// <returns>The span created</returns>
        Span<T> AsSpan();

        /// <summary>
        /// Resets the elements in the underlying collection to null
        /// </summary>
        void Clear();
        
        /// <summary>
        /// Searches for the specified object and returns the index of its first occurrence in the array
        /// </summary>
        /// <param name="item">The object to locate in the array</param>
        /// <returns>The index of the first occurrence of value in array, if found; otherwise, the lower bound of the array minus 1</returns>
        int IndexOf(T item);
        
        /// <summary>
        /// Changes the number of elements to the provided size
        /// </summary>
        /// <param name="size">The new size of the array</param>
        /// <returns>True if the collection was resized, false otherwise</returns>
        bool Resize(int size);
    }
}