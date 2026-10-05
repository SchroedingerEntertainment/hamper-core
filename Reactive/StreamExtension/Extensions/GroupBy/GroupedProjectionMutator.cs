// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System;
using System.Runtime.CompilerServices;

namespace Soe.Reactive
{
    /// <summary>
    /// Groups the elements of a sequence according to a specified key selector function
    /// </summary>
    /// <typeparam name="TKey">The type of the key returned by selector</typeparam>
    /// <typeparam name="TValue">The type of the elements of the data source</typeparam>
    /// <typeparam name="MutatorT">A conditional data flow controller</typeparam>
    #if EXPORT_HAMPER_CORE_REACTIVE
    public
    #else
    internal
    #endif
    readonly struct GroupedProjectionMutator<TKey, TValue, MutatorT> : IProjectionMutator<TValue, TKey, TValue>
        where MutatorT : struct, IGatingMutator<TValue>
    {
        private readonly Func<TValue,TKey> selector;
        private readonly MutatorT mutator;

        /// <summary>
        /// Initializes a new transformation instance
        /// </summary>
        /// <param name="mutator">A nested data processing unit to chain</param>
        /// <param name="selector">A function to extract the key for each element</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public GroupedProjectionMutator(MutatorT mutator, Func<TValue, TKey> selector)
        {
            this.selector = selector;
            this.mutator = mutator;
        }
        
        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Invoke(in TValue value, out TKey key, out TValue result)
        {
            if (mutator.Invoke(value))
            {
                key = selector(value);
                result = value;
                return true;
            }
            else
            {
                #pragma warning disable CS8601
                key = default;
                result = default;
                #pragma warning restore CS8601
                
                return false;
            }
        }
    }
    
    /// <summary>
    /// Groups the elements of a sequence according to a specified key selector function
    /// </summary>
    /// <typeparam name="T">The type of the elements of the data source</typeparam>
    /// <typeparam name="TKey">The type of the key returned by selector</typeparam>
    /// <typeparam name="TValue">The type of the transformed elements of the data source</typeparam>
    /// <typeparam name="MutatorT">A conditional data flow controller</typeparam>
    #if EXPORT_HAMPER_CORE_REACTIVE
    public
    #else
    internal
    #endif
    readonly struct GroupedProjectionMutator<T, TKey, TValue, MutatorT> : IProjectionMutator<T, TKey, TValue>
        where MutatorT : struct, IGatingMutator<T>
    {
        private readonly Func<T,TKey> keySelector;
        private readonly Func<T, TValue> valueSelector;
        private readonly MutatorT mutator;

        /// <summary>
        /// Initializes a new transformation instance
        /// </summary>
        /// <param name="mutator">A nested data processing unit to chain</param>
        /// <param name="keySelector">A function to extract the key for each element</param>
        /// <param name="valueSelector">A function to map each source element to an output element</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public GroupedProjectionMutator(MutatorT mutator, Func<T,TKey> keySelector, Func<T, TValue> valueSelector)
        {
            this.keySelector = keySelector;
            this.valueSelector = valueSelector;
            this.mutator = mutator;
        }
        
        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Invoke(in T value, out TKey key, out TValue result)
        {
            if (mutator.Invoke(value))
            {
                key = keySelector(value);
                result = valueSelector(value);
                return true;
            }
            else
            {
                #pragma warning disable CS8601
                key = default;
                result = default;
                #pragma warning restore CS8601
                
                return false;
            }
        }
    }
    
    /// <summary>
    /// Groups the elements of a sequence according to a specified key selector function
    /// </summary>
    /// <typeparam name="TIn">The type of the elements of the data source</typeparam>
    /// <typeparam name="TOut">The output type of the data processing unit to chain</typeparam>
    /// <typeparam name="TKey">The type of the key returned by selector</typeparam>
    /// <typeparam name="TValue">The type of the transformed elements of the data source</typeparam>
    /// <typeparam name="MutatorT">A data projection unit</typeparam>
    #if EXPORT_HAMPER_CORE_REACTIVE
    public
    #else
    internal
    #endif
    readonly struct GroupedProjectionMutator<TIn, TOut, TKey, TValue, MutatorT> : IProjectionMutator<TIn, TKey, TValue>
        where MutatorT : struct, IProjectionMutator<TIn, TOut>
    {
        private readonly Func<TOut,TKey> keySelector;
        private readonly Func<TOut, TValue> valueSelector;
        private readonly MutatorT mutator;

        /// <summary>
        /// Initializes a new transformation instance
        /// </summary>
        /// <param name="mutator">A nested data processing unit to chain</param>
        /// <param name="keySelector">A function to extract the key for each element</param>
        /// <param name="valueSelector">A function to map each source element to an output element</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public GroupedProjectionMutator(MutatorT mutator, Func<TOut,TKey> keySelector, Func<TOut, TValue> valueSelector)
        {
            this.keySelector = keySelector;
            this.valueSelector = valueSelector;
            this.mutator = mutator;
        }
        
        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Invoke(in TIn value, out TKey key, out TValue result)
        {
            if (mutator.Invoke(value, out TOut tmp))
            {
                key = keySelector(tmp);
                result = valueSelector(tmp);
                return true;
            }
            else
            {
                #pragma warning disable CS8601
                key = default;
                result = default;
                #pragma warning restore CS8601
                
                return false;
            }
        }
    }
}