// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Diagnostics.Contracts;

namespace Soe.Reactive
{
    /// <summary>
    /// Handles the dispose operation performed on a generic disposable object
    /// </summary>
    #if EXPORT_HAMPER_CORE_REACTIVE
    public
    #else
    internal
    #endif
    interface IDisposeHandler<in Target, in T>
        where Target : class
        where T : class
    {
        /// <summary>
        /// The function invoked when the underlying object is disposed
        /// </summary>
        /// <param name="target">The target instance to perform the operation on</param>
        /// <param name="instance">An instance parameter</param>
        // ReSharper disable PureAttributeOnVoidMethod
        
        [Pure]
        void Invoke(Target target, T instance);
        
        // ReSharper restore PureAttributeOnVoidMethod
    }
    
    /// <summary>
    /// Handles the dispose operation performed on a generic disposable object
    /// </summary>
    #if EXPORT_HAMPER_CORE_REACTIVE
    public
    #else
    internal
    #endif
    interface IDisposeHandler<in Target, TKey, in TValue>
        where Target : class
        where TValue : class
    {
        /// <summary>
        /// The function invoked when the underlying object is disposed
        /// </summary>
        /// <param name="target">The target instance to perform the operation on</param>
        /// <param name="key">The object that provides information about the observer</param>
        /// <param name="instance">An instance parameter</param>
        // ReSharper disable PureAttributeOnVoidMethod
        
        [Pure]
        void Invoke(Target target, in TKey key, TValue instance);
        
        // ReSharper restore PureAttributeOnVoidMethod
    }
}