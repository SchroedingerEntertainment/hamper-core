// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Soe.Collections.Embedded
{
    #if EXPORT_HAMPER_CORE_COLLECTIONS_EMBEDDED
    public
    #else
    internal
    #endif
    partial struct EmbeddedStack<T> /*: IEnumerable<T>, IReadOnlyCollection<T>*/
    {
        private T[]? buffer;

        public int Capacity
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return buffer?.Length ?? 0; }
        }

        private int count;
        
        public int Count
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return count; }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public EmbeddedStack()
        { }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public EmbeddedStack(int capacity)
        {
            EnsureCapacity(capacity);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void EnsureCapacity(int capacity)
        {
            capacity = Math.Max(4, capacity);
            if (Capacity < capacity)
            {
                Array.Resize(ref buffer, capacity.NextPowerOfTwo());
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Span<T> AsSpan()
        {
            return new Span<T>(buffer, 0, count);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlySpan<T> AsReadOnlySpan()
        {
            return new ReadOnlySpan<T>(buffer, 0, count);
        }
    }
}