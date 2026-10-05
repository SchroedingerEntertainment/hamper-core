// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

namespace Soe.Composable
{
    /// <summary>
    /// Manages chunks of memory
    /// </summary>
    #if EXPORT_HAMPER_CORE_COMPOSABLE
    public
    #else
    internal
    #endif
    interface IMemoryAllocator : IDisposable
    {
        /// <summary>
        /// Accesses the memory block at the given index
        /// </summary>
        /// <param name="handle">A handle pointing to a block in memory</param>
        /// <param name="index">The index of the memory region relative to <paramref name="handle"/></param>
        /// <param name="entity">The entity to write into memory</param>
        /// <exception cref="InsufficientMemoryException">The memory page was discarded or otherwise freed</exception>
        /// <exception cref="IndexOutOfRangeException">The handle points to a location not in bounds of the memory unit</exception>
        public void Access(in MemoryHandle handle, int index, in EntityId entity);

        /// <summary>
        /// Accesses the memory block at the given index
        /// </summary>
        /// <param name="handle">A handle pointing to a block in memory</param>
        /// <param name="index">The index of the memory region relative to <paramref name="handle"/></param>
        /// <returns>The entity stored at the given index</returns>
        /// <exception cref="InsufficientMemoryException">The memory was discarded or otherwise freed</exception>
        /// <exception cref="IndexOutOfRangeException">The handle points to a location not in bounds of the memory unit</exception>
        public EntityId Access(in MemoryHandle handle, int index);

        /// <summary>
        /// Acquires a new block of the provided size
        /// </summary>
        /// <param name="size">The minimum size of the block</param>
        /// <returns>A handle pointing to the block acquired</returns>
        /// <exception cref="OutOfMemoryException">The memory manager ran out of blocks</exception>
        public MemoryHandle Allocate(int size);

        /// <summary>
        /// Returns the handle to a block in a memory page
        /// </summary>
        /// <param name="handle">A handle pointing to a block in memory</param>
        /// <exception cref="IndexOutOfRangeException">The handle points to a location not in bounds of the memory unit</exception>
        public void Free(in MemoryHandle handle);

        /// <summary>
        /// Initializes the memory block with default values
        /// </summary>
        /// <param name="handle">A handle pointing to a block in memory</param>
        /// <param name="index">The index of the memory region relative to <paramref name="handle"/></param>
        /// <param name="entity">The default values to write into memory</param>
        /// <exception cref="InsufficientMemoryException">The memory was discarded or otherwise freed</exception>
        /// <exception cref="IndexOutOfRangeException">The handle points to a location not in bounds of the memory unit</exception>
        public void InitializeBlock(in MemoryHandle handle, int index, in EntityId entity);
    }
}