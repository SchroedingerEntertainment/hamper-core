// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

namespace Soe.Runtime
{
    /// <summary>
    /// Declares a runtime module that can be loaded from a host application
    /// </summary>
    #if EXPORT_HAMPER_CORE_RUNTIME_MODULES
    public
    #else
    internal
    #endif
    interface IRuntimeModule
    {
        /// <summary>
        /// A callback executed when the module has been loaded
        /// </summary>
        static abstract void Load();

        /// <summary>
        /// A callback executed when the module is about to unload
        /// </summary>
        static abstract void Unload();
    }
}