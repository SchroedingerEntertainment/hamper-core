// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Soe.Collections.HashSet;

namespace Soe.Collections.Embedded
{
    /// <summary>
    /// Represents a collection of keys and values
    /// </summary>
    /// <typeparam name="TKey">The type of object identifying elements in the container</typeparam>
    /// <typeparam name="TValue">The type of elements in the container</typeparam>
    /// <typeparam name="ArrayBuffer">The array accessor to use</typeparam>
    /// <remarks>Robin Hood hashing is an open addressing scheme that reduces variance in probe lengths by moving elements with
    /// shorter probe distances away to make room for elements that are farther from their ideal hash position</remarks>
    [StructLayout(LayoutKind.Auto)]
    #if EXPORT_HAMPER_CORE_COLLECTIONS_EMBEDDED
    public
    #else
    internal
    #endif
    partial struct EmbeddedDictionary<TKey, TValue, ArrayBuffer> : IIterable<EmbeddedDictionary<TKey, TValue, ArrayBuffer>.HashEntry, EmbeddedDictionary<TKey, TValue, ArrayBuffer>.IteratorStrategy>, IReadOnlyIterable<EmbeddedDictionary<TKey, TValue, ArrayBuffer>.HashEntry, EmbeddedDictionary<TKey, TValue, ArrayBuffer>.ReadOnlyIteratorStrategy>, ISequence<EmbeddedDictionary<TKey, TValue, ArrayBuffer>.HashEntry>
        where ArrayBuffer : struct, IArrayAccessor<EmbeddedDictionary<TKey, TValue, ArrayBuffer>.HashEntry>
    {
        private HashSet<TKey, ArrayBuffer, HashEntry> hashSet;
        
        /// <summary>
        /// Gets the total numbers of elements the internal data structure can hold without resizing
        /// </summary>
        public int Capacity
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return hashSet.Capacity; }
        }
        
        /// <summary>
        /// Gets the number of key/value pairs contained in the container
        /// </summary>
        public int Count
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return hashSet.Count; }
        }
        
        /// <summary>
        /// Gets or sets the value associated with the specified key
        /// </summary>
        /// <param name="key">The key of the value to get or set</param>
        public ref TValue this[in TKey key]
        {
            get
            {
                int hash = key!.GetHashCode();
                if (hashSet.Find(key, hash, out _, out _, out Ref<HashEntry> result))
                {
                    return ref result.Value.Value;
                }
                else throw new ArgumentOutOfRangeException();
            }
        }
        
        /// <summary>
        /// Initializes this container to the provided capacity
        /// </summary>
        /// <param name="capacity">The target capacity for this container to become</param>
        /// <param name="comparer">An object instance that can compare <typeparamref name="TValue"/> instances</param>
        /// <remarks>The container resizes to powers of two only</remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public EmbeddedDictionary(int capacity, EqualityComparer<TKey> comparer)
            : this(comparer)
        {
            Reserve(capacity);
        }
        /// <summary>
        /// Initializes this container to the provided capacity
        /// </summary>
        /// <param name="capacity">The target capacity for this container to become</param>
        /// <remarks>The container resizes to powers of two only</remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public EmbeddedDictionary(int capacity)
         : this(capacity, EqualityComparer<TKey>.Default)
        { }
        /// <summary>
        /// Initializes an empty instance of this container
        /// </summary>
        /// <param name="comparer">An object instance that can compare <typeparamref name="TValue"/> instances</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public EmbeddedDictionary(EqualityComparer<TKey> comparer)
        {
            this.hashSet = new HashSet<TKey, ArrayBuffer, HashEntry>(comparer);
        }
        /// <summary>
        /// Initializes an empty instance of this container
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public EmbeddedDictionary()
         : this(EqualityComparer<TKey>.Default)
        { }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Span<HashEntry> AsSpan()
        {
            return hashSet.AsSpan();
        }
        
        /// <summary>
        /// Adds the specified key and value to the container
        /// </summary>
        /// <param name="key">The key of the element to add</param>
        /// <param name="value">The value of the element to add</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add(TKey key, TValue value)
        {
            int hash = key!.GetHashCode();
            if (!hashSet.Find(key, hash, out int index, out int distance, out _))
            {
                hashSet.Emplace(key, hash, index, distance, hashSet.Version) = new HashEntry(key, hash, value);
            }
            else throw new ArgumentException();
        }
        /// <summary>
        /// Adds the specified key and value to the container
        /// </summary>
        /// <param name="item">The <see cref="KeyValuePair{TKey,TValue}"/> of the element to add</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add(KeyValuePair<TKey, TValue> item)
        {
            Add(item.Key, item.Value);
        }

        /// <summary>
        /// Removes all keys and values from the container
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Clear()
        {
            hashSet.Clear();
        }

        /// <summary>
        /// Determines whether the container contains a specific element
        /// </summary>
        /// <param name="item">The <see cref="KeyValuePair{TKey,TValue}"/> of the element to find</param>
        /// <returns>True if the element was found in the container, false otherwise</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            int hash = item.Key!.GetHashCode();
            if (hashSet.Find(item.Key, hash, out _, out _, out Ref<HashEntry> result))
            {
                return EqualityComparer<TValue>.Default.Equals(item.Value, result.Value.Value);
            }
            else return false;
        }
        
        /// <summary>
        /// Determines whether the container contains the specified key
        /// </summary>
        /// <param name="key">The key to locate in the container</param>
        /// <returns>True if the container contains an element with the specified key, false otherwise</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool ContainsKey(TKey key)
        {
            int hash = key!.GetHashCode();
            if (hashSet.Find(key, hash, out _, out _, out _))
            {
                return true;
            }
            else return false;
        }

        /// <summary>
        /// Removes the value with the specified key from the container
        /// </summary>
        /// <param name="key">The key of the element to remove</param>
        /// <returns>True if the element is successfully found and removed, false otherwise</returns>
        public bool Remove(TKey key)
        {
            if (hashSet.Find(key, key!.GetHashCode(), out _, out _, out Ref<HashEntry> result))
            {
                result.Value = default;
                return true;
            }
            else return false;
        }
        /// <summary>
        /// Removes the first occurrence of a specific element from the container
        /// </summary>
        /// <param name="item">The <see cref="KeyValuePair{TKey,TValue}"/> of the element to remove</param>
        /// <returns>True if the object was successfully removed from the container, false otherwise</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return Remove(item.Key);
        }

        /// <summary>
        /// Resizes the container to the provided capacity
        /// </summary>
        /// <param name="capacity">The target capacity for this container to become</param>
        /// <remarks>The container resizes to powers of two only</remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Reserve(int capacity)
        {
            hashSet.Reserve(capacity);
        }

        /// <summary>
        /// Gets the value associated with the specified key
        /// </summary>
        /// <param name="key">The key of the value to get</param>
        /// <param name="value">When this method returns, contains the value associated with the specified key,
        /// if the key is found; otherwise, the default value for the type of the value parameter. This
        /// parameter is passed uninitialized</param>
        /// <returns>True if the container contains an element with the specified key, false otherwise</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGetValue(TKey key, out TValue value)
        {
            int hash = key!.GetHashCode();
            if (hashSet.Find(key, hash, out _, out _, out Ref<HashEntry> result))
            {
                value = result.Value.Value;
                return true;
            }
            else
            {
                value = default!;
                return false;
            }
        }
        
        #region IIterable Members
        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Iterator<HashEntry, IteratorStrategy> GetEnumerator()
        {
            return new Iterator<HashEntry, IteratorStrategy>(AsSpan());
        }
        #endregion
        #region IReadOnlyIterable Members
        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        ReadOnlyIterator<HashEntry, ReadOnlyIteratorStrategy> IReadOnlyIterable<HashEntry, ReadOnlyIteratorStrategy>.GetEnumerator()
        {
            return new ReadOnlyIterator<HashEntry, ReadOnlyIteratorStrategy>(AsSpan());
        }
        #endregion
    }
}