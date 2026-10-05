// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Soe.Reactive
{
    /// <summary>
    /// Represents an observable data source instance
    /// </summary>
    /// <typeparam name="T">The object that provides notification information</typeparam>
    /// <typeparam name="DispatchingStrategy">An object managing how and when data is propagated</typeparam>
    /// <typeparam name="SubscriptionStrategy">An object managing how subscribers are handled</typeparam>
    #if EXPORT_HAMPER_CORE_REACTIVE
    public
    #else
    internal
    #endif
    class AsyncObservable<T,  DispatchingStrategy, SubscriptionStrategy> : AsyncSource<T, DispatchingStrategy, SubscriptionStrategy>, IAsyncObserver<T>
        where DispatchingStrategy : struct, IAsyncDispatchingStrategy<T>
        where SubscriptionStrategy : struct, IAsyncSubscriptionStrategy<T>
    {
        #region IAsyncObserver<T> Members
        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        Task IAsyncObserver<T>.OnNextAsync(T value)
        {
            return this.OnNextAsync(value);
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        Task IAsyncObserver<T>.OnErrorAsync(Exception error)
        {
            return this.OnErrorAsync(error);
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        Task IAsyncObserver<T>.OnCompletedAsync()
        {
            return this.OnCompletedAsync();
        }
        #endregion
    }
}