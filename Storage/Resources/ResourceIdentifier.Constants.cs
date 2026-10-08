// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

namespace Soe.Storage
{
    #if EXPORT_HAMPER_CORE_STORAGE
    public
    #else
    internal
    #endif
    readonly partial struct ResourceIdentifier
    {
        /// <summary>
        /// A unified resource identifier pointing to the root node
        /// </summary>
        public static readonly ResourceIdentifier Root = new ResourceIdentifier("/", new ResourceMetaData(IdentifierFlags.Default, 0, 0, 1, 1, 1));

        /// <summary>
        /// A unified resource identifier pointing to the current path or location
        /// </summary>
        public static readonly ResourceIdentifier Current = new ResourceIdentifier("./", new ResourceMetaData(IdentifierFlags.IsRelative, 0, 0, 2, 2, 2));

        /// <summary>
        /// Determines the available character used when parsing
        /// </summary>
        public static class Character
        {
            /// <summary>
            /// Provides the character used to separate the drive identifier string in a unified resource identifier
            /// </summary>
            public const char DriveDelimiter = ':';

            /// <summary>
            /// Provides the character used to separate unified resource identifier levels
            /// </summary>
            public const char PathSeparator = '/';

            /// <summary>
            /// Provides an accepted character used to separate unified resource identifier levels. This
            /// character is replaced on validation
            /// </summary>
            public const char PathSeparatorAlternative = '\\';

            /// <summary>
            /// Provides the character used to perform implicit unified resource identifier shifting
            /// </summary>
            public const char PathShiftDelimiter = '.';

            /// <summary>
            /// Provides the character used to separate unified resource identifier extensions
            /// </summary>
            public const char FileExtensionSeparator = '.';

            /// <summary>
            /// Provides the character used to tag unified resource identifier optional arguments
            /// </summary>
            public const char ArgumentMarker = '?';

            /// <summary>
            /// Provides the character used to separate unified resource identifier optional arguments from
            /// their respective values
            /// </summary>
            public const char ArgumentValueMarker = '=';

            /// <summary>
            /// Provides the character used to separate unified resource identifier optional arguments
            /// </summary>
            public const char ArgumentSeparator = '&';
        }
    }
}