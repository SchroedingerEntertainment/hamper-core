// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System;
using System.Runtime.CompilerServices;

namespace Soe.Reactive
{
    #if EXPORT_HAMPER_CORE_REACTIVE
    public
    #else
    internal
    #endif
    static partial class MutatorExtension
    {
        #region IObservable<T>
        #region IProjectionMutator<TIn, TOut>
        /// <summary>
        /// Materializes a representation of a data processing unit over a certain data source
        /// </summary>
        /// <param name="mutator">The representation of a data processing unit to materialize</param>
        /// <param name="strategy">A management object controlling the behavior of the materialized data source</param>
        /// <param name="disposable">A reference to an interface that allows observers to stop receiving notifications before
        /// the provider has finished sending them</param>
        /// <typeparam name="TIn">The object that provides notification information</typeparam>
        /// <typeparam name="TOut">The transformed object that provides notification information</typeparam>
        /// <typeparam name="MutatorT">A data processing unit</typeparam>
        /// <typeparam name="DispatchingStrategy">An object managing how and when data is propagated</typeparam>
        /// <typeparam name="SubscriptionStrategy">An object managing how subscribers are handled</typeparam>
        /// <returns>The materialized data processing unit attached to the source</returns>
        /// <exception cref="ChannelConnectionFailedException">An exception thrown when connecting to the data source failed</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Observable<TIn, TOut, MutatorT, DispatchingStrategy, SubscriptionStrategy> AsObservable<TIn, TOut, MutatorT, DispatchingStrategy, SubscriptionStrategy>(this Mutator<TIn, TOut, IObservable<TIn>, MutatorT> mutator, ObservableStrategy<TOut, DispatchingStrategy, SubscriptionStrategy> strategy, out IDisposable disposable)
            where MutatorT : struct, IProjectionMutator<TIn, TOut>
            where DispatchingStrategy : struct, IDispatchingStrategy<TOut>
            where SubscriptionStrategy : struct, ISubscriptionStrategy<TOut>
        {
            var result =  new Observable<TIn, TOut, MutatorT, DispatchingStrategy, SubscriptionStrategy>(mutator.Instance);
            disposable = mutator.Source.Subscribe(result);

            if (ReferenceEquals(disposable, Disposable.Empty))
            {
                throw new ChannelConnectionFailedException(mutator.Source, result);
            }
            else return result;
        }
        
        /// <summary>
        /// Materializes a representation of a data processing unit over a certain data source
        /// </summary>
        /// <param name="mutator">The representation of a data processing unit to materialize</param>
        /// <param name="strategy">A management object controlling the behavior of the materialized data source</param>
        /// <typeparam name="TIn">The object that provides notification information</typeparam>
        /// <typeparam name="TOut">The transformed object that provides notification information</typeparam>
        /// <typeparam name="MutatorT">A data processing unit</typeparam>
        /// <typeparam name="DispatchingStrategy">An object managing how and when data is propagated</typeparam>
        /// <typeparam name="SubscriptionStrategy">An object managing how subscribers are handled</typeparam>
        /// <returns>The materialized data processing unit attached to the source</returns>
        /// <exception cref="ChannelConnectionFailedException">An exception thrown when connecting to the data source failed</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Observable<TIn, TOut, MutatorT, DispatchingStrategy, SubscriptionStrategy> AsObservable<TIn, TOut, MutatorT, DispatchingStrategy, SubscriptionStrategy>(this Mutator<TIn, TOut, IObservable<TIn>, MutatorT> mutator, ObservableStrategy<TOut, DispatchingStrategy, SubscriptionStrategy> strategy)
            where MutatorT : struct, IProjectionMutator<TIn, TOut>
            where DispatchingStrategy : struct, IDispatchingStrategy<TOut>
            where SubscriptionStrategy : struct, ISubscriptionStrategy<TOut>
        {
            return AsObservable(mutator, strategy, out _);
        }
        
        /// <summary>
        /// Materializes a representation of a data processing unit over a certain data source
        /// </summary>
        /// <param name="mutator">The representation of a data processing unit to materialize</param>
        /// <param name="dispatcher">An object managing how and when data is propagated</param>
        /// <param name="subscribers">An object managing how subscribers are handled</param>
        /// <param name="disposable">A reference to an interface that allows observers to stop receiving notifications before
        /// the provider has finished sending them</param>
        /// <typeparam name="TIn">The object that provides notification information</typeparam>
        /// <typeparam name="TOut">The transformed object that provides notification information</typeparam>
        /// <typeparam name="MutatorT">A data processing unit</typeparam>
        /// <typeparam name="DispatchingStrategy">An object managing how and when data is propagated</typeparam>
        /// <typeparam name="SubscriptionStrategy">An object managing how subscribers are handled</typeparam>
        /// <returns>The materialized data processing unit attached to the source</returns>
        /// <exception cref="ChannelConnectionFailedException">An exception thrown when connecting to the data source failed</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Observable<TIn, TOut, MutatorT, DispatchingStrategy, SubscriptionStrategy> AsObservable<TIn, TOut, MutatorT, DispatchingStrategy, SubscriptionStrategy>(this Mutator<TIn, TOut, IObservable<TIn>, MutatorT> mutator, DispatchingStrategy dispatcher, SubscriptionStrategy subscribers, out IDisposable disposable)
            where MutatorT : struct, IProjectionMutator<TIn, TOut>
            where DispatchingStrategy : struct, IDispatchingStrategy<TOut>
            where SubscriptionStrategy : struct, ISubscriptionStrategy<TOut>
        {
            return AsObservable<TIn, TOut, MutatorT, DispatchingStrategy, SubscriptionStrategy>(mutator, default, out disposable);
        }
        
