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
            private SmallArray<TaskNode?, SmallArray8<TaskNode?>> tasks;
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
            public void Append<T, Policy>(TaskNode task)
                where T : class
                where Policy : struct, IAccessPolicy
            {
                
            Head:
                using (ScopedDisposable.Acquire<ConcurrentBuffer<TaskNode>, ConcurrentBuffer<TaskNode>.SharedOperation>(ref buffer))
                {
                    if (!buffer.TryEnqueue(ref tasks, task, out UInt32 index))
                    {
                        goto Grow;
                    }
                    else index--;
                    
                    SpinWait wait = new SpinWait(); 
                    PolicyResolutionFlags flags = PolicyResolutionFlags.None;
                    
                    // Iterate from the current emplacement index to the tail of the buffer to find potential parents
                    for (UInt32 moduloMask = (UInt32)(tasks.Length - 1), reserved = buffer.Tail - 1; index - reserved > 0; index--)
                    {
                        TaskNode? t;
                        do
                        {
                            // The task may be already populated but not initialized, spin until it is initialized
                            t = tasks[(int)(index & moduloMask)];
                        } 
                        while (t == null);
                        while (t!.IsPending)
                        {
                            wait.SpinOnce();
                        }
                        
                        PolicyResolutionFlags current = Policy.Resolve(t.GetAccess(uniqueId), flags);
                        if (current.FlagSet(PolicyResolutionFlags.Wait))
                        {
                            t!.AppendChild(task);
                        }
                        if (current.HasFlag(PolicyResolutionFlags.Barrier))
                        {
                            break;
                        }
                        else flags = current;
                    }
                    return;
                }
                
            Grow:
                using (ScopedDisposable.Acquire<ConcurrentBuffer<TaskNode>, ConcurrentBuffer<TaskNode>.ExclusiveOperation>(ref buffer))
                {
                    buffer.Grow(ref tasks);
                    goto Head;
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
                    if (buffer.IndexOf(ref tasks, task, out UInt32 index))
                    {
                        // Move item to the front of the buffer in order to keep the task order in the buffer
                        for (UInt32 moduloMask = (UInt32)(tasks.Length - 1), reserved = buffer.Tail - 1, i = index - 1; i - reserved > 0; i--, index--)
                        {
                            (tasks[(int)(i & moduloMask)], tasks[(int)(index & moduloMask)]) = (tasks[(int)(index & moduloMask)], tasks[(int)(i & moduloMask)]);
                        }
                        buffer.TryDequeue(ref tasks, out _);
                    }
                    else throw new IndexOutOfRangeException();
                }
            }
        }
    }
}