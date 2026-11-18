// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;

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
        /// Iterates through this container from the first to the last valid element
        /// </summary>
        public struct ReadOnlyIteratorStrategy : IReadOnlyIteratorStrategy<RuntimeModule>
        {
            /// <inheritdoc/>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static int Begin(ReadOnlySpan<RuntimeModule> source)
            {
                return -1;
            }

            /// <inheritdoc/>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool MoveNext(ReadOnlySpan<RuntimeModule> source, ref int index)
            {
                for (index++; index < source.Length; index++)
                {
                    if(source[index].IsValid)
                    {
                        break;
                    }
                }
                return (index < source.Length);
            }
        }
    }
}