        /// <summary>
        /// Materializes a representation of a data processing unit over a certain data source
        /// </summary>
        /// <param name="mutator">The representation of a data processing unit to materialize</param>
        /// <param name="dispatcher">An object managing how and when data is propagated</param>
        /// <param name="subscribers">An object managing how subscribers are handled</param>
        /// <typeparam name="TIn">The object that provides notification information</typeparam>
        /// <typeparam name="TOut">The transformed object that provides notification information</typeparam>
        /// <typeparam name="MutatorT">A data processing unit</typeparam>
        /// <typeparam name="DispatchingStrategy">An object managing how and when data is propagated</typeparam>
        /// <typeparam name="SubscriptionStrategy">An object managing how subscribers are handled</typeparam>
        /// <returns>The materialized data processing unit attached to the source</returns>
        /// <exception cref="ChannelConnectionFailedException">An exception thrown when connecting to the data source failed</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Observable<TIn, TOut, MutatorT, DispatchingStrategy, SubscriptionStrategy> AsObservable<TIn, TOut, MutatorT, DispatchingStrategy, SubscriptionStrategy>(this Mutator<TIn, TOut, IObservable<TIn>, MutatorT> mutator, DispatchingStrategy dispatcher, SubscriptionStrategy subscribers)
            where MutatorT : struct, IProjectionMutator<TIn, TOut>
            where DispatchingStrategy : struct, IDispatchingStrategy<TOut>
            where SubscriptionStrategy : struct, ISubscriptionStrategy<TOut>
        {
            return AsObservable<TIn, TOut, MutatorT, DispatchingStrategy, SubscriptionStrategy>(mutator, default, out _);
        }
        #endregion
        
        #region IProjectionMutator<TIn, TKey, TValue>
        /// <summary>
        /// Materializes a representation of a data processing unit over a certain data source
        /// </summary>
        /// <param name="mutator">The representation of a data processing unit to materialize</param>
        /// <param name="strategy">A management object controlling the behavior of the materialized data source</param>
        /// <param name="disposable">A reference to an interface that allows observers to stop receiving notifications before
        /// the provider has finished sending them</param>
        /// <typeparam name="TIn">The object that provides notification information</typeparam>
        /// <typeparam name="TKey">The object that provides information about the target subscriptions</typeparam>
        /// <typeparam name="TValue">The transformed object that provides notification information</typeparam>
        /// <typeparam name="MutatorT">A data processing unit</typeparam>
        /// <typeparam name="DispatchingStrategy">An object managing how and when data is propagated</typeparam>
        /// <typeparam name="SubscriptionStrategy">An object managing how subscribers are handled</typeparam>
        /// <returns>The materialized data processing unit attached to the source</returns>
        /// <exception cref="ChannelConnectionFailedException">An exception thrown when connecting to the data source failed</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Observable<TIn, TKey, TValue, MutatorT, DispatchingStrategy, SubscriptionStrategy> AsObservable<TIn, TKey, TValue, MutatorT, DispatchingStrategy, SubscriptionStrategy>(this Mutator<TIn, TKey, TValue, IObservable<TIn>, MutatorT> mutator, ObservableStrategy<TKey, TValue, DispatchingStrategy, SubscriptionStrategy> strategy, out IDisposable disposable)
            where MutatorT : struct, IProjectionMutator<TIn, TKey, TValue>
            where DispatchingStrategy : struct, IDispatchingStrategy<TValue>
            where SubscriptionStrategy : struct, ISubscriptionStrategy<TKey, TValue>
        {
            var result =  new Observable<TIn, TKey, TValue, MutatorT, DispatchingStrategy, SubscriptionStrategy>(mutator.Instance);
            disposable = mutator.Source.Subscribe(result);

            if (ReferenceEquals(disposable, Disposable.Empty))
            {
                throw new ChannelConnectionFailedException(mutator.Source, result);
            }
            else return result;
        }
        
        /// <summary>
        /// Materializes a representation of a data processing unit over a certain data source
        /// </summary>
        /// <param name="mutator">The representation of a data processing unit to materialize</param>
        /// <param name="strategy">A management object controlling the behavior of the materialized data source</param>
        /// <typeparam name="TIn">The object that provides notification information</typeparam>
        /// <typeparam name="TKey">The object that provides information about the target subscriptions</typeparam>
        /// <typeparam name="TValue">The transformed object that provides notification information</typeparam>
        /// <typeparam name="MutatorT">A data processing unit</typeparam>
        /// <typeparam name="DispatchingStrategy">An object managing how and when data is propagated</typeparam>
        /// <typeparam name="SubscriptionStrategy">An object managing how subscribers are handled</typeparam>
        /// <returns>The materialized data processing unit attached to the source</returns>
        /// <exception cref="ChannelConnectionFailedException">An exception thrown when connecting to the data source failed</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Observable<TIn, TKey, TValue, MutatorT, DispatchingStrategy, SubscriptionStrategy> AsObservable<TIn, TKey, TValue, MutatorT, DispatchingStrategy, SubscriptionStrategy>(this Mutator<TIn, TKey, TValue, IObservable<TIn>, MutatorT> mutator, ObservableStrategy<TKey, TValue, DispatchingStrategy, SubscriptionStrategy> strategy)
            where MutatorT : struct, IProjectionMutator<TIn, TKey, TValue>
            where DispatchingStrategy : struct, IDispatchingStrategy<TValue>
            where SubscriptionStrategy : struct, ISubscriptionStrategy<TKey, TValue>
        {
            return AsObservable(mutator, strategy, out _);
        }
        
