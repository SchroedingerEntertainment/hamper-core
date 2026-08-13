// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Soe.Collections.Embedded
{
    #if EXPORT_HAMPER_CORE_COLLECTIONS_EMBEDDED
    public
    #else
    internal
    #endif
    partial struct EmbeddedDictionary<TKey, TValue>
    {
        public struct Enumerator : IEnumerator<KeyValuePair<TKey, TValue>>
        {
            private readonly HashEntry[]? buffer;
            private readonly int count;
            private int index;

            public KeyValuePair<TKey, TValue> Current
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get { return new KeyValuePair<TKey,TValue>(buffer![index].Key, buffer![index].Value); }
            }

            object? IEnumerator.Current
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get { return Current; }
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal Enumerator(HashEntry[]? buffer, int count)
            {
                this.buffer = buffer;
                this.count = count;
                this.index = -1;
            }

            /// <inheritdoc/>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool MoveNext()
            {
                if (index + 1 < count)
                {
                    index++;
                    for (; index < count; index++)
                    {
                        if (buffer![index].IsValid)
                            return true;
                    }
                    return false;
                }
                else return false;
            }

            /// <inheritdoc/>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void Reset()
            {
                index = 0;
            }

            /// <inheritdoc/>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void Dispose()
            { }
        }
    }
}