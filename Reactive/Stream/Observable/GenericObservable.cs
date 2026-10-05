// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System;
using System.Runtime.CompilerServices;

namespace Soe.Reactive
{
    /// <summary>
    /// Represents a conditional data source instance
    /// </summary>
    /// <typeparam name="T">The object that provides notification information</typeparam>
    /// <typeparam name="MutatorT">A data flow controller</typeparam>
    /// <typeparam name="DispatchingStrategy">An object managing how and when data is propagated</typeparam>
    /// <typeparam name="SubscriptionStrategy">An object managing how subscribers are handled</typeparam>
    #if EXPORT_HAMPER_CORE_REACTIVE
    public
    #else
    internal
    #endif
    class Observable<T, MutatorT, DispatchingStrategy, SubscriptionStrategy> : Source<T, DispatchingStrategy, SubscriptionStrategy>, IObserver<T>
        where MutatorT : struct, IGatingMutator<T>
        where DispatchingStrategy : struct, IDispatchingStrategy<T>
        where SubscriptionStrategy : struct, ISubscriptionStrategy<T>
    {
        private readonly MutatorT mutator;
        
        /// <summary>
        /// Initializes the object with a certain data flow controller
        /// </summary>
        /// <param name="mutator">A data flow controlling object</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Observable(MutatorT mutator)
        {
            this.mutator = mutator;
        }
        
        #region IObserver<TIn> Members
        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void IObserver<T>.OnNext(T value)
        {
            if (mutator.Invoke(value))
            {
                this.OnNext(value);
            }
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
    
    /// <summary>
    /// Represents a transformation data source instance
    /// </summary>
    /// <typeparam name="TIn">The object that provides notification information</typeparam>
    /// <typeparam name="TOut">The transformed object that provides notification information</typeparam>
    /// <typeparam name="MutatorT">A data projection unit</typeparam>
    /// <typeparam name="DispatchingStrategy">An object managing how and when data is propagated</typeparam>
    /// <typeparam name="SubscriptionStrategy">An object managing how subscribers are handled</typeparam>
    #if EXPORT_HAMPER_CORE_REACTIVE
    public
    #else
    internal
    #endif
    class Observable<TIn, TOut, MutatorT, DispatchingStrategy, SubscriptionStrategy> : Source<TOut, DispatchingStrategy, SubscriptionStrategy>, IObserver<TIn>
        where MutatorT : struct, IProjectionMutator<TIn, TOut>
        where DispatchingStrategy : struct, IDispatchingStrategy<TOut>
        where SubscriptionStrategy : struct, ISubscriptionStrategy<TOut>
    {
        private readonly MutatorT mutator;
        
        /// <summary>
        /// Initializes the object with a certain data projection unit
        /// </summary>
        /// <param name="mutator">A data projection object</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Observable(MutatorT mutator)
        {
            this.mutator = mutator;
        }
        
        #region IObserver<TIn> Members
        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void IObserver<TIn>.OnNext(TIn value)
        {
            if (mutator.Invoke(value, out TOut result))
            {
                this.OnNext(result);
            }
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void IObserver<TIn>.OnError(Exception error)
        {
            this.OnError(error);
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void IObserver<TIn>.OnCompleted()
        {
            this.OnCompleted();
        }
        #endregion
    }
    
    /// <summary>
    /// Represents a conditional transformation data source instance
    /// </summary>
    /// <typeparam name="TIn">The object that provides notification information</typeparam>
    /// <typeparam name="TKey">The object that provides information about the target subscriptions</typeparam>
    /// <typeparam name="TValue">The transformed object that provides notification information</typeparam>
    /// <typeparam name="MutatorT">A data projection unit</typeparam>
    /// <typeparam name="DispatchingStrategy">An object managing how and when data is propagated</typeparam>
    /// <typeparam name="SubscriptionStrategy">An object managing how subscribers are handled</typeparam>
    #if EXPORT_HAMPER_CORE_REACTIVE
    public
    #else
    internal
    #endif
    class Observable<TIn, TKey, TValue, MutatorT, DispatchingStrategy, SubscriptionStrategy> : Source<TKey, TValue, DispatchingStrategy, SubscriptionStrategy>, IObserver<TIn>
        where MutatorT : struct, IProjectionMutator<TIn, TKey, TValue>
        where DispatchingStrategy : struct, IDispatchingStrategy<TValue>
        where SubscriptionStrategy : struct, ISubscriptionStrategy<TKey, TValue>
    {
        private readonly MutatorT mutator;
        
        /// <summary>
        /// Initializes the object with a certain data projection unit
        /// </summary>
        /// <param name="mutator">A data projection object</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Observable(MutatorT mutator)
        {
            this.mutator = mutator;
        }
        
        #region IObserver<TIn> Members
        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void IObserver<TIn>.OnNext(TIn value)
        {
            if (mutator.Invoke(value, out TKey key, out TValue result))
            {
                this.OnNext(key, result);
            }
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void IObserver<TIn>.OnError(Exception error)
        {
            this.OnError(error);
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void IObserver<TIn>.OnCompleted()
        {
            this.OnCompleted();
        }
        #endregion
    }
}