        /// <summary>
        /// Materializes a representation of a data processing unit over a certain data source
        /// </summary>
        /// <param name="mutator">The representation of a data processing unit to materialize</param>
        /// <param name="dispatcher">An object managing how and when data is propagated</param>
        /// <param name="subscribers">An object managing how subscribers are handled</param>
        /// <param name="disposable">A reference to an interface that allows observers to stop receiving notifications before
        /// the provider has finished sending them</param>
        /// <typeparam name="TIn">The object that provides notification information</typeparam>
        /// <typeparam name="TKey">The object that provides information about the target subscriptions</typeparam>
        /// <typeparam name="TValue">The transformed object that provides notification information</typeparam>
        /// <typeparam name="MutatorT">A data processing unit</typeparam>
        /// <typeparam name="DispatchingStrategy">An object managing how and when data is propagated</typeparam>
        /// <typeparam name="SubscriptionStrategy">An object managing how subscribers are handled</typeparam>
        /// <returns>The materialized data processing unit attached to the source</returns>
        /// <exception cref="ChannelConnectionFailedException">An exception thrown when connecting to the data source failed</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Observable<TIn, TKey, TValue, MutatorT, DispatchingStrategy, SubscriptionStrategy> AsObservable<TIn, TKey, TValue, MutatorT, DispatchingStrategy, SubscriptionStrategy>(this Mutator<TIn, TKey, TValue, IObservable<TIn>, MutatorT> mutator, DispatchingStrategy dispatcher, SubscriptionStrategy subscribers, out IDisposable disposable)
            where MutatorT : struct, IProjectionMutator<TIn, TKey, TValue>
            where DispatchingStrategy : struct, IDispatchingStrategy<TValue>
            where SubscriptionStrategy : struct, ISubscriptionStrategy<TKey, TValue>
        {
            return AsObservable<TIn, TKey, TValue, MutatorT, DispatchingStrategy, SubscriptionStrategy>(mutator, default, out disposable);
        }
        
        /// <summary>
        /// Materializes a representation of a data processing unit over a certain data source
        /// </summary>
        /// <param name="mutator">The representation of a data processing unit to materialize</param>
        /// <param name="dispatcher">An object managing how and when data is propagated</param>
        /// <param name="subscribers">An object managing how subscribers are handled</param>
        /// <typeparam name="TIn">The object that provides notification information</typeparam>
        /// <typeparam name="TKey">The object that provides information about the target subscriptions</typeparam>
        /// <typeparam name="TValue">The transformed object that provides notification information</typeparam>
        /// <typeparam name="MutatorT">A data processing unit</typeparam>
        /// <typeparam name="DispatchingStrategy">An object managing how and when data is propagated</typeparam>
        /// <typeparam name="SubscriptionStrategy">An object managing how subscribers are handled</typeparam>
        /// <returns>The materialized data processing unit attached to the source</returns>
        /// <exception cref="ChannelConnectionFailedException">An exception thrown when connecting to the data source failed</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Observable<TIn, TKey, TValue, MutatorT, DispatchingStrategy, SubscriptionStrategy> AsObservable<TIn, TKey, TValue, MutatorT, DispatchingStrategy, SubscriptionStrategy>(this Mutator<TIn, TKey, TValue, IObservable<TIn>, MutatorT> mutator, DispatchingStrategy dispatcher, SubscriptionStrategy subscribers)
            where MutatorT : struct, IProjectionMutator<TIn, TKey, TValue>
            where DispatchingStrategy : struct, IDispatchingStrategy<TValue>
            where SubscriptionStrategy : struct, ISubscriptionStrategy<TKey, TValue>
        {
            return AsObservable<TIn, TKey, TValue, MutatorT, DispatchingStrategy, SubscriptionStrategy>(mutator, default, out _);
        }
        #endregion
        
        #region IGatingMutator<T>
        /// <summary>
        /// Materializes a representation of a data processing unit over a certain data source
        /// </summary>
        /// <param name="mutator">The representation of a data processing unit to materialize</param>
        /// <param name="strategy">A management object controlling the behavior of the materialized data source</param>
        /// <param name="disposable">A reference to an interface that allows observers to stop receiving notifications before
        /// the provider has finished sending them</param>
        /// <typeparam name="T">The object that provides notification information</typeparam>
        /// <typeparam name="MutatorT">A data processing unit</typeparam>
        /// <typeparam name="DispatchingStrategy">An object managing how and when data is propagated</typeparam>
        /// <typeparam name="SubscriptionStrategy">An object managing how subscribers are handled</typeparam>
        /// <returns>The materialized data processing unit attached to the source</returns>
        /// <exception cref="ChannelConnectionFailedException">An exception thrown when connecting to the data source failed</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Observable<T, MutatorT, DispatchingStrategy, SubscriptionStrategy> AsObservable<T, MutatorT, DispatchingStrategy, SubscriptionStrategy>(this Mutator<T, IObservable<T>, MutatorT> mutator, ObservableStrategy<T, DispatchingStrategy, SubscriptionStrategy> strategy, out IDisposable disposable)
            where MutatorT : struct, IGatingMutator<T>
            where DispatchingStrategy : struct, IDispatchingStrategy<T>
            where SubscriptionStrategy : struct, ISubscriptionStrategy<T>
        {
            var result =  new Observable<T, MutatorT, DispatchingStrategy, SubscriptionStrategy>(mutator.Instance);
            disposable = mutator.Source.Subscribe(result);

            if (ReferenceEquals(disposable, Disposable.Empty))
            {
                throw new ChannelConnectionFailedException(mutator.Source, result);
            }
            else return result;
        }
        
