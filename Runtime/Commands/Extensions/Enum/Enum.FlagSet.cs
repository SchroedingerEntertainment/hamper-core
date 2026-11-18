// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;

namespace Soe.Runtime
{
    #if EXPORT_HAMPER_CORE_RUNTIME_COMMANDS
    public
    #else
    internal
    #endif
    static partial class EnumExtension
    {
        // ReSharper disable InvalidXmlDocComment
        
        /// <summary>
        /// Tests if the provided flag bits are set in this enum
        /// </summary>
        /// <param name="flags">The flag bits to test</param>
        /// <returns>True if the flag bits are present in this value, false otherwise</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool FlagSet(this CommandReaderOptions value, CommandReaderOptions flags)
        {
            return ((value & flags) == flags);
        }
        
        /// <summary>
        /// Tests if the provided flag bits are set in this enum
        /// </summary>
        /// <param name="flags">The flag bits to test</param>
        /// <returns>True if the flag bits are present in this value, false otherwise</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool FlagSet(this Option value, OptionFlags flags)
        {
            return ((value.Flags & flags) == flags);
        }
        
        // ReSharper restore InvalidXmlDocComment
    }
}