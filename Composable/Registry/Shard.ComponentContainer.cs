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
        /// <summary>
        /// Stores the populated component pool of a pure data structure defining specific properties of an entity
        /// </summary>
        /// <param name="instance">The component pool instance to store</param>
        /// <param name="hash">The hash code of the key</param>
        /// <param name="key">The runtime <see cref="Type"/> of the pure data structure</param>
        [method: MethodImpl(MethodImplOptions.AggressiveInlining)]
        readonly struct ComponentContainer(IComponent instance, int hash, Type key) : IHashContainer<Type>
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

            /// <summary>
            /// Tries to get the component container instance for <typeparamref name="T"/>
            /// </summary>
            /// <param name="result">The component container instances belonging to <typeparamref name="T"/></param>
            /// <typeparam name="T">The pure data structure defining specific properties</typeparam>
            /// <returns>True if the container stores the component pool for <typeparamref name="T"/>, false otherwise</returns>
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

            /// <summary>
            /// Removes the component data stored in this container
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void Clear()
            {
                instance.Clear();
            }
        }
    }
}