// Copyright (C) 2017 Schroedinger Entertainment
// Distributed under the Schroedinger Entertainment EULA (See EULA.md for details)

using System.Runtime.CompilerServices;

namespace Soe.Runtime
{
    /// <summary>
    /// A command option to compare
    /// </summary>
    /// <param name="name">The short- or long-name of this command</param>
    /// <param name="flags"><see cref="OptionFlags"/> controlling the behavior</param>
    /// <param name="description">An optional description provided to this option</param>
    /// <param name="parameter">An optional string parameter reserved for conditional parsing</param>
    [method: MethodImpl(MethodImplOptions.AggressiveInlining)]
    #if EXPORT_HAMPER_CORE_RUNTIME_COMMANDS
    public
    #else
    internal
    #endif
    readonly struct Option(string name, OptionFlags flags, string? description = null, string? parameter = null)
    {
        /// <summary>
        /// Gets the name of this option
        /// </summary>
        public string Name
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return name; }
        }

        /// <summary>
        /// Gets the <see cref="OptionFlags"/> controlling the behavior of this option
        /// </summary>
        public OptionFlags Flags
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return flags; }
        }
        
        /// <summary>
        /// Gets an optional description of this option
        /// </summary>
        public string? Description
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return description; }
        }

        /// <summary>
        /// Gets an optional string parameter reserved for conditional parsing
        /// </summary>
        public string? Parameter
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return parameter; }
        }
    }
}