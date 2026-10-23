// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;

namespace Soe.Threading
{
    /// <summary>
    /// Helper struct to make the scheduler operation awaitable
    /// </summary>
    /// <param name="instance">An object instance to request access to</param>
    /// <typeparam name="T">A reference type</typeparam>
    /// <typeparam name="Policy">The desired access policy</typeparam>
    [method: MethodImpl(MethodImplOptions.AggressiveInlining)]
    #if EXPORT_HAMPER_CORE_THREADING
    public
    #else
    internal
    #endif
    readonly struct BorrowAwaitable<T, Policy>(T instance)
        where T : class
        where Policy : struct, IAccessPolicy
    {
        /// <summary>
        /// Gets an awaiter used to await the requested scope to be accessible
        /// </summary>
        /// <returns>An awaiter instance</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BorrowAwaiter GetAwaiter()
        {
            return new BorrowAwaiter(AccessManager.BorrowAsync<T, Policy>(instance));
        }
    }

    /// <summary>
    /// Helper struct to make the scheduler operation awaitable
    /// </summary>
    /// <param name="i1">An object instance to request access to</param>
    /// <param name="i2">An object instance to request access to</param>
    /// <typeparam name="T1">A reference type</typeparam>
    /// <typeparam name="Policy1">The desired access policy</typeparam>
    /// <typeparam name="T2">A reference type</typeparam>
    /// <typeparam name="Policy2">The desired access policy</typeparam>
    [method: MethodImpl(MethodImplOptions.AggressiveInlining)]
    #if EXPORT_HAMPER_CORE_THREADING
    public
    #else
    internal
    #endif
    readonly struct BorrowAwaitable<T1, Policy1, T2, Policy2>(T1 i1, T2 i2)
        where T1 : class
        where T2 : class
        where Policy1 : struct, IAccessPolicy
        where Policy2 : struct, IAccessPolicy
    {
        /// <summary>
        /// Gets an awaiter used to await the requested scope to be accessible
        /// </summary>
        /// <returns>An awaiter instance</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BorrowAwaiter GetAwaiter()
        {
            return new BorrowAwaiter(AccessManager.BorrowAsync<T1, Policy1, T2, Policy2>(i1, i2));
        }
    }

    /// <summary>
    /// Helper struct to make the scheduler operation awaitable
    /// </summary>
    /// <param name="i1">An object instance to request access to</param>
    /// <param name="i2">An object instance to request access to</param>
    /// <param name="i3">An object instance to request access to</param>
    /// <typeparam name="T1">A reference type</typeparam>
    /// <typeparam name="Policy1">The desired access policy</typeparam>
    /// <typeparam name="T2">A reference type</typeparam>
    /// <typeparam name="Policy2">The desired access policy</typeparam>
    /// <typeparam name="T3">A reference type</typeparam>
    /// <typeparam name="Policy3">The desired access policy</typeparam>
    [method: MethodImpl(MethodImplOptions.AggressiveInlining)]
    #if EXPORT_HAMPER_CORE_THREADING
    public
    #else
    internal
    #endif
    readonly struct BorrowAwaitable<T1, Policy1, T2, Policy2, T3, Policy3>(T1 i1, T2 i2, T3 i3)
        where T1 : class
        where T2 : class
        where T3 : class
        where Policy1 : struct, IAccessPolicy
        where Policy2 : struct, IAccessPolicy
        where Policy3 : struct, IAccessPolicy
    {
        /// <summary>
        /// Gets an awaiter used to await the requested scope to be accessible
        /// </summary>
        /// <returns>An awaiter instance</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BorrowAwaiter GetAwaiter()
        {
            return new BorrowAwaiter(AccessManager.BorrowAsync<T1, Policy1, T2, Policy2, T3, Policy3>(i1, i2, i3));
        }
    }

