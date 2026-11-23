// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

namespace Soe.Storage
{
    /// <summary>
    /// Represents a provider instance getting information of a file in the underlying data source
    /// </summary>
    #if EXPORT_HAMPER_CORE_STORAGE
    public
    #else
    internal
    #endif
    interface IFileMetaDataProvider
    {
        /// <summary>
        /// Gets the length in bytes of this file
        /// </summary>
        /// <param name="target">The target location of this operation</param>
        /// <returns>The length in bytes of this file</returns>
        long GetLength(string target);

        /// <summary>
        /// Gets the UTC timestamp of the file's creation
        /// </summary>
        /// <param name="target">The target location of this operation</param>
        /// <returns>>The UTC timestamp</returns>
        DateTime GetCreationTime(string target);
        
        /// <summary>
        /// Gets the UTC timestamp of the file's las access
        /// </summary>
        /// <param name="target">The target location of this operation</param>
        /// <returns>>The UTC timestamp</returns>
        DateTime GetLastAccessTime(string target);
        
        /// <summary>
        /// Gets the UTC timestamp of the file's last modification
        /// </summary>
        /// <param name="target">The target location of this operation</param>
        /// <returns>>The UTC timestamp</returns>
        DateTime GetLastWriteTime(string target);
    }
}