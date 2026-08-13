// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System;
using System.Buffers;
using System.Diagnostics;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Numerics;
using System.Runtime.CompilerServices;
using Soe.Threading;

namespace Soe.Composable
{
    /// <summary>
    /// Manages memory pages used to store entities
    /// </summary>
    #if EXPORT_HAMPER_CORE_COMPOSABLE
    public
    #else
    internal
    #endif
    partial class PageAllocator : FinalizerObject, IMemoryAllocator
    {
        public const int MaxPageCount = UInt16.MaxValue;
        
        private readonly MemoryMappedFile pages;
        private UInt32 lockVariable;
        private int firstFreeIndex;
        private ChunkList chunks;
        
        /// <summary>
        /// Initializes the allocator
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PageAllocator()
        {
            pages = MemoryMappedFile.CreateNew(null, MemoryAllocator.PageSize * MaxPageCount, MemoryMappedFileAccess.ReadWrite, MemoryMappedFileOptions.DelayAllocatePages, HandleInheritability.None);
            firstFreeIndex = 0;
            chunks = default;
        }
        
        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Access(in MemoryHandle handle, int index, in EntityId entity)
        {
            index *= sizeof(UInt64);
            using(ScopedDisposable.Acquire<UInt32, SynchronizationBarrier.SharedOperation>(ref lockVariable))
            {
                Span<Chunk> list = chunks.AsSpan();
                if (handle.PageIndex < list.Length && handle.BlockIndex + handle.BlockSize <= MemoryAllocator.PageSize)
                {
                    if (list[handle.PageIndex].Handler is MemoryMappedViewAccessor accessor)
                    {
                        accessor.Write(handle.BlockIndex + index, entity);
                    }
                    else throw new InsufficientMemoryException();
                }
                else throw new IndexOutOfRangeException();
            }
        }
        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public EntityId Access(in MemoryHandle handle, int index)
        {
            index *= sizeof(UInt64);
            using(ScopedDisposable.Acquire<UInt32, SynchronizationBarrier.SharedOperation>(ref lockVariable))
            {
                Span<Chunk> list = chunks.AsSpan();
                if (handle.PageIndex < list.Length && handle.BlockIndex + handle.BlockSize <= MemoryAllocator.PageSize)
                {
                    if (list[handle.PageIndex].Handler is MemoryMappedViewAccessor accessor)
                    {
                        return accessor.ReadUInt64(handle.BlockIndex + index);
                    }
                    else throw new InsufficientMemoryException();
                }
                else throw new IndexOutOfRangeException();
            }
        }
        
        /// <inheritdoc/>
        /// <remarks>Blocks are always allocated as multiples of <see cref="MemoryAllocator.BlockSize"/></remarks>
        public MemoryHandle Allocate(int size)
        {
            // Map the size to power of two value
            if (size < MemoryAllocator.BlockSize)
            {
                size = MemoryAllocator.BlockSize;
            }
            size = size.NextPowerOfTwo();
            int blockCount = size >> 6;
            Span<Chunk> list;
            
        TryInsert:
            using(ScopedDisposable.Acquire<UInt32, SynchronizationBarrier.SharedOperation>(ref lockVariable))
            {
                list = chunks.AsSpan();
                for (int i = Volatile.Read(ref firstFreeIndex); i < list.Length; i++)
                {
                    ref Chunk chunk = ref list[i];
                    if (chunk.IsEmpty)
                    {
                        // Allocate a new page
                        object? result = Interlocked.CompareExchange(ref chunk.Handler, pages.CreateViewAccessor(i * MemoryAllocator.PageSize, MemoryAllocator.PageSize), null);
                        if (result is MemoryMappedViewAccessor accessor)
                        {
                            accessor.Dispose();
                        }
                    }
                Retry:
                    UInt64 freeList = Volatile.Read(ref chunk.FreeList);
                    
                    // Search for the next free block in this page
                    for (int bit = GetFirstFreeIndex(freeList); bit < 64; bit++)
                    {
                        int count = BitOperations.TrailingZeroCount(freeList >> bit) - bit;
                        if (count >= blockCount)
                        {
                            UInt64 newFreeList = freeList | ((1ul << blockCount) - 1) << bit;
                            if (Interlocked.CompareExchange(ref chunk.FreeList, newFreeList, freeList) != freeList)
                            {
                                goto Retry;
                            }

                            int freeIndex = Volatile.Read(ref firstFreeIndex);
                            if (freeIndex == i && newFreeList == UInt64.MaxValue)
                            {
                                // Increase the hint for the allocator where to look for free blocks
                                Interlocked.CompareExchange(ref firstFreeIndex, freeIndex + 1, freeIndex);
                            }
                            return new MemoryHandle((UInt16)size, (UInt32)i, (UInt16)(bit * MemoryAllocator.BlockSize));
                        }
                    }
                }
            }
            using (ScopedDisposable.Acquire<UInt32, SynchronizationBarrier.ExclusiveOperation>(ref lockVariable))
            {
                if (list.Length * 2 <= MaxPageCount)
                {
                    // There are more pages reserved, grow and retry
                    chunks.Resize(list.Length * 2);
                    goto TryInsert;
                }
                else throw new OutOfMemoryException();
            }
        }

