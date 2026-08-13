// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Soe.Composable
{
    /// <summary>
    /// Represents the block in a memory page
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    #if EXPORT_HAMPER_CORE_COMPOSABLE
    public
    #else
    internal
    #endif
    readonly partial struct MemoryHandle : IEquatable<MemoryHandle>
    {
        public static readonly MemoryHandle Reserved = new MemoryHandle(UInt16.MaxValue, 0, 0);
        
        [FieldOffset(0)]
        readonly UInt64 value;

        [FieldOffset(0)]
        readonly UInt16 blockSize;
        /// <summary>
        /// Gets the size of the allocated block
        /// </summary>
        public int BlockSize
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return blockSize; }
        }
        
        [FieldOffset(2)]
        readonly UInt32 pageIndex;
        /// <summary>
        /// Gets the memory page of the allocated block
        /// </summary>
        public int PageIndex
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return (int)pageIndex; }
        }

        [FieldOffset(6)]
        readonly UInt16 blockIndex;
        /// <summary>
        /// Gets the index of the block in the corresponding memory page
        /// </summary>
        public int BlockIndex
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return blockIndex; }
        }

        /// <summary>
        /// Determines if this blocks dimensions are valid
        /// </summary>
        public bool IsValid
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                // ReSharper disable MergeIntoPattern
                
                return (blockSize >= MemoryAllocator.BlockSize && blockSize <= MemoryAllocator.PageSize);
                
                // ReSharper restore MergeIntoPattern
            }
        }
        
        /// <summary>
        /// Initializes the memory handle from its components
        /// </summary>
        /// <param name="blockSize">The block size of this handle</param>
        /// <param name="pageIndex">The index of the memory page the block exists in</param>
        /// <param name="blockIndex">The index of the block allocated</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		public MemoryHandle(UInt16 blockSize, UInt32 pageIndex, UInt16 blockIndex)
        {
            this.blockSize = blockSize;
            this.pageIndex = pageIndex;
            this.blockIndex = blockIndex;
        }
        /// <summary>
        /// Initializes the memory handle from value
        /// </summary>
        /// <param name="value">A 64 bit value to represent a memory handle</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		public MemoryHandle(UInt64 value)
        {
            this.value = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator MemoryHandle(UInt64 value)
        {
            return new MemoryHandle(value);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator UInt64(MemoryHandle handle)
        {
            return handle.value;
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(MemoryHandle other)
        {
            return value == other.value;
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object? obj)
        {
            return obj is MemoryHandle other && Equals(other);
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode()
        {
            return value.GetHashCode();
        }
    }
}
