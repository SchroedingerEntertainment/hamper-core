// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

namespace System.Buffers
{
    /// <summary>
    /// Provides a resource pool that enables reusing instances of type <typeparamref name="T"/>
    /// </summary>
    /// <typeparam name="T">A reference type</typeparam>
    #if HAMPER_CORE_SHARP
    public
    #else
    internal
    #endif
    interface IObjectPool<T>
        where T : class
    {
        /// <summary>
        /// Clears all objects currently pooled
        /// </summary>
        public void Clear();

        /// <summary>
        /// Obtains an object instance of type <typeparamref name="T"/> from the pool or creates one
        /// </summary>
        /// <returns>An object instance</returns>
        public T Rent();

        /// <summary>
        /// Returns an object instance of type <typeparamref name="T"/> to the pool 
        /// </summary>
        /// <param name="instance">The object instance</param>
        public void Return(T instance);
    }
}