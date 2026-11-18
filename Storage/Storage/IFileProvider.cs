// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

namespace Soe.Storage
{
    /// <summary>
    /// Represents a provider instance operating on a single file of the underlying data source
    /// </summary>
    #if EXPORT_HAMPER_CORE_STORAGE
    public
    #else
    internal
    #endif
    interface IFileProvider
    {
        /// <summary>
        /// Copies the provided file to the destination specified
        /// </summary>
        /// <param name="target">The target location of this operation</param>
        /// <param name="destination">The destination to copy the file to</param>
        /// <param name="overwrite">Sets if the file should override any existing files</param>
        void CopyTo(string target, string destination, bool overwrite);
        /// <summary>
        /// Copies the provided file to the destination specified
        /// </summary>
        /// <param name="target">The target location of this operation</param>
        /// <param name="destination">The destination to copy the file to</param>
        void CopyTo(string target, string destination);
        
        /// <summary>
        /// Creates a new file this object is pointing to
        /// </summary>
        /// <param name="target">The target location of this operation</param>
        /// <param name="length">The initial memory in bytes</param>
        /// <returns>A <see cref="Stream"/> object pointing to the file's memory</returns>
        Stream Create(string target, int length);
        
        /// <summary>
        /// Removes the file this object is pointing to
        /// </summary>
        /// <param name="target">The target location of this operation</param>
        void Delete(string target);
        
        /// <summary>
        /// Determines if the provided file exists
        /// </summary>
        /// <param name="target">The target location of this operation</param>
        /// <returns>True if the file exists, false otherwise</returns>
        bool Exists(string target);
        
        /// <summary>
        /// Returns an <see cref="FileMetaData"/> object of this file
        /// </summary>
        /// <param name="target">The target location of this operation</param>
        /// <returns>The <see cref="FileMetaData"/> object providing information about this file</returns>
        FileMetaData GetMetaData(string target);
        
        /// <summary>
        /// Moves the provided file to the destination specified
        /// </summary>
        /// <param name="target">The target location of this operation</param>
        /// <param name="destination">The destination to move this file to</param>
        /// <param name="overwrite">Sets if the file should override any existing files</param>
        void MoveTo(string target, string destination, bool overwrite);
        /// <summary>
        /// Moves the provided file to the destination specified
        /// </summary>
        /// <param name="target">The target location of this operation</param>
        /// <param name="destination">The destination to move this file to</param>
        void MoveTo(string target, string destination);
        
        /// <summary>
        /// Provides access to the file's underlying data managed by the <see cref="IStorageProvider"/>
        /// </summary>
        /// <param name="target">The target location of this operation</param>
        /// <param name="mode">A mode value that specifies whether a file is created if one does not exist,
        /// and determines whether the contents of existing files are retained or overwritten</param>
        /// <param name="access">An access value that specifies the operations that can be performed on the file</param>
        /// <param name="share">A share value specifying the type of access other threads have to the file</param>
        /// <returns>A <see cref="Stream"/> object pointing to the file's memory</returns>
        Stream Open(string target, FileMode mode, FileAccess access, FileShare share);
        /// <summary>
        /// Provides access to the file's underlying data managed by the <see cref="IStorageProvider"/>
        /// </summary>
        /// <param name="target">The target location of this operation</param>
        /// <param name="mode">A mode value that specifies whether a file is created if one does not exist,
        /// and determines whether the contents of existing files are retained or overwritten</param>
        /// <param name="access">An access value that specifies the operations that can be performed on the file</param>
        /// <returns>A <see cref="Stream"/> object pointing to the file's memory</returns>
        Stream Open(string target, FileMode mode, FileAccess access);
    }
}