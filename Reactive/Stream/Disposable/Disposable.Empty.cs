// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System;
using System.Runtime.CompilerServices;

namespace Soe.Reactive
{
    #if EXPORT_HAMPER_CORE_REACTIVE
    public
    #else
    internal
    #endif
    abstract partial class Disposable
    {
        /// <summary>
        /// A default implementation
        /// </summary>
        private class EmptyDisposable : Disposable
        {
            /// <summary>
            /// Initializes the object to an already disposed state
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public EmptyDisposable()
                : base(true)
            { }
            
            /// <inheritdoc/>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            protected override void DisposeInternal()
            { }
        }

        private static readonly EmptyDisposable empty;

        /// <summary>
        /// A default disposable implementation
        /// </summary>
        public static IDisposable Empty
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return empty; }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static Disposable()
        {
            empty = new EmptyDisposable();
        }
    }
}