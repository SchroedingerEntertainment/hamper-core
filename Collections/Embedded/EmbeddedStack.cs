// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;

namespace Soe.Collections.Embedded
{
    /// <summary>
    /// Represents a simple last-in-first-out collection of objects
    /// </summary>
    /// <typeparam name="T">The type of elements in the container</typeparam>
    /// <typeparam name="ArrayBuffer">The array accessor to use</typeparam>
    #if EXPORT_HAMPER_CORE_COLLECTIONS_EMBEDDED
    public
    #else
    internal
    #endif
    struct EmbeddedStack<T, ArrayBuffer> : IIterable<T, Iterator<T>.DefaultStrategy>, IReadOnlyIterable<T, ReadOnlyIterator<T>.DefaultStrategy>, ISequence<T>
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
        /// Initializes this container instance to its default value
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public EmbeddedStack()
        {
            buffer = new ArrayBuffer();
        }
        /// <summary>
        /// Initializes this container instance by a given default capacity
        /// </summary>
        /// <param name="capacity">The number of elements that the container can initially store</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public EmbeddedStack(int capacity)
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
            return buffer.IndexOf(item) != -1;
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
        /// Returns the object at the top of the container without removing it
        /// </summary>
        /// <returns>The object at the top of the container</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Peek()
        {
            if (TryPeek(out T result))
            {
                return result;
            }
            else throw new InvalidOperationException();
        }

        /// <summary>
        /// Removes and returns the object at the top of the container
        /// </summary>
        /// <returns>The object removed from the top of the container</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Pop()
        {
            if (TryPop(out T result))
            {
                return result;
            }
            else throw new InvalidOperationException();
        }

        /// <summary>
        /// Inserts an object at the top of the container
        /// </summary>
        /// <param name="item">The object to push onto the top of the container</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Push(T item)
        {
            EnsureCapacity(count + 1);
            buffer[count++] = item;
        }

        /// <summary>
        /// Returns a value that indicates whether there is an object at the top of the container, and if one
        /// is present, copies it to the result parameter. The object is not removed from the container
        /// </summary>
        /// <param name="result">If present, the object at the top of the container, the default value of T otherwise</param>
        /// <returns>True if there is an object at the top of the container, false otherwise</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryPeek(out T result)
        {
            if (count > 0)
            {
                result = buffer[count - 1];
                return true;
            }
            else
            {
                result = default!;
                return false;
            }
        }

        /// <summary>
        /// Returns a value that indicates whether there is an object at the top of the container, and if one
        /// is present, copies it to the result parameter, and removes it from the container
        /// </summary>
        /// <param name="result">If present, the object at the top of the container, the default value of T otherwise</param>
        /// <returns>True if there is an object at the top of the container, false otherwise</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryPop(out T result)
        {
            if (count > 0)
            {
                result = buffer[--count];
                buffer[count] = default!;

                return true;
            }
            else
            {
                result = default!;
                return false;
            }
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