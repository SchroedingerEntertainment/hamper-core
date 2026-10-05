// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Diagnostics.Contracts;

namespace Soe.Reactive
{
    /// <summary>
    /// A generic data flow controller
    /// </summary>
    /// <typeparam name="T">The object that provides notification information</typeparam>
    #if EXPORT_HAMPER_CORE_REACTIVE
    public
    #else
    internal
    #endif
    interface IGatingMutator<T>
    {
        /// <summary>
        /// Provides a branching point in a certain data flow
        /// </summary>
        /// <param name="value">The object that provides notification information</param>
        /// <returns>True if the object provided fulfills the branching criteria, false otherwise</returns>
        [Pure]
        bool Invoke(in T value);
    }
}