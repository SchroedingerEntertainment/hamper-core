// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Text;

namespace Soe.Runtime
{
    /// <summary>
    /// Declares a module that can be executed similar to a standalone program
    /// </summary>
    #if EXPORT_HAMPER_CORE_RUNTIME_MODULES
    public
    #else
    internal
    #endif
    interface IMainModule
    {
        /// <summary>
        /// The entry point of this module
        /// </summary>
        /// <param name="args">A source of command line arguments applied to the module</param>
        /// <returns>The return code of the execution</returns>
        static abstract int Run<Source>(ref Source args)
            where Source : struct, IStreamSource;
    }
}