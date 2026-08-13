// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;

namespace Soe.Composable
{
    /// <summary>
    /// Stores a set of linear <see cref="EntityId"/> via Robin Hood hash algorithm in order to achieve a more compact memory layout
    /// </summary>
    /// <remarks>Robin Hood hashing is an open addressing scheme that reduces variance in probe lengths by moving elements with
    /// shorter probe distances away to make room for elements that are farther from their ideal hash position</remarks>
    #if EXPORT_HAMPER_CORE_COMPOSABLE
    public
    #else
    internal
    #endif
    abstract partial class SparseMap
    {
        /// <summary>
        /// The maximum amount of elements stored before the container resizes in order to ensure
        /// an optimal hash calculation
        /// </summary>
        public const float LoadFactor = 0.86f;
        private int moduloMask;
        
        protected SparseElement[]? data;
        /// <summary>
        /// Gets the maximum number of elements that can be stored
        /// </summary>
        public int Capacity
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return data?.Length ?? 0; }
        }
        
        protected int count;
        /// <summary>
        /// Gets the current number of elements stored
        /// </summary>
        public int Count
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return count; }
        }
        
        private int version;
        /// <summary>
        /// Gets the current container version
        /// </summary>
        /// <remarks>Version is increased whenever the container layout changes</remarks>
        public int Version
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return version; }
        }
        
        /// <summary>
        /// Initializes this container to the provided capacity
        /// </summary>
        /// <param name="capacity">The target capacity for this container to become</param>
        /// <remarks>The container resizes to powers of two only</remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected SparseMap(int capacity)
        {
            Reserve(capacity);
        }
        /// <summary>
        /// Initializes an empty instance of this container
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected SparseMap()
        { }
        
        /// <summary>
        /// Gets the element of the current slot or creates one
        /// </summary>
        /// <param name="slot">The slot index of the element</param>
        /// <param name="index">The current search index of the element</param>
        /// <param name="distance">The current distance of the element</param>
        /// <param name="currentVersion">The container version before this operation</param>
        /// <returns>A reference to the memory handle the entity is stored in or empty</returns>
        protected ref MemoryHandle Emplace(int slot, int index, int distance, int currentVersion)
        {
        Head:
            if (version == currentVersion || !Find(slot, out index, out distance, out Ref<MemoryHandle> handle))
            {
                if (data == null || count >= data.Length * LoadFactor)
                {
                    Grow();
                    goto Head;
                }

                ref SparseElement element = ref data![Emplace(index, distance)];
                element = new SparseElement(slot);

                version++;
                return ref element.Handle;
            }
            else return ref handle.Value;
        }
        int Emplace(int index, int distance)
        {
            if (data![index].IsEmpty)
            {
                // Insert as slot is already empty
                
                count++;
                return index;
            }
            else
            {
                SparseElement element = default;
                {
                    int i = distance;
                    int tmp = GetDistance(index);
                    int result; 
                    
                    // Check if distance is smaller than the current distance
                    if(distance > tmp)
                    {
                        // Swap elements so the new element is closer to index
                        Swap(ref data[index], ref element);
                        result = index;
                        distance = tmp;
                    }
                    else result = -1;
                    
                    index = (index + 1) & moduloMask;
                    distance++;

                    // Move the current element to the next free slot
                    for (; i < data.Length; i++, index = (index + 1) & moduloMask)
                    {
                        // Free slot found to put the element into
                        if (data[index].IsEmpty)
                        {
                            count++;
                            if (!element.IsEmpty)
                            {
                                Swap(ref data[index], ref element);
                                
                                #if DEBUG
                                if(result < 0)
                                {
                                    throw new IndexOutOfRangeException();
                                }
                                else
                                #endif
                                return result;
                            }
                            else return index;
                        }
                        else
                        {
                            // Check if distance is smaller than the current distance and swap
                            tmp = GetDistance(index);
                            if (distance > tmp)
                            {
                                if (element.IsEmpty)
                                {
                                    result = index;
                                }
                                Swap(ref data[index], ref element);
                                distance = tmp;
                            }
                            distance++;
                        }
                    }
                }
                throw new OverflowException();
            }
        }

        /// <summary>
        /// Tries to get the element at the given slot
        /// </summary>
        /// <param name="slot">The slot index of the element</param>
        /// <param name="index">The current search index of the element</param>
        /// <param name="distance">The current distance of the element</param>
        /// <param name="memoryHandle">A reference to the memory handle the entity is stored in or empty</param>
        /// <returns>True if the element was found in this collection, false otherwise</returns>
        protected bool Find(int slot, out int index, out int distance, out Ref<MemoryHandle> memoryHandle)
        {
            index = (slot & moduloMask);
            if (count > 0)
            {
                distance = 0;

                for (int length = data?.Length ?? 0; distance < length; distance++, index = (index + 1) & moduloMask)
                {
                    if (data![index].IsValid)
                    {
                        if (slot == data[index].Slot)
                        {
                            memoryHandle = new Ref<MemoryHandle>(ref data[index].Handle);
                            return true;
                        }
                        else if (distance > GetDistance(index))
                            break;
                    }
                    else break;
                }
            }
            else distance = 0;
            
            memoryHandle = Ref<MemoryHandle>.CreateEmpty(); 
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        int GetDistance(int index)
        {
            int tmp = (data![index].Slot & moduloMask);
            if (tmp > index)
            {
                return (index + (data.Length - tmp));
            }
            else return (index - tmp);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void Grow()
        {
            Reserve(data == null ? 4 : data.Length + 1);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void Swap(ref SparseElement lhs, ref SparseElement rhs)
        {
            (lhs, rhs) = (rhs, lhs);
        }

        /// <summary>
        /// Resizes the container to the provided capacity
        /// </summary>
        /// <param name="capacity">The target capacity for this container to become</param>
        /// <remarks>The container resizes to powers of two only</remarks>
        public void Reserve(int capacity)
        {
            capacity = capacity.NextPowerOfTwo();
            moduloMask = (capacity - 1);
            count = 0;

            SparseElement[]? tmp = data;
            data = new SparseElement[capacity];
            if (tmp != null)
            {
                // Reinsert existing elements
                for (int i = 0; i < tmp.Length; i++)
                {
                    if (tmp[i].IsValid)
                    {
                        int index = Emplace(tmp[i].Slot & moduloMask, 0);
                        data[index] = tmp[i];
                    }
                }
            }
            version++;
        }
    }
}