// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

namespace Soe.Runtime
{
    /// <summary>
    /// Defines the type of a workspace
    /// </summary>
    #if EXPORT_HAMPER_CORE_RUNTIME_APPLICATION
    public
    #else
    internal
    #endif
    enum WorkspaceType
    {
        /// <summary>
        /// Defines the root workspace
        /// </summary>
        Root,
        /// <summary>
        /// Defines a local workspace
        /// </summary>
        /// <remarks>This can be equal to <see cref="WorkspaceType.Root"/></remarks>
        Tree
    }
}