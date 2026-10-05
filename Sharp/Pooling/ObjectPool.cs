// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;
using Soe.Collections.Inline;
using Soe.Threading;

namespace System.Buffers
{
    /// <summary>
    /// Provides a resource pool that enables reusing instances of type <typeparamref name="T"/>
    /// </summary>
    /// <typeparam name="T">A reference type</typeparam>
    /// <typeparam name="Policy">A policy managing type <typeparamref name="T"/></typeparam>
    #if HAMPER_CORE_SHARP
    public
    #else
    internal
    #endif
    partial class ObjectPool<T, Policy> : FinalizerObject, IObjectPool<T>
        where T : class
        where Policy : struct, IPoolPolicy<T>
    {
        private readonly Policy policy;
        private FixedArray<T?, FixedArray16<T?>> array;
        private ConcurrentBuffer<T> pool;
        
        /// <summary>
        /// Gets the amount of objects currently pooled
        /// </summary>
        public int Count
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return pool.Count; }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ObjectPool()
        {
            this.policy = default;
            this.array = default;
            this.pool = default;
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Clear()
        {
            if (!Disposed)
            {
                for (; pool.TryDequeue(ref array, out T? instance);)
                {
                    if(instance != null)
                    {
                        // ReSharper disable PossiblyImpureMethodCallOnReadonlyVariable
                        
                        policy.OnDispose(instance);
                        
                        // ReSharper restore PossiblyImpureMethodCallOnReadonlyVariable
                    }
                }
            }
            else throw ThrowOnDisposed();
        }

        /// <summary>
        /// Obtains a disposable instance of type <typeparamref name="T"/> from the pool or creates one
        /// </summary>
        /// <returns>A disposable instance to use in a temporary block</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PooledObjectDisposable<T> Loan()
        {
            return new PooledObjectDisposable<T>(this, Rent());
        }
        
        /// <inheritdoc/>
        /// <exception cref="ObjectDisposedException">The pool has been disposed</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Rent()
        {
            if (!Disposed)
            {
                using(ScopedDisposable.Acquire<ConcurrentBuffer<T>, ConcurrentBuffer<T>.ExclusiveOperation>(ref pool))
                {
                    if (pool.TryDequeue(ref array, out T? instance))
                    {
                        if (instance != null)
                        {
                            // ReSharper disable PossiblyImpureMethodCallOnReadonlyVariable

                            policy.OnRent(ref instance);

                            // ReSharper restore PossiblyImpureMethodCallOnReadonlyVariable

                            return instance;
                        }
                    }
                }
                // ReSharper disable PossiblyImpureMethodCallOnReadonlyVariable
                
                return policy.CreateInstance();
                
                // ReSharper restore PossiblyImpureMethodCallOnReadonlyVariable
            }
            else throw ThrowOnDisposed();
        }

        /// <inheritdoc/>
        /// <exception cref="ObjectDisposedException">The pool has been disposed</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Return(T instance)
        {
            if (!Disposed)
            {
                // ReSharper disable PossiblyImpureMethodCallOnReadonlyVariable
                
                policy.OnReturn(ref instance);
                
                // ReSharper restore PossiblyImpureMethodCallOnReadonlyVariable
                
                if (!pool.TryEnqueue(ref array, instance, out _))
                {
                    // ReSharper disable PossiblyImpureMethodCallOnReadonlyVariable
                        
                    policy.OnDispose(instance);
                        
                    // ReSharper restore PossiblyImpureMethodCallOnReadonlyVariable
                }
            }
            else throw ThrowOnDisposed();
        }
        
        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected override bool Dispose(bool disposing)
        {
            if (base.Dispose(disposing))
            {
                array.Clear();
                pool.Reset();
                return true;
            }
            else return false;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        ObjectDisposedException ThrowOnDisposed()
        {
            return new ObjectDisposedException(GetType().Name);
        }
    }
}