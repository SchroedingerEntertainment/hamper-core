// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;

namespace System
{
    /// <summary>
    /// Provides a unified accessor over a sequence of <see cref="ReadOnlySpan{T}"/>
    /// </summary>
    /// <typeparam name="T">The type of items in this sequence</typeparam>
    /// <param name="left">The left part of the sequence</param>
    /// <param name="right">The right part of the sequence</param>
    [method: MethodImpl(MethodImplOptions.AggressiveInlining)]
    #if EXPORT_HAMPER_CORE_SHARP
    public
    #else
    internal
    #endif
    readonly ref struct ReadOnlySpanSequence<T>(ReadOnlySpan<T> left, ReadOnlyUniteSpan<T> right)
    {
        private readonly ReadOnlySpan<T> left = left;
        /// <summary>
        /// Returns the left portion of this sequence
        /// </summary>
        public ReadOnlySpan<T> Left
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return left; }
        }
        
        private readonly ReadOnlyUniteSpan<T> right = right;
        /// <summary>
        /// Returns the right portion of this sequence
        /// </summary>
        public ReadOnlyUniteSpan<T> Right
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return right; }
        }
        
        /// <summary>
        /// The number of items in this sequence
        /// </summary>
        public int Length
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return left.Length + right.Length; }
        }
        
        /// <summary>
        /// Gets a value indicating whether this sequence is empty.
        /// </summary>
        public bool IsEmpty
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return Length == 0; }
        }

        /// <summary>
        /// Gets the item at the provided index
        /// </summary>
        /// <param name="index">A zero-based index to get the item for</param>
        public T this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                if (index >= left.Length)
                {
                    return right[index - left.Length];
                }
                else return left[index];
            }
        }
        
        /// <summary>
        /// Copies the contents of this sequence into a destination <see cref="Span{T}"/>
        /// </summary>
        /// <param name="destination">The destination <see cref="Span{T}"/> object</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void CopyTo(Span<T> destination)
        {
            left.CopyTo(destination);
            right.CopyTo(destination.Slice(left.Length));
        }
        
        /// <summary>
        /// Copies the contents of this sequence into a new array. This heap
        /// allocates, so should generally be avoided, however it is sometimes
        /// necessary to bridge the gap with APIs written in terms of arrays.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T[] ToArray()
        {
            if (IsEmpty)
            {
                return Array.Empty<T>();
            }
            else
            {
                T[] destination = new T[Length];
                CopyTo(destination);
                return destination;
            }
        }
    }
}