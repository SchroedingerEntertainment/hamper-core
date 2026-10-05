// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Diagnostics.Contracts;

namespace Soe.Threading
{
    /// <summary>
    /// Manages an instance type
    /// </summary>
    #if EXPORT_HAMPER_CORE_THREADING
    public
    #else
    internal
    #endif
    interface IScopePolicy<T>
    {
        // ReSharper disable PureAttributeOnVoidMethod
        
        /// <summary>
        /// Performs an initialization action based on the provided parameter
        /// </summary>
        /// <param name="parameter">A value</param>
        [Pure]
        static abstract void Initialize(in T parameter);
        
        /// <summary>
        /// Performs a deinitialization action based on the provided parameter
        /// </summary>
        /// <param name="parameter">A value</param>
        [Pure]
        static abstract void Dispose(in T parameter);
        
        // ReSharper restore PureAttributeOnVoidMethod
    }
}