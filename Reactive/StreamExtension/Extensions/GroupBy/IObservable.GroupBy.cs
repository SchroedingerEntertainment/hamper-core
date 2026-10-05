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
    static partial class IObservableExtension
    {
        /// <summary>
        /// Groups the elements of a sequence according to a specified key selector function
        /// </summary>
        /// <param name="observable">The object that is to receive notifications</param>
        /// <param name="selector">A function to extract the key for each element</param>
        /// <typeparam name="TKey">The type of the key returned by selector</typeparam>
        /// <typeparam name="TValue">The type of the elements of the data source</typeparam>
        /// <returns>A representation of a data projection unit</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Mutator<TValue, TKey, TValue, IObservable<TValue>, GroupProjectionMutator<TKey, TValue>> GroupBy<TKey, TValue>(this IObservable<TValue> observable, Func<TValue, TKey> selector)
        {
            return new Mutator<TValue, TKey, TValue, IObservable<TValue>, GroupProjectionMutator<TKey, TValue>>(observable, new  GroupProjectionMutator<TKey, TValue>(selector));
        }
        
        /// <summary>
        /// Groups the elements of a sequence according to a specified key selector function
        /// </summary>
        /// <param name="observable">The object that is to receive notifications</param>
        /// <param name="selector">A function to extract the key for each element</param>
        /// <typeparam name="TObservableKey">The type of the selection key of the data source</typeparam>
        /// <typeparam name="TKey">The type of the key returned by selector</typeparam>
        /// <typeparam name="TValue">The type of the elements of the data source</typeparam>
        /// <returns>A representation of a data projection unit</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Mutator<TValue, TKey, TValue, IObservable<TObservableKey, TValue>, GroupProjectionMutator<TKey, TValue>> GroupBy<TObservableKey, TKey, TValue>(this IObservable<TObservableKey, TValue> observable, Func<TValue, TKey> selector)
        {
            return new Mutator<TValue, TKey, TValue, IObservable<TObservableKey, TValue>, GroupProjectionMutator<TKey, TValue>>(observable, new  GroupProjectionMutator<TKey, TValue>(selector));
        }
        
        /// <summary>
        /// Groups the elements of a sequence according to a specified key selector function
        /// </summary>
        /// <param name="observable">The object that is to receive notifications</param>
        /// <param name="keySelector">A function to extract the key for each element</param>
        /// <param name="valueSelector">A function to map each source element to an output element</param>
        /// <typeparam name="TIn">The type of the elements of the data source</typeparam>
        /// <typeparam name="TKey">The type of the key returned by selector</typeparam>
        /// <typeparam name="TValue">The type of the transformed elements of the data source</typeparam>
        /// <returns>A representation of a data projection unit</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Mutator<TIn, TKey, TValue, IObservable<TIn>, GroupProjectionMutator<TIn, TKey, TValue>> GroupBy<TIn, TKey, TValue>(this IObservable<TIn> observable, Func<TIn, TKey> keySelector, Func<TIn, TValue> valueSelector)
        {
            return new Mutator<TIn, TKey, TValue, IObservable<TIn>, GroupProjectionMutator<TIn, TKey, TValue>>(observable, new  GroupProjectionMutator<TIn, TKey, TValue>(keySelector, valueSelector));
        }
        
        /// <summary>
        /// Groups the elements of a sequence according to a specified key selector function
        /// </summary>
        /// <param name="observable">The object that is to receive notifications</param>
        /// <param name="keySelector">A function to extract the key for each element</param>
        /// <param name="valueSelector">A function to map each source element to an output element</param>
        /// <typeparam name="TObservableKey">The type of the selection key of the data source</typeparam>
        /// <typeparam name="TIn">The type of the elements of the data source</typeparam>
        /// <typeparam name="TKey">The type of the key returned by selector</typeparam>
        /// <typeparam name="TValue">The type of the transformed elements of the data source</typeparam>
        /// <returns>A representation of a data projection unit</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Mutator<TIn, TKey, TValue, IObservable<TObservableKey, TIn>, GroupProjectionMutator<TIn, TKey, TValue>> GroupBy<TObservableKey, TIn, TKey, TValue>(this IObservable<TObservableKey, TIn> observable, Func<TIn, TKey> keySelector, Func<TIn, TValue> valueSelector)
        {
            return new Mutator<TIn, TKey, TValue, IObservable<TObservableKey, TIn>, GroupProjectionMutator<TIn, TKey, TValue>>(observable, new  GroupProjectionMutator<TIn, TKey, TValue>(keySelector, valueSelector));
        }
    }
}