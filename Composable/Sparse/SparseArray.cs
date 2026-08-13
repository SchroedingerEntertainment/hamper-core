// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;

namespace Soe.Composable
{
    /// <summary>
    /// Stores a set of linear <see cref="EntityId"/> in linear memory
    /// </summary>
    #if EXPORT_HAMPER_CORE_COMPOSABLE
    public
    #else
    internal
    #endif
    abstract partial class SparseArray
    {
        protected MemoryHandle[]? data;
        /// <summary>
        /// Gets the current number of elements in the array
        /// </summary>
        public int Length
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return data?.Length ?? 0; }
        }
        
        private int version;
        /// <summary>
        /// Gets the current array version
        /// </summary>
        /// <remarks>Version is increased whenever the array layout changes</remarks>
        public int Version
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return version; }
        }
        
        /// <summary>
        /// Initializes this array to the provided capacity
        /// </summary>
        /// <param name="capacity">The target capacity for this array to become</param>
        /// <remarks>The array resizes to powers of two only</remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected SparseArray(int capacity)
        {
            Reserve(capacity.NextPowerOfTwo());
        }
        /// <summary>
        /// Initializes an empty instance of this array
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected SparseArray()
        { }
        
        /// <summary>
        /// Gets the element of the current slot or creates one
        /// </summary>
        /// <param name="slot">The slot index of the element</param>
        /// <param name="currentVersion">The array version before this operation</param>
        /// <returns>A reference to the memory handle the entity is stored in or empty</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected ref MemoryHandle Emplace(int slot, int currentVersion)
        {
            if (version == currentVersion || !Find(slot, out _))
            {
                if (data == null || slot >= Length)
                {
                    Reserve((slot + 1).NextPowerOfTwo());
                }
                version++;
            }
            return ref data![slot];
        }

        /// <summary>
        /// Tries to get the element at the given slot
        /// </summary>
        /// <param name="slot">The slot index of the element</param>
        /// <param name="memoryHandle">A reference to the memory handle the entity is stored in or empty</param>
        /// <returns>True if the element was found in this collection, false otherwise</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected bool Find(int slot, out Ref<MemoryHandle> memoryHandle)
        {
            if (Length > slot)
            {
                memoryHandle = new Ref<MemoryHandle>(ref data![slot]);
                return true;
            }
            else
            {
                memoryHandle = Ref<MemoryHandle>.CreateEmpty();
                return false;
            }
        }

        /// <summary>
        /// Resizes the array to the provided capacity
        /// </summary>
        /// <param name="capacity">The target capacity for this array to become</param>
        /// <remarks>The array resizes to powers of two only</remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Reserve(int capacity)
        {
            Array.Resize(ref data, Math.Max(4, capacity));
            version++;
        }
    }
}