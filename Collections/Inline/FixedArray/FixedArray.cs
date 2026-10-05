// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System;
using System.Runtime.CompilerServices;

namespace Soe.Collections.Inline
{
    /// <summary>
    /// An array of pre-allocated elements of type <typeparamref name="T"/> on the stack
    /// </summary>
    /// <typeparam name="T">A reference type to be stored</typeparam>
    /// <typeparam name="FixedArrayN">A FixedArray of type <typeparamref name="T"/> defining the amount of inline elements</typeparam>
    #if EXPORT_HAMPER_CORE_COLLECTIONS_INLINE
    public
    #else
    internal
    #endif
    struct FixedArray<T, FixedArrayN> : IArrayAccessor<T>
        where T : class?
        where FixedArrayN : struct, IFixedArray<T>
    {
        #pragma warning disable CS0649
        private FixedArrayN array;
        #pragma warning restore CS0649

        /// <inheritdoc/>
        public int Length
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return array.AsSpan().Length; }
        }
            
        /// <inheritdoc/>
        public ref T this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return ref array.AsSpan()[index]; }
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
            array.AsSpan().Clear();
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int IndexOf(T item)
        {
            return array.IndexOf(item);
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Resize(int size)
        {
            return false;
        }
    }
}