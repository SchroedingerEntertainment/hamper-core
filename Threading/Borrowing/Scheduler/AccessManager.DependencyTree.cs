// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Soe.Threading
{
    #if EXPORT_HAMPER_CORE_THREADING
    public
    #else
    internal
    #endif
    static partial class AccessManager
    {
        /// <summary>
        /// An element container used in <see cref="DependencyTree"/>
        /// </summary>
        /// <param name="key">The unique key of this element</param>
        /// <param name="parent">A parent element if applicable</param>
        /// <param name="flags">A flag set for the Red-black probing</param>
        [StructLayout(LayoutKind.Explicit)]
        [method: MethodImpl(MethodImplOptions.AggressiveInlining)]
        struct DependencyTreeNode(UInt32 key, byte parent = DependencyTreeNode.Empty, byte flags = DependencyTreeNode.RedBlackBit)
        {
            private const byte RedBlackBit = 0x80;
            /// <summary>
            /// Declares an element reference to be not set and therefore empty
            /// </summary>
            public const byte Empty = 0xFF;
            
            /// <summary>
            /// A delegate object used to keep generic typed operators
            /// </summary>
            public delegate ManagerDelegate? ManagerDelegate(object? instance, TaskNode node);
            
            /// <summary>
            /// The unique key of this element
            /// </summary>
            [FieldOffset(0)] 
            public readonly UInt32 Key = key;
            
            /// <summary>
            /// The parent of this element if applicable
            /// </summary>
            [FieldOffset(4)]
            public byte Parent = parent;
            /// <summary>
            /// The left child of this element if applicable
            /// </summary>
            [FieldOffset(5)]
            public byte Left = Empty;
            /// <summary>
            /// The right child of this element if applicable
            /// </summary>
            [FieldOffset(6)]
            public byte Right = Empty;
            /// <summary>
            /// Flags set for this element
            /// </summary>
            /// <remarks>This field is a compound value of the reserved bit at 0x80 and any combination of
            /// bits received from an <see cref="AccessType"/></remarks>
            [FieldOffset(7)]
            public byte Flags = flags;

            /// <summary>
            /// The object instance of this element
            /// </summary>
            [FieldOffset(8)]
            public object Instance;

            /// <summary>
            /// The current generic typed operator for this object
            /// </summary>
            [FieldOffset(16)]
            public ManagerDelegate Delegate;
            
            /// <summary>
            /// Gets if this element has the black bit set
            /// </summary>
            public bool IsBlack
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get { return (Flags & RedBlackBit) == 0; }
            }
            /// <summary>
            /// Gets if this element has the red bit set
            /// </summary>
            public bool IsRed
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get { return (Flags & RedBlackBit) == RedBlackBit; }
            }

            /// <summary>
            /// Sets the black bit for this element
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void MakeBlack()
            {
                Flags &= 0x7F;
            }
            /// <summary>
            /// Sets the red bit for this element
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void MakeRed()
            {
                Flags |= RedBlackBit;
            }
        }

        /// <summary>
        /// Stores a sorted set of elements via red-black hash algorithm
        /// </summary>
        /// <remarks>A red-black tree is a self-balancing binary search tree where every node carries an extra bit of data for
        /// color—either red or black—to ensure operations stay efficient</remarks>
        struct DependencyTree
        {
            private const int MaxElementCount = 16;
            
            private int root;
            /// <summary>
            /// Gets the current root element of this tree
            /// </summary>
            public int Root
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get { return root; }
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
            
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            bool IsRed(DependencyTreeNode[] array, byte index)
            {
                return (index != DependencyTreeNode.Empty && array[index].IsRed);
            }
            
            /// <summary>
            /// Attempts to add an element to the container
            /// </summary>
            /// <param name="array">The array this hash set instance operates on</param>
            /// <param name="key">The unique key of the element to add</param>
            /// <returns>A reference to the added or existing element</returns>
            /// <exception cref="OverflowException">Thrown if the maximum number of elements is exceeded</exception>
            public ref DependencyTreeNode Emplace(DependencyTreeNode[] array, UInt32 key)
            {
                if (count == 0)
                {
                    array[count++] = new DependencyTreeNode(key, DependencyTreeNode.Empty,0);
                    return ref array[0];
                }
                else if (!Find(array, key, root, out int parent, out Ref<DependencyTreeNode> result))
                {
                    // Element not found, add new one if possible
                    
                    if (count >= MaxElementCount)
                    {
                        throw new OverflowException();
                    }
                    int index = count++;
                    
                    ref DependencyTreeNode node = ref array[index];
                    node = new DependencyTreeNode(key, (byte)parent);
                    
                    if (key < array[parent].Key)
                    {
                        array[parent].Left = (byte)index;
                    }
                    else array[parent].Right = (byte)index;
                    InsertFixup(array, (byte)parent, (byte)index);
                    
                    return ref node;
                }
                else return ref result.Value;
            }

            /// <summary>
            /// Determines whether the container contains a specific element
            /// </summary>
            /// <param name="array">The array this hash set instance operates on</param>
            /// <param name="key">The unique key of the element to find</param>
            /// <param name="root">The root element index to start finding from</param>
            /// <param name="parent">The parent element of the found element if applicable</param>
            /// <param name="result">If successful, a reference to the element in this container</param>
            /// <returns>True if this container contains an element with the specified key, false otherwise</returns>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool Find(DependencyTreeNode[] array, UInt32 key, int root, out int parent, out Ref<DependencyTreeNode> result)
            {
                parent = DependencyTreeNode.Empty;
                if (root != DependencyTreeNode.Empty)
                {
                    for (byte current = (byte)root; current != DependencyTreeNode.Empty; current = array[current].Key > key ? array[current].Left : array[current].Right)
                    {
                        if (array[current].Key == key)
                        {
                            result = new Ref<DependencyTreeNode>(ref array[current]);
                            return true;
                        }
                        else parent = current;
                    }
                }
                
                result = Ref<DependencyTreeNode>.CreateEmpty(); 
                return false;
            }
            
            void InsertFixup(DependencyTreeNode[] array, byte parent, byte index)
            {
                while (index != root && array[parent].IsRed)
                {
                    byte grandParent = array[parent].Parent;
                    if (parent == array[grandParent].Left)
                    {
                        byte uncle = array[grandParent].Right;
                        if (IsRed(array, uncle))
                        {
                            array[parent].MakeBlack();
                            array[uncle].MakeBlack();
                            array[grandParent].MakeRed();

                            index = grandParent;
                        }
                        else
                        {
                            if (index == array[parent].Right)
                            {
                                index = parent;
                                RotateLeft(array, index);

                                parent = array[index].Parent;
                                grandParent = array[parent].Parent;
                            }
                            array[parent].MakeBlack();
                            array[grandParent].MakeRed();
                            RotateRight(array, grandParent);
                        }
                    }
                    else
                    {
                        byte uncle = array[grandParent].Left;
                        if (IsRed(array, uncle))
                        {
                            array[parent].MakeBlack();
                            array[uncle].MakeBlack();
                            array[grandParent].MakeRed();

                            index = grandParent;
                        }
                        else
                        {
                            if (index == array[parent].Left)
                            {
                                index = parent;
                                RotateRight(array, index);

                                parent = array[index].Parent;
                                grandParent = array[parent].Parent;
                            }
                            array[parent].MakeBlack();
                            array[grandParent].MakeRed();
                            RotateLeft(array, grandParent);
                        }
                    }
                }
                array[root].MakeBlack();
            }

            void RotateLeft(DependencyTreeNode[] array, byte index)
            {
                byte i = array[index].Right;
                array[index].Right = array[i].Left;

                if (array[i].Left != DependencyTreeNode.Empty)
                {
                    array[array[i].Left].Parent = index;
                }
                array[i].Parent = array[index].Parent;

                if (array[index].Parent == DependencyTreeNode.Empty)
                {
                    root = i;
                }
                else if (index == array[array[index].Parent].Left)
                {
                    array[array[index].Parent].Left = i;
                }
                else array[array[index].Parent].Right = i;
                array[index].Parent = i;
                array[i].Left = index;
            }

            void RotateRight(DependencyTreeNode[] array, byte index)
            {
                byte i = array[index].Left;
                array[index].Left = array[i].Right;

                if (array[i].Right != DependencyTreeNode.Empty)
                {
                    array[array[i].Right].Parent = index;
                }
                array[i].Parent = array[index].Parent;

                if (array[index].Parent == DependencyTreeNode.Empty)
                {
                    root = i;
                }
                else if (index == array[array[index].Parent].Right)
                {
                    array[array[index].Parent].Right = i;
                }
                else array[array[index].Parent].Left = i;
                array[index].Parent = i;
                array[i].Right = index;
            }

            /// <summary>
            /// Finds the first element index in iteration order
            /// </summary>
            /// <param name="array">The array this hash set instance operates on</param>
            /// <returns>The element index found, Empty otherwise</returns>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public int Begin(DependencyTreeNode[] array)
            {
                return Begin(array, root);
            }
            /// <summary>
            /// Finds the first element index in iteration order
            /// </summary>
            /// <param name="array">The array this hash set instance operates on</param>
            /// <param name="root">The current root index of this hash set</param>
            /// <returns>The element index found, Empty otherwise</returns>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static int Begin(DependencyTreeNode[] array, int root)
            {
                byte current = (byte)root;
                if (current == DependencyTreeNode.Empty)
                {
                    return DependencyTreeNode.Empty;
                }
                while (array[current].Left != DependencyTreeNode.Empty)
                {
                    // Find most left element if applicable
                    
                    current = array[current].Left;
                }
                return current;
            }

            /// <summary>
            /// Iterates to the next element in iteration order
            /// </summary>
            /// <param name="array">The array this hash set instance operates on</param>
            /// <param name="index">The current element index</param>
            /// <returns>The element index found, Empty otherwise</returns>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static int Next(DependencyTreeNode[] array, int index)
            {
                if (index == DependencyTreeNode.Empty)
                {
                    return DependencyTreeNode.Empty;
                }
                byte right = array[index].Right;
                if (right != DependencyTreeNode.Empty)
                {
                    byte current = right;
                    while (array[current].Left != DependencyTreeNode.Empty)
                    {
                        current = array[current].Left;
                    }
                    return current;
                }
                byte currentIndex = (byte)index;
                byte parent = array[currentIndex].Parent;
                while (parent != DependencyTreeNode.Empty && currentIndex == array[parent].Right)
                {
                    currentIndex = parent;
                    parent = array[currentIndex].Parent;
                }
                return parent;
            }
        }

        /// <summary>
        /// Provides a safe representation of the current access request
        /// </summary>
        public readonly ref struct DependencyTreeResolver
        {
            private readonly DependencyTreeNode[] array;
            private readonly ref DependencyTree tree;
            
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            DependencyTreeResolver(DependencyTreeNode[] array, ref DependencyTree tree)
            {
                this.array = array;
                this.tree = ref tree;
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            static DependencyTreeResolver()
            {
                CreateResolver = CreateInstance;
            }
            
            /// <summary>
            /// Attempts to add an instance of type <typeparamref name="T"/> to the access request
            /// </summary>
            /// <param name="uniqueId">The unique object id for this instance</param>
            /// <param name="instance">An instance to get access</param>
            /// <typeparam name="T">An instance type to get access</typeparam>
            /// <typeparam name="Policy">The access policy for this instance type</typeparam>
            /// <returns>True if a new instance was added, false if the current instance was merged</returns>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool Add<T, Policy>(UInt32 uniqueId, object instance)
                where T : class
                where Policy : struct, IAccessPolicy
            {
                // ReSharper disable BitwiseOperatorOnEnumWithoutFlags
                
                ref DependencyTreeNode tn = ref tree.Emplace(array, uniqueId);
                tn.Flags |= (byte)(Policy.Order & ~AccessType.Reserved);
                tn.Delegate = Append<T, Policy>;
                
                bool result = (tn.Instance == null);
                tn.Instance = instance;
                
                return result;
                
                // ReSharper restore BitwiseOperatorOnEnumWithoutFlags
            }
            
            /// <summary>
            /// Attempts to add an instance of type <typeparamref name="T"/> to the access request
            /// </summary>
            /// <param name="uniqueId">The unique object id for this instance</param>
            /// <param name="instance">An instance to get access</param>
            /// <typeparam name="T">An instance type to get access</typeparam>
            /// <typeparam name="Policy">The access policy for this instance type</typeparam>
            /// <returns>True if a new instance was added, false otherwise</returns>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool AddConditional<T, Policy>(UInt32 uniqueId, object instance)
                where T : class
                where Policy : struct, IAccessPolicy
            {
                // ReSharper disable BitwiseOperatorOnEnumWithoutFlags
                
                ref DependencyTreeNode tn = ref tree.Emplace(array, uniqueId);
                if (tn.Instance == null)
                {
                    tn.Flags |= (byte)(Policy.Order & ~AccessType.Reserved);
                    tn.Delegate = Append<T, Policy>;
                    tn.Instance = instance;

                    return true;
                }
                else return false;

                // ReSharper restore BitwiseOperatorOnEnumWithoutFlags
            }

            /// <summary>
            /// Gets the desired access type for the given object instance
            /// </summary>
            /// <param name="uniqueId">The unique object id for the requested instance</param>
            /// <returns>The desired access type for the given object instance if present, Empty otherwise</returns>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public AccessType GetAccess(UInt32 uniqueId)
            {
                if (DependencyTree.Find(array!, uniqueId, tree.Root, out _, out Ref<DependencyTreeNode> result))
                {
                    // ReSharper disable BitwiseOperatorOnEnumWithoutFlags
                    
                    return (AccessType)result.Value.Flags & ~AccessType.Reserved;
                    
                    // ReSharper restore BitwiseOperatorOnEnumWithoutFlags
                }
                else return AccessType.Reserved;
            }
            
            /// <summary>
            /// Determines the unique object id for a given instance
            /// </summary>
            /// <param name="instance">An instance to get a unique id</param>
            /// <typeparam name="T">An instance type to get a unique id</typeparam>
            /// <returns>The unique object id for this instance</returns>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public UInt32 GetUniqueId<T>(T instance)
                where T : class
            {
                return Dependency<T>.GetUniqueId(instance);
            }
            
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            static DependencyTreeResolver CreateInstance(DependencyTreeNode[] instanceArray, ref DependencyTree dependencyTree)
            {
                return new DependencyTreeResolver(instanceArray, ref dependencyTree);
            }
        }
    }
}