        /// <summary>
        /// Materializes a representation of a data processing unit over a certain data source
        /// </summary>
        /// <param name="mutator">The representation of a data processing unit to materialize</param>
        /// <param name="strategy">A management object controlling the behavior of the materialized data source</param>
        /// <typeparam name="T">The object that provides notification information</typeparam>
        /// <typeparam name="MutatorT">A data processing unit</typeparam>
        /// <typeparam name="DispatchingStrategy">An object managing how and when data is propagated</typeparam>
        /// <typeparam name="SubscriptionStrategy">An object managing how subscribers are handled</typeparam>
        /// <returns>The materialized data processing unit attached to the source</returns>
        /// <exception cref="ChannelConnectionFailedException">An exception thrown when connecting to the data source failed</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Observable<T, MutatorT, DispatchingStrategy, SubscriptionStrategy> AsObservable<T, MutatorT, DispatchingStrategy, SubscriptionStrategy>(this Mutator<T, IObservable<T>, MutatorT> mutator, ObservableStrategy<T, DispatchingStrategy, SubscriptionStrategy> strategy)
            where MutatorT : struct, IGatingMutator<T>
            where DispatchingStrategy : struct, IDispatchingStrategy<T>
            where SubscriptionStrategy : struct, ISubscriptionStrategy<T>
        {
            return AsObservable(mutator, strategy, out _);
        }
        
        /// <summary>
        /// Materializes a representation of a data processing unit over a certain data source
        /// </summary>
        /// <param name="mutator">The representation of a data processing unit to materialize</param>
        /// <param name="dispatcher">An object managing how and when data is propagated</param>
        /// <param name="subscribers">An object managing how subscribers are handled</param>
        /// <param name="disposable">A reference to an interface that allows observers to stop receiving notifications before
        /// the provider has finished sending them</param>
        /// <typeparam name="T">The object that provides notification information</typeparam>
        /// <typeparam name="MutatorT">A data processing unit</typeparam>
        /// <typeparam name="DispatchingStrategy">An object managing how and when data is propagated</typeparam>
        /// <typeparam name="SubscriptionStrategy">An object managing how subscribers are handled</typeparam>
        /// <returns>The materialized data processing unit attached to the source</returns>
        /// <exception cref="ChannelConnectionFailedException">An exception thrown when connecting to the data source failed</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Observable<T, MutatorT, DispatchingStrategy, SubscriptionStrategy> AsObservable<T, MutatorT, DispatchingStrategy, SubscriptionStrategy>(this Mutator<T, IObservable<T>, MutatorT> mutator, DispatchingStrategy dispatcher, SubscriptionStrategy subscribers, out IDisposable disposable)
            where MutatorT : struct, IGatingMutator<T>
            where DispatchingStrategy : struct, IDispatchingStrategy<T>
            where SubscriptionStrategy : struct, ISubscriptionStrategy<T>
        {
            return AsObservable<T, MutatorT, DispatchingStrategy, SubscriptionStrategy>(mutator, default, out disposable);
        }
        
        /// <summary>
        /// Materializes a representation of a data processing unit over a certain data source
        /// </summary>
        /// <param name="mutator">The representation of a data processing unit to materialize</param>
        /// <param name="dispatcher">An object managing how and when data is propagated</param>
        /// <param name="subscribers">An object managing how subscribers are handled</param>
        /// <typeparam name="T">The object that provides notification information</typeparam>
        /// <typeparam name="MutatorT">A data processing unit</typeparam>
        /// <typeparam name="DispatchingStrategy">An object managing how and when data is propagated</typeparam>
        /// <typeparam name="SubscriptionStrategy">An object managing how subscribers are handled</typeparam>
        /// <returns>The materialized data processing unit attached to the source</returns>
        /// <exception cref="ChannelConnectionFailedException">An exception thrown when connecting to the data source failed</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Observable<T, MutatorT, DispatchingStrategy, SubscriptionStrategy> AsObservable<T, MutatorT, DispatchingStrategy, SubscriptionStrategy>(this Mutator<T, IObservable<T>, MutatorT> mutator, DispatchingStrategy dispatcher, SubscriptionStrategy subscribers)
            where MutatorT : struct, IGatingMutator<T>
            where DispatchingStrategy : struct, IDispatchingStrategy<T>
            where SubscriptionStrategy : struct, ISubscriptionStrategy<T>
        {
            return AsObservable<T, MutatorT, DispatchingStrategy, SubscriptionStrategy>(mutator, default, out _);
        }
        #endregion
        #endregion
        
        #region IObservable<TKey, TValue>
        #region IProjectionMutator<TIn, TOut>
        /// <summary>
        /// Materializes a representation of a data processing unit over a certain data source
        /// </summary>
        /// <param name="mutator">The representation of a data processing unit to materialize</param>
        /// <param name="subscriptionKey">The object that provides subscription information about the observer</param>
        /// <param name="strategy">A management object controlling the behavior of the materialized data source</param>
        /// <param name="disposable">A reference to an interface that allows observers to stop receiving notifications before
        /// the provider has finished sending them</param>
        /// <typeparam name="TIn">The object that provides notification information</typeparam>
        /// <typeparam name="TOut">The transformed object that provides notification information</typeparam>
        /// <typeparam name="MutatorT">A data processing unit</typeparam>
        /// <typeparam name="DispatchingStrategy">An object managing how and when data is propagated</typeparam>
        /// <typeparam name="SubscriptionStrategy">An object managing how subscribers are handled</typeparam>
        /// <returns>The materialized data processing unit attached to the source</returns>
        /// <typeparam name="TKey">The object that provides information about the target subscriptions</typeparam>
        /// <exception cref="ChannelConnectionFailedException">An exception thrown when connecting to the data source failed</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Observable<TIn, TOut, MutatorT, DispatchingStrategy, SubscriptionStrategy> AsObservable<TKey, TIn, TOut, MutatorT, DispatchingStrategy, SubscriptionStrategy>(this Mutator<TIn, TOut, IObservable<TKey, TIn>, MutatorT> mutator, in TKey subscriptionKey, ObservableStrategy<TOut, DispatchingStrategy, SubscriptionStrategy> strategy, out IDisposable disposable)
            where MutatorT : struct, IProjectionMutator<TIn, TOut>
            where DispatchingStrategy : struct, IDispatchingStrategy<TOut>
            where SubscriptionStrategy : struct, ISubscriptionStrategy<TOut>
        {
            var result =  new Observable<TIn, TOut, MutatorT, DispatchingStrategy, SubscriptionStrategy>(mutator.Instance);
            disposable = mutator.Source.Subscribe(subscriptionKey, result);

            if (ReferenceEquals(disposable, Disposable.Empty))
            {
                throw new ChannelConnectionFailedException(mutator.Source, result);
            }
            else return result;
        }
        
