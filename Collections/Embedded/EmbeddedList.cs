// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;

namespace Soe.Collections.Embedded
{
    /// <summary>
    /// Represents a strongly typed container of objects that can be accessed by index
    /// </summary>
    /// <typeparam name="T">The type of elements in the container</typeparam>
    /// <typeparam name="ArrayBuffer">The array accessor to use</typeparam>
    #if EXPORT_HAMPER_CORE_COLLECTIONS_EMBEDDED
    public
    #else
    internal
    #endif
    struct EmbeddedList<T, ArrayBuffer> : IIterable<T, Iterator<T>.DefaultStrategy>, IReadOnlyIterable<T, ReadOnlyIterator<T>.DefaultStrategy>, ISequence<T>
        where ArrayBuffer : struct, IArrayAccessor<T>
    {
        private ArrayBuffer buffer;

        /// <summary>
        /// Gets the total number of elements the internal data structure can hold without resizing
        /// </summary>
        public int Capacity
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return buffer.Length; }
        }

        private int count;
        /// <summary>
        /// Gets the number of elements contained
        /// </summary>
        public int Count
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return count; }
        }

        /// <summary>
        /// Gets or sets the element at the specified index
        /// </summary>
        /// <param name="index">The zero-based index of the element to get or set</param>
        public ref T this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return ref buffer[index]; }
        }

        /// <summary>
        /// Initializes this container instance to its default value
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public EmbeddedList()
        {
            buffer = new ArrayBuffer();
        }
        /// <summary>
        /// Initializes this container instance by a given default capacity
        /// </summary>
        /// <param name="capacity">The number of elements that the container can initially store</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public EmbeddedList(int capacity)
            : this()
        {
            EnsureCapacity(capacity);
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Span<T> AsSpan()
        {
            return buffer.AsSpan()
                .Slice(0, count);
        }
        
        /// <summary>
        /// Adds an object to the end of the container
        /// </summary>
        /// <param name="item">The object to be added to the end of the container</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add(T item)
        {
            EnsureCapacity(++count);
            buffer[count - 1] = item;
        }

        /// <summary>
        /// Removes all elements from the container
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Clear()
        {
            buffer.Clear();
            count = 0;
        }

        /// <summary>
        /// Determines whether an element is in the container
        /// </summary>
        /// <param name="item">The object to locate in the container</param>
        /// <returns>True if the item was found in the container, false otherwise</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Contains(T item)
        {
            return IndexOf(item) != -1;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void EnsureCapacity(int capacity)
        {
            capacity = Math.Max(4, capacity);
            if (Capacity < capacity)
            {
                buffer.Resize(capacity.NextPowerOfTwo());
            }
        }

        /// <summary>
        /// Returns the zero-based index of the first occurrence of a value in the container
        /// </summary>
        /// <param name="item">The object to locate in the container</param>
        /// <returns>The zero-based index of the first occurrence of item within the container if found, -1 otherwise</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int IndexOf(T item)
        {
            return buffer.IndexOf(item);
        }
        
        /// <summary>
        /// Inserts an element into the container at the specified index
        /// </summary>
        /// <param name="index">The zero-based index at which the element should be inserted</param>
        /// <param name="item">The object to insert</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Insert(int index, T item)
        {
            EnsureCapacity(count + 1);
            Span<T> span = buffer.AsSpan();
            span.Slice(index, count - index)
                .CopyTo(span.Slice(index + 1));
            
            buffer[index] = item;
        }
        
        /// <summary>
        /// Removes the element at the specified index of the container
        /// </summary>
        /// <param name="index">The zero-based index of the element to remove</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RemoveAt(int index)
        {
            if(index < count - 1)
            {
                Swap(index, count - 1);
            }
            buffer[--count] = default!;
        }
        
        /// <summary>
        /// Removes the first occurrence of a specific object from the container
        /// </summary>
        /// <param name="item">The object to remove from the container</param>
        /// <returns>True if the element was successfully removed, false otherwise</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Remove(T item)
        {
            int index = IndexOf(item);
            if (index != -1)
            {
                RemoveAt(index);
                return true;
            }
            else return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void Swap(int sourceIndex, int destinationIndex)
        {
            (buffer[sourceIndex], buffer[destinationIndex]) = (buffer[destinationIndex], buffer[sourceIndex]);
        }

        #region IIterable Members
        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Iterator<T, Iterator<T>.DefaultStrategy> GetEnumerator()
        {
            return new Iterator<T, Iterator<T>.DefaultStrategy>(AsSpan());
        }
        #endregion
        #region IReadOnlyIterable Members
        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        ReadOnlyIterator<T, ReadOnlyIterator<T>.DefaultStrategy> IReadOnlyIterable<T, ReadOnlyIterator<T>.DefaultStrategy>.GetEnumerator()
        {
            return new ReadOnlyIterator<T, ReadOnlyIterator<T>.DefaultStrategy>(AsSpan());
        }
        #endregion
    }
}