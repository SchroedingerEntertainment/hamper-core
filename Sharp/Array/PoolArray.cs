// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Buffers;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.JavaScript;

namespace System
{
    /// <summary>
    /// Represents a collection of elements of type <typeparamref name="T"/> from a memory pool, accessible by their index
    /// </summary>
    /// <typeparam name="T">The type to be stored</typeparam>
    /// <remarks>The array managed has no defined size. The size is guaranteed to be at least the requested size but
    /// the array might be larger. The calling code is responsible to ensure correct length is used</remarks>
    #if EXPORT_HAMPER_CORE_SHARP
    public
    #else
    internal
    #endif
    struct PoolArray<T> : IArrayAccessor<T>
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
        public PoolArray()
        {
            this.array = Array.Empty<T>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator T[](PoolArray<T> heapArray)
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
        /// <remarks>The size of the underlying array is at least the desired size</remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Resize(int size)
        {
            if (size > 0)
            {
                T[]? tmp = array;
                array = ArrayPool<T>.Shared.Rent(size);
                if (tmp?.Length > 0)
                {
                    Array.Copy(tmp, 0, array, 0, Math.Min(tmp.Length, array.Length));
                    ArrayPool<T>.Shared.Return(tmp!);
                }
            }
            else if (Length > 0)
            {
                ArrayPool<T>.Shared.Return(array!);
                array = Array.Empty<T>();
            }
            return true;
        }
    }
}