        /// <summary>
        /// Materializes a representation of a data processing unit over a certain data source
        /// </summary>
        /// <param name="mutator">The representation of a data processing unit to materialize</param>
        /// <param name="subscriptionKey">The object that provides subscription information about the observer</param>
        /// <param name="strategy">A management object controlling the behavior of the materialized data source</param>
        /// <typeparam name="TIn">The object that provides notification information</typeparam>
        /// <typeparam name="TOut">The transformed object that provides notification information</typeparam>
        /// <typeparam name="MutatorT">A data processing unit</typeparam>
        /// <typeparam name="DispatchingStrategy">An object managing how and when data is propagated</typeparam>
        /// <typeparam name="SubscriptionStrategy">An object managing how subscribers are handled</typeparam>
        /// <returns>The materialized data processing unit attached to the source</returns>
        /// <typeparam name="TKey">The object that provides information about the target subscriptions</typeparam>
        /// <exception cref="ChannelConnectionFailedException">An exception thrown when connecting to the data source failed</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Observable<TIn, TOut, MutatorT, DispatchingStrategy, SubscriptionStrategy> AsObservable<TKey, TIn, TOut, MutatorT, DispatchingStrategy, SubscriptionStrategy>(this Mutator<TIn, TOut, IObservable<TKey, TIn>, MutatorT> mutator, in TKey subscriptionKey, ObservableStrategy<TOut, DispatchingStrategy, SubscriptionStrategy> strategy)
            where MutatorT : struct, IProjectionMutator<TIn, TOut>
            where DispatchingStrategy : struct, IDispatchingStrategy<TOut>
            where SubscriptionStrategy : struct, ISubscriptionStrategy<TOut>
        {
            return AsObservable(mutator, subscriptionKey, strategy, out _);
        }
        
        /// <summary>
        /// Materializes a representation of a data processing unit over a certain data source
        /// </summary>
        /// <param name="mutator">The representation of a data processing unit to materialize</param>
        /// <param name="subscriptionKey">The object that provides subscription information about the observer</param>
        /// <param name="dispatcher">An object managing how and when data is propagated</param>
        /// <param name="subscribers">An object managing how subscribers are handled</param>
        /// <param name="disposable">A reference to an interface that allows observers to stop receiving notifications before
        /// the provider has finished sending them</param>
        /// <typeparam name="TIn">The object that provides notification information</typeparam>
        /// <typeparam name="TOut">The transformed object that provides notification information</typeparam>
        /// <typeparam name="MutatorT">A data processing unit</typeparam>
        /// <typeparam name="DispatchingStrategy">An object managing how and when data is propagated</typeparam>
        /// <typeparam name="SubscriptionStrategy">An object managing how subscribers are handled</typeparam>
        /// <returns>The materialized data processing unit attached to the source</returns>
        /// <typeparam name="TKey">The object that provides information about the target subscriptions</typeparam>
        /// <exception cref="ChannelConnectionFailedException">An exception thrown when connecting to the data source failed</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Observable<TIn, TOut, MutatorT, DispatchingStrategy, SubscriptionStrategy> AsObservable<TKey, TIn, TOut, MutatorT, DispatchingStrategy, SubscriptionStrategy>(this Mutator<TIn, TOut, IObservable<TKey, TIn>, MutatorT> mutator, in TKey subscriptionKey, DispatchingStrategy dispatcher, SubscriptionStrategy subscribers, out IDisposable disposable)
            where MutatorT : struct, IProjectionMutator<TIn, TOut>
            where DispatchingStrategy : struct, IDispatchingStrategy<TOut>
            where SubscriptionStrategy : struct, ISubscriptionStrategy<TOut>
        {
            return AsObservable<TKey, TIn, TOut, MutatorT, DispatchingStrategy, SubscriptionStrategy>(mutator, subscriptionKey, default, out disposable);
        }
        
