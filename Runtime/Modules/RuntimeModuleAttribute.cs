// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license

using System.Text;

namespace Soe.Runtime
{
    /// <summary>
    /// Declares a runtime module within an assembly
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
    internal abstract class RuntimeModuleAttribute : Attribute
    {
        /// <summary>
        /// Gets the name of this module
        /// </summary>
        public abstract string ModuleName
        {
            get;
        }
        
        /// <summary>
        /// Gets if this module can be executed similar to a standalone program
        /// </summary>
        public abstract bool IsMainModule
        {
            get;
        }
        
        /// <summary>
        /// Executes this modules load routine
        /// </summary>
        public abstract void Load();
        
        /// <summary>
        /// Executes this module's entry point
        /// </summary>
        /// <param name="args">A source of command line arguments applied to the module</param>
        /// <returns>The return code of the execution</returns>
        public abstract int Run<Source>(ref Source args)
            where Source : struct, IStreamSource;
        
        /// <summary>
        /// Executes this modules release routine
        /// </summary>
        public abstract void Unload();
    }
}