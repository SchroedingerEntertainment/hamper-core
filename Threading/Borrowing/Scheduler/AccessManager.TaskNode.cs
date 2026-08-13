// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Diagnostics;
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
        /// Provides different states of a <see cref="TaskNode"/> instance
        /// </summary>
        enum TaskNodeState : int
        {
            Created = 0,
            Initialized = 1,
            Running = 2,
            Completed = 3
        }

        /// <summary>
        /// Represents a single task in a designated access graph
        /// </summary>
        class TaskNode : IAccessHandle
        {
            SmallArray<TaskNode?, SmallArray4<TaskNode?>> array;
            ConcurrentBuffer<TaskNode> children;

            private InstanceArray instances;
            
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
            
            TaskCompletionSource<IAccessHandle>? signal;

            private int state;
            /// <summary>
            /// Gets the current state of this task 
            /// </summary>
            public TaskNodeState State
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get { return (TaskNodeState)Volatile.Read(ref state); }
            }

            private int dependencyCount;
            /// <summary>
            /// Gets the amount of tasks this one is currently waiting for
            /// </summary>
            public int DependencyCount
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get { return Volatile.Read(ref dependencyCount); }
            }

            /// <summary>
            /// Initializes this instance to its default state
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public TaskNode()
            {
                this.array = default;
                this.children = default;
                this.instances = default;
            }

            // ReSharper disable ParameterHidesMember
            
            /// <summary>
            /// Prepares this task to be appended into an access graph
            /// </summary>
            /// <returns>A memory object to set the object instances this task accesses</returns>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public ref InstanceArray Initialize()
            {
                this.state = (int)TaskNodeState.Created;
                this.signal = new TaskCompletionSource<IAccessHandle>();

                return ref instances;
            }
            
            // ReSharper restore ParameterHidesMember
            
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static implicit operator Task<IAccessHandle>(TaskNode node)
            {
                return node.signal!.Task;
            }
            
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            void AddParent()
            {
                Interlocked.Increment(ref dependencyCount);
            }

            /// <summary>
            /// Appends the provided <see cref="TaskNode"/> to this tasks children and increases its dependency count
            /// </summary>
            /// <param name="child">A task instance to append</param>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void AppendChild(TaskNode child)
            {
                children.Enqueue(ref array, child);
                child.AddParent();
            }
            
            /// <summary>
            /// Finishes this tasks execution. Decreases the dependency counter for all children currently attached and
            /// releases any used resource
            /// </summary>
            /// <param name="dispatchableNodes">An array to be filled with tasks ready to run next</param>
            /// <returns>The amount of tasks added to the provided array</returns>
            public int Finish<Accessor>(ref Accessor dispatchableNodes)
                where Accessor : IArrayAccessor<TaskNode>
            {
                // Remove from dependencies
                int index = DependencyTree.First(ref instances, root);
                do
                {
                    instances[index].Delegate(instances[index].Instance, this);
                    index = DependencyTree.Next(ref instances, index);
                }
                while(index != DependencyTreeNode.Empty);
                
                Volatile.Write(ref state, (int)TaskNodeState.Completed);
                using(ScopedDisposable.Acquire<ConcurrentBuffer<TaskNode>, ConcurrentBuffer<TaskNode>.ExclusiveOperation>(ref children))
                {
                    Span<TaskNode> nodes = dispatchableNodes.AsSpan();
                    int nodeCount = 0;
                    
                    for (int i = children.Tail, count = children.Count; count > 0; i++, count--)
                    {
                        if (array[i] != null)
                        {
                            if (array[i]!.RemoveParent())
                            {
                                if (nodes.Length == nodeCount)
                                {
                                    dispatchableNodes.Resize(nodeCount * 2);
                                    nodes = dispatchableNodes.AsSpan();
                                }
                                nodes[nodeCount] = array[i]!;
                                nodeCount++;
                            }
                            array[i] = null;
                        }
                    }
                    children.Reset();
                    instances = default;
                    array.Clear();
                    array.Resize(0);
                    
                    return nodeCount;
                }
            }
            
            /// <inheritdoc/>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public AccessType GetAccess(UInt32 uniqueId)
            {
                if (DependencyTree.Find(ref instances, uniqueId, root, out _, out Ref<DependencyTreeNode> result))
                {
                    // ReSharper disable BitwiseOperatorOnEnumWithoutFlags
                    
                    return (AccessType)result.Value.Flags & ~AccessType.Reserved;
                    
                    // ReSharper restore BitwiseOperatorOnEnumWithoutFlags
                }
                else return AccessType.Reserved;
            }
            
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            bool RemoveParent()
            {
                return (Interlocked.Decrement(ref dependencyCount) == 0);
            }
            
            /// <summary>
            /// Signals this task to be fully initialized. All dependencies have been attached and the task is
            /// ready to act as parent for other tasks
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void SetInitialized()
            {
                if (State < TaskNodeState.Initialized)
                {
                    Volatile.Write(ref state, (int)TaskNodeState.Initialized);
                }
            }

            /// <summary>
            /// Signals that the underlying <see cref="System.Threading.Tasks.Task"/> can enter the requested scope
            /// </summary>
            public void SignalNode()
            {
                if (signal!.TrySetResult(this))
                {
                    Volatile.Write(ref state, (int)TaskNodeState.Running);
                }
                else Debugger.Break();
            }
        }
    }
}