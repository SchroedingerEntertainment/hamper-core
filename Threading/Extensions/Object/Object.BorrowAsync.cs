// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;

namespace Soe.Threading
{
    #if EXPORT_HAMPER_CORE_THREADING
    public
    #else
    internal
    #endif
    static partial class ObjectExtensions
    {
        // ReSharper disable InvalidXmlDocComment
        
        /// <summary>
        /// Requests access to the provided object instance and awaits execution to be scheduled
        /// </summary>
        /// <typeparam name="T">An instance type to get access</typeparam>
        /// <typeparam name="Policy">The access policy for this instance type</typeparam>
        /// <returns>An awaitable object to enter the requested scope</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BorrowAwaitable<T, Policy> BorrowAsync<T, Policy>(this T instance)
            where T : class
            where Policy : struct, IAccessPolicy
        {
            return new BorrowAwaitable<T, Policy>(instance);
        }
        
        /// <summary>
        /// Requests access to the provided object instance and awaits execution to be scheduled
        /// </summary>
        /// <param name="policy">The access policy for this instance type</param>
        /// <typeparam name="T">An instance type to get access</typeparam>
        /// <typeparam name="Policy">The access policy for this instance type</typeparam>
        /// <returns>An awaitable object to enter the requested scope</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BorrowAwaitable<T, Policy> BorrowAsync<T, Policy>(this T instance, Policy policy)
            
            where T : class
            where Policy : struct, IAccessPolicy
        {
            return BorrowAsync<T, Policy>(instance);
        }
        
        // ReSharper restore InvalidXmlDocComment
    }
}