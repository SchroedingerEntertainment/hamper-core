// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

namespace Soe.Collections.HashSet
{
    /// <summary>
    /// An element container for type <typeparamref name="T"/>, used in an <see cref="HashSet{T,Container}"/>
    /// </summary>
    /// <typeparam name="T">The element type used as key</typeparam>
    /// <remarks>This interface can be implemented to specialize the HashSet in order to provide certain {Key, Value} pairs
    /// or other operations involving a key value as the addressing element</remarks>
    #if EXPORT_HAMPER_CORE_COLLECTIONS_HASHSET
    public
    #else
    internal
    #endif
    interface IHashContainer<out T>
    {
        /// <summary>
        /// Gets the hash code of the key
        /// </summary>
        int Hash
        {
            get;
        }

        /// <summary>
        /// The key value used for this element
        /// </summary>
        T Key
        {
            get;
        }

        /// <summary>
        /// Determines if this element has a proper key set
        /// </summary>
        bool IsValid
        {
            get;
        }
    }
}