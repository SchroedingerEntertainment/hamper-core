// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;

namespace System
{
    /// <summary>
    /// Handles iteration over a collection of read-only elements of type <typeparamref name="T"/>
    /// </summary>
    /// <typeparam name="T">The element type of the collection</typeparam>
    #if EXPORT_HAMPER_CORE_SHARP
    public
    #else
    internal
    #endif
    static class ReadOnlyIterator<T>
    {
        /// <summary>
        /// Iterates through a collection from the first to the last element
        /// </summary>
        public struct DefaultStrategy : IReadOnlyIteratorStrategy<T>
        {
            /// <inheritdoc/>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static int Begin(ReadOnlySpan<T> source)
            {
                return -1;
            }

            /// <inheritdoc/>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool MoveNext(ReadOnlySpan<T> source, ref int index)
            {
                return (++index < source.Length);
            }
        }
    }
    /// <summary>
    /// Handles iteration over a collection of read-only elements of type <typeparamref name="T"/>
    /// </summary>
    /// <typeparam name="T">The element type of the collection</typeparam>
    /// <typeparam name="Strategy">A strategy controlling the behavior of the iterator</typeparam>
    /// <param name="source">The collection this iterator instance handles</param>
    [method: MethodImpl(MethodImplOptions.AggressiveInlining)]
    #if EXPORT_HAMPER_CORE_SHARP
    public
    #else
    internal
    #endif
    ref struct ReadOnlyIterator<T, Strategy>(ReadOnlySpan<T> source)
        where Strategy : struct, IReadOnlyIteratorStrategy<T>
    {
        private readonly ReadOnlySpan<T> source = source;
        private int index = Strategy.Begin(source);
        
        /// <summary>
        /// Gets the element in the collection at the current position of this iterator
        /// </summary>
        public T Current
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return source[index]; }
        }

        /// <summary>
        /// Advances the iterator to the next element of the collection
        /// </summary>
        /// <returns>True if the iterator was successfully advanced to the next element, false otherwise</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool MoveNext()
        {
            return Strategy.MoveNext(source, ref index);
        }

        /// <summary>
        /// Sets the iterator to its initial position
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Reset()
        {
            index = Strategy.Begin(source);
        }
    }
    /// <summary>
    /// Handles iteration over a collection of read-only elements of type <typeparamref name="T"/>
    /// </summary>
    /// <typeparam name="T1">The element type of the collection</typeparam>
    /// <typeparam name="Strategy1">A strategy controlling the behavior of the iterator</typeparam>
    /// <typeparam name="T2">The element type of the collection</typeparam>
    /// <typeparam name="Strategy2">A strategy controlling the behavior of the iterator</typeparam>
    /// <param name="s1">The collection this iterator instance handles</param>
    /// <param name="s2">The collection this iterator instance handles</param>
    [method: MethodImpl(MethodImplOptions.AggressiveInlining)]
    #if EXPORT_HAMPER_CORE_SHARP
    public
    #else
    internal
    #endif
    ref struct ReadOnlyIterator<T1, Strategy1, T2, Strategy2>(ReadOnlySpan<T1> s1, ReadOnlySpan<T2> s2)
        where Strategy1 : struct, IReadOnlyIteratorStrategy<T1>
        where Strategy2 : struct, IReadOnlyIteratorStrategy<T2>
    {
        private readonly ReadOnlySpan<T1> s1 = s1;
        private readonly ReadOnlySpan<T2> s2 = s2;
        private int i1 = Strategy1.Begin(s1);
        private int i2 = Strategy2.Begin(s2);
        
        /// <summary>
        /// Gets the element in the collection at the current position of this iterator
        /// </summary>
        public ValueTuple<T1, T2> Current
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ValueTuple<T1, T2>(s1[i1], s2[i2]); }
        }

        /// <summary>
        /// Advances the iterator to the next element of the collection
        /// </summary>
        /// <returns>True if the iterator was successfully advanced to the next element, false otherwise</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool MoveNext()
        {
            return Strategy1.MoveNext(s1, ref i1) &&
                   Strategy2.MoveNext(s2, ref i2);
        }

        /// <summary>
        /// Sets the iterator to its initial position
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Reset()
        {
            i1 = Strategy1.Begin(s1);
            i2 = Strategy2.Begin(s2);
        }
    }
    /// <summary>
    /// Handles iteration over a collection of read-only elements of type <typeparamref name="T"/>
    /// </summary>
    /// <typeparam name="T1">The element type of the collection</typeparam>
    /// <typeparam name="Strategy1">A strategy controlling the behavior of the iterator</typeparam>
    /// <typeparam name="T2">The element type of the collection</typeparam>
    /// <typeparam name="Strategy2">A strategy controlling the behavior of the iterator</typeparam>
    /// <typeparam name="T3">The element type of the collection</typeparam>
    /// <typeparam name="Strategy3">A strategy controlling the behavior of the iterator</typeparam>
    /// <param name="s1">The collection this iterator instance handles</param>
    /// <param name="s2">The collection this iterator instance handles</param>
    /// <param name="s3">The collection this iterator instance handles</param>
    [method: MethodImpl(MethodImplOptions.AggressiveInlining)]
    #if EXPORT_HAMPER_CORE_SHARP
    public
    #else
    internal
    #endif
    ref struct ReadOnlyIterator<T1, Strategy1, T2, Strategy2, T3, Strategy3>(ReadOnlySpan<T1> s1, ReadOnlySpan<T2> s2, ReadOnlySpan<T3> s3)
        where Strategy1 : struct, IReadOnlyIteratorStrategy<T1>
        where Strategy2 : struct, IReadOnlyIteratorStrategy<T2>
        where Strategy3 : struct, IReadOnlyIteratorStrategy<T3>
    {
        private readonly ReadOnlySpan<T1> s1 = s1;
        private readonly ReadOnlySpan<T2> s2 = s2;
        private readonly ReadOnlySpan<T3> s3 = s3;
        private int i1 = Strategy1.Begin(s1);
        private int i2 = Strategy2.Begin(s2);
        private int i3 = Strategy3.Begin(s3);
        
        /// <summary>
        /// Gets the element in the collection at the current position of this iterator
        /// </summary>
        public ValueTuple<T1, T2, T3> Current
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ValueTuple<T1, T2, T3>(s1[i1], s2[i2], s3[i3]); }
        }

        /// <summary>
        /// Advances the iterator to the next element of the collection
        /// </summary>
        /// <returns>True if the iterator was successfully advanced to the next element, false otherwise</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool MoveNext()
        {
            return Strategy1.MoveNext(s1, ref i1) &&
                   Strategy2.MoveNext(s2, ref i2) &&
                   Strategy3.MoveNext(s3, ref i3);
        }

        /// <summary>
        /// Sets the iterator to its initial position
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Reset()
        {
            i1 = Strategy1.Begin(s1);
            i2 = Strategy2.Begin(s2);
            i3 = Strategy3.Begin(s3);
        }
    }
    /// <summary>
    /// Handles iteration over a collection of read-only elements of type <typeparamref name="T"/>
    /// </summary>
    /// <typeparam name="T1">The element type of the collection</typeparam>
    /// <typeparam name="Strategy1">A strategy controlling the behavior of the iterator</typeparam>
    /// <typeparam name="T2">The element type of the collection</typeparam>
    /// <typeparam name="Strategy2">A strategy controlling the behavior of the iterator</typeparam>
    /// <typeparam name="T3">The element type of the collection</typeparam>
    /// <typeparam name="Strategy3">A strategy controlling the behavior of the iterator</typeparam>
    /// <typeparam name="T4">The element type of the collection</typeparam>
    /// <typeparam name="Strategy4">A strategy controlling the behavior of the iterator</typeparam>
    /// <param name="s1">The collection this iterator instance handles</param>
    /// <param name="s2">The collection this iterator instance handles</param>
    /// <param name="s3">The collection this iterator instance handles</param>
    /// <param name="s4">The collection this iterator instance handles</param>
    [method: MethodImpl(MethodImplOptions.AggressiveInlining)]
    #if EXPORT_HAMPER_CORE_SHARP
    public
    #else
    internal
    #endif
    ref struct ReadOnlyIterator<T1, Strategy1, T2, Strategy2, T3, Strategy3, T4, Strategy4>(ReadOnlySpan<T1> s1, ReadOnlySpan<T2> s2, ReadOnlySpan<T3> s3, ReadOnlySpan<T4> s4)
        where Strategy1 : struct, IReadOnlyIteratorStrategy<T1>
        where Strategy2 : struct, IReadOnlyIteratorStrategy<T2>
        where Strategy3 : struct, IReadOnlyIteratorStrategy<T3>
        where Strategy4 : struct, IReadOnlyIteratorStrategy<T4>
    {
        private readonly ReadOnlySpan<T1> s1 = s1;
        private readonly ReadOnlySpan<T2> s2 = s2;
        private readonly ReadOnlySpan<T3> s3 = s3;
        private readonly ReadOnlySpan<T4> s4 = s4;
        private int i1 = Strategy1.Begin(s1);
        private int i2 = Strategy2.Begin(s2);
        private int i3 = Strategy3.Begin(s3);
        private int i4 = Strategy4.Begin(s4);
        
        /// <summary>
        /// Gets the element in the collection at the current position of this iterator
        /// </summary>
        public ValueTuple<T1, T2, T3, T4> Current
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ValueTuple<T1, T2, T3, T4>(s1[i1], s2[i2], s3[i3], s4[i4]); }
        }

        /// <summary>
        /// Advances the iterator to the next element of the collection
        /// </summary>
        /// <returns>True if the iterator was successfully advanced to the next element, false otherwise</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool MoveNext()
        {
            return Strategy1.MoveNext(s1, ref i1) &&
                   Strategy2.MoveNext(s2, ref i2) &&
                   Strategy3.MoveNext(s3, ref i3) &&
                   Strategy4.MoveNext(s4, ref i4);
        }

        /// <summary>
        /// Sets the iterator to its initial position
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Reset()
        {
            i1 = Strategy1.Begin(s1);
            i2 = Strategy2.Begin(s2);
            i3 = Strategy3.Begin(s3);
            i4 = Strategy4.Begin(s4);
        }
    }
    /// <summary>
    /// Handles iteration over a collection of read-only elements of type <typeparamref name="T"/>
    /// </summary>
    /// <typeparam name="T1">The element type of the collection</typeparam>
    /// <typeparam name="Strategy1">A strategy controlling the behavior of the iterator</typeparam>
    /// <typeparam name="T2">The element type of the collection</typeparam>
    /// <typeparam name="Strategy2">A strategy controlling the behavior of the iterator</typeparam>
    /// <typeparam name="T3">The element type of the collection</typeparam>
    /// <typeparam name="Strategy3">A strategy controlling the behavior of the iterator</typeparam>
    /// <typeparam name="T4">The element type of the collection</typeparam>
    /// <typeparam name="Strategy4">A strategy controlling the behavior of the iterator</typeparam>
    /// <typeparam name="T5">The element type of the collection</typeparam>
    /// <typeparam name="Strategy5">A strategy controlling the behavior of the iterator</typeparam>
    /// <param name="s1">The collection this iterator instance handles</param>
    /// <param name="s2">The collection this iterator instance handles</param>
    /// <param name="s3">The collection this iterator instance handles</param>
    /// <param name="s4">The collection this iterator instance handles</param>
    /// <param name="s5">The collection this iterator instance handles</param>
    [method: MethodImpl(MethodImplOptions.AggressiveInlining)]
    #if EXPORT_HAMPER_CORE_SHARP
    public
    #else
    internal
    #endif
    ref struct ReadOnlyIterator<T1, Strategy1, T2, Strategy2, T3, Strategy3, T4, Strategy4, T5, Strategy5>(ReadOnlySpan<T1> s1, ReadOnlySpan<T2> s2, ReadOnlySpan<T3> s3, ReadOnlySpan<T4> s4, ReadOnlySpan<T5> s5)
        where Strategy1 : struct, IReadOnlyIteratorStrategy<T1>
        where Strategy2 : struct, IReadOnlyIteratorStrategy<T2>
        where Strategy3 : struct, IReadOnlyIteratorStrategy<T3>
        where Strategy4 : struct, IReadOnlyIteratorStrategy<T4>
        where Strategy5 : struct, IReadOnlyIteratorStrategy<T5>
    {
        private readonly ReadOnlySpan<T1> s1 = s1;
        private readonly ReadOnlySpan<T2> s2 = s2;
        private readonly ReadOnlySpan<T3> s3 = s3;
        private readonly ReadOnlySpan<T4> s4 = s4;
        private readonly ReadOnlySpan<T5> s5 = s5;
        private int i1 = Strategy1.Begin(s1);
        private int i2 = Strategy2.Begin(s2);
        private int i3 = Strategy3.Begin(s3);
        private int i4 = Strategy4.Begin(s4);
        private int i5 = Strategy5.Begin(s5);
        
        /// <summary>
        /// Gets the element in the collection at the current position of this iterator
        /// </summary>
        public ValueTuple<T1, T2, T3, T4, T5> Current
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ValueTuple<T1, T2, T3, T4, T5>(s1[i1], s2[i2], s3[i3], s4[i4], s5[i5]); }
        }

        /// <summary>
        /// Advances the iterator to the next element of the collection
        /// </summary>
        /// <returns>True if the iterator was successfully advanced to the next element, false otherwise</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool MoveNext()
        {
            return Strategy1.MoveNext(s1, ref i1) &&
                   Strategy2.MoveNext(s2, ref i2) &&
                   Strategy3.MoveNext(s3, ref i3) &&
                   Strategy4.MoveNext(s4, ref i4) &&
                   Strategy5.MoveNext(s5, ref i5);
        }

        /// <summary>
        /// Sets the iterator to its initial position
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Reset()
        {
            i1 = Strategy1.Begin(s1);
            i2 = Strategy2.Begin(s2);
            i3 = Strategy3.Begin(s3);
            i4 = Strategy4.Begin(s4);
            i5 = Strategy5.Begin(s5);
        }
    }
    /// <summary>
    /// Handles iteration over a collection of read-only elements of type <typeparamref name="T"/>
    /// </summary>
    /// <typeparam name="T1">The element type of the collection</typeparam>
    /// <typeparam name="Strategy1">A strategy controlling the behavior of the iterator</typeparam>
    /// <typeparam name="T2">The element type of the collection</typeparam>
    /// <typeparam name="Strategy2">A strategy controlling the behavior of the iterator</typeparam>
    /// <typeparam name="T3">The element type of the collection</typeparam>
    /// <typeparam name="Strategy3">A strategy controlling the behavior of the iterator</typeparam>
    /// <typeparam name="T4">The element type of the collection</typeparam>
    /// <typeparam name="Strategy4">A strategy controlling the behavior of the iterator</typeparam>
    /// <typeparam name="T5">The element type of the collection</typeparam>
    /// <typeparam name="Strategy5">A strategy controlling the behavior of the iterator</typeparam>
    /// <typeparam name="T6">The element type of the collection</typeparam>
    /// <typeparam name="Strategy6">A strategy controlling the behavior of the iterator</typeparam>
    /// <param name="s1">The collection this iterator instance handles</param>
    /// <param name="s2">The collection this iterator instance handles</param>
    /// <param name="s3">The collection this iterator instance handles</param>
    /// <param name="s4">The collection this iterator instance handles</param>
    /// <param name="s5">The collection this iterator instance handles</param>
    /// <param name="s6">The collection this iterator instance handles</param>
    [method: MethodImpl(MethodImplOptions.AggressiveInlining)]
    #if EXPORT_HAMPER_CORE_SHARP
    public
    #else
    internal
    #endif
    ref struct ReadOnlyIterator<T1, Strategy1, T2, Strategy2, T3, Strategy3, T4, Strategy4, T5, Strategy5, T6, Strategy6>(ReadOnlySpan<T1> s1, ReadOnlySpan<T2> s2, ReadOnlySpan<T3> s3, ReadOnlySpan<T4> s4, ReadOnlySpan<T5> s5, ReadOnlySpan<T6> s6)
        where Strategy1 : struct, IReadOnlyIteratorStrategy<T1>
        where Strategy2 : struct, IReadOnlyIteratorStrategy<T2>
        where Strategy3 : struct, IReadOnlyIteratorStrategy<T3>
        where Strategy4 : struct, IReadOnlyIteratorStrategy<T4>
        where Strategy5 : struct, IReadOnlyIteratorStrategy<T5>
        where Strategy6 : struct, IReadOnlyIteratorStrategy<T6>
    {
        private readonly ReadOnlySpan<T1> s1 = s1;
        private readonly ReadOnlySpan<T2> s2 = s2;
        private readonly ReadOnlySpan<T3> s3 = s3;
        private readonly ReadOnlySpan<T4> s4 = s4;
        private readonly ReadOnlySpan<T5> s5 = s5;
        private readonly ReadOnlySpan<T6> s6 = s6;
        private int i1 = Strategy1.Begin(s1);
        private int i2 = Strategy2.Begin(s2);
        private int i3 = Strategy3.Begin(s3);
        private int i4 = Strategy4.Begin(s4);
        private int i5 = Strategy5.Begin(s5);
        private int i6 = Strategy6.Begin(s6);
        
        /// <summary>
        /// Gets the element in the collection at the current position of this iterator
        /// </summary>
        public ValueTuple<T1, T2, T3, T4, T5, T6> Current
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new ValueTuple<T1, T2, T3, T4, T5, T6>(s1[i1], s2[i2], s3[i3], s4[i4], s5[i5], s6[i6]); }
        }

        /// <summary>
        /// Advances the iterator to the next element of the collection
        /// </summary>
        /// <returns>True if the iterator was successfully advanced to the next element, false otherwise</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool MoveNext()
        {
            return Strategy1.MoveNext(s1, ref i1) &&
                   Strategy2.MoveNext(s2, ref i2) &&
                   Strategy3.MoveNext(s3, ref i3) &&
                   Strategy4.MoveNext(s4, ref i4) &&
                   Strategy5.MoveNext(s5, ref i5) &&
                   Strategy6.MoveNext(s6, ref i6);
        }

        /// <summary>
        /// Sets the iterator to its initial position
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Reset()
        {
            i1 = Strategy1.Begin(s1);
            i2 = Strategy2.Begin(s2);
            i3 = Strategy3.Begin(s3);
            i4 = Strategy4.Begin(s4);
            i5 = Strategy5.Begin(s5);
            i6 = Strategy6.Begin(s6);
        }
    }
}