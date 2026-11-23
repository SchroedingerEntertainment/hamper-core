// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;
using System.Text;

namespace Soe.Runtime
{
    /// <summary>
    /// Declares a submodule module within an assembly
    /// </summary>
    /// <remarks>This is a forwarder type. The host returns an internal implementation of this attribute
    /// when requested by the CLR</remarks>
    #if EXPORT_HAMPER_CORE_RUNTIME_MODULES
    public
    #else
    internal
    #endif
    #if COMPILER_FLAG_RUNTIME_MODULE
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
    class SubmoduleAttribute<T> : Attribute
    #else
    class SubmoduleAttribute<T> : SubmoduleAttribute
    #endif
        where T : IRuntimeModule
    {
        private delegate int RunDelegate<Source>(ref Source args)
            where Source : struct, IStreamSource;
        
        private readonly string parentModuleName;
        
        /// <summary>
        /// Gets the name of this module's parent
        /// </summary>
        #if COMPILER_FLAG_RUNTIME_MODULE
        public string ModuleName
        #else
        public override string ParentModuleName
        #endif
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return parentModuleName; }
        }

        /// <summary>
        /// Creates a new instance of this attribute
        /// </summary>
        /// <param name="parentModuleName">The name of this module's parent</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SubmoduleAttribute(string parentModuleName)
        {
            this.parentModuleName = parentModuleName;
        }
        
        /// <summary>
        /// Executes this modules load routine
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        #if COMPILER_FLAG_RUNTIME_MODULE
        public void Load()
        #else
        public override void Load()
        #endif
        {
            T.Load();
        }
        
        /// <summary>
        /// Executes this modules release routine
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        #if COMPILER_FLAG_RUNTIME_MODULE
        public void Unload()
        #else
        public override void Unload()
        #endif
        {
            T.Unload();
        }
    }
}