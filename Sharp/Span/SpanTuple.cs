// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;

namespace System
{
    /// <summary>
    /// Represents an n-value tuple
    /// </summary>
    #if EXPORT_HAMPER_CORE_SHARP
    public
    #else
    internal
    #endif
    static partial class SpanTuple
    {
        /// <summary>
        /// Creates a new n-value tuple, where n is 1
        /// </summary>
        /// <param name="item1">The tuple's first element</param>
        /// <typeparam name="T">The type of the tuple's first element</typeparam>
        /// <returns>The newly created tuple</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SpanTuple<T> Create<T>(Span<T> item1)
        {
            return new SpanTuple<T>(item1);
        }
        /// <summary>
        /// Creates a new n-value tuple, where n is 7
        /// </summary>
        /// <param name="item1">The tuple's first element</param>
        /// <param name="item2">The tuple's second element</param>
        /// <typeparam name="T1">The type of the tuple's first element</typeparam>
        /// <typeparam name="T2">The type of the tuple's second element</typeparam>
        /// <returns>The newly created tuple</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SpanTuple<T1, T2> Create<T1, T2>(Span<T1> item1, Span<T2> item2)
        {
            return new SpanTuple<T1, T2>(item1, item2);
        }
        /// <summary>
        /// Creates a new n-value tuple, where n is 7
        /// </summary>
        /// <param name="item1">The tuple's first element</param>
        /// <param name="items">The tuple's second elements</param>
        /// <typeparam name="T1">The type of the tuple's first element</typeparam>
        /// <typeparam name="T2">The type of the tuple's second element</typeparam>
        /// <returns>The newly created tuple</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SpanTuple<T1, T2> Create<T1, T2>(Span<T1> item1, SpanTuple<T2> items)
        {
            return new SpanTuple<T1, T2>(item1, items.Item1);
        }
        /// <summary>
        /// Creates a new combined n-value tuple, where n is 7
        /// </summary>
        /// <param name="items">The tuple's first elements</param>
        /// <param name="item2">The tuple's second element</param>
        /// <typeparam name="T1">The type of the tuple's first element</typeparam>
        /// <typeparam name="T2">The type of the tuple's second element</typeparam>
        /// <returns>The newly created tuple</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SpanTuple<T1, T2> Create<T1, T2>(SpanTuple<T1> items, Span<T2> item2)
        {
            return new SpanTuple<T1, T2>(items.Item1, item2);
        }
        /// <summary>
        /// Creates a new n-value tuple, where n is 7
        /// </summary>
        /// <param name="item1">The tuple's first element</param>
        /// <param name="item2">The tuple's second element</param>
        /// <param name="item3">The tuple's third element</param>
        /// <typeparam name="T1">The type of the tuple's first element</typeparam>
        /// <typeparam name="T2">The type of the tuple's second element</typeparam>
        /// <typeparam name="T3">The type of the tuple's third element</typeparam>
        /// <returns>The newly created tuple</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SpanTuple<T1, T2, T3> Create<T1, T2, T3>(Span<T1> item1, Span<T2> item2, Span<T3> item3)
        {
            return new SpanTuple<T1, T2, T3>(item1, item2, item3);
        }
        /// <summary>
        /// Creates a new n-value tuple, where n is 7
        /// </summary>
        /// <param name="item1">The tuple's first element</param>
        /// <param name="items">The tuple's second elements</param>
        /// <typeparam name="T1">The type of the tuple's first element</typeparam>
        /// <typeparam name="T2">The type of the tuple's second element</typeparam>
        /// <typeparam name="T3">The type of the tuple's third element</typeparam>
        /// <returns>The newly created tuple</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SpanTuple<T1, T2, T3> Create<T1, T2, T3>(Span<T1> item1, SpanTuple<T2, T3> items)
        {
            return new SpanTuple<T1, T2, T3>(item1, items.Item1, items.Item2);
        }
        /// <summary>
        /// Creates a new combined n-value tuple, where n is 7
        /// </summary>
        /// <param name="items">The tuple's first elements</param>
        /// <param name="item3">The tuple's third element</param>
        /// <typeparam name="T1">The type of the tuple's first element</typeparam>
        /// <typeparam name="T2">The type of the tuple's second element</typeparam>
        /// <typeparam name="T3">The type of the tuple's third element</typeparam>
        /// <returns>The newly created tuple</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SpanTuple<T1, T2, T3> Create<T1, T2, T3>(SpanTuple<T1, T2> items, Span<T3> item3)
        {
            return new SpanTuple<T1, T2, T3>(items.Item1, items.Item2, item3);
        }
        /// <summary>
        /// Creates a new n-value tuple, where n is 7
        /// </summary>
        /// <param name="item1">The tuple's first element</param>
        /// <param name="item2">The tuple's second element</param>
        /// <param name="item3">The tuple's third element</param>
        /// <param name="item4">The tuple's fourth element</param>
        /// <typeparam name="T1">The type of the tuple's first element</typeparam>
        /// <typeparam name="T2">The type of the tuple's second element</typeparam>
        /// <typeparam name="T3">The type of the tuple's third element</typeparam>
        /// <typeparam name="T4">The type of the tuple's fourth element</typeparam>
        /// <returns>The newly created tuple</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SpanTuple<T1, T2, T3, T4> Create<T1, T2, T3, T4>(Span<T1> item1, Span<T2> item2, Span<T3> item3, Span<T4> item4)
        {
            return new SpanTuple<T1, T2, T3, T4>(item1, item2, item3, item4);
        }
        /// <summary>
        /// Creates a new n-value tuple, where n is 7
        /// </summary>
        /// <param name="item1">The tuple's first element</param>
        /// <param name="items">The tuple's second elements</param>
        /// <typeparam name="T1">The type of the tuple's first element</typeparam>
        /// <typeparam name="T2">The type of the tuple's second element</typeparam>
        /// <typeparam name="T3">The type of the tuple's third element</typeparam>
        /// <typeparam name="T4">The type of the tuple's fourth element</typeparam>
        /// <returns>The newly created tuple</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SpanTuple<T1, T2, T3, T4> Create<T1, T2, T3, T4>(Span<T1> item1, SpanTuple<T2, T3, T4> items)
        {
            return new SpanTuple<T1, T2, T3, T4>(item1, items.Item1, items.Item2, items.Item3);
        }
        /// <summary>
        /// Creates a new combined n-value tuple, where n is 7
        /// </summary>
        /// <param name="items">The tuple's first elements</param>
        /// <param name="item4">The tuple's fourth element</param>
        /// <typeparam name="T1">The type of the tuple's first element</typeparam>
        /// <typeparam name="T2">The type of the tuple's second element</typeparam>
        /// <typeparam name="T3">The type of the tuple's third element</typeparam>
        /// <typeparam name="T4">The type of the tuple's fourth element</typeparam>
        /// <returns>The newly created tuple</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SpanTuple<T1, T2, T3, T4> Create<T1, T2, T3, T4>(SpanTuple<T1, T2, T3> items, Span<T4> item4)
        {
            return new SpanTuple<T1, T2, T3, T4>(items.Item1, items.Item2, items.Item3, item4);
        }
        /// <summary>
        /// Creates a new n-value tuple, where n is 7
        /// </summary>
        /// <param name="item1">The tuple's first element</param>
        /// <param name="item2">The tuple's second element</param>
        /// <param name="item3">The tuple's third element</param>
        /// <param name="item4">The tuple's fourth element</param>
        /// <param name="item5">The tuple's fifth element</param>
        /// <typeparam name="T1">The type of the tuple's first element</typeparam>
        /// <typeparam name="T2">The type of the tuple's second element</typeparam>
        /// <typeparam name="T3">The type of the tuple's third element</typeparam>
        /// <typeparam name="T4">The type of the tuple's fourth element</typeparam>
        /// <typeparam name="T5">The type of the tuple's fifth element</typeparam>
        /// <returns>The newly created tuple</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SpanTuple<T1, T2, T3, T4, T5> Create<T1, T2, T3, T4, T5>(Span<T1> item1, Span<T2> item2, Span<T3> item3, Span<T4> item4, Span<T5> item5)
        {
            return new SpanTuple<T1, T2, T3, T4, T5>(item1, item2, item3, item4, item5);
        }
        /// <summary>
        /// Creates a new n-value tuple, where n is 7
        /// </summary>
        /// <param name="item1">The tuple's first element</param>
        /// <param name="items">The tuple's second elements</param>
        /// <typeparam name="T1">The type of the tuple's first element</typeparam>
        /// <typeparam name="T2">The type of the tuple's second element</typeparam>
        /// <typeparam name="T3">The type of the tuple's third element</typeparam>
        /// <typeparam name="T4">The type of the tuple's fourth element</typeparam>
        /// <typeparam name="T5">The type of the tuple's fifth element</typeparam>
        /// <returns>The newly created tuple</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SpanTuple<T1, T2, T3, T4, T5> Create<T1, T2, T3, T4, T5>(Span<T1> item1, SpanTuple<T2, T3, T4, T5> items)
        {
            return new SpanTuple<T1, T2, T3, T4, T5>(item1, items.Item1, items.Item2, items.Item3, items.Item4);
        }
        /// <summary>
        /// Creates a new combined n-value tuple, where n is 7
        /// </summary>
        /// <param name="items">The tuple's first elements</param>
        /// <param name="item5">The tuple's fifth element</param>
        /// <typeparam name="T1">The type of the tuple's first element</typeparam>
        /// <typeparam name="T2">The type of the tuple's second element</typeparam>
        /// <typeparam name="T3">The type of the tuple's third element</typeparam>
        /// <typeparam name="T4">The type of the tuple's fourth element</typeparam>
        /// <typeparam name="T5">The type of the tuple's fifth element</typeparam>
        /// <returns>The newly created tuple</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SpanTuple<T1, T2, T3, T4, T5> Create<T1, T2, T3, T4, T5>(SpanTuple<T1, T2, T3, T4> items, Span<T5> item5)
        {
            return new SpanTuple<T1, T2, T3, T4, T5>(items.Item1, items.Item2, items.Item3, items.Item4, item5);
        }
        /// <summary>
        /// Creates a new n-value tuple, where n is 7
        /// </summary>
        /// <param name="item1">The tuple's first element</param>
        /// <param name="item2">The tuple's second element</param>
        /// <param name="item3">The tuple's third element</param>
        /// <param name="item4">The tuple's fourth element</param>
        /// <param name="item5">The tuple's fifth element</param>
        /// <param name="item6">The tuple's sixth element</param>
        /// <typeparam name="T1">The type of the tuple's first element</typeparam>
        /// <typeparam name="T2">The type of the tuple's second element</typeparam>
        /// <typeparam name="T3">The type of the tuple's third element</typeparam>
        /// <typeparam name="T4">The type of the tuple's fourth element</typeparam>
        /// <typeparam name="T5">The type of the tuple's fifth element</typeparam>
        /// <typeparam name="T6">The type of the tuple's sixth element</typeparam>
        /// <returns>The newly created tuple</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SpanTuple<T1, T2, T3, T4, T5, T6> Create<T1, T2, T3, T4, T5, T6>(Span<T1> item1, Span<T2> item2, Span<T3> item3, Span<T4> item4, Span<T5> item5, Span<T6> item6)
        {
            return new SpanTuple<T1, T2, T3, T4, T5, T6>(item1, item2, item3, item4, item5, item6);
        }
        /// <summary>
        /// Creates a new n-value tuple, where n is 7
        /// </summary>
        /// <param name="item1">The tuple's first element</param>
        /// <param name="items">The tuple's second elements</param>
        /// <typeparam name="T1">The type of the tuple's first element</typeparam>
        /// <typeparam name="T2">The type of the tuple's second element</typeparam>
        /// <typeparam name="T3">The type of the tuple's third element</typeparam>
        /// <typeparam name="T4">The type of the tuple's fourth element</typeparam>
        /// <typeparam name="T5">The type of the tuple's fifth element</typeparam>
        /// <typeparam name="T6">The type of the tuple's sixth element</typeparam>
        /// <returns>The newly created tuple</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SpanTuple<T1, T2, T3, T4, T5, T6> Create<T1, T2, T3, T4, T5, T6>(Span<T1> item1, SpanTuple<T2, T3, T4, T5, T6> items)
        {
            return new SpanTuple<T1, T2, T3, T4, T5, T6>(item1, items.Item1, items.Item2, items.Item3, items.Item4, items.Item5);
        }
        /// <summary>
        /// Creates a new combined n-value tuple, where n is 7
        /// </summary>
        /// <param name="items">The tuple's first elements</param>
        /// <param name="item6">The tuple's sixth element</param>
        /// <typeparam name="T1">The type of the tuple's first element</typeparam>
        /// <typeparam name="T2">The type of the tuple's second element</typeparam>
        /// <typeparam name="T3">The type of the tuple's third element</typeparam>
        /// <typeparam name="T4">The type of the tuple's fourth element</typeparam>
        /// <typeparam name="T5">The type of the tuple's fifth element</typeparam>
        /// <typeparam name="T6">The type of the tuple's sixth element</typeparam>
        /// <returns>The newly created tuple</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SpanTuple<T1, T2, T3, T4, T5, T6> Create<T1, T2, T3, T4, T5, T6>(SpanTuple<T1, T2, T3, T4, T5> items, Span<T6> item6)
        {
            return new SpanTuple<T1, T2, T3, T4, T5, T6>(items.Item1, items.Item2, items.Item3, items.Item4, items.Item5, item6);
        }
        /// <summary>
        /// Creates a new n-value tuple, where n is 7
        /// </summary>
        /// <param name="item1">The tuple's first element</param>
        /// <param name="item2">The tuple's second element</param>
        /// <param name="item3">The tuple's third element</param>
        /// <param name="item4">The tuple's fourth element</param>
        /// <param name="item5">The tuple's fifth element</param>
        /// <param name="item6">The tuple's sixth element</param>
        /// <param name="item7">The tuple's seventh element</param>
        /// <typeparam name="T1">The type of the tuple's first element</typeparam>
        /// <typeparam name="T2">The type of the tuple's second element</typeparam>
        /// <typeparam name="T3">The type of the tuple's third element</typeparam>
        /// <typeparam name="T4">The type of the tuple's fourth element</typeparam>
        /// <typeparam name="T5">The type of the tuple's fifth element</typeparam>
        /// <typeparam name="T6">The type of the tuple's sixth element</typeparam>
        /// <typeparam name="T7">The type of the tuple's seventh element</typeparam>
        /// <returns>The newly created tuple</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SpanTuple<T1, T2, T3, T4, T5, T6, T7> Create<T1, T2, T3, T4, T5, T6, T7>(Span<T1> item1, Span<T2> item2, Span<T3> item3, Span<T4> item4, Span<T5> item5, Span<T6> item6, Span<T7> item7)
        {
            return new SpanTuple<T1, T2, T3, T4, T5, T6, T7>(item1, item2, item3, item4, item5, item6, item7);
        }
        /// <summary>
        /// Creates a new n-value tuple, where n is 7
        /// </summary>
        /// <param name="item1">The tuple's first element</param>
        /// <param name="items">The tuple's second elements</param>
        /// <typeparam name="T1">The type of the tuple's first element</typeparam>
        /// <typeparam name="T2">The type of the tuple's second element</typeparam>
        /// <typeparam name="T3">The type of the tuple's third element</typeparam>
        /// <typeparam name="T4">The type of the tuple's fourth element</typeparam>
        /// <typeparam name="T5">The type of the tuple's fifth element</typeparam>
        /// <typeparam name="T6">The type of the tuple's sixth element</typeparam>
        /// <typeparam name="T7">The type of the tuple's seventh element</typeparam>
        /// <returns>The newly created tuple</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SpanTuple<T1, T2, T3, T4, T5, T6, T7> Create<T1, T2, T3, T4, T5, T6, T7>(Span<T1> item1, SpanTuple<T2, T3, T4, T5, T6, T7> items)
        {
            return new SpanTuple<T1, T2, T3, T4, T5, T6, T7>(item1, items.Item1, items.Item2, items.Item3, items.Item4, items.Item5, items.Item6);
        }
        /// <summary>
        /// Creates a new combined n-value tuple, where n is 7
        /// </summary>
        /// <param name="items">The tuple's first elements</param>
        /// <param name="item7">The tuple's seventh element</param>
        /// <typeparam name="T1">The type of the tuple's first element</typeparam>
        /// <typeparam name="T2">The type of the tuple's second element</typeparam>
        /// <typeparam name="T3">The type of the tuple's third element</typeparam>
        /// <typeparam name="T4">The type of the tuple's fourth element</typeparam>
        /// <typeparam name="T5">The type of the tuple's fifth element</typeparam>
        /// <typeparam name="T6">The type of the tuple's sixth element</typeparam>
        /// <typeparam name="T7">The type of the tuple's seventh element</typeparam>
        /// <returns>The newly created tuple</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SpanTuple<T1, T2, T3, T4, T5, T6, T7> Create<T1, T2, T3, T4, T5, T6, T7>(SpanTuple<T1, T2, T3, T4, T5, T6> items, Span<T7> item7)
        {
            return new SpanTuple<T1, T2, T3, T4, T5, T6, T7>(items.Item1, items.Item2, items.Item3, items.Item4, items.Item5, items.Item6, item7);
        }
        /// <summary>
        /// Creates a new n-value tuple, where n is 8 or greater
        /// </summary>
        /// <param name="item1">The tuple's first element</param>
        /// <param name="item2">The tuple's second element</param>
        /// <param name="item3">The tuple's third element</param>
        /// <param name="item4">The tuple's fourth element</param>
        /// <param name="item5">The tuple's fifth element</param>
        /// <param name="item6">The tuple's sixth element</param>
        /// <param name="item7">The tuple's seventh element</param>
        /// <param name="rest">An instance of any tuple type that contains the values of the tuple's remaining elements</param>
        /// <typeparam name="T1">The type of the tuple's first element</typeparam>
        /// <typeparam name="T2">The type of the tuple's second element</typeparam>
        /// <typeparam name="T3">The type of the tuple's third element</typeparam>
        /// <typeparam name="T4">The type of the tuple's fourth element</typeparam>
        /// <typeparam name="T5">The type of the tuple's fifth element</typeparam>
        /// <typeparam name="T6">The type of the tuple's sixth element</typeparam>
        /// <typeparam name="T7">The type of the tuple's seventh element</typeparam>
        /// <typeparam name="TRest">Any generic tuple instance that defines the types of the tuple's remaining elements</typeparam>
        /// <returns>The newly created tuple</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SpanTuple<T1, T2, T3, T4, T5, T6, T7, TRest> Create<T1, T2, T3, T4, T5, T6, T7, TRest>(Span<T1> item1, Span<T2> item2, Span<T3> item3, Span<T4> item4, Span<T5> item5, Span<T6> item6, Span<T7> item7, TRest rest)
            where TRest : struct, ISpanTuple
        {
            return new SpanTuple<T1, T2, T3, T4, T5, T6, T7, TRest>(item1, item2, item3, item4, item5, item6, item7, rest);
        }
    }
    /// <summary>
    /// Represents an n-value tuple, where n is 1
    /// </summary>
    /// <param name="item1">The tuple's first element</param>
    /// <typeparam name="T">The type of the tuple's first element</typeparam>
    [method: MethodImpl(MethodImplOptions.AggressiveInlining)]
    #if EXPORT_HAMPER_CORE_SHARP
    public
    #else
    internal
    #endif
    readonly ref struct SpanTuple<T>(Span<T> item1) : ISpanTuple
    {
        private readonly Span<T> item1 = item1;
        /// <summary>
        /// Gets the item at the first position of this tuple
        /// </summary>
        public Span<T> Item1
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return item1; }
        }
        
        /// <summary>
        /// Gets the number of elements in this tuple
        /// </summary>
        public int Count
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return 1; }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator ReadOnlySpanTuple<T>(SpanTuple<T> tuple)
        {
            return new ReadOnlySpanTuple<T>(tuple.item1);
        }
        
        /// <inheritdoc/>
        public int Length
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return item1.Length; }
        }
    }
    /// <summary>
    /// Represents an n-value tuple, where n is 2
    /// </summary>
    /// <param name="item1">The tuple's first element</param>
    /// <param name="item2">The tuple's second element.</param>
    /// <typeparam name="T1">The type of the tuple's first element</typeparam>
    /// <typeparam name="T2">The type of the tuple's second element</typeparam>
    [method: MethodImpl(MethodImplOptions.AggressiveInlining)]
    #if EXPORT_HAMPER_CORE_SHARP
    public
    #else
    internal
    #endif
    readonly ref struct SpanTuple<T1, T2>(Span<T1> item1, Span<T2> item2) : ISpanTuple
    {
        private readonly Span<T1> item1 = item1;
        /// <summary>
        /// Gets the item at the first position of this tuple
        /// </summary>
        public Span<T1> Item1
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return item1; }
        }
        
        private readonly Span<T2> item2 = item2;
        /// <summary>
        /// Gets the item at the second position of this tuple
        /// </summary>
        public Span<T2> Item2
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return item2; }
        }
        
        /// <summary>
        /// Gets the number of elements in this tuple
        /// </summary>
        public int Count
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return 2; }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator ReadOnlySpanTuple<T1, T2>(SpanTuple<T1, T2> tuple)
        {
            return new ReadOnlySpanTuple<T1, T2>(tuple.item1, tuple.item2);
        }
        
        /// <inheritdoc/>
        public int Length
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return (item1.Length + item2.Length); }
        }
    }
    /// <summary>
    /// Represents an n-value tuple, where n is 3
    /// </summary>
    /// <param name="item1">The tuple's first element</param>
    /// <param name="item2">The tuple's second element.</param>
    /// <param name="item3">The tuple's third element.</param>
    /// <typeparam name="T1">The type of the tuple's first element</typeparam>
    /// <typeparam name="T2">The type of the tuple's second element</typeparam>
    /// <typeparam name="T3">The type of the tuple's third element</typeparam>
    [method: MethodImpl(MethodImplOptions.AggressiveInlining)]
    #if EXPORT_HAMPER_CORE_SHARP
    public
    #else
    internal
    #endif
    readonly ref struct SpanTuple<T1, T2, T3>(Span<T1> item1, Span<T2> item2, Span<T3> item3) : ISpanTuple
    {
        private readonly Span<T1> item1 = item1;
        /// <summary>
        /// Gets the item at the first position of this tuple
        /// </summary>
        public Span<T1> Item1
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return item1; }
        }
        
        private readonly Span<T2> item2 = item2;
        /// <summary>
        /// Gets the item at the second position of this tuple
        /// </summary>
        public Span<T2> Item2
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return item2; }
        }
        
        private readonly Span<T3> item3 = item3;
        /// <summary>
        /// Gets the item at the third position of this tuple
        /// </summary>
        public Span<T3> Item3
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return item3; }
        }
        
        /// <summary>
        /// Gets the number of elements in this tuple
        /// </summary>
        public int Count
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return 3; }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator ReadOnlySpanTuple<T1, T2, T3>(SpanTuple<T1, T2, T3> tuple)
        {
            return new ReadOnlySpanTuple<T1, T2, T3>(tuple.item1, tuple.item2, tuple.item3);
        }
        
        /// <inheritdoc/>
        public int Length
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return (item1.Length + item2.Length + item3.Length); }
        }
    }
    /// <summary>
    /// Represents an n-value tuple, where n is 4
    /// </summary>
    /// <param name="item1">The tuple's first element</param>
    /// <param name="item2">The tuple's second element.</param>
    /// <param name="item3">The tuple's third element.</param>
    /// <param name="item4">The tuple's fourth element.</param>
    /// <typeparam name="T1">The type of the tuple's first element</typeparam>
    /// <typeparam name="T2">The type of the tuple's second element</typeparam>
    /// <typeparam name="T3">The type of the tuple's third element</typeparam>
    /// <typeparam name="T4">The type of the tuple's fourth element</typeparam>
    [method: MethodImpl(MethodImplOptions.AggressiveInlining)]
    #if EXPORT_HAMPER_CORE_SHARP
    public
    #else
    internal
    #endif
    readonly ref struct SpanTuple<T1, T2, T3, T4>(Span<T1> item1, Span<T2> item2, Span<T3> item3, Span<T4> item4) : ISpanTuple
    {
        private readonly Span<T1> item1 = item1;
        /// <summary>
        /// Gets the item at the first position of this tuple
        /// </summary>
        public Span<T1> Item1
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return item1; }
        }
        
        private readonly Span<T2> item2 = item2;
        /// <summary>
        /// Gets the item at the second position of this tuple
        /// </summary>
        public Span<T2> Item2
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return item2; }
        }
        
        private readonly Span<T3> item3 = item3;
        /// <summary>
        /// Gets the item at the third position of this tuple
        /// </summary>
        public Span<T3> Item3
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return item3; }
        }
        
        private readonly Span<T4> item4 = item4;
        /// <summary>
        /// Gets the item at the fourth position of this tuple
        /// </summary>
        public Span<T4> Item4
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return item4; }
        }
        
        /// <summary>
        /// Gets the number of elements in this tuple
        /// </summary>
        public int Count
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return 4; }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator ReadOnlySpanTuple<T1, T2, T3, T4>(SpanTuple<T1, T2, T3, T4> tuple)
        {
            return new ReadOnlySpanTuple<T1, T2, T3, T4>(tuple.item1, tuple.item2, tuple.item3, tuple.item4);
        }
        
        /// <inheritdoc/>
        public int Length
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return (item1.Length + item2.Length + item3.Length + item4.Length); }
        }
    }
    /// <summary>
    /// Represents an n-value tuple, where n is 5
    /// </summary>
    /// <param name="item1">The tuple's first element</param>
    /// <param name="item2">The tuple's second element.</param>
    /// <param name="item3">The tuple's third element.</param>
    /// <param name="item4">The tuple's fourth element.</param>
    /// <param name="item5">The tuple's fifth element.</param>
    /// <typeparam name="T1">The type of the tuple's first element</typeparam>
    /// <typeparam name="T2">The type of the tuple's second element</typeparam>
    /// <typeparam name="T3">The type of the tuple's third element</typeparam>
    /// <typeparam name="T4">The type of the tuple's fourth element</typeparam>
    /// <typeparam name="T5">The type of the tuple's fifth element</typeparam>
    [method: MethodImpl(MethodImplOptions.AggressiveInlining)]
    #if EXPORT_HAMPER_CORE_SHARP
    public
    #else
    internal
    #endif
    readonly ref struct SpanTuple<T1, T2, T3, T4, T5>(Span<T1> item1, Span<T2> item2, Span<T3> item3, Span<T4> item4, Span<T5> item5) : ISpanTuple
    {
        private readonly Span<T1> item1 = item1;
        /// <summary>
        /// Gets the item at the first position of this tuple
        /// </summary>
        public Span<T1> Item1
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return item1; }
        }
        
        private readonly Span<T2> item2 = item2;
        /// <summary>
        /// Gets the item at the second position of this tuple
        /// </summary>
        public Span<T2> Item2
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return item2; }
        }
        
        private readonly Span<T3> item3 = item3;
        /// <summary>
        /// Gets the item at the third position of this tuple
        /// </summary>
        public Span<T3> Item3
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return item3; }
        }
        
        private readonly Span<T4> item4 = item4;
        /// <summary>
        /// Gets the item at the fourth position of this tuple
        /// </summary>
        public Span<T4> Item4
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return item4; }
        }
        
        private readonly Span<T5> item5 = item5;
        /// <summary>
        /// Gets the item at the fifth position of this tuple
        /// </summary>
        public Span<T5> Item5
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return item5; }
        }
        
        /// <summary>
        /// Gets the number of elements in this tuple
        /// </summary>
        public int Count
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return 5; }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator ReadOnlySpanTuple<T1, T2, T3, T4, T5>(SpanTuple<T1, T2, T3, T4, T5> tuple)
        {
            return new ReadOnlySpanTuple<T1, T2, T3, T4, T5>(tuple.item1, tuple.item2, tuple.item3, tuple.item4, tuple.item5);
        }
        
        /// <inheritdoc/>
        public int Length
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return (item1.Length + item2.Length + item3.Length + item4.Length + item5.Length); }
        }
    }
    /// <summary>
    /// Represents an n-value tuple, where n is 6
    /// </summary>
    /// <param name="item1">The tuple's first element</param>
    /// <param name="item2">The tuple's second element.</param>
    /// <param name="item3">The tuple's third element.</param>
    /// <param name="item4">The tuple's fourth element.</param>
    /// <param name="item5">The tuple's fifth element.</param>
    /// <param name="item6">The tuple's sixth element</param>
    /// <typeparam name="T1">The type of the tuple's first element</typeparam>
    /// <typeparam name="T2">The type of the tuple's second element</typeparam>
    /// <typeparam name="T3">The type of the tuple's third element</typeparam>
    /// <typeparam name="T4">The type of the tuple's fourth element</typeparam>
    /// <typeparam name="T5">The type of the tuple's fifth element</typeparam>
    /// <typeparam name="T6">The type of the tuple's sixth element</typeparam>
    [method: MethodImpl(MethodImplOptions.AggressiveInlining)]
    #if EXPORT_HAMPER_CORE_SHARP
    public
    #else
    internal
    #endif
    readonly ref struct SpanTuple<T1, T2, T3, T4, T5, T6>(Span<T1> item1, Span<T2> item2, Span<T3> item3, Span<T4> item4, Span<T5> item5, Span<T6> item6) : ISpanTuple
    {
        private readonly Span<T1> item1 = item1;
        /// <summary>
        /// Gets the item at the first position of this tuple
        /// </summary>
        public Span<T1> Item1
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return item1; }
        }
        
        private readonly Span<T2> item2 = item2;
        /// <summary>
        /// Gets the item at the second position of this tuple
        /// </summary>
        public Span<T2> Item2
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return item2; }
        }
        
        private readonly Span<T3> item3 = item3;
        /// <summary>
        /// Gets the item at the third position of this tuple
        /// </summary>
        public Span<T3> Item3
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return item3; }
        }
        
        private readonly Span<T4> item4 = item4;
        /// <summary>
        /// Gets the item at the fourth position of this tuple
        /// </summary>
        public Span<T4> Item4
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return item4; }
        }
        
        private readonly Span<T5> item5 = item5;
        /// <summary>
        /// Gets the item at the fifth position of this tuple
        /// </summary>
        public Span<T5> Item5
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return item5; }
        }
        
        private readonly Span<T6> item6 = item6;
        /// <summary>
        /// Gets the item at the sixth position of this tuple
        /// </summary>
        public Span<T6> Item6
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return item6; }
        }
        
        /// <summary>
        /// Gets the number of elements in this tuple
        /// </summary>
        public int Count
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return 6; }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator ReadOnlySpanTuple<T1, T2, T3, T4, T5, T6>(SpanTuple<T1, T2, T3, T4, T5, T6> tuple)
        {
            return new ReadOnlySpanTuple<T1, T2, T3, T4, T5, T6>(tuple.item1, tuple.item2, tuple.item3, tuple.item4, tuple.item5, tuple.item6);
        }
        
        /// <inheritdoc/>
        public int Length
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return (item1.Length + item2.Length + item3.Length + item4.Length + item5.Length + item6.Length); }
        }
    }
    /// <summary>
    /// Represents an n-value tuple, where n is 7
    /// </summary>
    /// <param name="item1">The tuple's first element</param>
    /// <param name="item2">The tuple's second element.</param>
    /// <param name="item3">The tuple's third element.</param>
    /// <param name="item4">The tuple's fourth element.</param>
    /// <param name="item5">The tuple's fifth element.</param>
    /// <param name="item6">The tuple's sixth element</param>
    /// <param name="item7">The tuple's seventh element</param>
    /// <typeparam name="T1">The type of the tuple's first element</typeparam>
    /// <typeparam name="T2">The type of the tuple's second element</typeparam>
    /// <typeparam name="T3">The type of the tuple's third element</typeparam>
    /// <typeparam name="T4">The type of the tuple's fourth element</typeparam>
    /// <typeparam name="T5">The type of the tuple's fifth element</typeparam>
    /// <typeparam name="T6">The type of the tuple's sixth element</typeparam>
    /// <typeparam name="T7">The type of the tuple's seventh element</typeparam>
    [method: MethodImpl(MethodImplOptions.AggressiveInlining)]
    #if EXPORT_HAMPER_CORE_SHARP
    public
    #else
    internal
    #endif
    readonly ref struct SpanTuple<T1, T2, T3, T4, T5, T6, T7>(Span<T1> item1, Span<T2> item2, Span<T3> item3, Span<T4> item4, Span<T5> item5, Span<T6> item6, Span<T7> item7) : ISpanTuple
    {
        private readonly Span<T1> item1 = item1;
        /// <summary>
        /// Gets the item at the first position of this tuple
        /// </summary>
        public Span<T1> Item1
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return item1; }
        }
        
        private readonly Span<T2> item2 = item2;
        /// <summary>
        /// Gets the item at the second position of this tuple
        /// </summary>
        public Span<T2> Item2
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return item2; }
        }
        
        private readonly Span<T3> item3 = item3;
        /// <summary>
        /// Gets the item at the third position of this tuple
        /// </summary>
        public Span<T3> Item3
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return item3; }
        }
        
        private readonly Span<T4> item4 = item4;
        /// <summary>
        /// Gets the item at the fourth position of this tuple
        /// </summary>
        public Span<T4> Item4
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return item4; }
        }
        
        private readonly Span<T5> item5 = item5;
        /// <summary>
        /// Gets the item at the fifth position of this tuple
        /// </summary>
        public Span<T5> Item5
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return item5; }
        }
        
        private readonly Span<T6> item6 = item6;
        /// <summary>
        /// Gets the item at the sixth position of this tuple
        /// </summary>
        public Span<T6> Item6
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return item6; }
        }
        
        private readonly Span<T7> item7 = item7;
        /// <summary>
        /// Gets the item at the seventh position of this tuple
        /// </summary>
        public Span<T7> Item7
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return item7; }
        }
        
        /// <summary>
        /// Gets the number of elements in this tuple
        /// </summary>
        public int Count
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return 7; }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator ReadOnlySpanTuple<T1, T2, T3, T4, T5, T6, T7>(SpanTuple<T1, T2, T3, T4, T5, T6, T7> tuple)
        {
            return new ReadOnlySpanTuple<T1, T2, T3, T4, T5, T6, T7>(tuple.item1, tuple.item2, tuple.item3, tuple.item4, tuple.item5, tuple.item6, tuple.item7);
        }
        
        /// <inheritdoc/>
        public int Length
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return (item1.Length + item2.Length + item3.Length + item4.Length + item5.Length + item6.Length + item7.Length); }
        }
    }
    /// <summary>
    /// Represents an n-value tuple, where n is 8 or greater
    /// </summary>
    /// <param name="item1">The tuple's first element</param>
    /// <param name="item2">The tuple's second element.</param>
    /// <param name="item3">The tuple's third element.</param>
    /// <param name="item4">The tuple's fourth element.</param>
    /// <param name="item5">The tuple's fifth element.</param>
    /// <param name="item6">The tuple's sixth element</param>
    /// <param name="item7">The tuple's seventh element</param>
    /// <param name="rest">An instance of any tuple type that contains the values of the tuple's remaining elements</param>
    /// <typeparam name="T1">The type of the tuple's first element</typeparam>
    /// <typeparam name="T2">The type of the tuple's second element</typeparam>
    /// <typeparam name="T3">The type of the tuple's third element</typeparam>
    /// <typeparam name="T4">The type of the tuple's fourth element</typeparam>
    /// <typeparam name="T5">The type of the tuple's fifth element</typeparam>
    /// <typeparam name="T6">The type of the tuple's sixth element</typeparam>
    /// <typeparam name="T7">The type of the tuple's seventh element</typeparam>
    /// <typeparam name="TRest">Any generic tuple instance that defines the types of the tuple's remaining elements</typeparam>
    [method: MethodImpl(MethodImplOptions.AggressiveInlining)]
    #if EXPORT_HAMPER_CORE_SHARP
    public
    #else
    internal
    #endif
    readonly ref struct SpanTuple<T1, T2, T3, T4, T5, T6, T7, TRest>(Span<T1> item1, Span<T2> item2, Span<T3> item3, Span<T4> item4, Span<T5> item5, Span<T6> item6, Span<T7> item7, TRest rest) : ISpanTuple, IReadOnlySpanTuple
        where TRest : struct, ISpanTuple
    {
        private readonly Span<T1> item1 = item1;
        /// <summary>
        /// Gets the item at the first position of this tuple
        /// </summary>
        public Span<T1> Item1
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return item1; }
        }
        
        private readonly Span<T2> item2 = item2;
        /// <summary>
        /// Gets the item at the second position of this tuple
        /// </summary>
        public Span<T2> Item2
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return item2; }
        }
        
        private readonly Span<T3> item3 = item3;
        /// <summary>
        /// Gets the item at the third position of this tuple
        /// </summary>
        public Span<T3> Item3
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return item3; }
        }
        
        private readonly Span<T4> item4 = item4;
        /// <summary>
        /// Gets the item at the fourth position of this tuple
        /// </summary>
        public Span<T4> Item4
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return item4; }
        }
        
        private readonly Span<T5> item5 = item5;
        /// <summary>
        /// Gets the item at the fifth position of this tuple
        /// </summary>
        public Span<T5> Item5
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return item5; }
        }
        
        private readonly Span<T6> item6 = item6;
        /// <summary>
        /// Gets the item at the sixth position of this tuple
        /// </summary>
        public Span<T6> Item6
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return item6; }
        }
        
        private readonly Span<T7> item7 = item7;
        /// <summary>
        /// Gets the item at the seventh position of this tuple
        /// </summary>
        public Span<T7> Item7
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return item7; }
        }
        
        /// <summary>
        /// Gets the remaining items in this tuple
        /// </summary>
        public TRest Rest
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return rest; }
        }
        
        /// <summary>
        /// Gets the number of elements in this tuple
        /// </summary>
        public int Count
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return (7 + rest.Count); }
        }
        
        /// <summary>
        /// Gets the total length of the memory managed by elements in this tuple
        /// </summary>
        public int Length
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return (item1.Length + item2.Length + item3.Length + item4.Length + item5.Length + item6.Length + item7.Length + rest.Length); }
        }
    }
}