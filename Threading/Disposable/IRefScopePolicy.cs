// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Diagnostics.Contracts;

namespace Soe.Threading
{
    /// <summary>
    /// Manages a reference based value
    /// </summary>
    #if EXPORT_HAMPER_CORE_THREADING
    public
    #else
    internal
    #endif
    interface IRefScopePolicy<T>
    {
        /// <summary>
        /// Acquires a certain state based on the provided parameter
        /// </summary>
        /// <param name="parameter">A reference value</param>
        static abstract void Acquire(ref T parameter);
        
        // ReSharper disable PureAttributeOnVoidMethod
        
        /// <summary>
        /// Releases the state of the provided parameter
        /// </summary>
        /// <param name="parameter">A reference value</param>
        [Pure]
        static abstract void Dispose(ref T parameter);
        
        // ReSharper restore PureAttributeOnVoidMethod
    }
}