    /// <summary>
    /// Helper struct to make the scheduler operation awaitable
    /// </summary>
    /// <param name="i1">An object instance to request access to</param>
    /// <param name="i2">An object instance to request access to</param>
    /// <param name="i3">An object instance to request access to</param>
    /// <param name="i4">An object instance to request access to</param>
    /// <typeparam name="T1">A reference type</typeparam>
    /// <typeparam name="Policy1">The desired access policy</typeparam>
    /// <typeparam name="T2">A reference type</typeparam>
    /// <typeparam name="Policy2">The desired access policy</typeparam>
    /// <typeparam name="T3">A reference type</typeparam>
    /// <typeparam name="Policy3">The desired access policy</typeparam>
    /// <typeparam name="T4">A reference type</typeparam>
    /// <typeparam name="Policy4">The desired access policy</typeparam>
    [method: MethodImpl(MethodImplOptions.AggressiveInlining)]
    #if EXPORT_HAMPER_CORE_THREADING
    public
    #else
    internal
    #endif
    readonly struct BorrowAwaitable<T1, Policy1, T2, Policy2, T3, Policy3, T4, Policy4>(T1 i1, T2 i2, T3 i3, T4 i4)
        where T1 : class
        where T2 : class
        where T3 : class
        where T4 : class
        where Policy1 : struct, IAccessPolicy
        where Policy2 : struct, IAccessPolicy
        where Policy3 : struct, IAccessPolicy
        where Policy4 : struct, IAccessPolicy
    {
        /// <summary>
        /// Gets an awaiter used to await the requested scope to be accessible
        /// </summary>
        /// <returns>An awaiter instance</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BorrowAwaiter GetAwaiter()
        {
            return new BorrowAwaiter(AccessManager.BorrowAsync<T1, Policy1, T2, Policy2, T3, Policy3, T4, Policy4>(i1, i2, i3, i4));
        }
    }

    /// <summary>
    /// Helper struct to make the scheduler operation awaitable
    /// </summary>
    /// <param name="i1">An object instance to request access to</param>
    /// <param name="i2">An object instance to request access to</param>
    /// <param name="i3">An object instance to request access to</param>
    /// <param name="i4">An object instance to request access to</param>
    /// <param name="i5">An object instance to request access to</param>
    /// <typeparam name="T1">A reference type</typeparam>
    /// <typeparam name="Policy1">The desired access policy</typeparam>
    /// <typeparam name="T2">A reference type</typeparam>
    /// <typeparam name="Policy2">The desired access policy</typeparam>
    /// <typeparam name="T3">A reference type</typeparam>
    /// <typeparam name="Policy3">The desired access policy</typeparam>
    /// <typeparam name="T4">A reference type</typeparam>
    /// <typeparam name="Policy4">The desired access policy</typeparam>
    /// <typeparam name="T5">A reference type</typeparam>
    /// <typeparam name="Policy5">The desired access policy</typeparam>
    [method: MethodImpl(MethodImplOptions.AggressiveInlining)]
    #if EXPORT_HAMPER_CORE_THREADING
    public
    #else
    internal
    #endif
    readonly struct BorrowAwaitable<T1, Policy1, T2, Policy2, T3, Policy3, T4, Policy4, T5, Policy5>(T1 i1, T2 i2, T3 i3, T4 i4, T5 i5)
        where T1 : class
        where T2 : class
        where T3 : class
        where T4 : class
        where T5 : class
        where Policy1 : struct, IAccessPolicy
        where Policy2 : struct, IAccessPolicy
        where Policy3 : struct, IAccessPolicy
        where Policy4 : struct, IAccessPolicy
        where Policy5 : struct, IAccessPolicy
    {
        /// <summary>
        /// Gets an awaiter used to await the requested scope to be accessible
        /// </summary>
        /// <returns>An awaiter instance</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BorrowAwaiter GetAwaiter()
        {
            return new BorrowAwaiter(AccessManager.BorrowAsync<T1, Policy1, T2, Policy2, T3, Policy3, T4, Policy4, T5, Policy5>(i1, i2, i3, i4, i5));
        }
    }

