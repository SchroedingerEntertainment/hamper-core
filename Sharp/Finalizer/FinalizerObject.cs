// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;
using System.Threading;

namespace System
{
    /// <summary>
    /// Performs cleanup operations on unmanaged resources held by the current object before the object is destroyed
    /// </summary>
    #if EXPORT_HAMPER_CORE_SHARP
    public
    #else
    internal
    #endif
    class FinalizerObject : IDisposable
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
        /// Creates a new interconnected object instance
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected FinalizerObject()
        { }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        ~FinalizerObject()
        {
            Dispose(false);
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            if (Dispose(true))
            {
                GC.SuppressFinalize(this);
            }
        }
        
        /// <summary>
        /// Called when the object is about to being disposed either by user interaction or the GC
        /// </summary>
        /// <remarks>An overriding method must check for the return value and perform any critical operations
        /// when the flag ist set. Setting the flag is exclusive and the calling thread becomes responsible to
        /// release any resources currently held by this object</remarks>
        /// <param name="disposing">Signals if dispose is performed by user interaction</param>
        /// <returns>True if the flag was set, false otherwise</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected virtual bool Dispose(bool disposing)
        {
            if (disposing)
            {
                return (Interlocked.CompareExchange(ref isDisposed, 1, 0) == 0);
            }
            else return false;
        }
    }
}