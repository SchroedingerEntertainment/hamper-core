// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System;
using System.Runtime.CompilerServices;

namespace Soe.Reactive
{
    /// <summary>
    /// Projects each element of a sequence into a new form
    /// </summary>
    /// <typeparam name="TIn">The type of the elements of the data source</typeparam>
    /// <typeparam name="TOut">The type of the value returned by selector</typeparam>
    #if EXPORT_HAMPER_CORE_REACTIVE
    public
    #else
    internal
    #endif
    readonly struct SelectProjectionMutator<TIn, TOut> : IProjectionMutator<TIn, TOut>
    {
        private readonly Func<TIn, TOut> selector;

        /// <summary>
        /// Initializes this transformation instance
        /// </summary>
        /// <param name="selector">A transform function to apply to each element</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SelectProjectionMutator(Func<TIn, TOut> selector)
        {
            this.selector = selector;
        }
        
        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Invoke(in TIn value, out TOut result)
        {
            result = selector(value);
            return true;
        }
    }
    
    /// <summary>
    /// Projects each element of a sequence into a new form
    /// </summary>
    /// <typeparam name="TIn">The type of the elements of the data source</typeparam>
    /// <typeparam name="TOut">The type of the value returned by selector</typeparam>
    /// <typeparam name="MutatorT">A conditional data flow controller</typeparam>
    #if EXPORT_HAMPER_CORE_REACTIVE
    public
    #else
    internal
    #endif
    readonly struct SelectProjectionMutator<TIn, TOut, MutatorT> : IProjectionMutator<TIn, TOut>
        where MutatorT : struct, IGatingMutator<TIn>
    {
        private readonly Func<TIn, TOut> selector;
        private readonly MutatorT mutator;
        
        /// <summary>
        /// Initializes this transformation instance
        /// </summary>
        /// <param name="mutator">A nested data processing unit to chain</param>
        /// <param name="selector">A transform function to apply to each element</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SelectProjectionMutator(MutatorT mutator, Func<TIn, TOut> selector)
        {
            this.selector = selector;
            this.mutator = mutator;
        }
        
        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Invoke(in TIn value, out TOut result)
        {
            if (mutator.Invoke(value))
            {
                result = selector(value);
                return true;
            }
            else
            {
                #pragma warning disable CS8601
                result = default;
                #pragma warning restore CS8601
                
                return false;
            }
        }
    }
    
    /// <summary>
    /// Projects each element of a sequence into a new form
    /// </summary>
    /// <typeparam name="TIn">The type of the elements of the data source</typeparam>
    /// <typeparam name="TTmp">The output type of the data processing unit to chain</typeparam>
    /// <typeparam name="TOut">The type of the value returned by selector</typeparam>
    /// <typeparam name="MutatorT">A data transformation unit</typeparam>
    #if EXPORT_HAMPER_CORE_REACTIVE
    public
    #else
    internal
    #endif
    readonly struct SelectProjectionMutator<TIn, TTmp, TOut, MutatorT> : IProjectionMutator<TIn, TOut>
        where MutatorT : struct, IProjectionMutator<TIn, TTmp>
    {
        private readonly Func<TTmp, TOut> selector;
        private readonly MutatorT mutator;
        
        /// <summary>
        /// Initializes this transformation instance
        /// </summary>
        /// <param name="mutator">A nested data processing unit to chain</param>
        /// <param name="selector">A transform function to apply to each element</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SelectProjectionMutator(MutatorT mutator, Func<TTmp, TOut> selector)
        {
            this.selector = selector;
            this.mutator = mutator;
        }
        
        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Invoke(in TIn value, out TOut result)
        {
            if (mutator.Invoke(value, out TTmp tmp))
            {
                result = selector(tmp);
                return true;
            }
            else
            {
                
                #pragma warning disable CS8601
                result = default;
                #pragma warning restore CS8601
                
                return false;
            }
        }
    }
}