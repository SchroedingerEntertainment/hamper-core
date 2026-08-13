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
        [InlineArray(16)]
        struct InstanceArray
        {
            private DependencyTreeNode element0;
        }

        [StructLayout(LayoutKind.Explicit)]
        [method: MethodImpl(MethodImplOptions.AggressiveInlining)]
        struct DependencyTreeNode(UInt32 key, byte parent, byte flags = DependencyTreeNode.RedBlackBit)
        {
            private const byte RedBlackBit = 0x80;
            public const byte Empty = 0xFF;
            
            public delegate ManagerDelegate? ManagerDelegate(object? instance, TaskNode node);
            
            [FieldOffset(0)] 
            public readonly UInt32 Key = key;
            
            [FieldOffset(4)]
            public byte Parent = parent;
            [FieldOffset(5)]
            public byte Left = Empty;
            [FieldOffset(6)]
            public byte Right = Empty;
            [FieldOffset(7)]
            public byte Flags = flags;

            [FieldOffset(8)]
            public object Instance;

            [FieldOffset(16)]
            public ManagerDelegate Delegate;
            
            public bool IsBlack
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get { return (Flags & RedBlackBit) == 0; }
            }
            public bool IsRed
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get { return (Flags & RedBlackBit) == 1; }
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void MakeBlack()
            {
                Flags &= 0x7F;
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void MakeRed()
            {
                Flags |= RedBlackBit;
            }
        }

        struct DependencyTree
        {
            private int root;
            
            public int Root
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get { return root; }
            }
            
            private int count;

            public int Count
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get { return count; }
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            bool IsRed(ref InstanceArray array, byte index)
            {
                return (index != DependencyTreeNode.Empty && array[index].IsRed);
            }
            
            public ref DependencyTreeNode Emplace(ref InstanceArray array, UInt32 key)
            {
                if (count == 0)
                {
                    array[count++] = new DependencyTreeNode(key, DependencyTreeNode.Empty,0);
                    return ref array[0];
                }
                else if (!Find(ref array, key, root, out int parent, out Ref<DependencyTreeNode> result))
                {
                    if (count >= 16)
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
                    else
                    {
                        array[parent].Right = (byte)index;
                    }
                    
                    InsertFixup(ref array, (byte)parent, (byte)index);
                    return ref node;
                }
                else return ref result.Value;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool Find(ref InstanceArray array, UInt32 key, int root, out int parent, out Ref<DependencyTreeNode> result)
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
            
            void InsertFixup(ref InstanceArray array, byte parent, byte index)
            {
                while (index != root && array[parent].IsRed)
                {
                    byte grandParent = array[parent].Parent;
                    if (parent == array[grandParent].Left)
                    {
                        byte uncle = array[grandParent].Right;
                        if (IsRed(ref array, uncle))
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
                                RotateLeft(ref array, index);

                                parent = array[index].Parent;
                                grandParent = array[parent].Parent;
                            }

                            array[parent].MakeBlack();
                            array[grandParent].MakeRed();
                            RotateRight(ref array, grandParent);
                        }
                    }
                    else
                    {
                        byte uncle = array[grandParent].Left;
                        if (IsRed(ref array, uncle))
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
                                RotateRight(ref array, index);

                                parent = array[index].Parent;
                                grandParent = array[parent].Parent;
                            }

                            array[parent].MakeBlack();
                            array[grandParent].MakeRed();
                            RotateLeft(ref array, grandParent);
                        }
                    }
                }
                array[root].MakeBlack();
            }

            void RotateLeft(ref InstanceArray array, byte index)
            {
                byte y = array[index].Right;

                array[index].Right = array[y].Left;

                if (array[y].Left != DependencyTreeNode.Empty)
                    array[array[y].Left].Parent = index;

                array[y].Parent = array[index].Parent;

                if (array[index].Parent == DependencyTreeNode.Empty)
                {
                    root = y;
                }
                else if (index == array[array[index].Parent].Left)
                {
                    array[array[index].Parent].Left = y;
                }
                else
                {
                    array[array[index].Parent].Right = y;
                }

                array[y].Left = index;
                array[index].Parent = y;
            }

            void RotateRight(ref InstanceArray array, byte index)
            {
                byte y = array[index].Left;

                array[index].Left = array[y].Right;

                if (array[y].Right != DependencyTreeNode.Empty)
                    array[array[y].Right].Parent = index;

                array[y].Parent = array[index].Parent;

                if (array[index].Parent == DependencyTreeNode.Empty)
                {
                    root = y;
                }
                else if (index == array[array[index].Parent].Right)
                {
                    array[array[index].Parent].Right = y;
                }
                else
                {
                    array[array[index].Parent].Left = y;
                }

                array[y].Right = index;
                array[index].Parent = y;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public int First(ref InstanceArray array)
            {
                return First(ref array, root);
            }
            
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static int First(ref InstanceArray array, int root)
            {
                byte current = (byte)root;

                if (current == DependencyTreeNode.Empty)
                    return DependencyTreeNode.Empty;

                while (array[current].Left != DependencyTreeNode.Empty)
                    current = array[current].Left;

                return current;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static int Next(ref InstanceArray array, int index)
            {
                if (index == DependencyTreeNode.Empty)
                    return DependencyTreeNode.Empty;

                byte right = array[index].Right;

                if (right != DependencyTreeNode.Empty)
                {
                    byte current = right;

                    while (array[current].Left != DependencyTreeNode.Empty)
                        current = array[current].Left;

                    return current;
                }

                byte currentIndex = (byte)index;
                byte parent = array[currentIndex].Parent;

                while (parent != DependencyTreeNode.Empty &&
                       currentIndex == array[parent].Right)
                {
                    currentIndex = parent;
                    parent = array[currentIndex].Parent;
                }

                return parent;
            }
        }
    }
}