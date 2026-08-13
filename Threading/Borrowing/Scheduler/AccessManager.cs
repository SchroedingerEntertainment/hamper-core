// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Buffers;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Soe.Collections.Inline;

namespace Soe.Threading
{
    /// <summary>
    /// Allows to create typed scopes in which an object instance can be safely used according to a certain policy
    /// </summary>
    #if EXPORT_HAMPER_CORE_THREADING
    public
    #else
    internal
    #endif
    static partial class AccessManager
    {
        [ThreadStatic]
        private static IAccessHandle? current;
        
        /// <summary>
        /// Schedules access to the provided instances according to the desired access policies
        /// </summary>
        /// <param name="i1">An instance to get access</param>
        /// <param name="i2">An instance to get access</param>
        /// <typeparam name="T1">An instance type to get access</typeparam>
        /// <typeparam name="Policy1">The access policy for this instance type</typeparam>
        /// <typeparam name="T2">An instance type to get access</typeparam>
        /// <typeparam name="Policy2">The access policy for this instance type</typeparam>
        /// <returns>The task object representing the asynchronous operation</returns>
        public static Task<IAccessHandle> BorrowAsync<T1, Policy1, T2, Policy2>(T1 i1, T2 i2)
            where T1 : class
            where T2 : class
            where Policy1 : struct, IAccessPolicy
            where Policy2 : struct, IAccessPolicy
        {
            TaskNode node = GenericPool<TaskNode, GenericPolicy<TaskNode>>.Shared.Rent();
            ref InstanceArray array = ref node.Initialize();
            
            DependencyTree tree = default;
            
            // ReSharper disable BitwiseOperatorOnEnumWithoutFlags
            
            ref DependencyTreeNode tn = ref tree.Emplace(ref array, Dependency<T1>.GetUniqueId(i1));
            tn.Flags |= (byte)(Policy1.Order & ~AccessType.Reserved);
            tn.Delegate = Append<T1, Policy1>;
            tn.Instance = i1;
            
            tn = ref tree.Emplace(ref array, Dependency<T2>.GetUniqueId(i2));
            tn.Flags |= (byte)(Policy2.Order & ~AccessType.Reserved);
            tn.Delegate = Append<T2, Policy2>;
            tn.Instance = i2;
            
            // ReSharper restore BitwiseOperatorOnEnumWithoutFlags

            node.Root = tree.Root;
            int index = tree.First(ref array);
            do
            {
                array[index].Delegate = array[index].Delegate(array[index].Instance, node)!;
                index = DependencyTree.Next(ref array, index);
            }
            while(index != DependencyTreeNode.Empty);
            if (node.DependencyCount == 0)
            {
                node.SignalNode();
            }
            else node.SetInitialized();
            return node;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static DependencyTreeNode.ManagerDelegate? Append<T, Policy>(object? instance, TaskNode node)
            where T : class
            where Policy : struct, IAccessPolicy
        {
            Dependency<T>.Append<Policy>(instance!, node);
            return Remove<T>;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static DependencyTreeNode.ManagerDelegate? Remove<T>(object? instance, TaskNode node)
            where T : class
        {
            Dependency<T>.Remove(instance, node);
            return null;
        }

        /// <summary>
        /// Gets if the current thread is running under scheduled access
        /// </summary>
        /// <returns>True if called from within a scheduled access chain, false otherwise</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsScheduledTask()
        {
            return (current != null);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void Return(IAccessHandle handle)
        {
            if (handle is TaskNode task)
            {
                SmallArray<TaskNode, SmallArray16<TaskNode>> dispatchableNodes = default;
                for (int i = task.Finish(ref dispatchableNodes) - 1; i >= 0; i--)
                {
                    dispatchableNodes[i].SignalNode();
                }
                GenericPool<TaskNode, GenericPolicy<TaskNode>>.Shared.Return(task);
            }
            else throw new ArgumentException(nameof(handle));
        }

        /// <summary>
        /// Releases the provided reference if possible
        /// </summary>
        /// <param name="instance">An object to release the dependency graph for</param>
        /// <typeparam name="T">The reference type to release</typeparam>
        /// <returns>True if the dependency graph for this instance was successfully released, false otherwise</returns>
        /// <remarks>Releasing of the object will fail if the dependency graph is not empty, which means one or more tasks
        /// are pending and the scheduler is waiting for a trigger to run</remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ReleaseReference<T>(T instance)
            where T : class
        {
            return Dependency<T>.Release(instance);
        }
        
        /// <summary>
        /// Throws if the calling thread is executed in an owned scope and the desired access doesn't match
        /// </summary>
        /// <param name="instance">The object instance to for this operation</param>
        /// <param name="access">The desired access type for this operation</param>
        /// <typeparam name="T">The reference type to check</typeparam>
        /// <exception cref="ThreadOwnershipViolationException">Thrown if executed within an owned scope and the access doesn't match</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowOnAccessViolation<T>(object instance, AccessType access)
            where T : class
        {
            if ((current?.GetAccess(Dependency<T>.GetUniqueId(instance)) ?? access) != access)
                throw new ThreadOwnershipViolationException();
        }
        /// <summary>
        /// Throws if the calling thread is executed in an owned scope and the desired access is less permissive as expected
        /// </summary>
        /// <param name="instance">The object instance to for this operation</param>
        /// <param name="access">The minimum access type for this operation</param>
        /// <typeparam name="T">The reference type to check</typeparam>
        /// <exception cref="ThreadOwnershipViolationException">Thrown if executed within an owned scope and the access is less permissive as expected</exception>
        /// <remarks>Access is an ordered ID growing from zero, where zero means most permissive access</remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowOnLessAccessible<T>(object instance, AccessType access)
            where T : class
        {
            if ((current?.GetAccess(Dependency<T>.GetUniqueId(instance)) ?? access) > access)
                throw new ThreadOwnershipViolationException();
        }
    }
}