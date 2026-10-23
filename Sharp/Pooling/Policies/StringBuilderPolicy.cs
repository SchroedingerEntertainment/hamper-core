// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;
using System.Text;

namespace System.Buffers
{
    /// <summary>
    /// A policy managing instances of type <typeef name="StringBuilder"/>
    /// </summary>
    #if HAMPER_CORE_SHARP
    public
    #else
    internal
    #endif
    readonly struct StringBuilderPolicy : IPoolPolicy<StringBuilder>
    {
        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public StringBuilder CreateInstance()
        {
            return new StringBuilder(32);
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void OnDispose(StringBuilder instance)
        {
            instance.Clear();
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void OnRent(ref StringBuilder instance)
        { }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void OnReturn(ref StringBuilder instance)
        {
            instance.Clear();
        }
    }
}