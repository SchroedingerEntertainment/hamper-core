// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Soe.Composable
{
    /// <summary>
    /// Represents the identity of a composable object that can carry various <see cref="Component{T}"/>
    /// </summary>
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
        /// <summary>
        /// Gets the index of this object in the registry
        /// </summary>
        public int Index
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return index; }
        }

        [FieldOffset(4)]
        readonly UInt16 version;
        /// <summary>
        /// Gets the current version of this object
        /// </summary>
        public int Version
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return version; }
        }

        [FieldOffset(6)]
        readonly byte shardId;
        /// <summary>
        /// Gets the <see cref="Shard"/> this object belongs to
        /// </summary>
        public int ShardId
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return shardId; }
        }

        [FieldOffset(7)]
        readonly EntityFlags flags;
        /// <summary>
        /// Gets a combination of flags specifying various constraints to this object
        /// </summary>
        public EntityFlags Flags
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return flags; }
        }
        
        /// <summary>
        /// Initializes this object with the provided identity
        /// </summary>
        /// <param name="index">The index of this object</param>
        /// <param name="version">The version of this object</param>
        /// <param name="shard">The <see cref="Shard"/> this object belongs to</param>
        /// <param name="flags">A combination of flags specifying various constraints to this object</param>
		public EntityId(int index, int version, int shard, EntityFlags flags)
        {
            this.index = index;
            this.version = (UInt16)version;
            this.shardId = (byte)shard;
            this.flags = flags;
        }
        /// <summary>
        /// Initializes this object with the provided identity
        /// </summary>
        /// <param name="value">This objects identity</param>
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
