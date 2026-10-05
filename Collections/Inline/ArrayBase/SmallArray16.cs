// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Soe.Collections.Inline
{
    /// <summary>
    /// An array of 16 pre-allocated elements of type <typeparamref name="T"/> on the stack, expanding into the heap if needed
    /// </summary>
    /// <typeparam name="T">A reference type to be stored</typeparam>
    [InlineArray(16)]
    #if EXPORT_HAMPER_CORE_COLLECTIONS_INLINE
    public
    #else
    internal
    #endif
    struct SmallArray16<T> : ISmallArray<T>
        where T : class?
    {
        private const int DefaultCapacity = 16;
            
        private object element0;

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Span<T> AsSpan()
        {
            if (element0 is T[] array)
            {
                return array.AsSpan();
            }
            else return AsSpanInternal();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        Span<T> AsSpanInternal()
        {
            return MemoryMarshal.CreateSpan(ref Unsafe.As<object, T>(ref element0), DefaultCapacity);
        }
        
        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int IndexOf(T item)
        {
            if (element0 is T[] array)
            {
                return Array.IndexOf(array, item);
            }
            else return MemoryMarshal.CreateSpan(ref Unsafe.As<object, IntPtr>(ref element0), DefaultCapacity).IndexOf(Unsafe.As<T, IntPtr>(ref item));
        }

        /// <inheritdoc/>
        public bool Resize(int size)
        {
            // Size can't be less than the inline portion
            size = Math.Max(size, DefaultCapacity);

            Span<T> span = AsSpan();
            if (size > span.Length)
            {
                // Grow
                T[] newArray = new T[size];
                span.CopyTo(newArray);

                if (span.Length == DefaultCapacity)
                {
                    // Resets the inline elements to prevent false GC references
                    span.Clear();
                }
                
                element0 = newArray;
            }
            else if (size < span.Length)
            {
                // Shrink
                if (size > DefaultCapacity)
                {
                    T[] newArray = new T[size];
                    span.Slice(0, size).CopyTo(newArray);

                    element0 = newArray;
                }
                else span.Slice(0, size).CopyTo(AsSpanInternal());
            }
            else return false;
            return true;
        }
    }
}