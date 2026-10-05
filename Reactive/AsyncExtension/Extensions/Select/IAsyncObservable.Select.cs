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
    static partial class IAsyncObservableExtension
    {
        /// <summary>
        /// Projects each element of a sequence into a new form
        /// </summary>
        /// <param name="observable">The object that is to receive notifications</param>
        /// <param name="selector">A transform function to apply to each element</param>
        /// <typeparam name="TIn">The type of the elements of the data source</typeparam>
        /// <typeparam name="TOut">The type of the value returned by selector</typeparam>
        /// <returns>A representation of a data projection unit</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Mutator<TIn, TOut, IAsyncObservable<TIn>, SelectProjectionMutator<TIn, TOut>> Select<TIn, TOut>(this IAsyncObservable<TIn> observable, Func<TIn, TOut> selector)
        {
            return new Mutator<TIn, TOut, IAsyncObservable<TIn>, SelectProjectionMutator<TIn, TOut>>(observable, new SelectProjectionMutator<TIn, TOut>(selector));
        }
        
        /// <summary>
        /// Projects each element of a sequence into a new form
        /// </summary>
        /// <param name="observable">The object that is to receive notifications</param>
        /// <param name="selector">A transform function to apply to each element</param>
        /// <typeparam name="TKey">The type of the selection key of the data source</typeparam>
        /// <typeparam name="TIn">The type of the elements of the data source</typeparam>
        /// <typeparam name="TOut">The type of the value returned by selector</typeparam>
        /// <returns>A representation of a data projection unit</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Mutator<TIn, TOut, IAsyncObservable<TKey, TIn>, SelectProjectionMutator<TIn, TOut>> Select<TKey, TIn, TOut>(this IAsyncObservable<TKey, TIn> observable, Func<TIn, TOut> selector)
        {
            return new Mutator<TIn, TOut, IAsyncObservable<TKey, TIn>, SelectProjectionMutator<TIn, TOut>>(observable, new SelectProjectionMutator<TIn, TOut>(selector));
        }
    }
}