// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;

namespace Soe.Runtime
{
    #if EXPORT_HAMPER_CORE_RUNTIME_APPLICATION
    public
    #else
    internal
    #endif
    static partial class Application
    {
        /// <summary>
        /// Gets the root target of the provided workspace
        /// </summary>
        /// <param name="type">The workspace to determine the root for</param>
        /// <returns>A string pointing to the target root</returns>
        public static string? FindWorkspaceRoot(WorkspaceType type)
        {
            ReadOnlySpan<char> source = (type == WorkspaceType.Root ? BaseDirectory : LocalDirectory);
            Span<char> systemSeparator = stackalloc char[1];
            systemSeparator[0] = Path.DirectorySeparatorChar;

            if (source[source.Length - 1] == '/' || source[source.Length - 1] == '\\')
            {
                source = source.Slice(0, source.Length - 1);
            }
            while (!source.IsEmpty)
            {
                if (Directory.Exists(string.Concat(source, systemSeparator, "Config")))
                {
                    return source.ToString();
                }
                else source = GetParent(source);
            }
            return null;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static ReadOnlySpan<char> GetParent(ReadOnlySpan<char> source)
        {
            for (int i = source.Length - 1; i >= 0; i--)
            {
                if (source[i] == '/' || source[i] == '\\')
                {
                    return source.Slice(0, i);
                }
            }
            return ReadOnlySpan<char>.Empty;
        }
    }
}