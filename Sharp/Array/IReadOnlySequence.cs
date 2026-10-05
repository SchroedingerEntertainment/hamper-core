// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

namespace System
{
    /// <summary>
    /// Provides read-only access to a range of elements of type <typeparamref name="T"/>
    /// </summary>
    /// <typeparam name="T">The type of elements in the range</typeparam>
    #if EXPORT_HAMPER_CORE_SHARP
    public
    #else
    internal
    #endif
    interface IReadOnlySequence<T>
    {
        /// <summary>
        /// Gets the underlying sequence of elements
        /// </summary>
        /// <returns>An in-memory representation of the sequence</returns>
        ReadOnlySpan<T> AsReadOnlySpan();
    }
}