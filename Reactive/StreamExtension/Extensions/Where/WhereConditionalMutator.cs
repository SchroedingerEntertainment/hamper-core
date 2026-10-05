// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System;
using System.Runtime.CompilerServices;

namespace Soe.Reactive
{
    /// <summary>
    /// A filter on a sequence of values based on a predicate
    /// </summary>
    /// <typeparam name="T">The type of the elements of the data source</typeparam>
    #if EXPORT_HAMPER_CORE_REACTIVE
    public
    #else
    internal
    #endif
    readonly struct WhereConditionalMutator<T> : IGatingMutator<T>
    {
        private readonly Func<T, bool> predicate;

        /// <summary>
        /// Initializes this filter instance
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public WhereConditionalMutator(Func<T, bool> predicate)
        {
            this.predicate = predicate;
        }
        
        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Invoke(in T value)
        {
            return predicate(value);
        }
    }
    
    /// <summary>
    /// A filter on a sequence of values based on a predicate
    /// </summary>
    /// <typeparam name="T">The type of the elements of the data source</typeparam>
    /// <typeparam name="MutatorT">A conditional data flow controller</typeparam>
    #if EXPORT_HAMPER_CORE_REACTIVE
    public
    #else
    internal
    #endif
    readonly struct WhereConditionalMutator<T, MutatorT> : IGatingMutator<T>
        where MutatorT : struct, IGatingMutator<T>
    {
        private readonly Func<T, bool> predicate;
        private readonly MutatorT mutator;

        /// <summary>
        /// Initializes this filter instance
        /// </summary>
        /// <param name="mutator">A nested data processing unit to chain</param>
        /// <param name="predicate">A function to test each element for a condition</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public WhereConditionalMutator(MutatorT mutator, Func<T, bool> predicate)
        {
            this.predicate = predicate;
            this.mutator = mutator;
        }
        
        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Invoke(in T value)
        {
            return mutator.Invoke(value) && predicate(value);
        }
    }
    
    /// <summary>
    /// A filter on a sequence of values based on a predicate
    /// </summary>
    /// <typeparam name="TIn">The type of the elements of the data source</typeparam>
    /// <typeparam name="TOut">The transformed type of the elements of the data source</typeparam>
    /// <typeparam name="MutatorT">A data transformation unit</typeparam>
    #if EXPORT_HAMPER_CORE_REACTIVE
    public
    #else
    internal
    #endif
    readonly struct WhereConditionalMutator<TIn, TOut, MutatorT> : IProjectionMutator<TIn, TOut>
        where MutatorT : struct, IProjectionMutator<TIn, TOut>
    {
        private readonly Func<TOut, bool> predicate;
        private readonly MutatorT mutator;
        
        /// <summary>
        /// Initializes this filter instance
        /// </summary>
        /// <param name="mutator">A nested data processing unit to chain</param>
        /// <param name="predicate">A function to test each element for a condition</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public WhereConditionalMutator(MutatorT mutator, Func<TOut, bool> predicate)
        {
            this.predicate = predicate;
            this.mutator = mutator;
        }
        
        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Invoke(in TIn value, out TOut result)
        {
            if (mutator.Invoke(value, out result))
            {
                return predicate(result);
            }
            else return false;
        }
    }
}