// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Collections;
using System.Runtime.CompilerServices;

namespace Soe.Collections.HashSet
{
    /// <summary>
    /// Stores a set of elements via Robin Hood hash algorithm
    /// </summary>
    /// <remarks>Robin Hood hashing is an open addressing scheme that reduces variance in probe lengths by moving elements with
    /// shorter probe distances away to make room for elements that are farther from their ideal hash position</remarks>
    #if EXPORT_HAMPER_CORE_COLLECTIONS_HASHSET
    public
    #else
    internal
    #endif
    static partial class HashSet
    {
        /// <summary>
        /// The default amount of elements stored before the container resizes in order to ensure
        /// an optimal hash calculation
        /// </summary>
        public const float DefaultLoadFactor = 0.86f;
    }
    
    /// <summary>
    /// Stores a set of <typeparamref name="T"/> via Robin Hood hash algorithm
    /// </summary>
    /// <param name="comparer">An <seealso cref="IEqualityComparer{T}"/> to handle element comparison</param>
    /// <remarks>Robin Hood hashing is an open addressing scheme that reduces variance in probe lengths by moving elements with
    /// shorter probe distances away to make room for elements that are farther from their ideal hash position</remarks>
    [method: MethodImpl(MethodImplOptions.AggressiveInlining)]
    #if EXPORT_HAMPER_CORE_COLLECTIONS_HASHSET
    public
    #else
    internal
    #endif
    partial struct HashSet<T, Container>(IEqualityComparer<T> comparer, float loadFactor = HashSet.DefaultLoadFactor) : ISequence<Container>
        where Container : struct, IHashContainer<T>
    {
        private int moduloMask = 0;
        
        private Container[]? items;
        /// <summary>
        /// Gets the items managed by this container
        /// </summary>
        public Container[]? Items
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return items; }
        }
        
        /// <summary>
        /// Gets the maximum number of elements that can be stored
        /// </summary>
        public int Capacity
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return items?.Length ?? 0; }
        }
        
        private int count;
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

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Span<Container> AsSpan()
        {
            return items.AsSpan();
        }
        
        /// <summary>
        /// Clears the contents of this container to its default value
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Clear()
        {
            if (Capacity > 0)
            {
                Array.Clear(items!);
            }
            count = 0;
        }
        
        /// <summary>
        /// Attempts to add an element to the container
        /// </summary>
        /// <param name="key">The element to add</param>
        /// <param name="hash">The hash code of the element to add</param>
        /// <param name="index">The start index to insert as (hash % Capacity)</param>
        /// <param name="distance">The search distance</param>
        /// <param name="currentVersion">The container Version as a reference parameter</param>
        /// <returns>A reference to the added or existing element</returns>
        public ref Container Emplace(in T key, int hash, int index, int distance, int currentVersion)
        {
        Head:
            if (version == currentVersion || !Find(in key, hash, out index, out distance, out Ref<Container> result))
            {
                if (items == null || count >= items.Length * loadFactor)
                {
                    Grow();
                    goto Head;
                }

                version++;
                return ref items![Emplace(index, distance)];
            }
            else return ref result.Value;
        }
        /// <summary>
        /// Attempts to add an element to the container
        /// </summary>
        /// <param name="key">The element to add</param>
        /// <param name="hash">The hash code of the element to add</param>
        /// <returns>A reference to the added or existing element</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref Container Emplace(in T key, int hash)
        {
            return ref Emplace(in key, hash, 0, 0, version + 1);
        }
        int Emplace(int index, int distance)
        {
            if (!items![index].IsValid)
            {
                // Insert as slot is already empty
                
                count++;
                return index;
            }
            else
            {
                Container element = default;
                {
                    int i = distance;
                    int tmp = GetDistance(index);
                    int result; 
                    
                    // Check if distance is smaller than the current distance
                    if(distance > tmp)
                    {
                        // Swap elements so the new element is closer to index
                        Swap(ref items[index], ref element);
                        result = index;
                        distance = tmp;
                    }
                    else result = -1;
                    
                    index = (index + 1) & moduloMask;
                    distance++;

                    // Move the current element to the next free slot
                    for (; i < items.Length; i++, index = (index + 1) & moduloMask)
                    {
                        // Free slot found to put the element into
                        if (!items[index].IsValid)
                        {
                            count++;
                            if (element.IsValid)
                            {
                                Swap(ref items[index], ref element);
                                
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
                                if (!element.IsValid)
                                {
                                    result = index;
                                }
                                Swap(ref items[index], ref element);
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
        /// Determines whether the container contains a specific element
        /// </summary>
        /// <param name="key">The element to find</param>
        /// <param name="hash">The hash code of the element to find</param>
        /// <param name="index">The last index tested</param>
        /// <param name="distance">The current search distance</param>
        /// <param name="result">If successful, a reference to the element in this container</param>
        /// <returns>True if this container contains an element with the specified value, false otherwise</returns>
        public bool Find(in T key, int hash, out int index, out int distance, out Ref<Container> result)
        {
            index = (hash & moduloMask);
            if (count > 0)
            {
                distance = 0;

                for (int length = items?.Length ?? 0; distance < length; distance++, index = (index + 1) & moduloMask)
                {
                    if (items![index].IsValid)
                    {
                        if (hash == items[index].Hash && comparer.Equals(key, items[index].Key))
                        {
                            result = new Ref<Container>(ref items[index]);
                            return true;
                        }
                        else if (distance > GetDistance(index))
                            break;
                    }
                    else break;
                }
            }
            else distance = 0;
            
            result = Ref<Container>.CreateEmpty(); 
            return false;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        int GetDistance(int index)
        {
            int tmp = (items![index].Hash & moduloMask);
            if (tmp > index)
            {
                return (index + (items.Length - tmp));
            }
            else return (index - tmp);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void Grow()
        {
            Reserve(items == null ? 4 : items.Length + 1);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void Swap(ref Container lhs, ref Container rhs)
        {
            (lhs, rhs) = (rhs, lhs);
        }

        /// <summary>
        /// Removes an element by its key index
        /// </summary>
        /// <param name="key">The element to remove</param>
        /// <param name="hash">The hash code of the element to remove</param>
        /// <param name="index">The last index of the element to remove</param>
        /// <returns>True if the element was successfully removed, false otherwise</returns>
        public bool Remove(in T key, int hash, int index)
        {
            for (int i = 0, next = ((index + 1) & moduloMask); i < items!.Length; i++, index = ((index + 1) & moduloMask), next = ((next + 1) & moduloMask))
            {
                if (!items[next].IsValid || GetDistance(next) == 0)
                {
                    items[index] = default;
                    count--;

                    return true;
                }
                Swap(ref items[index], ref items[next]);
            }
            return false;
        }
        /// <summary>
        /// Removes an element by its key index
        /// </summary>
        /// <param name="key">The element to remove</param>
        /// <param name="hash">The hash code of the element to remove</param>
        /// <returns>True if the element was successfully removed, false otherwise</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Remove(in T key, int hash)
        {
            if (Find(in key, hash, out int index, out _, out _))
            {
                return Remove(in key, hash, index);
            }
            else return false;
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

            Container[]? tmp = items;
            items = new Container[capacity];
            if (tmp != null)
            {
                // Reinsert existing elements
                for (int i = 0; i < tmp.Length; i++)
                {
                    if (tmp[i].IsValid)
                    {
                        int index = Emplace(tmp[i].Hash & moduloMask, 0);
                        items[index] = tmp[i];
                    }
                }
            }
            version++;
        }
    }
}