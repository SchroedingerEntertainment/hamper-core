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
        public const int PageSize = 4096;
        public const int BlockSize = PageSize >> 6;
        public const int BlockShift = 3;
        public const int BlockMask = (BlockSize >> BlockShift) - 1;
    }
}