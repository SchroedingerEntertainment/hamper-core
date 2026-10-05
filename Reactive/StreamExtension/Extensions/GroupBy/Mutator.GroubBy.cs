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
        /// Groups the elements of a sequence according to a specified key selector function
        /// </summary>
        /// <param name="mutator">The representation of a data processing unit to chain</param>
        /// <param name="selector">A function to extract the key for each element</param>
        /// <typeparam name="TKey">The type of the key returned by selector</typeparam>
        /// <typeparam name="TValue">The type of the elements of the data source</typeparam>
        /// <returns>A representation of a data projection unit</returns>
        /// <typeparam name="MutatorT">A conditional data flow controller</typeparam>
        /// <typeparam name="ObservableT">An observable data source to modify</typeparam>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Mutator<TValue, TKey, TValue, ObservableT, GroupedProjectionMutator<TKey, TValue, MutatorT>> GroupBy<TKey, TValue, ObservableT, MutatorT>(this Mutator<TValue, ObservableT, MutatorT> mutator, Func<TValue, TKey> selector)
            where ObservableT : class
            where MutatorT : struct, IGatingMutator<TValue>
        {
            return new Mutator<TValue, TKey, TValue, ObservableT, GroupedProjectionMutator<TKey, TValue, MutatorT>>(mutator.Source, new GroupedProjectionMutator<TKey, TValue, MutatorT>(mutator.Instance, selector));
        }
        
        /// <summary>
        /// Groups the elements of a sequence according to a specified key selector function
        /// </summary>
        /// <param name="mutator">The representation of a data processing unit to chain</param>
        /// <param name="keySelector">A function to extract the key for each element</param>
        /// <param name="valueSelector">A function to map each source element to an output element</param>
        /// <typeparam name="T">The type of the elements of the data source</typeparam>
        /// <typeparam name="TKey">The type of the key returned by selector</typeparam>
        /// <typeparam name="TValue">The type of the transformed elements of the data source</typeparam>
        /// <returns>A representation of a data projection unit</returns>
        /// <typeparam name="MutatorT">A conditional data flow controller</typeparam>
        /// <typeparam name="ObservableT">An observable data source to modify</typeparam>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Mutator<T, TKey, TValue, ObservableT, GroupedProjectionMutator<T, TKey, TValue, MutatorT>> GroupBy<T, TKey, TValue, ObservableT, MutatorT>(this Mutator<T, ObservableT, MutatorT> mutator, Func<T, TKey> keySelector, Func<T, TValue> valueSelector)
            where ObservableT : class
            where MutatorT : struct, IGatingMutator<T>
        {
            return new Mutator<T, TKey, TValue, ObservableT, GroupedProjectionMutator<T, TKey, TValue, MutatorT>>(mutator.Source, new GroupedProjectionMutator<T, TKey, TValue, MutatorT>(mutator.Instance, keySelector, valueSelector));
        }
        
        /// <summary>
        /// Groups the elements of a sequence according to a specified key selector function
        /// </summary>
        /// <param name="mutator">The representation of a data processing unit to chain</param>
        /// <param name="selector">A function to extract the key for each element</param>
        /// <typeparam name="T">The type of the elements of the data source</typeparam>
        /// <typeparam name="TKey">The type of the key returned by selector</typeparam>
        /// <typeparam name="TValue">The type of the transformed elements of the data source</typeparam>
        /// <returns>A representation of a data projection unit</returns>
        /// <typeparam name="MutatorT">A data projection unit</typeparam>
        /// <typeparam name="ObservableT">An observable data source to modify</typeparam>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Mutator<T, TKey, TValue, ObservableT, GroupProjectionMutator<T, TKey, TValue, MutatorT>> GroupBy<T, TKey, TValue, ObservableT, MutatorT>(this Mutator<T, TValue, ObservableT, MutatorT> mutator, Func<TValue, TKey> selector)
            where ObservableT : class
            where MutatorT : struct, IProjectionMutator<T, TValue>
        {
            return new Mutator<T, TKey, TValue, ObservableT, GroupProjectionMutator<T, TKey, TValue, MutatorT>>(mutator.Source, new GroupProjectionMutator<T, TKey, TValue, MutatorT>(mutator.Instance, selector));
        }
        
        /// <summary>
        /// Groups the elements of a sequence according to a specified key selector function
        /// </summary>
        /// <param name="mutator">The representation of a data processing unit to chain</param>
        /// <param name="keySelector">A function to extract the key for each element</param>
        /// <param name="valueSelector">A function to map each source element to an output element</param>
        /// <typeparam name="TIn">The type of the elements of the data source</typeparam>
        /// <typeparam name="TOut">The output type of the data processing unit to chain</typeparam>
        /// <typeparam name="TKey">The type of the key returned by selector</typeparam>
        /// <typeparam name="TValue">The type of the transformed elements of the data source</typeparam>
        /// <returns>A representation of a data projection unit</returns>
        /// <typeparam name="MutatorT">A data projection unit</typeparam>
        /// <typeparam name="ObservableT">An observable data source to modify</typeparam>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Mutator<TIn, TKey, TValue, ObservableT, GroupedProjectionMutator<TIn, TOut, TKey, TValue, MutatorT>> GroupBy<TIn, TOut, TKey, TValue, ObservableT, MutatorT>(this Mutator<TIn, TOut, ObservableT, MutatorT> mutator, Func<TOut, TKey> keySelector, Func<TOut, TValue> valueSelector)
            where ObservableT : class
            where MutatorT : struct, IProjectionMutator<TIn, TOut>
        {
            return new Mutator<TIn, TKey, TValue, ObservableT, GroupedProjectionMutator<TIn, TOut, TKey, TValue, MutatorT>>(mutator.Source, new GroupedProjectionMutator<TIn, TOut, TKey, TValue, MutatorT>(mutator.Instance, keySelector, valueSelector));
        }
    }
}