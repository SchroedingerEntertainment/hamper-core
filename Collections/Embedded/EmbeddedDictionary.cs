// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Soe.Collections.HashSet;

namespace Soe.Collections.Embedded
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>Robin Hood hashing is an open addressing scheme that reduces variance in probe lengths by moving elements with
    /// shorter probe distances away to make room for elements that are farther from their ideal hash position</remarks>
    #if EXPORT_HAMPER_CORE_COLLECTIONS_EMBEDDED
    public
    #else
    internal
    #endif
    partial struct EmbeddedDictionary<TKey, TValue> : IDictionary<TKey, TValue>, IReadOnlyDictionary<TKey, TValue>
    {
        private HashSet<TKey, HashEntry> hashSet;
        
        /// <summary>
        /// Gets the maximum number of elements that can be stored
        /// </summary>
        public int Capacity
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return hashSet.Capacity; }
        }
        
        /// <summary>
        /// Gets the current number of elements stored
        /// </summary>
        public int Count
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return hashSet.Count; }
        }

        /// <inheritdoc/>
        public bool IsReadOnly
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return false; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
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

        /// <inheritdoc/>
        TValue IDictionary<TKey, TValue>.this[TKey key]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return this[key]; }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set { this[key] = value; }
        }
        
        /// <inheritdoc/>
        TValue IReadOnlyDictionary<TKey, TValue>.this[TKey key]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return this[key]; }
        }
        
        /// <inheritdoc/>
        public ICollection<TKey> Keys
        {
            get { throw new NotImplementedException(); }
        }
        /// <inheritdoc/>
        public ICollection<TValue> Values
        {
            get { throw new NotImplementedException(); }
        }
        /// <inheritdoc/>
        IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return Keys; }
        }
        /// <inheritdoc/>
        IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return Values; }
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
            this.hashSet = new HashSet<TKey, HashEntry>(comparer);
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
        public void Add(TKey key, TValue value)
        {
            int hash = key!.GetHashCode();
            if (!hashSet.Find(key, hash, out int index, out int distance, out _))
            {
                hashSet.Emplace(key, hash, index, distance, hashSet.Version) = new HashEntry(key, hash, value);
            }
            else throw new ArgumentException();
        }
        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add(KeyValuePair<TKey, TValue> item)
        {
            Add(item.Key, item.Value);
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Clear()
        {
            hashSet.Clear();
        }

        /// <inheritdoc/>
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
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
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

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            if (array.Length - arrayIndex >= hashSet.Count)
            {
                HashEntry[] items = hashSet.Items!;
                for (int i = 0, length = items.Length; i < length; i++)
                {
                    if (items[i].IsValid)
                        array[arrayIndex++] = new KeyValuePair<TKey, TValue>(items[i].Key, items[i].Value);
                }
            }
            else throw new ArgumentException();
        }

        /// <inheritdoc/>
        public bool Remove(TKey key)
        {
            if (hashSet.Find(key, key!.GetHashCode(), out _, out _, out Ref<HashEntry> result))
            {
                result.Value = default;
                return true;
            }
            else return false;
        }

        /// <inheritdoc/>
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
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
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
        
        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return new Enumerator(hashSet.Items, hashSet.Count);
        }
        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}