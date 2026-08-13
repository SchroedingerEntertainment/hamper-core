// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Soe.Composable
{
    [StructLayout(LayoutKind.Explicit)]
    #if EXPORT_HAMPER_CORE_COMPOSITION
    public
    #else
    internal
    #endif
    readonly partial struct EntityId : IEquatable<EntityId>
    {
        [FieldOffset(0)]
        readonly UInt64 value;

        [FieldOffset(0)]
        readonly int index;

        public int Index
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return index; }
        }

        [FieldOffset(4)]
        readonly UInt16 version;

        public int Version
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return version; }
        }

        [FieldOffset(6)]
        readonly byte shardId;

        public int ShardId
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return shardId; }
        }

        [FieldOffset(7)]
        readonly EntityFlags flags;

        public EntityFlags Flags
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return flags; }
        }
        
		public EntityId(int index, int version, int shard, EntityFlags flags)
        {
            this.index = index;
            this.version = (UInt16)version;
            this.shardId = (byte)shard;
            this.flags = flags;
        }
        
		public EntityId(UInt64 value)
        {
            this.value = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator EntityId(UInt64 value)
        {
            return new EntityId(value);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator UInt64(EntityId entity)
        {
            return entity.value;
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(EntityId other)
        {
            return value == other.value;
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object? obj)
        {
            return obj is EntityId other && Equals(other);
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode()
        {
            return value.GetHashCode();
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString()
        {
            return $"{{Index: {index}, Version: {version}, Shard: {shardId}, Flags: {flags}}}";
        }
    }
}
