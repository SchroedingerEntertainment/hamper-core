// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Diagnostics.Contracts;

namespace Soe.Reactive
{
    /// <summary>
    /// A generic data projection unit
    /// </summary>
    /// <typeparam name="TIn">The object that provides notification information</typeparam>
    /// <typeparam name="TOut">The transformed object that provides notification information</typeparam>
    #if EXPORT_HAMPER_CORE_REACTIVE
    public
    #else
    internal
    #endif
    interface IProjectionMutator<TIn, TOut>
    {
        /// <summary>
        /// Provides a transformation operation in the data flow
        /// </summary>
        /// <param name="value">The object that provides notification information</param>
        /// <param name="result">The transformed object that provides notification information</param>
        /// <returns>True if the transformation is valid, false otherwise</returns>
        [Pure]
        bool Invoke(in TIn value, out TOut result);
    }
    
    /// <summary>
    /// A conditional data projection unit
    /// </summary>
    /// <typeparam name="T">The object that provides notification information</typeparam>
    /// <typeparam name="TKey">The object that provides information to the conditional branching</typeparam>
    /// <typeparam name="TValue">The transformed object that provides notification information</typeparam>
    #if EXPORT_HAMPER_CORE_REACTIVE
    public
    #else
    internal
    #endif
    interface IProjectionMutator<T, TKey, TValue>
    {
        /// <summary>
        /// Provides a conditional transformation operation in the data flow
        /// </summary>
        /// <param name="value">The object that provides notification information</param>
        /// <param name="key">The object that provides information to the conditional branching</param>
        /// <param name="result">The transformed object that provides notification information</param>
        /// <returns>True if the transformation is valid, false otherwise</returns>
        [Pure]
        bool Invoke(in T value, out TKey key, out TValue result);
    }
}