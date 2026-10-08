// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace Soe.Runtime
{
    /// <summary>
    /// Declares a runtime module within an assembly
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
    class RuntimeModuleAttribute<T> : Attribute
    #else
    class RuntimeModuleAttribute<T> : RuntimeModuleAttribute
    #endif
        where T : IRuntimeModule
    {
        private delegate int RunDelegate<Source>(ref Source args)
            where Source : struct, IStreamSource;
        
        private readonly string moduleName;
        
        /// <summary>
        /// Gets the name of this module
        /// </summary>
        #if COMPILER_FLAG_RUNTIME_MODULE
        public string ModuleName
        #else
        public override string ModuleName
        #endif
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return moduleName; }
        }
        
        /// <summary>
        /// Gets if this module can be executed similar to a standalone program
        /// </summary>
        #if COMPILER_FLAG_RUNTIME_MODULE
        public bool IsMainModule
        #else
        public override bool IsMainModule
        #endif
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return typeof(IMainModule).IsAssignableFrom(typeof(T)); }
        }

        /// <summary>
        /// Creates a new instance of this attribute
        /// </summary>
        /// <param name="moduleName">The name of this module</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public RuntimeModuleAttribute(string moduleName)
        {
            this.moduleName = moduleName;
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
        /// Executes this module's entry point
        /// </summary>
        /// <param name="args">A source of command line arguments applied to the module</param>
        /// <returns>The return code of the execution</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        #if COMPILER_FLAG_RUNTIME_MODULE
        public int Run<Source>(ref Source args)
        #else
        public override int Run<Source>(ref Source args)
        #endif
        {
            if (IsMainModule)
            {
                MethodInfo? mi = typeof(T).GetMethod(nameof(IMainModule.Run), BindingFlags.Static | BindingFlags.Public);
                mi = mi?.MakeGenericMethod(typeof(Source));

                if(mi != null)
                {
                    return mi.CreateDelegate<RunDelegate<Source>>()(ref args);
                }
            }
            Application.Log(VerbosityFlags.Error, SeverityFlags.None, "Executing a module with no entry point defined");
            return ReturnCode.Error;
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