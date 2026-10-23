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
    static partial class AccessManager
    {
        /// <summary>
        /// A policy that manages the trigger chain of scheduled tasks
        /// </summary>
        public readonly struct ManagedAccessPolicy : IScopePolicy<IAccessHandle>
        {
            /// <summary>
            /// Sets the instance of the access handle to be used by this thread
            /// </summary>
            /// <param name="instance">The access handle managing the current scope</param>
            public static void Initialize(in IAccessHandle instance)
            {
                current = instance;
            }

            /// <summary>
            /// Returns the instance of the access handle and triggers waiting tasks if any
            /// </summary>
            /// <param name="instance">The access handle managing the current scope</param>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void Dispose(in IAccessHandle instance)
            {
                current = null;
                Return(instance);
            }
        }
    }
}