    /// <summary>
    /// Helper struct to make the scheduler operation awaitable
    /// </summary>
    /// <param name="i1">An object instance to request access to</param>
    /// <param name="i2">An object instance to request access to</param>
    /// <param name="i3">An object instance to request access to</param>
    /// <param name="i4">An object instance to request access to</param>
    /// <param name="i5">An object instance to request access to</param>
    /// <param name="i6">An object instance to request access to</param>
    /// <typeparam name="T1">A reference type</typeparam>
    /// <typeparam name="Policy1">The desired access policy</typeparam>
    /// <typeparam name="T2">A reference type</typeparam>
    /// <typeparam name="Policy2">The desired access policy</typeparam>
    /// <typeparam name="T3">A reference type</typeparam>
    /// <typeparam name="Policy3">The desired access policy</typeparam>
    /// <typeparam name="T4">A reference type</typeparam>
    /// <typeparam name="Policy4">The desired access policy</typeparam>
    /// <typeparam name="T5">A reference type</typeparam>
    /// <typeparam name="Policy5">The desired access policy</typeparam>
    /// <typeparam name="T6">A reference type</typeparam>
    /// <typeparam name="Policy6">The desired access policy</typeparam>
    [method: MethodImpl(MethodImplOptions.AggressiveInlining)]
    #if EXPORT_HAMPER_CORE_THREADING
    public
    #else
    internal
    #endif
    readonly struct BorrowAwaitable<T1, Policy1, T2, Policy2, T3, Policy3, T4, Policy4, T5, Policy5, T6, Policy6>(T1 i1, T2 i2, T3 i3, T4 i4, T5 i5, T6 i6)
        where T1 : class
        where T2 : class
        where T3 : class
        where T4 : class
        where T5 : class
        where T6 : class
        where Policy1 : struct, IAccessPolicy
        where Policy2 : struct, IAccessPolicy
        where Policy3 : struct, IAccessPolicy
        where Policy4 : struct, IAccessPolicy
        where Policy5 : struct, IAccessPolicy
        where Policy6 : struct, IAccessPolicy
    {
        /// <summary>
        /// Gets an awaiter used to await the requested scope to be accessible
        /// </summary>
        /// <returns>An awaiter instance</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BorrowAwaiter GetAwaiter()
        {
            return new BorrowAwaiter(AccessManager.BorrowAsync<T1, Policy1, T2, Policy2, T3, Policy3, T4, Policy4, T5, Policy5, T6, Policy6>(i1, i2, i3, i4, i5, i6));
        }
    }

    /// <summary>
    /// Helper struct to make the scheduler operation awaitable
    /// </summary>
    /// <param name="i1">An object instance to request access to</param>
    /// <param name="i2">An object instance to request access to</param>
    /// <param name="i3">An object instance to request access to</param>
    /// <param name="i4">An object instance to request access to</param>
    /// <param name="i5">An object instance to request access to</param>
    /// <param name="i6">An object instance to request access to</param>
    /// <param name="i7">An object instance to request access to</param>
    /// <typeparam name="T1">A reference type</typeparam>
    /// <typeparam name="Policy1">The desired access policy</typeparam>
    /// <typeparam name="T2">A reference type</typeparam>
    /// <typeparam name="Policy2">The desired access policy</typeparam>
    /// <typeparam name="T3">A reference type</typeparam>
    /// <typeparam name="Policy3">The desired access policy</typeparam>
    /// <typeparam name="T4">A reference type</typeparam>
    /// <typeparam name="Policy4">The desired access policy</typeparam>
    /// <typeparam name="T5">A reference type</typeparam>
    /// <typeparam name="Policy5">The desired access policy</typeparam>
    /// <typeparam name="T6">A reference type</typeparam>
    /// <typeparam name="Policy6">The desired access policy</typeparam>
    /// <typeparam name="T7">A reference type</typeparam>
    /// <typeparam name="Policy7">The desired access policy</typeparam>
    [method: MethodImpl(MethodImplOptions.AggressiveInlining)]
    #if EXPORT_HAMPER_CORE_THREADING
    public
    #else
    internal
    #endif
    readonly struct BorrowAwaitable<T1, Policy1, T2, Policy2, T3, Policy3, T4, Policy4, T5, Policy5, T6, Policy6, T7, Policy7>(T1 i1, T2 i2, T3 i3, T4 i4, T5 i5, T6 i6, T7 i7)
        where T1 : class
        where T2 : class
        where T3 : class
        where T4 : class
        where T5 : class
        where T6 : class
        where T7 : class
        where Policy1 : struct, IAccessPolicy
        where Policy2 : struct, IAccessPolicy
        where Policy3 : struct, IAccessPolicy
        where Policy4 : struct, IAccessPolicy
        where Policy5 : struct, IAccessPolicy
        where Policy6 : struct, IAccessPolicy
        where Policy7 : struct, IAccessPolicy
    {
        /// <summary>
        /// Gets an awaiter used to await the requested scope to be accessible
        /// </summary>
        /// <returns>An awaiter instance</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BorrowAwaiter GetAwaiter()
        {
            return new BorrowAwaiter(AccessManager.BorrowAsync<T1, Policy1, T2, Policy2, T3, Policy3, T4, Policy4, T5, Policy5, T6, Policy6, T7, Policy7>(i1, i2, i3, i4, i5, i6, i7));
        }
    }

