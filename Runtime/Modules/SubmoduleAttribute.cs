// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license

using System.Text;

namespace Soe.Runtime
{
    /// <summary>
    /// Declares a submodule within an assembly
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
    internal abstract class SubmoduleAttribute : Attribute
    {
        /// <summary>
        /// Gets the name of module's parent
        /// </summary>
        public abstract string ParentModuleName
        {
            get;
        }
        
        /// <summary>
        /// Executes this modules load routine
        /// </summary>
        public abstract void Load();
        
        /// <summary>
        /// Executes this modules release routine
        /// </summary>
        public abstract void Unload();
    }
}