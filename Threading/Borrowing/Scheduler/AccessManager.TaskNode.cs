// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Buffers;
using System.Runtime.CompilerServices;
using Soe.Collections.Inline;

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
        /// Represents a single task in a designated access graph
        /// </summary>
        class TaskNode : IAccessHandle
        {
            #if DEBUG
            private static UInt64 globalID;
            private readonly UInt64 id;
            #endif
            
            private DependencyTreeNode[]? instances;
            private SmallArray<TaskNode?, SmallArray8<TaskNode?>> array;
            private ConcurrentBuffer<TaskNode> children;
            
            private int root;

            /// <summary>
            /// 
            /// </summary>
            public int Root
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get { return root; }
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                set { root = value; }
            }

            private TaskCompletionSource<IAccessHandle>? signal;
            private UInt16 initializationState;
            private int dependencyCount;

            public bool IsPending
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get { return (Volatile.Read(ref initializationState) == 0); }
            }
            
            /// <summary>
            /// Initializes this instance to its default state
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public TaskNode()
            {
                #if DEBUG
                this.id = Interlocked.Increment(ref globalID);
                #endif
                
                this.instances = null;
                this.children = default;
                this.array = default;
            }
            
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static implicit operator Task<IAccessHandle>(TaskNode node)
            {
                return node.signal!.Task;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void AppendChild(TaskNode child)
            {
                children.Enqueue(ref array, child);
                Interlocked.Increment(ref child.dependencyCount);
            }
            
            /// <summary>
            /// Prepares this task to be appended into an access graph
            /// </summary>
            /// <returns>A memory object to set the object instances this task accesses</returns>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public DependencyTreeNode[] BeginInitialize()
            {
                this.dependencyCount = 1;
                this.initializationState = 0;
                this.signal = new TaskCompletionSource<IAccessHandle>(TaskCreationOptions.RunContinuationsAsynchronously);
                this.instances = ArrayPool<DependencyTreeNode>.Shared.Rent(16);
                
                return instances;
            }
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool EndInitialize()
            {
                Volatile.Write(ref initializationState, 1);
                return (Interlocked.Decrement(ref dependencyCount) == 0);
            }

            /// <summary>
            /// Finishes this tasks execution. Decreases the dependency counter for all children currently attached and
            /// releases any used resource
            /// </summary>
            public void Finish()
            {
                // Remove from dependencies
                int index = DependencyTree.Begin(instances!, root);
                do
                {
                    instances![index].Delegate(instances[index].Instance, this);
                    index = DependencyTree.Next(instances, index);
                }
                while(index != DependencyTreeNode.Empty);
                
                // Removed this from all dependencies, no need for locking
                for (int i = 0, count = array.Length; i < count; i++)
                {
                    if (array[i] != null && Interlocked.Decrement(ref array[i]!.dependencyCount) == 0)
                    { 
                        array[i]!.SignalNode();
                    }
                }
                
                array.Clear();
                array.Resize(0);
                ArrayPool<DependencyTreeNode>.Shared.Return(instances);
                instances = null;
                children.Reset();
            }
            
            /// <inheritdoc/>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public AccessType GetAccess(UInt32 uniqueId)
            {
                if (DependencyTree.Find(instances!, uniqueId, root, out _, out Ref<DependencyTreeNode> result))
                {
                    // ReSharper disable BitwiseOperatorOnEnumWithoutFlags
                    
                    return (AccessType)result.Value.Flags & ~AccessType.Reserved;
                    
                    // ReSharper restore BitwiseOperatorOnEnumWithoutFlags
                }
                else return AccessType.Reserved;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void SignalNode()
            {
                signal!.SetResult(this);
            }
        }
    }
}