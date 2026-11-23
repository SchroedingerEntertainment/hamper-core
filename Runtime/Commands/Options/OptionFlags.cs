// Copyright (C) 2017 Schroedinger Entertainment
// Distributed under the Schroedinger Entertainment EULA (See EULA.md for details)

namespace Soe.Runtime
{
    /// <summary>
    /// Controls the processing of command options
    /// </summary>
    [Flags]
    #if EXPORT_HAMPER_CORE_RUNTIME_COMMANDS
    public
    #else
    internal
    #endif
    enum OptionFlags : int
    {
        Invalid = 0,
        /// <summary>
        /// The option is mandatory
        /// </summary>
        Required = 0x1,
        /// <summary>
        /// The option is optional
        /// </summary>
        Optional = 0x2,
        /// <summary>
        /// The option is a flag and will not consume a <see cref="CommandToken.ArgumentValue"/>
        /// </summary>
        Flag = 0x4,
        /// <summary>
        /// The option requires a parameter, <see cref="Option.Parameter"/> will be used as default
        /// </summary>
        RequiresParameter = 0x8,
        /// <summary>
        /// The option is an alias for the option defined in <see cref="Option.Parameter"/>
        /// </summary>
        Alias = 0x10,
        /// <summary>
        /// The option is a verb value. This flag will not have any effect while parsing
        /// </summary>
        Verb = 0x20,
        /// <summary>
        /// The option won't be displayed in the header. This flag will not have any effect while parsing
        /// </summary>
        NoHeader = 0x40
    }
}