        /// <summary>
        /// Materializes a representation of a data processing unit over a certain data source
        /// </summary>
        /// <param name="mutator">The representation of a data processing unit to materialize</param>
        /// <param name="subscriptionKey">The object that provides subscription information about the observer</param>
        /// <param name="dispatcher">An object managing how and when data is propagated</param>
        /// <param name="subscribers">An object managing how subscribers are handled</param>
        /// <typeparam name="TIn">The object that provides notification information</typeparam>
        /// <typeparam name="TOut">The transformed object that provides notification information</typeparam>
        /// <typeparam name="MutatorT">A data processing unit</typeparam>
        /// <typeparam name="DispatchingStrategy">An object managing how and when data is propagated</typeparam>
        /// <typeparam name="SubscriptionStrategy">An object managing how subscribers are handled</typeparam>
        /// <returns>The materialized data processing unit attached to the source</returns>
        /// <typeparam name="TKey">The object that provides information about the target subscriptions</typeparam>
        /// <exception cref="ChannelConnectionFailedException">An exception thrown when connecting to the data source failed</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Observable<TIn, TOut, MutatorT, DispatchingStrategy, SubscriptionStrategy> AsObservable<TKey, TIn, TOut, MutatorT, DispatchingStrategy, SubscriptionStrategy>(this Mutator<TIn, TOut, IObservable<TKey, TIn>, MutatorT> mutator, in TKey subscriptionKey, DispatchingStrategy dispatcher, SubscriptionStrategy subscribers)
            where MutatorT : struct, IProjectionMutator<TIn, TOut>
            where DispatchingStrategy : struct, IDispatchingStrategy<TOut>
            where SubscriptionStrategy : struct, ISubscriptionStrategy<TOut>
        {
            return AsObservable<TKey, TIn, TOut, MutatorT, DispatchingStrategy, SubscriptionStrategy>(mutator, subscriptionKey, default, out _);
        }
        #endregion
        
        #region IProjectionMutator<TIn, TKey, TValue>
        /// <summary>
        /// Materializes a representation of a data processing unit over a certain data source
        /// </summary>
        /// <param name="mutator">The representation of a data processing unit to materialize</param>
        /// <param name="subscriptionKey">The object that provides subscription information about the observer</param>
        /// <param name="strategy">A management object controlling the behavior of the materialized data source</param>
        /// <param name="disposable">A reference to an interface that allows observers to stop receiving notifications before
        /// the provider has finished sending them</param>
        /// <typeparam name="TIn">The object that provides notification information</typeparam>
        /// <typeparam name="TKey">The object that provides information about the target subscriptions</typeparam>
        /// <typeparam name="TValue">The transformed object that provides notification information</typeparam>
        /// <typeparam name="MutatorT">A data processing unit</typeparam>
        /// <typeparam name="DispatchingStrategy">An object managing how and when data is propagated</typeparam>
        /// <typeparam name="SubscriptionStrategy">An object managing how subscribers are handled</typeparam>
        /// <returns>The materialized data processing unit attached to the source</returns>
        /// <typeparam name="TObservableKey">The object that provides information about the target subscriptions</typeparam>
        /// <exception cref="ChannelConnectionFailedException">An exception thrown when connecting to the data source failed</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Observable<TIn, TKey, TValue, MutatorT, DispatchingStrategy, SubscriptionStrategy> AsObservable<TObservableKey, TIn, TKey, TValue, MutatorT, DispatchingStrategy, SubscriptionStrategy>(this Mutator<TIn, TKey, TValue, IObservable<TObservableKey, TIn>, MutatorT> mutator, in TObservableKey subscriptionKey, ObservableStrategy<TKey, TValue, DispatchingStrategy, SubscriptionStrategy> strategy, out IDisposable disposable)
            where MutatorT : struct, IProjectionMutator<TIn, TKey, TValue>
            where DispatchingStrategy : struct, IDispatchingStrategy<TValue>
            where SubscriptionStrategy : struct, ISubscriptionStrategy<TKey, TValue>
        {
            var result =  new Observable<TIn, TKey, TValue, MutatorT, DispatchingStrategy, SubscriptionStrategy>(mutator.Instance);
            disposable = mutator.Source.Subscribe(subscriptionKey, result);

            if (ReferenceEquals(disposable, Disposable.Empty))
            {
                throw new ChannelConnectionFailedException(mutator.Source, result);
            }
            else return result;
        }
        
        /// <summary>
        /// Materializes a representation of a data processing unit over a certain data source
        /// </summary>
        /// <param name="mutator">The representation of a data processing unit to materialize</param>
        /// <param name="subscriptionKey">The object that provides subscription information about the observer</param>
        /// <param name="strategy">A management object controlling the behavior of the materialized data source</param>
        /// <typeparam name="TIn">The object that provides notification information</typeparam>
        /// <typeparam name="TKey">The object that provides information about the target subscriptions</typeparam>
        /// <typeparam name="TValue">The transformed object that provides notification information</typeparam>
        /// <typeparam name="MutatorT">A data processing unit</typeparam>
        /// <typeparam name="DispatchingStrategy">An object managing how and when data is propagated</typeparam>
        /// <typeparam name="SubscriptionStrategy">An object managing how subscribers are handled</typeparam>
        /// <returns>The materialized data processing unit attached to the source</returns>
        /// <typeparam name="TObservableKey">The object that provides information about the target subscriptions</typeparam>
        /// <exception cref="ChannelConnectionFailedException">An exception thrown when connecting to the data source failed</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Observable<TIn, TKey, TValue, MutatorT, DispatchingStrategy, SubscriptionStrategy> AsObservable<TObservableKey, TIn, TKey, TValue, MutatorT, DispatchingStrategy, SubscriptionStrategy>(this Mutator<TIn, TKey, TValue, IObservable<TObservableKey, TIn>, MutatorT> mutator, in TObservableKey subscriptionKey, ObservableStrategy<TKey, TValue, DispatchingStrategy, SubscriptionStrategy> strategy)
            where MutatorT : struct, IProjectionMutator<TIn, TKey, TValue>
            where DispatchingStrategy : struct, IDispatchingStrategy<TValue>
            where SubscriptionStrategy : struct, ISubscriptionStrategy<TKey, TValue>
        {
            return AsObservable(mutator, subscriptionKey, strategy, out _);
        }
        