    /// <summary>
    /// Helper struct to make the scheduler operation awaitable
    /// </summary>
    /// <param name="i1">An object instance to request access to</param>
    /// <param name="i2">An object instance to request access to</param>
    /// <param name="i3">An object instance to request access to</param>
    /// <param name="i4">An object instance to request access to</param>
    /// <param name="i5">An object instance to request access to</param>
    /// <param name="i6">An object instance to request access to</param>
    /// <param name="i7">An object instance to request access to</param>
    /// <param name="i8">An object instance to request access to</param>
    /// <typeparam name="T1">A reference type</typeparam>
    /// <typeparam name="Policy1">The desired access policy</typeparam>
    /// <typeparam name="T2">A reference type</typeparam>
    /// <typeparam name="Policy2">The desired access policy</typeparam>
    /// <typeparam name="T3">A reference type</typeparam>
    /// <typeparam name="Policy3">The desired access policy</typeparam>
    /// <typeparam name="T4">A reference type</typeparam>
    /// <typeparam name="Policy4">The desired access policy</typeparam>
    /// <typeparam name="T5">A reference type</typeparam>
    /// <typeparam name="Policy5">The desired access policy</typeparam>
    /// <typeparam name="T6">A reference type</typeparam>
    /// <typeparam name="Policy6">The desired access policy</typeparam>
    /// <typeparam name="T7">A reference type</typeparam>
    /// <typeparam name="Policy7">The desired access policy</typeparam>
    /// <typeparam name="T8">A reference type</typeparam>
    /// <typeparam name="Policy8">The desired access policy</typeparam>
    [method: MethodImpl(MethodImplOptions.AggressiveInlining)]
    #if EXPORT_HAMPER_CORE_THREADING
    public
    #else
    internal
    #endif
    readonly struct BorrowAwaitable<T1, Policy1, T2, Policy2, T3, Policy3, T4, Policy4, T5, Policy5, T6, Policy6, T7, Policy7, T8, Policy8>(T1 i1, T2 i2, T3 i3, T4 i4, T5 i5, T6 i6, T7 i7, T8 i8)
        where T1 : class
        where T2 : class
        where T3 : class
        where T4 : class
        where T5 : class
        where T6 : class
        where T7 : class
        where T8 : class
        where Policy1 : struct, IAccessPolicy
        where Policy2 : struct, IAccessPolicy
        where Policy3 : struct, IAccessPolicy
        where Policy4 : struct, IAccessPolicy
        where Policy5 : struct, IAccessPolicy
        where Policy6 : struct, IAccessPolicy
        where Policy7 : struct, IAccessPolicy
        where Policy8 : struct, IAccessPolicy
    {
        /// <summary>
        /// Gets an awaiter used to await the requested scope to be accessible
        /// </summary>
        /// <returns>An awaiter instance</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BorrowAwaiter GetAwaiter()
        {
            return new BorrowAwaiter(AccessManager.BorrowAsync<T1, Policy1, T2, Policy2, T3, Policy3, T4, Policy4, T5, Policy5, T6, Policy6, T7, Policy7, T8, Policy8>(i1, i2, i3, i4, i5, i6, i7, i8));
        }

    }
}