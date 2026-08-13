// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;
using Soe.Collections.Inline;
using Soe.Threading;

namespace Soe.Composable
{
    #if EXPORT_HAMPER_CORE_COMPOSITION
    public
    #else
    internal
    #endif
    partial class Shard
    {
        private static SmallArray<Shard?, SmallArray8<Shard?>> shards;
        private static UInt32 lockVariable;
        private static int firstFreeIndex;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static Shard()
        {
            shards = default;
            lockVariable = 0;
            firstFreeIndex = 0;
        }

        static int GetNextId(Shard shard)
        {
        Insert:
            using (ScopedDisposable.Acquire<UInt32, SynchronizationBarrier.SharedOperation>(ref lockVariable))
            {
                int startIndex = Volatile.Read(ref firstFreeIndex);
                for (int i = startIndex, length = Math.Min(shards.Length, byte.MaxValue); i < length; i++)
                {
                    if (Interlocked.CompareExchange(ref shards[i], shard, null) == null)
                    {
                        Interlocked.CompareExchange(ref firstFreeIndex, i + 1, startIndex);
                        return i;
                    }
                }
            }
            using (ScopedDisposable.Acquire<UInt32, SynchronizationBarrier.SharedOperation>(ref lockVariable))
            {
                if (shards.Length < byte.MaxValue)
                {
                    shards.Resize(shards.Length * 2);
                    goto Insert;
                }
                else throw new OverflowException();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryGetShard(int id, out Shard? shard)
        {
            using (ScopedDisposable.Acquire<UInt32, SynchronizationBarrier.SharedOperation>(ref lockVariable))
            {
                if(id < shards.Length && shards[id] != null)
                {
                    shard = shards[id];
                    return true;
                }
                else
                {
                    shard = null;
                    return false;
                }
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void ReturnId(int id)
        {
            using (ScopedDisposable.Acquire<UInt32, SynchronizationBarrier.SharedOperation>(ref lockVariable))
            {
                int startIndex = Volatile.Read(ref firstFreeIndex);
                
                Interlocked.Exchange(ref shards[id], null);
                Interlocked.CompareExchange(ref firstFreeIndex, id, startIndex);
            }
        }
    }
}