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
    static class MemoryAllocator
    {
        /// <summary>
        /// The default page size in bytes assumed
        /// </summary>
        public const int PageSize = 4096;
        
        /// <summary>
        /// The size of a single block in a page in bytes
        /// </summary>
        public const int BlockSize = PageSize >> 6;
        
        /// <summary>
        /// A bitmask to determine the index of an entity within a block
        /// </summary>
        public const int BlockShift = 3;
        
        /// <summary>
        /// A bitmask to determine the block index of an entity within a page
        /// </summary>
        public const int BlockMask = (BlockSize >> BlockShift) - 1;
    }
}