// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;

namespace System.Buffers
{
    /// <summary>
    /// Provides a resource pool for reusable instances of <typeef name="StringBuilder"/>
    /// </summary>
    #if HAMPER_CORE_SHARP
    public
    #else
    internal
    #endif
    sealed class GenericPool<T, Policy> : ObjectPool<T, Policy>
        where T : class, new()
        where Policy : struct, IPoolPolicy<T>
    {
        private static readonly GenericPool<T, Policy> instance;
        /// <summary>
        /// A shared instance of the <typeparamref name="T"/> pool
        /// </summary>
        public static GenericPool<T, Policy> Shared
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return instance; }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static GenericPool()
        {
            instance = new GenericPool<T, Policy>();
        }
        
        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected override bool Dispose(bool disposing)
        {
            if (disposing && this == instance)
            {
                throw new InvalidOperationException(string.Concat(nameof(GenericPool<T, Policy>), ".Dispose of shared instance"));
            }
            else return base.Dispose(disposing);
        }
    }
}