        /// <summary>
        /// Materializes a representation of a data processing unit over a certain data source
        /// </summary>
        /// <param name="mutator">The representation of a data processing unit to materialize</param>
        /// <param name="subscriptionKey">The object that provides subscription information about the observer</param>
        /// <param name="dispatcher">An object managing how and when data is propagated</param>
        /// <param name="subscribers">An object managing how subscribers are handled</param>
        /// <param name="disposable">A reference to an interface that allows observers to stop receiving notifications before
        /// the provider has finished sending them</param>
        /// <typeparam name="TIn">The object that provides notification information</typeparam>
        /// <typeparam name="TKey">The object that provides information about the target subscriptions</typeparam>
        /// <typeparam name="TValue">The transformed object that provides notification information</typeparam>
        /// <typeparam name="MutatorT">A data processing unit</typeparam>
        /// <typeparam name="DispatchingStrategy">An object managing how and when data is propagated</typeparam>
        /// <typeparam name="SubscriptionStrategy">An object managing how subscribers are handled</typeparam>
        /// <returns>The materialized data processing unit attached to the source</returns>
        /// <typeparam name="TObservableKey">The object that provides information about the target subscriptions</typeparam>
        /// <exception cref="ChannelConnectionFailedException">An exception thrown when connecting to the data source failed</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Observable<TIn, TKey, TValue, MutatorT, DispatchingStrategy, SubscriptionStrategy> AsObservable<TObservableKey, TIn, TKey, TValue, MutatorT, DispatchingStrategy, SubscriptionStrategy>(this Mutator<TIn, TKey, TValue, IObservable<TObservableKey, TIn>, MutatorT> mutator, in TObservableKey subscriptionKey, DispatchingStrategy dispatcher, SubscriptionStrategy subscribers, out IDisposable disposable)
            where MutatorT : struct, IProjectionMutator<TIn, TKey, TValue>
            where DispatchingStrategy : struct, IDispatchingStrategy<TValue>
            where SubscriptionStrategy : struct, ISubscriptionStrategy<TKey, TValue>
        {
            return AsObservable<TObservableKey, TIn, TKey, TValue, MutatorT, DispatchingStrategy, SubscriptionStrategy>(mutator, subscriptionKey, default, out disposable);
        }
        
        /// <summary>
        /// Materializes a representation of a data processing unit over a certain data source
        /// </summary>
        /// <param name="mutator">The representation of a data processing unit to materialize</param>
        /// <param name="subscriptionKey">The object that provides subscription information about the observer</param>
        /// <param name="dispatcher">An object managing how and when data is propagated</param>
        /// <param name="subscribers">An object managing how subscribers are handled</param>
        /// <typeparam name="TIn">The object that provides notification information</typeparam>
        /// <typeparam name="TKey">The object that provides information about the target subscriptions</typeparam>
        /// <typeparam name="TValue">The transformed object that provides notification information</typeparam>
        /// <typeparam name="MutatorT">A data processing unit</typeparam>
        /// <typeparam name="DispatchingStrategy">An object managing how and when data is propagated</typeparam>
        /// <typeparam name="SubscriptionStrategy">An object managing how subscribers are handled</typeparam>
        /// <returns>The materialized data processing unit attached to the source</returns>
        /// <typeparam name="TObservableKey">The object that provides information about the target subscriptions</typeparam>
        /// <exception cref="ChannelConnectionFailedException">An exception thrown when connecting to the data source failed</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Observable<TIn, TKey, TValue, MutatorT, DispatchingStrategy, SubscriptionStrategy> AsObservable<TObservableKey, TIn, TKey, TValue, MutatorT, DispatchingStrategy, SubscriptionStrategy>(this Mutator<TIn, TKey, TValue, IObservable<TObservableKey, TIn>, MutatorT> mutator, in TObservableKey subscriptionKey, DispatchingStrategy dispatcher, SubscriptionStrategy subscribers)
            where MutatorT : struct, IProjectionMutator<TIn, TKey, TValue>
            where DispatchingStrategy : struct, IDispatchingStrategy<TValue>
            where SubscriptionStrategy : struct, ISubscriptionStrategy<TKey, TValue>
        {
            return AsObservable<TObservableKey, TIn, TKey, TValue, MutatorT, DispatchingStrategy, SubscriptionStrategy>(mutator, subscriptionKey, default, out _);
        }
        #endregion
        
        #region IGatingMutator<T>
        /// <summary>
        /// Materializes a representation of a data processing unit over a certain data source
        /// </summary>
        /// <param name="mutator">The representation of a data processing unit to materialize</param>
        /// <param name="subscriptionKey">The object that provides subscription information about the observer</param>
        /// <param name="strategy">A management object controlling the behavior of the materialized data source</param>
        /// <param name="disposable">A reference to an interface that allows observers to stop receiving notifications before
        /// the provider has finished sending them</param>
        /// <typeparam name="T">The object that provides notification information</typeparam>
        /// <typeparam name="MutatorT">A data processing unit</typeparam>
        /// <typeparam name="DispatchingStrategy">An object managing how and when data is propagated</typeparam>
        /// <typeparam name="SubscriptionStrategy">An object managing how subscribers are handled</typeparam>
        /// <returns>The materialized data processing unit attached to the source</returns>
        /// <typeparam name="TKey">The object that provides information about the target subscriptions</typeparam>
        /// <exception cref="ChannelConnectionFailedException">An exception thrown when connecting to the data source failed</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Observable<T, MutatorT, DispatchingStrategy, SubscriptionStrategy> AsObservable<TKey, T, MutatorT, DispatchingStrategy, SubscriptionStrategy>(this Mutator<T, IObservable<TKey, T>, MutatorT> mutator, in TKey subscriptionKey, ObservableStrategy<T, DispatchingStrategy, SubscriptionStrategy> strategy, out IDisposable disposable)
            where MutatorT : struct, IGatingMutator<T>
            where DispatchingStrategy : struct, IDispatchingStrategy<T>
            where SubscriptionStrategy : struct, ISubscriptionStrategy<T>
        {
            var result =  new Observable<T, MutatorT, DispatchingStrategy, SubscriptionStrategy>(mutator.Instance);
            disposable = mutator.Source.Subscribe(subscriptionKey, result);

            if (ReferenceEquals(disposable, Disposable.Empty))
            {
                throw new ChannelConnectionFailedException(mutator.Source, result);
            }
            else return result;
        }
        
