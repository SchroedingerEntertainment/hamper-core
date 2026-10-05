// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;

namespace System
{
    /// <summary>
    /// Represents a collection of elements of type <typeparamref name="T"/> on the heap, accessible by their index
    /// </summary>
    /// <typeparam name="T">A reference type to be stored</typeparam>
    #if EXPORT_HAMPER_CORE_SHARP
    public
    #else
    internal
    #endif
    struct HeapArray<T> : IArrayAccessor<T>
        where T : class?
    {
        private T[] array;

        /// <inheritdoc/>
        public int Length
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return array.Length; }
        }

        /// <inheritdoc/>
        public ref T this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return ref array[index]; }
        }

        /// <summary>
        /// Initializes to an empty array
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public HeapArray()
        {
            array = Array.Empty<T>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator T[](HeapArray<T> heapArray)
        {
            return heapArray.array;
        }
        
        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Span<T> AsSpan()
        {
            return array.AsSpan();
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Clear()
        {
            Array.Clear(array);
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int IndexOf(T item)
        {
            return Array.IndexOf(array, item);
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Resize(int size)
        {
            Array.Resize(ref array, size);
            return true;
        }
    }
}