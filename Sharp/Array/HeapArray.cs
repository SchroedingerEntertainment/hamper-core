// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Drawing;
using System.Runtime.CompilerServices;

namespace System
{
    /// <summary>
    /// Represents a collection of elements of type <typeparamref name="T"/> on the heap, accessible by their index
    /// </summary>
    /// <typeparam name="T">The type to be stored</typeparam>
    #if EXPORT_HAMPER_CORE_SHARP
    public
    #else
    internal
    #endif
    struct HeapArray<T> : IArrayAccessor<T>
    {
        private T[]? array;

        /// <inheritdoc/>
        public int Length
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return array?.Length ?? 0; }
        }

        /// <inheritdoc/>
        public ref T this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return ref array![index]; }
        }

        /// <summary>
        /// Initializes to an empty array
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public HeapArray()
        {
            this.array = Array.Empty<T>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator T[](HeapArray<T> heapArray)
        {
            return heapArray.array ?? Array.Empty<T>();
        }
        
        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Span<T> AsSpan()
        {
            return new Span<T>(array ?? Array.Empty<T>());
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Clear()
        {
            if(array != null)
            {
                Array.Clear(array);
            }
        }
        
        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Dispose()
        {
            Resize(0);
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int IndexOf(T item)
        {
            if (array == null)
            {
                return Array.IndexOf(array!, item);
            }
            else return -1;
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Resize(int size)
        {
            if(size > 0)
            {
                Array.Resize(ref array, size);
            }
            else array = Array.Empty<T>();
            return true;
        }
    }
}