// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System;
using System.Runtime.CompilerServices;

namespace Soe.Composable
{
    #if EXPORT_HAMPER_CORE_COMPOSITION
    public
    #else
    internal
    #endif
    readonly partial struct EntityId
    {
        /// <summary>
        /// Attempts to add a component of type <typeparamref name="T"/> to this object
        /// </summary>
        /// <returns>A reference to the added or existing component data</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref T AddComponent<T>()
            where T : struct
        {
            if (Shard.TryGetShard(shardId, out Shard? shard) && (shard?.TryGetComponent(out Component<T>? component) ?? false))
            {
                return ref component!.Add(this);
            }
            else throw new InvalidOperationException();
        }
        
        /// <summary>
        /// Tries to receive a reference to this object's component data
        /// </summary>
        /// <param name="result">A reference to this object's component data</param>
        /// <returns>True if the component pool contains data for this object, false otherwise</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGetComponent<T>(out Ref<T> result)
            where T : struct
        {
            if (Shard.TryGetShard(shardId, out Shard? shard) && (shard?.TryGetComponent(out Component<T>? component) ?? false))
            {
                return component!.TryGet(this, out result);
            }
            else throw new InvalidOperationException();
        }

        /// <summary>
        /// Tries to set certain flag bits for this object's component data
        /// </summary>
        /// <param name="componentFlags">Any combination of bits received from <see cref="EntityFlags"/></param>
        /// <returns>True if the component flags for this object were successfully set, false otherwise</returns>
        public bool TrySetComponentFlags<T>(EntityFlags componentFlags)
            where T : struct
        {
            if (Shard.TryGetShard(shardId, out Shard? shard) && (shard?.TryGetComponent(out Component<T>? component) ?? false))
            {
                return component!.TrySetFlags(this, componentFlags);
            }
            else throw new InvalidOperationException();
        }
        
        /// <summary>
        /// Attempts to remove a component of type <typeparamref name="T"/> and it's component data from this object
        /// </summary>
        /// <returns>True if the component data was successfully removed, false otherwise</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool RemoveComponent<T>()
            where T : struct
        {
            if (Shard.TryGetShard(shardId, out Shard? shard) && (shard?.TryGetComponent(out Component<T>? component) ?? false))
            {
                return component!.Remove(this);
            }
            else throw new InvalidOperationException();
        }
    }
}