// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System;

namespace Soe.Reactive
{
    /// <summary>
    /// Defines a provider for push-based notification
    /// </summary>
    /// <typeparam name="TValue">The object that provides notification information</typeparam>
    /// <typeparam name="TKey">The object that provides information about the target subscriptions</typeparam>
    #if EXPORT_HAMPER_CORE_REACTIVE
    public
    #else
    internal
    #endif
    interface IObservable<TKey, out TValue>
    {
        /// <summary>
        /// Notifies the provider that an observer is to receive notifications
        /// </summary>
        /// <param name="key">The object that provides information about the observer</param>
        /// <param name="observer">The object that is to receive notifications</param>
        /// <returns>A reference to an interface that allows observers to stop receiving notifications before
        /// the provider has finished sending them</returns>
        IDisposable Subscribe(in TKey key, IObserver<TValue> observer);
    }
}