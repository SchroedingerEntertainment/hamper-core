// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;
using Soe.Collections.HashSet;

namespace Soe.Composable
{
    #if EXPORT_HAMPER_CORE_COMPOSITION
    public
    #else
    internal
    #endif
    partial class Shard
    {
        [method: MethodImpl(MethodImplOptions.AggressiveInlining)]
        struct ComponentContainer(IComponent instance, int hash, Type key) : IHashContainer<Type>
        {
            /// <inheritdoc/>
            public int Hash
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get { return hash; }
            }

            /// <inheritdoc/>
            public Type Key
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get { return key; }
            }

            /// <inheritdoc/>
            public bool IsValid
            {
                get { return (hash != 0 && key != null && instance != null); }
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool GetInstance<T>(out Component<T>? result)
                where T : struct
            {
                if (instance is Component<T> component)
                {
                    result = component;
                    return true;
                }
                else
                {
                    result = null;
                    return false;
                }
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void Clear()
            {
                instance.Clear();
            }
        }
    }
}