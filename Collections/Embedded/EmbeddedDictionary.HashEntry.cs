// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;
using Soe.Collections.HashSet;

namespace Soe.Collections.Embedded
{
    #if EXPORT_HAMPER_CORE_COLLECTIONS_EMBEDDED
    public
    #else
    internal
    #endif
    partial struct EmbeddedDictionary<TKey, TValue>
    {
        internal struct HashEntry : IHashContainer<TKey>
        {
            private readonly TKey key;
            /// <summary>
            /// Gets this elements key
            /// </summary>
            public TKey Key
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get { return key; }
            }
            
            /// <summary>
            /// This elements value
            /// </summary>
            public TValue Value;
            
            private readonly int hash;
            /// <summary>
            /// Gets this elements hash code
            /// </summary>
            public int Hash
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get { return hash; }
            }
            
            /// <summary>
            /// Gets if this element has a memory handle assigned to
            /// </summary>
            public bool IsValid
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get { return hash != 0; }
            }

            /// <summary>
            /// Initializes this element with the corresponding slot index and a reserved handle
            /// </summary>
            /// <param name="key">This elements key</param>
            /// <param name="hash">This elements hash value</param>
            /// <param name="value">The initial value of the element</param>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public HashEntry(in TKey key, int hash, in TValue value)
            {
                this.key = key;
                this.Value = value!;
                this.hash = hash;
            }
        }
    }
}