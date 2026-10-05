// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Soe.Collections.Inline
{
    /// <summary>
    /// An array of 2 pre-allocated elements of type <typeparamref name="T"/> on the stack
    /// </summary>
    /// <typeparam name="T">A reference type to be stored</typeparam>
    [InlineArray(2)]
    #if EXPORT_HAMPER_CORE_COLLECTIONS_INLINE
    public
    #else
    internal
    #endif
    struct FixedArray2<T> : IFixedArray<T>
        where T : class?
    {
        private const int DefaultCapacity = 2;
            
        private object element0;

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Span<T> AsSpan()
        {
            return MemoryMarshal.CreateSpan(ref Unsafe.As<object, T>(ref element0), DefaultCapacity);
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int IndexOf(T item)
        {
            return MemoryMarshal.CreateSpan(ref Unsafe.As<object, IntPtr>(ref element0), DefaultCapacity).IndexOf(Unsafe.As<T, IntPtr>(ref item));
        }
    }
}