        /// <summary>
        /// Materializes a representation of a data processing unit over a certain data source
        /// </summary>
        /// <param name="mutator">The representation of a data processing unit to materialize</param>
        /// <param name="subscriptionKey">The object that provides subscription information about the observer</param>
        /// <param name="strategy">A management object controlling the behavior of the materialized data source</param>
        /// <typeparam name="T">The object that provides notification information</typeparam>
        /// <typeparam name="MutatorT">A data processing unit</typeparam>
        /// <typeparam name="DispatchingStrategy">An object managing how and when data is propagated</typeparam>
        /// <typeparam name="SubscriptionStrategy">An object managing how subscribers are handled</typeparam>
        /// <returns>The materialized data processing unit attached to the source</returns>
        /// <typeparam name="TKey">The object that provides information about the target subscriptions</typeparam>
        /// <exception cref="ChannelConnectionFailedException">An exception thrown when connecting to the data source failed</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Observable<T, MutatorT, DispatchingStrategy, SubscriptionStrategy> AsObservable<TKey, T, MutatorT, DispatchingStrategy, SubscriptionStrategy>(this Mutator<T, IObservable<TKey, T>, MutatorT> mutator, in TKey subscriptionKey, ObservableStrategy<T, DispatchingStrategy, SubscriptionStrategy> strategy)
            where MutatorT : struct, IGatingMutator<T>
            where DispatchingStrategy : struct, IDispatchingStrategy<T>
            where SubscriptionStrategy : struct, ISubscriptionStrategy<T>
        {
            return AsObservable(mutator, subscriptionKey, strategy, out _);
        }
        
        /// <summary>
        /// Materializes a representation of a data processing unit over a certain data source
        /// </summary>
        /// <param name="mutator">The representation of a data processing unit to materialize</param>
        /// <param name="subscriptionKey">The object that provides subscription information about the observer</param>
        /// <param name="dispatcher">An object managing how and when data is propagated</param>
        /// <param name="subscribers">An object managing how subscribers are handled</param>
        /// <param name="disposable">A reference to an interface that allows observers to stop receiving notifications before
        /// the provider has finished sending them</param>
        /// <typeparam name="T">The object that provides notification information</typeparam>
        /// <typeparam name="MutatorT">A data processing unit</typeparam>
        /// <typeparam name="DispatchingStrategy">An object managing how and when data is propagated</typeparam>
        /// <typeparam name="SubscriptionStrategy">An object managing how subscribers are handled</typeparam>
        /// <returns>The materialized data processing unit attached to the source</returns>
        /// <typeparam name="TKey">The object that provides information about the target subscriptions</typeparam>
        /// <exception cref="ChannelConnectionFailedException">An exception thrown when connecting to the data source failed</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Observable<T, MutatorT, DispatchingStrategy, SubscriptionStrategy> AsObservable<TKey, T, MutatorT, DispatchingStrategy, SubscriptionStrategy>(this Mutator<T, IObservable<TKey, T>, MutatorT> mutator, in TKey subscriptionKey, DispatchingStrategy dispatcher, SubscriptionStrategy subscribers, out IDisposable disposable)
            where MutatorT : struct, IGatingMutator<T>
            where DispatchingStrategy : struct, IDispatchingStrategy<T>
            where SubscriptionStrategy : struct, ISubscriptionStrategy<T>
        {
            return AsObservable<TKey, T, MutatorT, DispatchingStrategy, SubscriptionStrategy>(mutator, subscriptionKey, default, out disposable);
        }
        
        /// <summary>
        /// Materializes a representation of a data processing unit over a certain data source
        /// </summary>
        /// <param name="mutator">The representation of a data processing unit to materialize</param>
        /// <param name="subscriptionKey">The object that provides subscription information about the observer</param>
        /// <param name="dispatcher">An object managing how and when data is propagated</param>
        /// <param name="subscribers">An object managing how subscribers are handled</param>
        /// <typeparam name="T">The object that provides notification information</typeparam>
        /// <typeparam name="MutatorT">A data processing unit</typeparam>
        /// <typeparam name="DispatchingStrategy">An object managing how and when data is propagated</typeparam>
        /// <typeparam name="SubscriptionStrategy">An object managing how subscribers are handled</typeparam>
        /// <returns>The materialized data processing unit attached to the source</returns>
        /// <typeparam name="TKey">The object that provides information about the target subscriptions</typeparam>
        /// <exception cref="ChannelConnectionFailedException">An exception thrown when connecting to the data source failed</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Observable<T, MutatorT, DispatchingStrategy, SubscriptionStrategy> AsObservable<TKey, T, MutatorT, DispatchingStrategy, SubscriptionStrategy>(this Mutator<T, IObservable<TKey, T>, MutatorT> mutator, in TKey subscriptionKey, DispatchingStrategy dispatcher, SubscriptionStrategy subscribers)
            where MutatorT : struct, IGatingMutator<T>
            where DispatchingStrategy : struct, IDispatchingStrategy<T>
            where SubscriptionStrategy : struct, ISubscriptionStrategy<T>
        {
            return AsObservable<TKey, T, MutatorT, DispatchingStrategy, SubscriptionStrategy>(mutator, subscriptionKey, default, out _);
        }
        #endregion
        #endregion
    }
}