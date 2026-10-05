// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;

namespace System.Buffers
{
    /// <summary>
    /// Manages a single instance of type <typeparamref name="T"/> from the given pool
    /// </summary>
    /// <typeparam name="T">A reference type</typeparam>
    [method: MethodImpl(MethodImplOptions.AggressiveInlining)]
    #if HAMPER_CORE_SHARP
    public
    #else
    internal
    #endif
    readonly struct PooledObjectDisposable<T>(IObjectPool<T> pool, T instance) : IDisposable
        where T : class
    {
        private readonly T instance = instance;

        /// <summary>
        /// The instance rented from the underlying pool
        /// </summary>
        public T Instance
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return instance; }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator T(PooledObjectDisposable<T> disposable)
        {
            return disposable.instance;
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Dispose()
        {
            pool.Return(instance);
        }
    }
}