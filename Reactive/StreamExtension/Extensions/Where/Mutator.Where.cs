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
        /// <summary>
        /// Filters a sequence of values based on a predicate
        /// </summary>
        /// <param name="mutator">The representation of a data processing unit to chain</param>
        /// <param name="predicate">A function to test each element for a condition</param>
        /// <typeparam name="T">The type of the elements of the data source</typeparam>
        /// <returns>A representation of a conditional data flow controller</returns>
        /// <typeparam name="MutatorT">A conditional data flow controller</typeparam>
        /// <typeparam name="ObservableT">An observable data source to modify</typeparam>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Mutator<T, ObservableT, WhereConditionalMutator<T, MutatorT>> Where<T, ObservableT, MutatorT>(this Mutator<T, ObservableT, MutatorT> mutator, Func<T, bool> predicate)
            where ObservableT : class
            where MutatorT : struct, IGatingMutator<T>
        {
            return new Mutator<T, ObservableT, WhereConditionalMutator<T, MutatorT>>(mutator.Source, new WhereConditionalMutator<T, MutatorT>(mutator.Instance, predicate));
        }

        /// <summary>
        /// Filters a sequence of values based on a predicate
        /// </summary>
        /// <param name="mutator">The representation of a data processing unit to chain</param>
        /// <param name="predicate">A function to test each element for a condition</param>
        /// <typeparam name="TIn">The type of the elements of the data source</typeparam>
        /// <typeparam name="TOut">The transformed type of the elements of the data source</typeparam>
        /// <returns>A representation of a conditional data flow controller</returns>
        /// <typeparam name="MutatorT">A conditional data flow controller</typeparam>
        /// <typeparam name="ObservableT">An observable data source to modify</typeparam>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Mutator<TIn, TOut, ObservableT, WhereConditionalMutator<TIn,TOut, MutatorT>> Where<TIn, TOut, ObservableT, MutatorT>(this Mutator<TIn, TOut, ObservableT, MutatorT> mutator, Func<TOut, bool> predicate)
            where ObservableT : class
            where MutatorT : struct, IProjectionMutator<TIn, TOut>
        {
            return new Mutator<TIn, TOut, ObservableT, WhereConditionalMutator<TIn, TOut, MutatorT>>(mutator.Source, new WhereConditionalMutator<TIn, TOut, MutatorT>(mutator.Instance, predicate));
        }
    }
}