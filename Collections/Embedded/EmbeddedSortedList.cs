// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;

namespace Soe.Collections.Embedded
{
    /// <summary>
    /// Represents a strongly typed container of sorted objects that can be accessed by index
    /// </summary>
    /// <typeparam name="T">The type of elements in the container</typeparam>
    #if EXPORT_HAMPER_CORE_COLLECTIONS_EMBEDDED
    public
    #else
    internal
    #endif
    partial struct EmbeddedSortedList<T>
    {
        /// <summary>
        /// Represents an unset index
        /// </summary>
        public const int InvalidIndex = -1;
    }
    
    /// <summary>
    /// Represents a strongly typed container of sorted objects that can be accessed by index
    /// </summary>
    /// <typeparam name="T">The type of elements in the container</typeparam>
    /// <typeparam name="ArrayBuffer">The array accessor to use</typeparam>
    #if EXPORT_HAMPER_CORE_COLLECTIONS_EMBEDDED
    public
    #else
    internal
    #endif
    partial struct EmbeddedSortedList<T, ArrayBuffer> : IIterable<EmbeddedSortedList<T>.ListNode, bool, EmbeddedSortedList<T>.IteratorStrategy>, IReadOnlyIterable<EmbeddedSortedList<T>.ListNode, bool, EmbeddedSortedList<T>.ReadOnlyIteratorStrategy>, IDisposable
        where ArrayBuffer : struct, IArrayAccessor<EmbeddedSortedList<T>.ListNode>
    {
        private readonly IComparer<T> comparer;
        private ArrayBuffer buffer;

        private int freeList;
        private int head;

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
        /// Gets the element at the specified index
        /// </summary>
        /// <param name="index">The zero-based index of the element to get</param>
        public T this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return buffer[index]; }
        }
        
        /// <summary>
        /// Initializes this container instance to its default value
        /// </summary>
        /// <param name="comparer">An <seealso cref="IEqualityComparer{T}"/> to handle element comparison</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public EmbeddedSortedList(IComparer<T> comparer)
        {
            this.comparer = comparer;
            this.freeList = EmbeddedSortedList<T>.InvalidIndex;
            this.head = EmbeddedSortedList<T>.InvalidIndex;
            this.buffer = default;
            this.count = 0;
        }
        /// <summary>
        /// Initializes this container instance by a given default capacity
        /// </summary>
        /// <param name="capacity">The number of elements that the container can initially store</param>
        /// <param name="comparer">An <seealso cref="IEqualityComparer{T}"/> to handle element comparison</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public EmbeddedSortedList(int capacity, IComparer<T> comparer)
            : this(comparer)
        {
            EnsureCapacity(capacity);
        }

        /// <summary>
        /// Inserts an object at the right index into the container
        /// </summary>
        /// <param name="item">The object to be added to the container</param>
        public void Add(T item)
        {
            int index; if (freeList != EmbeddedSortedList<T>.InvalidIndex)
            {
                index = freeList;
                freeList = buffer[index].Next;
            }
            else
            {
                EnsureCapacity(count + 1);
                index = count++;
            }
            
            buffer[index] = new EmbeddedSortedList<T>.ListNode(item);
            if (head != EmbeddedSortedList<T>.InvalidIndex)
            {
                int last = head;
                for(int current = head; current != EmbeddedSortedList<T>.InvalidIndex; current = buffer[last].Next)
                {
                    if (comparer.Compare(buffer[current].Value, item) > 0)
                    {
                        if (buffer[current].Previous != EmbeddedSortedList<T>.InvalidIndex)
                        {
                            buffer[buffer[current].Previous].Next = index;
                        }
                        else head = index;
                        
                        buffer[index].Previous = buffer[current].Previous;
                        buffer[index].Next = current;
                        buffer[current].Previous = index;
                        
                        return;
                    }
                    else last = current;
                }
                buffer[last].Next = index;
                buffer[index].Previous = last;
            }
            else head = index;
        }
        
        /// <summary>
        /// Determines whether an element is in the container
        /// </summary>
        /// <param name="item">The object to locate in the container</param>
        /// <returns>True if the item was found in the container, false otherwise</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Contains(T item)
        {
            return IndexOf(item) != EmbeddedSortedList<T>.InvalidIndex;
        }
        
        /// <summary>
        /// Removes all elements from the container
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Clear()
        {
            buffer.Clear();
            head = EmbeddedSortedList<T>.InvalidIndex;
            freeList = EmbeddedSortedList<T>.InvalidIndex;
            count = 0;
        }
        
        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Dispose()
        {
            Clear();
            buffer.Dispose();
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
        public int IndexOf(T item)
        {
            for (int current = head; current != EmbeddedSortedList<T>.InvalidIndex; current = buffer[current].Next)
            {
                if (comparer.Compare(buffer[current].Value, item) == 0)
                {
                    return current;
                }
            }
            return EmbeddedSortedList<T>.InvalidIndex;
        }

        /// <summary>
        /// Removes the element at the specified index of the container
        /// </summary>
        /// <param name="index">The zero-based index of the element to remove</param>
        public void RemoveAt(int index)
        {
            if (buffer[index].IsValid)
            {
                if (buffer[index].Previous != EmbeddedSortedList<T>.InvalidIndex)
                {
                    buffer[buffer[index].Previous].Next = buffer[index].Next;
                }
                else head = buffer[index].Next;
                if (buffer[index].Next != EmbeddedSortedList<T>.InvalidIndex)
                {
                    buffer[buffer[index].Next].Previous = buffer[index].Previous;
                }

                buffer[index] = new EmbeddedSortedList<T>.ListNode();
                for (int current = freeList; current != EmbeddedSortedList<T>.InvalidIndex; current = buffer[current].Next)
                {
                    if (buffer[current].Next == EmbeddedSortedList<T>.InvalidIndex)
                    {
                        buffer[current].Next = index;
                        return;
                    }
                }
                freeList = index;
            }
            else throw new IndexOutOfRangeException();
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
        
        #region IIterable Members
        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Iterator<EmbeddedSortedList<T>.ListNode, bool, EmbeddedSortedList<T>.IteratorStrategy> GetEnumerator()
        {
            return new Iterator<EmbeddedSortedList<T>.ListNode, bool, EmbeddedSortedList<T>.IteratorStrategy>(buffer.AsSpan());
        }
        #endregion
        #region IReadOnlyIterable Members
        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        ReadOnlyIterator<EmbeddedSortedList<T>.ListNode, bool, EmbeddedSortedList<T>.ReadOnlyIteratorStrategy> IReadOnlyIterable<EmbeddedSortedList<T>.ListNode, bool, EmbeddedSortedList<T>.ReadOnlyIteratorStrategy>.GetEnumerator()
        {
            return new ReadOnlyIterator<EmbeddedSortedList<T>.ListNode, bool, EmbeddedSortedList<T>.ReadOnlyIteratorStrategy>(buffer.AsSpan());
        }
        #endregion
    }
}
