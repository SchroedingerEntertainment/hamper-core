// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;

namespace Soe.Runtime
{
    /// <summary>
    /// Represents localization settings for a user prompt
    /// </summary>
    #if EXPORT_HAMPER_CORE_RUNTIME_APPLICATION
    public
    #else
    internal
    #endif
    readonly struct PromptLocalisation(string errorMessage, char trueKey, char falseKey)
    {
        private static readonly PromptLocalisation instance;
        /// <summary>
        /// Gets the default localization settings
        /// </summary>
        public static PromptLocalisation Default
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return instance; }
        }

        /// <summary>
        /// Gets the language specific error message
        /// </summary>
        public string ErrorMessage
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return errorMessage; }
        }

        /// <summary>
        /// Gets the localized true key
        /// </summary>
        public char TrueKey
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return trueKey; }
        }

        /// <summary>
        /// Gets the localized false key
        /// </summary>
        public char FalseKey
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return falseKey; }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static PromptLocalisation()
        {
            instance = new PromptLocalisation
            (
                "Please enter either '{0}' or '{1}'",
                'y',
                'n'
            );
        }
    }
}