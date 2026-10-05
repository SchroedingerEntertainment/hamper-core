// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

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
    class AsyncObservable<T, MutatorT, DispatchingStrategy, SubscriptionStrategy> : AsyncSource<T, DispatchingStrategy, SubscriptionStrategy>, IAsyncObserver<T>
        where MutatorT : struct, IGatingMutator<T>
        where DispatchingStrategy : struct, IAsyncDispatchingStrategy<T>
        where SubscriptionStrategy : struct, IAsyncSubscriptionStrategy<T>
    {
        private readonly MutatorT mutator;
        
        /// <summary>
        /// Initializes the object with a certain data flow controller
        /// </summary>
        /// <param name="mutator">A data flow controlling object</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public AsyncObservable(MutatorT mutator)
        {
            this.mutator = mutator;
        }
        
        #region IAsyncObserver<TIn> Members
        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        Task IAsyncObserver<T>.OnNextAsync(T value)
        {
            if (mutator.Invoke(value))
            {
                return this.OnNextAsync(value);
            }
            else return Task.CompletedTask;
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
    class AsyncObservable<TIn, TOut, MutatorT, DispatchingStrategy, SubscriptionStrategy> : AsyncSource<TOut, DispatchingStrategy, SubscriptionStrategy>, IAsyncObserver<TIn>
        where MutatorT : struct, IProjectionMutator<TIn, TOut>
        where DispatchingStrategy : struct, IAsyncDispatchingStrategy<TOut>
        where SubscriptionStrategy : struct, IAsyncSubscriptionStrategy<TOut>
    {
        private readonly MutatorT mutator;
        
        /// <summary>
        /// Initializes the object with a certain data projection unit
        /// </summary>
        /// <param name="mutator">A data projection object</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public AsyncObservable(MutatorT mutator)
        {
            this.mutator = mutator;
        }
        
        #region IAsyncObserver<TIn> Members
        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        Task IAsyncObserver<TIn>.OnNextAsync(TIn value)
        {
            if (mutator.Invoke(value, out TOut result))
            {
                return this.OnNextAsync(result);
            }
            else return Task.CompletedTask;
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        Task IAsyncObserver<TIn>.OnErrorAsync(Exception error)
        {
            return this.OnErrorAsync(error);
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        Task IAsyncObserver<TIn>.OnCompletedAsync()
        {
            return this.OnCompletedAsync();
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
    class AsyncObservable<TIn, TKey, TValue, MutatorT, DispatchingStrategy, SubscriptionStrategy> : AsyncSource<TKey, TValue, DispatchingStrategy, SubscriptionStrategy>, IAsyncObserver<TIn>
        where MutatorT : struct, IProjectionMutator<TIn, TKey, TValue>
        where DispatchingStrategy : struct, IAsyncDispatchingStrategy<TValue>
        where SubscriptionStrategy : struct, IAsyncSubscriptionStrategy<TKey, TValue>
    {
        private readonly MutatorT mutator;
        
        /// <summary>
        /// Initializes the object with a certain data projection unit
        /// </summary>
        /// <param name="mutator">A data projection object</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public AsyncObservable(MutatorT mutator)
        {
            this.mutator = mutator;
        }
        
        #region IObserver<TIn> Members
        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        Task IAsyncObserver<TIn>.OnNextAsync(TIn value)
        {
            if (mutator.Invoke(value, out TKey key, out TValue result))
            {
                return this.OnNextAsync(key, result);
            }
            else return Task.CompletedTask;
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        Task IAsyncObserver<TIn>.OnErrorAsync(Exception error)
        {
            return this.OnErrorAsync(error);
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        Task IAsyncObserver<TIn>.OnCompletedAsync()
        {
            return this.OnCompletedAsync();
        }
        #endregion
    }
}