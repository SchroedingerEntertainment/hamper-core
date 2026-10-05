// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Buffers;
using System.Runtime.CompilerServices;

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
        /// <summary>
        /// Proxy to create <see cref="DependencyTreeResolver"/> instances without exposing embedded types
        /// </summary>
        delegate DependencyTreeResolver CreateInstanceDelegate(DependencyTreeNode[] array, ref DependencyTree tree);
        
        [ThreadStatic]
        private static IAccessHandle? current;
        private static CreateInstanceDelegate? CreateResolver;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static AccessManager()
        {
            typeof(DependencyTreeResolver).Initialize();
        }
        
        /// <summary>
        /// Schedules access to the provided instances according to the desired access policies
        /// </summary>
        /// <param name="instance">An instance to get access</param>
        /// <typeparam name="T">An instance type to get access</typeparam>
        /// <typeparam name="Policy">The access policy for this instance type</typeparam>
        /// <returns>The task object representing the asynchronous operation</returns>
        public static Task<IAccessHandle> BorrowAsync<T, Policy>(T instance)
            where T : class
            where Policy : struct, IAccessPolicy
        {
            TaskNode node = GenericPool<TaskNode, GenericPolicy<TaskNode>>.Shared.Rent();
            DependencyTreeNode[] array = node.BeginInitialize();
            DependencyTree tree = default;
            
            UInt32 uuid = Dependency<T>.GetUniqueId(instance);
            
            DependencyTreeResolver resolver = CreateResolver!(array, ref tree);
            resolver.Add<T, Policy>(uuid, instance);
            
            Resolve(resolver, uuid, instance);
            
            node.Root = tree.Root;
            int index = tree.Begin(array);
            do
            {
                array[index].Delegate = array[index].Delegate(array[index].Instance, node)!;
                index = DependencyTree.Next(array, index);
            }
            while(index != DependencyTreeNode.Empty);
            if (node.EndInitialize())
            {
                node.SignalNode();
            }
            return node;
        }
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
            DependencyTreeNode[] array = node.BeginInitialize();
            DependencyTree tree = default;
            
            UInt32 uuid1 = Dependency<T1>.GetUniqueId(i1);
            UInt32 uuid2 = Dependency<T2>.GetUniqueId(i2);
            
            DependencyTreeResolver resolver = CreateResolver!(array, ref tree);
            resolver.Add<T1, Policy1>(uuid1, i1);
            resolver.Add<T2, Policy2>(uuid2, i2);
            
            Resolve(resolver, uuid1, i1);
            Resolve(resolver, uuid2, i2);
            
            node.Root = tree.Root;
            int index = tree.Begin(array);
            do
            {
                array[index].Delegate = array[index].Delegate(array[index].Instance, node)!;
                index = DependencyTree.Next(array, index);
            }
            while(index != DependencyTreeNode.Empty);
            if (node.EndInitialize())
            {
                node.SignalNode();
            }
            return node;
        }
        /// <summary>
        /// Schedules access to the provided instances according to the desired access policies
        /// </summary>
        /// <param name="i1">An instance to get access</param>
        /// <param name="i2">An instance to get access</param>
        /// <param name="i3">An instance to get access</param>
        /// <typeparam name="T1">An instance type to get access</typeparam>
        /// <typeparam name="Policy1">The access policy for this instance type</typeparam>
        /// <typeparam name="T2">An instance type to get access</typeparam>
        /// <typeparam name="Policy2">The access policy for this instance type</typeparam>
        /// <typeparam name="T3">An instance type to get access</typeparam>
        /// <typeparam name="Policy3">The access policy for this instance type</typeparam>
        /// <returns>The task object representing the asynchronous operation</returns>
        public static Task<IAccessHandle> BorrowAsync<T1, Policy1, T2, Policy2, T3, Policy3>(T1 i1, T2 i2, T3 i3)
            where T1 : class
            where T2 : class
            where T3 : class
            where Policy1 : struct, IAccessPolicy
            where Policy2 : struct, IAccessPolicy
            where Policy3 : struct, IAccessPolicy
        {
            TaskNode node = GenericPool<TaskNode, GenericPolicy<TaskNode>>.Shared.Rent();
            DependencyTreeNode[] array = node.BeginInitialize();
            DependencyTree tree = default;
            
            UInt32 uuid1 = Dependency<T1>.GetUniqueId(i1);
            UInt32 uuid2 = Dependency<T2>.GetUniqueId(i2);
            UInt32 uuid3 = Dependency<T3>.GetUniqueId(i3);
            
            DependencyTreeResolver resolver = CreateResolver!(array, ref tree);
            resolver.Add<T1, Policy1>(uuid1, i1);
            resolver.Add<T2, Policy2>(uuid2, i2);
            resolver.Add<T3, Policy3>(uuid3, i3);
            
            Resolve(resolver, uuid1, i1);
            Resolve(resolver, uuid2, i2);
            Resolve(resolver, uuid3, i3);
            
            node.Root = tree.Root;
            int index = tree.Begin(array);
            do
            {
                array[index].Delegate = array[index].Delegate(array[index].Instance, node)!;
                index = DependencyTree.Next(array, index);
            }
            while(index != DependencyTreeNode.Empty);
            if (node.EndInitialize())
            {
                node.SignalNode();
            }
            return node;
        }
        /// <summary>
        /// Schedules access to the provided instances according to the desired access policies
        /// </summary>
        /// <param name="i1">An instance to get access</param>
        /// <param name="i2">An instance to get access</param>
        /// <param name="i3">An instance to get access</param>
        /// <param name="i4">An instance to get access</param>
        /// <typeparam name="T1">An instance type to get access</typeparam>
        /// <typeparam name="Policy1">The access policy for this instance type</typeparam>
        /// <typeparam name="T2">An instance type to get access</typeparam>
        /// <typeparam name="Policy2">The access policy for this instance type</typeparam>
        /// <typeparam name="T3">An instance type to get access</typeparam>
        /// <typeparam name="Policy3">The access policy for this instance type</typeparam>
        /// <typeparam name="T4">An instance type to get access</typeparam>
        /// <typeparam name="Policy4">The access policy for this instance type</typeparam>
        /// <returns>The task object representing the asynchronous operation</returns>
        public static Task<IAccessHandle> BorrowAsync<T1, Policy1, T2, Policy2, T3, Policy3, T4, Policy4>(T1 i1, T2 i2, T3 i3, T4 i4)
            where T1 : class
            where T2 : class
            where T3 : class
            where T4 : class
            where Policy1 : struct, IAccessPolicy
            where Policy2 : struct, IAccessPolicy
            where Policy3 : struct, IAccessPolicy
            where Policy4 : struct, IAccessPolicy
        {
            TaskNode node = GenericPool<TaskNode, GenericPolicy<TaskNode>>.Shared.Rent();
            DependencyTreeNode[] array = node.BeginInitialize();
            DependencyTree tree = default;
            
            UInt32 uuid1 = Dependency<T1>.GetUniqueId(i1);
            UInt32 uuid2 = Dependency<T2>.GetUniqueId(i2);
            UInt32 uuid3 = Dependency<T3>.GetUniqueId(i3);
            UInt32 uuid4 = Dependency<T4>.GetUniqueId(i4);
            
            DependencyTreeResolver resolver = CreateResolver!(array, ref tree);
            resolver.Add<T1, Policy1>(uuid1, i1);
            resolver.Add<T2, Policy2>(uuid2, i2);
            resolver.Add<T3, Policy3>(uuid3, i3);
            resolver.Add<T4, Policy4>(uuid4, i4);
            
            Resolve(resolver, uuid1, i1);
            Resolve(resolver, uuid2, i2);
            Resolve(resolver, uuid3, i3);
            Resolve(resolver, uuid4, i4);
            
            node.Root = tree.Root;
            int index = tree.Begin(array);
            do
            {
                array[index].Delegate = array[index].Delegate(array[index].Instance, node)!;
                index = DependencyTree.Next(array, index);
            }
            while(index != DependencyTreeNode.Empty);
            if (node.EndInitialize())
            {
                node.SignalNode();
            }
            return node;
        }
        /// <summary>
        /// Schedules access to the provided instances according to the desired access policies
        /// </summary>
        /// <param name="i1">An instance to get access</param>
        /// <param name="i2">An instance to get access</param>
        /// <param name="i3">An instance to get access</param>
        /// <param name="i4">An instance to get access</param>
        /// <param name="i5">An instance to get access</param>
        /// <typeparam name="T1">An instance type to get access</typeparam>
        /// <typeparam name="Policy1">The access policy for this instance type</typeparam>
        /// <typeparam name="T2">An instance type to get access</typeparam>
        /// <typeparam name="Policy2">The access policy for this instance type</typeparam>
        /// <typeparam name="T3">An instance type to get access</typeparam>
        /// <typeparam name="Policy3">The access policy for this instance type</typeparam>
        /// <typeparam name="T4">An instance type to get access</typeparam>
        /// <typeparam name="Policy4">The access policy for this instance type</typeparam>
        /// <typeparam name="T5">An instance type to get access</typeparam>
        /// <typeparam name="Policy5">The access policy for this instance type</typeparam>
        /// <returns>The task object representing the asynchronous operation</returns>
        public static Task<IAccessHandle> BorrowAsync<T1, Policy1, T2, Policy2, T3, Policy3, T4, Policy4, T5, Policy5>(T1 i1, T2 i2, T3 i3, T4 i4, T5 i5)
            where T1 : class
            where T2 : class
            where T3 : class
            where T4 : class
            where T5 : class
            where Policy1 : struct, IAccessPolicy
            where Policy2 : struct, IAccessPolicy
            where Policy3 : struct, IAccessPolicy
            where Policy4 : struct, IAccessPolicy
            where Policy5 : struct, IAccessPolicy
        {
            TaskNode node = GenericPool<TaskNode, GenericPolicy<TaskNode>>.Shared.Rent();
            DependencyTreeNode[] array = node.BeginInitialize();
            DependencyTree tree = default;
            
            UInt32 uuid1 = Dependency<T1>.GetUniqueId(i1);
            UInt32 uuid2 = Dependency<T2>.GetUniqueId(i2);
            UInt32 uuid3 = Dependency<T3>.GetUniqueId(i3);
            UInt32 uuid4 = Dependency<T4>.GetUniqueId(i4);
            UInt32 uuid5 = Dependency<T5>.GetUniqueId(i5);
            
            DependencyTreeResolver resolver = CreateResolver!(array, ref tree);
            resolver.Add<T1, Policy1>(uuid1, i1);
            resolver.Add<T2, Policy2>(uuid2, i2);
            resolver.Add<T3, Policy3>(uuid3, i3);
            resolver.Add<T4, Policy4>(uuid4, i4);
            resolver.Add<T5, Policy5>(uuid5, i5);
            
            Resolve(resolver, uuid1, i1);
            Resolve(resolver, uuid2, i2);
            Resolve(resolver, uuid3, i3);
            Resolve(resolver, uuid4, i4);
            Resolve(resolver, uuid5, i5);
            
            node.Root = tree.Root;
            int index = tree.Begin(array);
            do
            {
                array[index].Delegate = array[index].Delegate(array[index].Instance, node)!;
                index = DependencyTree.Next(array, index);
            }
            while(index != DependencyTreeNode.Empty);
            if (node.EndInitialize())
            {
                node.SignalNode();
            }
            return node;
        }
        /// <summary>
        /// Schedules access to the provided instances according to the desired access policies
        /// </summary>
        /// <param name="i1">An instance to get access</param>
        /// <param name="i2">An instance to get access</param>
        /// <param name="i3">An instance to get access</param>
        /// <param name="i4">An instance to get access</param>
        /// <param name="i5">An instance to get access</param>
        /// <param name="i6">An instance to get access</param>
        /// <typeparam name="T1">An instance type to get access</typeparam>
        /// <typeparam name="Policy1">The access policy for this instance type</typeparam>
        /// <typeparam name="T2">An instance type to get access</typeparam>
        /// <typeparam name="Policy2">The access policy for this instance type</typeparam>
        /// <typeparam name="T3">An instance type to get access</typeparam>
        /// <typeparam name="Policy3">The access policy for this instance type</typeparam>
        /// <typeparam name="T4">An instance type to get access</typeparam>
        /// <typeparam name="Policy4">The access policy for this instance type</typeparam>
        /// <typeparam name="T5">An instance type to get access</typeparam>
        /// <typeparam name="Policy5">The access policy for this instance type</typeparam>
        /// <typeparam name="T6">An instance type to get access</typeparam>
        /// <typeparam name="Policy6">The access policy for this instance type</typeparam>
        /// <returns>The task object representing the asynchronous operation</returns>
        public static Task<IAccessHandle> BorrowAsync<T1, Policy1, T2, Policy2, T3, Policy3, T4, Policy4, T5, Policy5, T6, Policy6>(T1 i1, T2 i2, T3 i3, T4 i4, T5 i5, T6 i6)
            where T1 : class
            where T2 : class
            where T3 : class
            where T4 : class
            where T5 : class
            where T6 : class
            where Policy1 : struct, IAccessPolicy
            where Policy2 : struct, IAccessPolicy
            where Policy3 : struct, IAccessPolicy
            where Policy4 : struct, IAccessPolicy
            where Policy5 : struct, IAccessPolicy
            where Policy6 : struct, IAccessPolicy
        {
            TaskNode node = GenericPool<TaskNode, GenericPolicy<TaskNode>>.Shared.Rent();
            DependencyTreeNode[] array = node.BeginInitialize();
            DependencyTree tree = default;
            
            UInt32 uuid1 = Dependency<T1>.GetUniqueId(i1);
            UInt32 uuid2 = Dependency<T2>.GetUniqueId(i2);
            UInt32 uuid3 = Dependency<T3>.GetUniqueId(i3);
            UInt32 uuid4 = Dependency<T4>.GetUniqueId(i4);
            UInt32 uuid5 = Dependency<T5>.GetUniqueId(i5);
            UInt32 uuid6 = Dependency<T6>.GetUniqueId(i6);
            
            DependencyTreeResolver resolver = CreateResolver!(array, ref tree);
            resolver.Add<T1, Policy1>(uuid1, i1);
            resolver.Add<T2, Policy2>(uuid2, i2);
            resolver.Add<T3, Policy3>(uuid3, i3);
            resolver.Add<T4, Policy4>(uuid4, i4);
            resolver.Add<T5, Policy5>(uuid5, i5);
            resolver.Add<T6, Policy6>(uuid6, i6);
            
            Resolve(resolver, uuid1, i1);
            Resolve(resolver, uuid2, i2);
            Resolve(resolver, uuid3, i3);
            Resolve(resolver, uuid4, i4);
            Resolve(resolver, uuid5, i5);
            Resolve(resolver, uuid6, i6);
            
            node.Root = tree.Root;
            int index = tree.Begin(array);
            do
            {
                array[index].Delegate = array[index].Delegate(array[index].Instance, node)!;
                index = DependencyTree.Next(array, index);
            }
            while(index != DependencyTreeNode.Empty);
            if (node.EndInitialize())
            {
                node.SignalNode();
            }
            return node;
        }
        /// <summary>
        /// Schedules access to the provided instances according to the desired access policies
        /// </summary>
        /// <param name="i1">An instance to get access</param>
        /// <param name="i2">An instance to get access</param>
        /// <param name="i3">An instance to get access</param>
        /// <param name="i4">An instance to get access</param>
        /// <param name="i5">An instance to get access</param>
        /// <param name="i6">An instance to get access</param>
        /// <param name="i7">An instance to get access</param>
        /// <param name="i8">An instance to get access</param>
        /// <typeparam name="T1">An instance type to get access</typeparam>
        /// <typeparam name="Policy1">The access policy for this instance type</typeparam>
        /// <typeparam name="T2">An instance type to get access</typeparam>
        /// <typeparam name="Policy2">The access policy for this instance type</typeparam>
        /// <typeparam name="T3">An instance type to get access</typeparam>
        /// <typeparam name="Policy3">The access policy for this instance type</typeparam>
        /// <typeparam name="T4">An instance type to get access</typeparam>
        /// <typeparam name="Policy4">The access policy for this instance type</typeparam>
        /// <typeparam name="T5">An instance type to get access</typeparam>
        /// <typeparam name="Policy5">The access policy for this instance type</typeparam>
        /// <typeparam name="T6">An instance type to get access</typeparam>
        /// <typeparam name="Policy6">The access policy for this instance type</typeparam>
        /// <typeparam name="T7">An instance type to get access</typeparam>
        /// <typeparam name="Policy7">The access policy for this instance type</typeparam>
        /// <returns>The task object representing the asynchronous operation</returns>
        public static Task<IAccessHandle> BorrowAsync<T1, Policy1, T2, Policy2, T3, Policy3, T4, Policy4, T5, Policy5, T6, Policy6, T7, Policy7>(T1 i1, T2 i2, T3 i3, T4 i4, T5 i5, T6 i6, T7 i7)
            where T1 : class
            where T2 : class
            where T3 : class
            where T4 : class
            where T5 : class
            where T6 : class
            where T7 : class
            where Policy1 : struct, IAccessPolicy
            where Policy2 : struct, IAccessPolicy
            where Policy3 : struct, IAccessPolicy
            where Policy4 : struct, IAccessPolicy
            where Policy5 : struct, IAccessPolicy
            where Policy6 : struct, IAccessPolicy
            where Policy7 : struct, IAccessPolicy
        {
            TaskNode node = GenericPool<TaskNode, GenericPolicy<TaskNode>>.Shared.Rent();
            DependencyTreeNode[] array = node.BeginInitialize();
            DependencyTree tree = default;
            
            UInt32 uuid1 = Dependency<T1>.GetUniqueId(i1);
            UInt32 uuid2 = Dependency<T2>.GetUniqueId(i2);
            UInt32 uuid3 = Dependency<T3>.GetUniqueId(i3);
            UInt32 uuid4 = Dependency<T4>.GetUniqueId(i4);
            UInt32 uuid5 = Dependency<T5>.GetUniqueId(i5);
            UInt32 uuid6 = Dependency<T6>.GetUniqueId(i6);
            UInt32 uuid7 = Dependency<T7>.GetUniqueId(i7);
            
            DependencyTreeResolver resolver = CreateResolver!(array, ref tree);
            resolver.Add<T1, Policy1>(uuid1, i1);
            resolver.Add<T2, Policy2>(uuid2, i2);
            resolver.Add<T3, Policy3>(uuid3, i3);
            resolver.Add<T4, Policy4>(uuid4, i4);
            resolver.Add<T5, Policy5>(uuid5, i5);
            resolver.Add<T6, Policy6>(uuid6, i6);
            resolver.Add<T7, Policy7>(uuid7, i7);
            
            Resolve(resolver, uuid1, i1);
            Resolve(resolver, uuid2, i2);
            Resolve(resolver, uuid3, i3);
            Resolve(resolver, uuid4, i4);
            Resolve(resolver, uuid5, i5);
            Resolve(resolver, uuid6, i6);
            Resolve(resolver, uuid7, i7);
            
            node.Root = tree.Root;
            int index = tree.Begin(array);
            do
            {
                array[index].Delegate = array[index].Delegate(array[index].Instance, node)!;
                index = DependencyTree.Next(array, index);
            }
            while(index != DependencyTreeNode.Empty);
            if (node.EndInitialize())
            {
                node.SignalNode();
            }
            return node;
        }
        /// <summary>
        /// Schedules access to the provided instances according to the desired access policies
        /// </summary>
        /// <param name="i1">An instance to get access</param>
        /// <param name="i2">An instance to get access</param>
        /// <param name="i3">An instance to get access</param>
        /// <param name="i4">An instance to get access</param>
        /// <param name="i5">An instance to get access</param>
        /// <param name="i6">An instance to get access</param>
        /// <param name="i7">An instance to get access</param>
        /// <param name="i8">An instance to get access</param>
        /// <typeparam name="T1">An instance type to get access</typeparam>
        /// <typeparam name="Policy1">The access policy for this instance type</typeparam>
        /// <typeparam name="T2">An instance type to get access</typeparam>
        /// <typeparam name="Policy2">The access policy for this instance type</typeparam>
        /// <typeparam name="T3">An instance type to get access</typeparam>
        /// <typeparam name="Policy3">The access policy for this instance type</typeparam>
        /// <typeparam name="T4">An instance type to get access</typeparam>
        /// <typeparam name="Policy4">The access policy for this instance type</typeparam>
        /// <typeparam name="T5">An instance type to get access</typeparam>
        /// <typeparam name="Policy5">The access policy for this instance type</typeparam>
        /// <typeparam name="T6">An instance type to get access</typeparam>
        /// <typeparam name="Policy6">The access policy for this instance type</typeparam>
        /// <typeparam name="T7">An instance type to get access</typeparam>
        /// <typeparam name="Policy7">The access policy for this instance type</typeparam>
        /// <typeparam name="T8">An instance type to get access</typeparam>
        /// <typeparam name="Policy8">The access policy for this instance type</typeparam>
        /// <returns>The task object representing the asynchronous operation</returns>
        public static Task<IAccessHandle> BorrowAsync<T1, Policy1, T2, Policy2, T3, Policy3, T4, Policy4, T5, Policy5, T6, Policy6, T7, Policy7, T8, Policy8>(T1 i1, T2 i2, T3 i3, T4 i4, T5 i5, T6 i6, T7 i7, T8 i8)
            where T1 : class
            where T2 : class
            where T3 : class
            where T4 : class
            where T5 : class
            where T6 : class
            where T7 : class
            where T8 : class
            where Policy1 : struct, IAccessPolicy
            where Policy2 : struct, IAccessPolicy
            where Policy3 : struct, IAccessPolicy
            where Policy4 : struct, IAccessPolicy
            where Policy5 : struct, IAccessPolicy
            where Policy6 : struct, IAccessPolicy
            where Policy7 : struct, IAccessPolicy
            where Policy8 : struct, IAccessPolicy
        {
            TaskNode node = GenericPool<TaskNode, GenericPolicy<TaskNode>>.Shared.Rent();
            DependencyTreeNode[] array = node.BeginInitialize();
            DependencyTree tree = default;

            UInt32 uuid1 = Dependency<T1>.GetUniqueId(i1);
            UInt32 uuid2 = Dependency<T2>.GetUniqueId(i2);
            UInt32 uuid3 = Dependency<T3>.GetUniqueId(i3);
            UInt32 uuid4 = Dependency<T4>.GetUniqueId(i4);
            UInt32 uuid5 = Dependency<T5>.GetUniqueId(i5);
            UInt32 uuid6 = Dependency<T6>.GetUniqueId(i6);
            UInt32 uuid7 = Dependency<T7>.GetUniqueId(i7);
            UInt32 uuid8 = Dependency<T8>.GetUniqueId(i8);
            
            DependencyTreeResolver resolver = CreateResolver!(array, ref tree);
            resolver.Add<T1, Policy1>(uuid1, i1);
            resolver.Add<T2, Policy2>(uuid2, i2);
            resolver.Add<T3, Policy3>(uuid3, i3);
            resolver.Add<T4, Policy4>(uuid4, i4);
            resolver.Add<T5, Policy5>(uuid5, i5);
            resolver.Add<T6, Policy6>(uuid6, i6);
            resolver.Add<T7, Policy7>(uuid7, i7);
            resolver.Add<T8, Policy8>(uuid8, i8);
            
            Resolve(resolver, uuid1, i1);
            Resolve(resolver, uuid2, i2);
            Resolve(resolver, uuid3, i3);
            Resolve(resolver, uuid4, i4);
            Resolve(resolver, uuid5, i5);
            Resolve(resolver, uuid6, i6);
            Resolve(resolver, uuid7, i7);
            Resolve(resolver, uuid8, i8);
            
            node.Root = tree.Root;
            int index = tree.Begin(array);
            do
            {
                array[index].Delegate = array[index].Delegate(array[index].Instance, node)!;
                index = DependencyTree.Next(array, index);
            }
            while(index != DependencyTreeNode.Empty);
            if (node.EndInitialize())
            {
                node.SignalNode();
            }
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
        static DependencyTreeNode.ManagerDelegate? Remove<T>(object? instance, TaskNode node)
            where T : class
        {
            Dependency<T>.Remove(instance, node);
            return null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void Return(IAccessHandle handle)
        {
            if (handle is TaskNode task)
            {
                task.Finish();
                GenericPool<TaskNode, GenericPolicy<TaskNode>>.Shared.Return(task);
            }
            else throw new ArgumentException(nameof(handle));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool Resolve<T>(DependencyTreeResolver resolver, UInt32 uuid, T instance)
            where T : class
        {
            if (instance is IBorrowAnchor anchor)
            {
                return anchor.OnNext(resolver, uuid);
            }
            else return false;
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