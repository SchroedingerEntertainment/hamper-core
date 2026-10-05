// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System;
using System.Runtime.CompilerServices;

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
    class Observable<T,  DispatchingStrategy, SubscriptionStrategy> : Source<T, DispatchingStrategy, SubscriptionStrategy>, IObserver<T>
        where DispatchingStrategy : struct, IDispatchingStrategy<T>
        where SubscriptionStrategy : struct, ISubscriptionStrategy<T>
    {
        #region IObserver<T> Members
        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void IObserver<T>.OnNext(T value)
        {
            this.OnNext(value);
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void IObserver<T>.OnError(Exception error)
        {
            this.OnError(error);
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void IObserver<T>.OnCompleted()
        {
            this.OnCompleted();
        }
        #endregion
    }
}