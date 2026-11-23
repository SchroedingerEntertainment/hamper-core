// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;

namespace Soe.Storage
{
    /// <summary>
    /// Represents the information of a file in the underlying data source
    /// </summary>
    #if EXPORT_HAMPER_CORE_STORAGE
    public
    #else
    internal
    #endif
    readonly ref struct FileMetaData(IFileMetaDataProvider provider, string target)
    {
        /// <summary>
        /// Gets the length in bytes of this file
        /// </summary>
        public long Length
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return provider.GetLength(target); }
        }

        /// <summary>
        /// Gets the UTC timestamp of the file's creation
        /// </summary>
        public DateTime CreationTime
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return provider.GetCreationTime(target); }
        }

        /// <summary>
        /// Gets the UTC timestamp of the file's las access
        /// </summary>
        public DateTime LastAccessTime
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return provider.GetLastAccessTime(target); }
        }

        /// <summary>
        /// Gets the UTC timestamp of the file's last modification
        /// </summary>
        public DateTime LastWriteTime
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return provider.GetLastWriteTime(target); }
        }
    }
}