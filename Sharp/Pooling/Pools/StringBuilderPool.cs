// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;
using System.Text;

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
    sealed class StringBuilderPool : ObjectPool<StringBuilder, StringBuilderPolicy>
    {
        private static readonly StringBuilderPool instance;
        /// <summary>
        /// A shared instance of the <typeef name="StringBuilder"/> pool
        /// </summary>
        public static StringBuilderPool Shared
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return instance; }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static StringBuilderPool()
        {
            instance = new StringBuilderPool();
        }
        
        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected override bool Dispose(bool disposing)
        {
            if (disposing && this == instance)
            {
                throw new InvalidOperationException(string.Concat(nameof(StringBuilderPool), ".Dispose of shared instance"));
            }
            else return base.Dispose(disposing);
        }
    }
}