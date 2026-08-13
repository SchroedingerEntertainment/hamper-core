// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;
using Soe.Collections.HashSet;
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
        /// Manages the dependency graph of the corresponding object instance
        /// </summary>
        struct TaskList : IHashContainer<object>
        {
            private SmallArray<TaskNode?, SmallArray4<TaskNode?>> tasks;
            private ConcurrentBuffer<TaskNode> buffer;
            
            private readonly UInt32 uniqueId;
            /// <summary>
            /// 
            /// </summary>
            public UInt32 UniqueId
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get { return uniqueId; }
            }
            
            private readonly int hash;
            /// <inheritdoc/>
            public int Hash
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get { return hash; }
            }

            private readonly object key;
            /// <inheritdoc/>
            public object Key
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get { return key; }
            }

            /// <inheritdoc/>
            public bool IsValid
            {
                get { return (hash != 0 && key != null); }
            }

            /// <summary>
            /// Gets the number of tasks currently in the dependency graph
            /// </summary>
            public int Count
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get { return buffer.Count; }
            }
            
            /// <summary>
            /// Initializes a new instance of the dependency graph
            /// </summary>
            /// <param name="hash">The hash code of the object instance</param>
            /// <param name="key">The corresponding object instance</param>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public TaskList(UInt32 uniqueId, int hash, object key)
            {
                this.uniqueId = uniqueId;
                this.key = key;
                this.hash = hash;
            }

            /// <summary>
            /// Appends a <see cref="TaskNode"/> instance to the access graph of the provided object
            /// </summary>
            /// <param name="task">The task instance to append</param>
            /// <typeparam name="T">A reference type</typeparam>
            /// <typeparam name="Policy">The desired access policy</typeparam>
            /// <returns>True if the node has other tasks to wait on, false otherwise</returns>
            public bool Append<T, Policy>(TaskNode task)
                where T : class
                where Policy : struct, IAccessPolicy
            {
                int index = buffer.Enqueue(ref tasks, task);
                
                SpinWait wait = new SpinWait();
                int i = -1;
                
            Retry:
                if (i >= 0)
                {
                    // Reached from an uninitialized task
                    wait.SpinOnce();
                }
                using (ScopedDisposable.Acquire<ConcurrentBuffer<TaskNode>, ConcurrentBuffer<TaskNode>.SharedOperation>(ref buffer))
                {
                    int moduloMask = tasks.Length - 1;
                    if (i < 0)
                    {
                        i = ((index - 1) & moduloMask);
                    }
                    
                    // Iterate from the current emplacement index to the tail of the buffer to find potential parents
                    for (int beforeTail = ((buffer.Tail - 1) & moduloMask); i != beforeTail; i = ((i - 1) & moduloMask))
                    {
                        if (tasks[i]?.State < TaskNodeState.Initialized)
                        {
                            // Wait until potential parent is fully initialized to prevent AB problems
                            goto Retry;
                        }
                        AccessType order = tasks[i]?.GetAccess(uniqueId) ?? AccessType.Reserved;
                        #if DEBUG
                        if (order >= AccessType.Reserved)
                        {
                            throw new ArgumentOutOfRangeException(typeof(T).Name);                            
                        }
                        #endif
                        if (Policy.IsConflicting(order))
                        { 
                            // A task conflicts with the desired access policy, add this as child
                            
                            tasks[i]!.AppendChild(task);
                            for (i = ((i - 1) & moduloMask); i != beforeTail; i = ((i - 1) & moduloMask)) 
                            {
                                // Test if there are other tasks with the same conflict, this needs to wait
                                // on all of those tasks to complete first
                                
                                AccessType nextOrder = tasks[i]?.GetAccess(uniqueId) ?? AccessType.Reserved;
                                #if DEBUG
                                if (nextOrder >= AccessType.Reserved)
                                {
                                    throw new ArgumentOutOfRangeException(typeof(T).Name);                            
                                }
                                #endif
                                if (order == nextOrder)
                                {
                                    tasks[i]!.AppendChild(task);
                                }
                                else break;
                            }
                            return true;
                        }
                    }
                    return false;
                }
            }

            /// <summary>
            /// Removes the appended <see cref="TaskNode"/> instance from the access graph of the provided object
            /// </summary>
            /// <param name="task">The corresponding task instance to remove</param>
            /// <exception cref="IndexOutOfRangeException">Thrown if the task was not found in the access graph</exception>
            public void Remove(TaskNode task)
            {
                using (ScopedDisposable.Acquire<ConcurrentBuffer<TaskNode>, ConcurrentBuffer<TaskNode>.ExclusiveOperation>(ref buffer))
                {
                    int index = tasks.IndexOf(task);
                    if (index >= 0)
                    {
                        int moduloMask = tasks.Length - 1;
                        
                        (tasks[index], tasks[buffer.Tail & moduloMask]) = (tasks[buffer.Tail & moduloMask], tasks[index]);
                        buffer.TryDequeue(ref tasks, out _);
                    }
                    else throw new IndexOutOfRangeException();
                }
            }
        }
    }
}