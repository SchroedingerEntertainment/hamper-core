// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;

namespace Soe.Threading
{
    /// <summary>
    /// Represents a short living disposable instance of a certain value or reference
    /// </summary>
    #if EXPORT_HAMPER_CORE_THREADING
    public
    #else
    internal
    #endif
    static class ScopedDisposable
    {
        /// <summary>
        /// Acquires a certain state based on the provided parameter
        /// </summary>
        /// <param name="parameter">A reference value</param>
        /// <typeparam name="T">Any type</typeparam>
        /// <typeparam name="Policy">A policy managing the lifetime and behavior of the state</typeparam>
        /// <returns>A short living disposable instance</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RefScopedDisposable<T, Policy> Acquire<T, Policy>(ref T parameter)
            where Policy : struct, IRefScopePolicy<T>
        {
            Policy.Acquire(ref parameter);
            return new RefScopedDisposable<T, Policy>(ref parameter);
        }

        /// <summary>
        /// Acquires a certain state based on the provided parameter
        /// </summary>
        /// <param name="parameter">A reference value</param>
        /// <param name="policy">A policy managing the lifetime and behavior of the state</param>
        /// <typeparam name="T">Any type</typeparam>
        /// <typeparam name="Policy">A policy managing the lifetime and behavior of the state</typeparam>
        /// <returns>A short living disposable instance</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RefScopedDisposable<T, Policy> Acquire<T, Policy>(ref T parameter, Policy policy)
            where Policy : struct, IRefScopePolicy<T>
        {
            return Acquire<T, Policy>(ref parameter);
        }
        
        /// <summary>
        /// Instantiates a disposable instance of the provided parameter without changing the state
        /// </summary>
        /// <param name="parameter">A reference value</param>
        /// <typeparam name="T">Any type</typeparam>
        /// <typeparam name="Policy">A policy managing the lifetime and behavior of the state</typeparam>
        /// <returns>A short living disposable instance</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RefScopedDisposable<T, Policy> Create<T, Policy>(ref T parameter)
            where Policy : struct, IRefScopePolicy<T>
        {
            return new RefScopedDisposable<T, Policy>(ref parameter);
        }

        /// <summary>
        /// Instantiates a disposable instance of the provided parameter without changing the state
        /// </summary>
        /// <param name="parameter">A reference value</param>
        /// <param name="policy">A policy managing the lifetime and behavior of the state</param>
        /// <typeparam name="T">Any type</typeparam>
        /// <typeparam name="Policy">A policy managing the lifetime and behavior of the state</typeparam>
        /// <returns>A short living disposable instance</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RefScopedDisposable<T, Policy> Create<T, Policy>(ref T parameter, Policy policy)
            where Policy : struct, IRefScopePolicy<T>
        {
            return Create<T, Policy>(ref parameter);
        }
    }
    
    /// <summary>
    /// Represents a short living disposable instance of a certain value or reference
    /// </summary>
    /// <typeparam name="T">Any type</typeparam>
    /// <typeparam name="Policy">A policy managing the lifetime and behavior of the value or reference</typeparam>
    #if EXPORT_HAMPER_CORE_THREADING
    public
    #else
    internal
    #endif
    readonly ref struct ScopedDisposable<T, Policy>
        where Policy : struct, IScopePolicy<T>
    {
        private readonly T parameter;

        /// <summary>
        /// Initializes the disposable instance
        /// </summary>
        /// <param name="parameter">A value</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ScopedDisposable(T parameter)
        {
            this.parameter = parameter;
            
            Policy.Initialize(parameter);
        }
        
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting a certain state or behavior
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Dispose()
        {
            Policy.Dispose(parameter);
        }
    }
    
    /// <summary>
    /// Represents a short living disposable instance of a certain value or reference
    /// </summary>
    /// <typeparam name="T">Any type</typeparam>
    /// <typeparam name="Policy">A policy managing the lifetime and behavior of the value or reference</typeparam>
    #if EXPORT_HAMPER_CORE_THREADING
    public
    #else
    internal
    #endif
    readonly ref struct RefScopedDisposable<T, Policy>
        where Policy : struct, IRefScopePolicy<T>
    {
        private readonly ref T parameter;

        /// <summary>
        /// Initializes the disposable instance
        /// </summary>
        /// <param name="parameter">A reference value</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public RefScopedDisposable(ref T parameter)
        {
            this.parameter = ref parameter;
        }
        
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting a certain state or behavior
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Dispose()
        {
            Policy.Dispose(ref parameter);
        }
    }
}