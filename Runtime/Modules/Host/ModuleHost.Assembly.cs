// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Loader;

namespace Soe.Runtime
{
    #if EXPORT_HAMPER_CORE_RUNTIME_MODULES
    public
    #else
    internal
    #endif
    static partial class ModuleHost
    {
        /// <summary>
        /// Determines a special isolated context to load modules into
        /// </summary>
        private class ModuleContainer : AssemblyLoadContext
        {
            /// <inheritdoc/>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            protected override Assembly? Load(AssemblyName assemblyName)
            {
                // Forwards the assembly resolution to the underlying AppDomain
                return base.Load(assemblyName);
            }
        }
    }
}