// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Soe.Reactive
{
    /// <summary>
    /// Represents a disposable resource that can be checked for its current state
    /// </summary>
    #if EXPORT_HAMPER_CORE_REACTIVE
    public
    #else
    internal
    #endif
    abstract partial class Disposable : IDisposable
    {
        private int isDisposed;
        
        /// <summary>
        /// Determines if this object has already been disposed
        /// </summary>
        public bool Disposed
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return (Volatile.Read(ref isDisposed) != 0); }
        }

        /// <summary>
        /// Constructor for internal use only
        /// </summary>
        /// <param name="isDisposed">True if the disposable should be sealed</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected internal Disposable(bool isDisposed)
        {
            this.isDisposed = (isDisposed ? 1 : 0);
        }
        /// <summary>
        /// Initializes a new disposable instance
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected Disposable()
            :this (false)
        { }
        
        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Dispose()
        {
            if (Interlocked.CompareExchange(ref isDisposed, 1, 0) != 0)
            {
                DisposeInternal();
            }
        }

        /// <summary>
        /// Called when the object is disposed for the first time
        /// </summary>
        /// <remarks>This method is called only once from whatever thread is disposing the object</remarks>
        protected abstract void DisposeInternal();
    }
}