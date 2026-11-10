// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

namespace Soe.Runtime
{
    /// <summary>
    /// Defines the behavior of a user prompt
    /// </summary>
    [Flags]
    #if EXPORT_HAMPER_CORE_RUNTIME_APPLICATION
    public
    #else
    internal
    #endif
    enum PromptSettings : int
    {
        /// <summary>
        /// The default behavior of the prompt
        /// </summary>
        Default = 0,
        /// <summary>
        /// The prompt returns true by default instead of false
        /// </summary>
        InvertDefault = 0x1,
        /// <summary>
        /// The prompt will show an error and wait for valid input if the provided
        /// input is neither <see cref="PromptLocalisation.TrueKey"/> nor <see cref="PromptLocalisation.FalseKey"/>
        /// </summary>
        ValidateInput = 0x2
    }
}