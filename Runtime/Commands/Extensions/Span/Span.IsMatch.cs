// Copyright (C) 2017 Schroedinger Entertainment
// Distributed under the Schroedinger Entertainment EULA (See EULA.md for details)

using System.Text;

namespace Soe.Runtime
{
    #if EXPORT_HAMPER_CORE_RUNTIME_COMMANDS
    public
    #else
    internal
    #endif
    static class SpanExtension
    {
        // ReSharper disable InvalidXmlDocComment
        
        /// <summary>
        /// Attempts to return the element with the provided name in this collection
        /// </summary>
        /// <param name="name">The name of an element in this collection</param>
        /// <param name="result">The command matching the provided name</param>
        /// <returns>True if an element with the provided name was found, false otherwise</returns>
        public static bool TryFindOption<Source>(this ReadOnlySpan<Option> options, TextSource<Source> name, out Option result)
            where Source : ITextSource
        {
            if(options.Length > 0)
            {
                int lo = 0;
                int hi = options.Length;
                int length = name.Length;
                char first = name[0];

                // Try finding the start of the window via binary search
                while (lo < hi)
                {
                    int mid = (lo + ((hi - lo) >> 1));
                    if (options[mid].Name[0] < first)
                    {
                        lo = mid + 1;
                    }
                    else if (options[mid].Name.Length == length && name.Equals(options[mid].Name))
                    {
                        // Early out

                        result = options[mid];
                        return true;
                    }
                    else hi = mid;
                }

                hi = options.Length;
                int start = lo;

                // Try finding the end of the window via binary search
                while (lo < hi)
                {
                    int mid = (lo + ((hi - lo) >> 1));
                    if (options[mid].Name[0] <= first)
                    {
                        lo = mid + 1;
                    }
                    else if (options[mid].Name.Length == length && name.Equals(options[mid].Name))
                    {
                        // Early out
                        
                        result = options[mid];
                        return true;
                    }
                    else hi = mid;
                }

                // Try finding the matching element from the given window
                for (int i = start; i < lo; i++)
                {
                    if (options[i].Name.Length == length && name.Equals(options[i].Name))
                    {
                        result = options[i];
                        return true;
                    }
                }
            }
            result = default;
            return false;
        }
        
        // ReSharper restore InvalidXmlDocComment
    }
}