        protected override bool Dispose(bool disposing)
        {
            if (base.Dispose(disposing))
            {
                using (ScopedDisposable.Acquire<UInt32, SynchronizationBarrier.ExclusiveOperation>(ref lockVariable))
                {
                    foreach(ref Chunk chunk in chunks.AsSpan())
                    {
                        if (chunk.Handler is MemoryMappedViewAccessor accessor)
                        {
                            accessor.Dispose();
                            chunk = default;
                        }
                    }
                    pages.Dispose();
                }
                return true;
            }
            else return false;
        }

        /// <inheritdoc/>
        public void Free(in MemoryHandle handle)
        {
            int index = handle.PageIndex;
            using(ScopedDisposable.Acquire<UInt32, SynchronizationBarrier.SharedOperation>(ref lockVariable))
            {
                Span<Chunk> list = chunks.AsSpan();
                if (index < list.Length && handle.BlockIndex + handle.BlockSize <= MemoryAllocator.PageSize)
                {
                    ref Chunk chunk = ref list[index];

                Retry:
                    UInt64 freeList = Volatile.Read(ref chunk.FreeList);
                    if (Interlocked.CompareExchange(ref chunk.FreeList, (freeList & ~(((1ul << (handle.BlockSize >> 6)) - 1) << handle.BlockIndex)), freeList) == freeList)
                    {
                        int freeIndex = Volatile.Read(ref firstFreeIndex);
                        if (index < freeIndex)
                        {
                            // Set the hint to where to look for free chunks to this page if possible
                            Interlocked.CompareExchange(ref firstFreeIndex, index, freeIndex);
                        }
                    }
                    else goto Retry;
                }
                else throw new IndexOutOfRangeException();
            }
            using (ScopedDisposable.Acquire<UInt32, SynchronizationBarrier.ExclusiveOperation>(ref lockVariable))
            {
                Span<Chunk> list = chunks.AsSpan();
                if (list[index].FreeList == 0 && list[index].Handler is  MemoryMappedViewAccessor accessor)
                {
                    accessor.Dispose();
                    list[index] = default;
                }
            }
        }
        
        /// <inheritdoc/>
        public void InitializeBlock(in MemoryHandle handle, int index, in EntityId entity)
        {
            int count = handle.BlockSize >> MemoryAllocator.BlockShift;
            EntityId[] ids = ArrayPool<EntityId>.Shared.Rent(count);
            try
            {
                ids.AsSpan().Slice(0, count)
                    .Fill(entity);

                using(ScopedDisposable.Acquire<UInt32, SynchronizationBarrier.SharedOperation>(ref lockVariable))
                {
                    Span<Chunk> list = chunks.AsSpan();
                    if (handle.PageIndex < list.Length && handle.BlockIndex + handle.BlockSize <= MemoryAllocator.PageSize)
                    {
                        if (list[handle.PageIndex].Handler is MemoryMappedViewAccessor accessor)
                        {
                            accessor.WriteArray(handle.BlockIndex + index, ids, 0, count);
                        }
                        else throw new InsufficientMemoryException();
                    }
                    else throw new IndexOutOfRangeException();
                }
            }
            finally
            {
                ArrayPool<EntityId>.Shared.Return(ids);
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static int GetFirstFreeIndex(UInt64 freeList)
        {
            freeList = ~freeList;
            if (freeList == 0)
            {
                return -1;
            }
            else return BitOperations.TrailingZeroCount(freeList);
        }
    }
}