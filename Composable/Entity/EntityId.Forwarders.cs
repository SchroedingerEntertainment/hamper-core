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