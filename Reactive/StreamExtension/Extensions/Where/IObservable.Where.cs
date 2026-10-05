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
        /// Filters a sequence of values based on a predicate
        /// </summary>
        /// <param name="observable">The object that is to receive notifications</param>
        /// <param name="predicate">A function to test each element for a condition</param>
        /// <typeparam name="T">The type of the elements of the data source</typeparam>
        /// <returns>A representation of a conditional data flow controller</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Mutator<T, IObservable<T>, WhereConditionalMutator<T>> Where<T>(this IObservable<T> observable, Func<T, bool> predicate)
        {
            return new Mutator<T, IObservable<T>, WhereConditionalMutator<T>>(observable, new WhereConditionalMutator<T>(predicate));
        }
        
        /// <summary>
        /// Filters a sequence of values based on a predicate
        /// </summary>
        /// <param name="observable">The object that is to receive notifications</param>
        /// <param name="predicate">A function to test each element for a condition</param>
        /// <typeparam name="TKey">The type of the selection key of the data source</typeparam>
        /// <typeparam name="TValue">The type of the elements of the data source</typeparam>
        /// <returns>A representation of a conditional data flow controller</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Mutator<TValue,IObservable<TKey, TValue>, WhereConditionalMutator<TValue>> Where<TKey, TValue>(this IObservable<TKey, TValue> observable, Func<TValue, bool> predicate)
        {
            return new Mutator<TValue, IObservable<TKey, TValue>, WhereConditionalMutator<TValue>>(observable, new WhereConditionalMutator<TValue>(predicate));
        }
    }
}