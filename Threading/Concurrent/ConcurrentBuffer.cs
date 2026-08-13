// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;

namespace Soe.Threading
{
    /// <summary>
    /// Manages thread-safe adding to and exclusive reading from the underlying array
    /// </summary>
    /// <typeparam name="T">A reference type</typeparam>
    #if EXPORT_HAMPER_CORE_THREADING
    public
    #else
    internal
    #endif
    partial struct ConcurrentBuffer<T>
        where T : class
    {
        private UInt32 lockVariable;
        private UInt32 head;
        private UInt32 tail;

        /// <summary>
        /// Gets the element index logically pointing to index[0] of the buffer
        /// </summary>
        public int Tail
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return (int)Volatile.Read(ref tail);
            }
        }
        
        /// <summary>
        /// Gets the logical number of elements in the buffer
        /// </summary>
        public int Count
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                UInt32 current = Volatile.Read(ref tail);
                return (int)(Volatile.Read(ref head) - current);
            }
        }

        /// <summary>
        /// Initializes this instance of the buffer
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ConcurrentBuffer()
        {
            this.lockVariable = 0;
            this.head = 0;
            this.tail = 0;
        }
        
        /// <summary>
        /// Enqueues the provided element into the buffer
        /// </summary>
        /// <param name="array">The array this buffer instance operates on</param>
        /// <param name="value">The element to add</param>
        /// <returns>The index at which the element was added into the array</returns>
        /// <remarks>This operation is operation may lead to an additional Grow operation to be performed. The
        /// calling code is responsible to secure other operations using this buffer's policies</remarks>
        public int Enqueue<Accessor>(ref Accessor array, T value)
            where Accessor : IArrayAccessor<T?>
        {
        Head:
            using(ScopedDisposable.Acquire<UInt32, SynchronizationBarrier.SharedOperation>(ref lockVariable))
            {
                if (TryEnqueue(ref array, value, out int index))
                {
                    return index;
                }
            }
            using(ScopedDisposable.Acquire<UInt32, SynchronizationBarrier.ExclusiveOperation>(ref lockVariable))
            {
                int capacityBits = array.Length - 1;
                if (head - tail < capacityBits)
                {
                    // Remove completed in the meantime, insert and return
                    Volatile.Write(ref array[(int)(head % capacityBits)], value);
                    head++;

                    return (int)(head % capacityBits);
                }
                else
                {
                    Grow(ref array);
                    goto Head;
                }
            }
        }

        /// <summary>
        /// Resizes the underlying array and moves the content of the buffer to keep integrity
        /// </summary>
        /// <param name="array">The array this buffer instance operates on</param>
        /// <remarks>This operation should be secured using the exclusive policy. The calling code is responsible to
        /// keep the buffer integrity intact</remarks>
        public void Grow<Accessor>(ref Accessor array)
            where Accessor : IArrayAccessor<T?>
        {
            int capacityBits = array.Length - 1;
            int oldCount = Count;
            
            head = (UInt32)(head & capacityBits);
            tail = (UInt32)(tail & capacityBits);
   
            array.Resize((array.Length + 1).NextPowerOfTwo());
            if (tail > head)
            {
                int mergeIndex = (int)(tail + oldCount - head);
                        
                Span<T?> span = array.AsSpan();
                Span<T?> slice = span.Slice(0, (int)head);
                slice.CopyTo(span.Slice(mergeIndex));
                slice.Clear();
                
                head = (UInt32)(mergeIndex + head);
            }
        }

        /// <summary>
        /// Tries to enqueue the provided element into the buffer
        /// </summary>
        /// <param name="array">The array this buffer instance operates on</param>
        /// <param name="value">The element to add</param>
        /// <param name="index">The index at which the element was added into the array</param>
        /// <returns>True if the element was successfully added into the buffer, false otherwise</returns>
        /// <remarks>This operation might be secured using the shared policy. The calling code is responsible to
        /// keep the buffer integrity intact</remarks>
        public bool TryEnqueue<Accessor>(ref Accessor array, T value, out int index)
            where Accessor : IArrayAccessor<T?>
        {
            int capacityBits = array.Length - 1;
            for (;;)
            {
                UInt32 current = Volatile.Read(ref head);
                if (current - Volatile.Read(ref tail) >= capacityBits)
                {
                    // List is full
                    index = -1;
                    return false;
                }
                else if (Interlocked.CompareExchange(ref head, current + 1, current) == current)
                {
                    Volatile.Write(ref array[(int)(current & capacityBits)], value);
                    index = (int)(current & capacityBits);
                    return true;
                }
            }
        }

        /// <summary>
        /// Tries to obtain the element at the logical index[0] and returns its values
        /// </summary>
        /// <param name="array">The array this buffer instance operates on</param>
        /// <param name="value">The element contained in the buffer</param>
        /// <returns>True if the element was obtained from the buffer, false otherwise</returns>
        /// <remarks>This operation might be secured using the shared policy. The calling code is responsible to
        /// keep the buffer integrity intact</remarks>
        public bool TryDequeue<Accessor>(ref Accessor array, out T? value)
            where Accessor : IArrayAccessor<T?>
        {
            value = null;
            if (Count > 0)
            {
                int capacityBits = array.Length - 1;
                (array[(int)(tail & capacityBits)], value) = (value, array[(int)(tail & capacityBits)]);

                Interlocked.Increment(ref tail);
                return (value != null);
            }
            else return false;
        }

        /// <summary>
        /// Resets the buffer to an empty state
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Reset()
        {
            head = 0;
            tail = 0;
        }
    }
}