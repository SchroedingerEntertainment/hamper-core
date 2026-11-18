// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Reflection;
using System.Runtime.CompilerServices;
using Soe.Collections.HashSet;

namespace Soe.Runtime
{
    /// <summary>
    /// Manages runtime modules within an application
    /// </summary>
    #if EXPORT_HAMPER_CORE_RUNTIME_MODULES
    public
    #else
    internal
    #endif
    static partial class ModuleHost
    {
        private const string ReservedName = "Soe.Generated.Reference";
        
        private static HeapArray<RuntimeModule> modules;
        private static int count;
        /// <summary>
        /// Gets the current number of modules loaded
        /// </summary>
        public static int ModuleCount
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return count; }
        }
        
        private static int version;
        /// <summary>
        /// Gets the current container version
        /// </summary>
        /// <remarks>Version is increased whenever the container layout changes</remarks>
        public static int Version
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return version; }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static ModuleHost()
        {
            AppDomain.CurrentDomain.AssemblyLoad += OnAssemblyLoad;
            AppDomain.CurrentDomain.AssemblyResolve += OnAssemblyResolve;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpan<RuntimeModule> AsReadOnlySpan()
        {
            return modules.AsSpan();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="moduleName"></param>
        /// <param name="module"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Find(string moduleName, out RuntimeModule module)
        {
            if(HashSet.Find<ModuleId, string, HeapArray<RuntimeModule>, RuntimeModule>(ref modules, count, Predicate, moduleName, (int)moduleName.ToLowerInvariant().Fnv32(), out _, out _, out Ref<RuntimeModule> result))
            {
                module = result.Value;
                return true;
            }
            else
            {
                module = default;
                return false;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool Predicate(in RuntimeModule module, in string moduleName)
        {
            return (string.Equals(module.ModuleName, moduleName, StringComparison.InvariantCultureIgnoreCase));
        }

        /// <summary>
        /// Gets a collection of submodules belonging to the provided module
        /// </summary>
        /// <param name="module">A runtime module to get submodules for</param>
        /// <returns>The collection of submodules</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<SubmoduleAttribute> GetSubmodules(in RuntimeModule module)
        {
            string moduleName = module.ModuleName;
            return module.Assembly.GetCustomAttributes<SubmoduleAttribute>()
                .Where(x => string.Equals(x.ParentModuleName, moduleName, StringComparison.InvariantCultureIgnoreCase));
        }

        /// <summary>
        /// 
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Initialize()
        {
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (RuntimeModuleAttribute runtimeAttribute in assembly.GetCustomAttributes<RuntimeModuleAttribute>())
                {
                    ModuleId id = new ModuleId(runtimeAttribute.ModuleName.ToLowerInvariant().Fnv32(), UniqueId.Next32());
                    ref RuntimeModule module = ref HashSet.Emplace<ModuleId, HeapArray<RuntimeModule>, RuntimeModule>(ref modules, ref count, EqualityComparer<ModuleId>.Default, in id, (int)id.HashCode, 0, 0, version - 1, ref version);
                    if (module.Assembly == null)
                    {
                       module = new RuntimeModule(id, assembly, runtimeAttribute);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        static Assembly? OnAssemblyResolve(object? sender, ResolveEventArgs args)
        {
            // Detect if the requested assembly is a generated reference assembly
            if (new AssemblyName(args.Name).Name == ReservedName)
            {
                /*
                 Modules are building against a generated reference assembly rather than embedding the necessary
                 packages into the module directly. The host assembly or one of it's generated capability assemblies
                 will be put into the role of the reference assembly in order to provide the necessary security level
                */
                return Assembly.GetExecutingAssembly();
            }
            else return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="assemblyLoadEventArgs"></param>
        static void OnAssemblyLoad(object? sender, AssemblyLoadEventArgs assemblyLoadEventArgs)
        {
            // Detect if the loaded assembly is a module
            // TODO
            
            /*assemblyLoadEventArgs.LoadedAssembly.GetTypes();
            foreach (RuntimeModuleAttribute runtimeAttribute in assemblyLoadEventArgs.LoadedAssembly.GetCustomAttributes<RuntimeModuleAttribute>())
            {
                ModuleId id = new ModuleId(runtimeAttribute.ModuleName.ToLowerInvariant().Fnv32(), UniqueId.Next32());
                ref RuntimeModule module = ref HashSet.Emplace<ModuleId, HeapArray<RuntimeModule>, RuntimeModule>(ref modules, ref count, EqualityComparer<ModuleId>.Default, in id, (int)id.HashCode, 0, 0, version + 1, ref version);
                if (module.Assembly == null)
                {
                    module = new RuntimeModule(id, assemblyLoadEventArgs.LoadedAssembly, runtimeAttribute);
                }
            }*/
        }
        
        /// <summary>
        /// Returns an iterator that iterates through a collection
        /// </summary>
        /// <returns>The iterator handling the underlying collection</returns>
        /// <remarks>The method must be named GetEnumerator to be recognized by foreach</remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlyIterator<RuntimeModule, ReadOnlyIteratorStrategy> GetEnumerator()
        {
            return new ReadOnlyIterator<RuntimeModule, ReadOnlyIteratorStrategy>(AsReadOnlySpan());
        }
    }
}