// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

namespace System.Buffers
{
    /// <summary>
    /// A policy managing the pooling behavior of type <typeparamref name="T"/>
    /// </summary>
    /// <typeparam name="T">A reference type</typeparam>
    #if HAMPER_CORE_SHARP
    public
    #else
    internal
    #endif
    interface IPoolPolicy<T>
        where T : class
    {
        /// <summary>
        /// Creates a new instance of type <typeparamref name="T"/>
        /// </summary>
        /// <returns>A non-null instance</returns>
        T CreateInstance();

        /// <summary>
        /// Invoked when the underlying pool disposes an instance of type <typeparamref name="T"/>
        /// </summary>
        /// <param name="instance">The instance to be disposed</param>
        void OnDispose(T instance);
        
        /// <summary>
        /// Invoked when the underlying pool is about to reuse a pooled instance of type <typeparamref name="T"/>
        /// </summary>
        /// <param name="instance">The instance to be returned to the caller</param>
        void OnRent(ref T instance);
        
        /// <summary>
        /// Invoked when the underlying pool received an instance to queue for reuse
        /// </summary>
        /// <param name="instance">The instance of type <typeparamref name="T"/> returned to the pool</param>
        void OnReturn(ref T instance);
    }
}