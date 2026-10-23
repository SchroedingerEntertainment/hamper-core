// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;

namespace System.Buffers
{
    /// <summary>
    /// A generic pooling policy over type <typeparamref name="T"/>
    /// </summary>
    /// <typeparam name="T">A type that contains the standard .ctor</typeparam>
    #if HAMPER_CORE_SHARP
    public
    #else
    internal
    #endif
    readonly struct GenericPolicy<T> : IPoolPolicy<T>
        where T : class, new()
    {
        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T CreateInstance()
        {
            return new T();
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void OnDispose(T instance)
        {
            if (instance is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void OnRent(ref T instance)
        { }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void OnReturn(ref T instance)
        { }
    }
}