// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;

namespace Soe.Threading
{
    /// <summary>
    /// An object responsible to manage the asynchronous borrowing operation
    /// </summary>
    [method: MethodImpl(MethodImplOptions.AggressiveInlining)]
    #if EXPORT_HAMPER_CORE_THREADING
    public
    #else
    internal
    #endif
    readonly struct BorrowAwaiter(Task<IAccessHandle> task) : INotifyCompletion
    {
        /// <summary>
        /// Gets if the underlying task is completed
        /// </summary>
        public bool IsCompleted
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return task.IsCompleted; }
        }

        /// <summary>
        /// Appends an action to the completion state of the underlying operation
        /// </summary>
        /// <param name="continuation">A method delegate called when the operation finishes</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void OnCompleted(Action continuation)
        {
            task.GetAwaiter().OnCompleted(continuation);
        }

        /// <summary>
        /// Gets the access scope managed by the corresponding <see cref="AccessManager"/>
        /// </summary>
        /// <returns>A disposable scope used in a using-block</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ScopedDisposable<IAccessHandle, AccessManager.ManagedAccessPolicy> GetResult()
        {
            return new ScopedDisposable<IAccessHandle, AccessManager.ManagedAccessPolicy>(task.Result);
        }
    }
}