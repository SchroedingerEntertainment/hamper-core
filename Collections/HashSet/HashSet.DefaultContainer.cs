// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;

namespace Soe.Collections.HashSet
{
    #if EXPORT_HAMPER_CORE_COLLECTIONS_HASHSET
    public
    #else
    internal
    #endif
    static partial class HashSet
    {
        /// <summary>
        /// A default element for this <see cref="HashSet{T,Container}"/>
        /// </summary>
        /// <param name="hash">The hash code of this element</param>
        /// <param name="key">The key of this element</param>
        [method: MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly struct DefaultContainer<T>(int hash, T key) : IHashContainer<T>
        {
            /// <inheritdoc/>
            public int Hash
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get { return hash; }
            }

            /// <inheritdoc/>
            public T Key
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get { return key; }
            }

            /// <inheritdoc/>
            public bool IsValid
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get { return (hash != 0 && key != null); }
            }
        }
    }
}