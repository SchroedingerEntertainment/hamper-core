// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

namespace Soe.Storage
{
    /// <summary>
    /// Represents a provider instance operating on directories of the underlying data source
    /// </summary>
    #if EXPORT_HAMPER_CORE_STORAGE
    public
    #else
    internal
    #endif
    interface IPathProvider
    {
        /// <summary>
        /// Clears the contents of this directory
        /// </summary>
        /// <param name="target">The target location of this operation</param>
        /// <param name="recursive">Defines if the directory should clear all subdirectories in the same turn</param>
        void Clear(string target, bool recursive);
        
        /// <summary>
        /// Creates a new directory this object is pointing to
        /// </summary>
        /// <param name="target">The target location of this operation</param>
        void Create(string target);
        
        /// <summary>
        /// Determines if a directory exists with the provided <paramref name="searchPattern"/>
        /// </summary>
        /// <param name="target">The target location of this operation</param>
        /// <param name="searchPattern">A string representing a pattern to match</param>
        /// <param name="searchOptions">Defines how the underlying <see cref="IStorageProvider{PathDescriptor,FileDescriptor,FileMetaData,TIterator}"/> handles the search</param>
        /// <returns>True if at least one directory matching the pattern exists, false otherwise</returns>
        bool Exists(string target, string searchPattern, SearchOption searchOptions);
        /// <summary>
        /// Determines if the provided directory exists
        /// </summary>
        /// <param name="target">The target location of this operation</param>
        /// <returns>True if the directory exists, false otherwise</returns>
        bool Exists(string target);
        
        /// <summary>
        /// Enumerates the underlying directories providing the <paramref name="searchPattern"/>
        /// </summary>
        /// <param name="target">The target location of this operation</param>
        /// <param name="searchPattern">A string representing a pattern to match</param>
        /// <param name="searchOptions">Defines how the underlying <see cref="IStorageProvider{PathDescriptor,FileDescriptor,FileMetaData,TIterator}"/> handles the search</param>
        /// <returns>An <see cref="StorageIterator"/> instance enumerating the result of this lookup</returns>
        StorageIterator GetDirectories(string target, string searchPattern, SearchOption searchOptions);
        /// <summary>
        /// Enumerates the underlying directories
        /// </summary>
        /// <param name="target">The target location of this operation</param>
        /// <returns>An <see cref="StorageIterator"/> instance enumerating the result of this lookup</returns>
        StorageIterator GetDirectories(string target);

        /// <summary>
        /// Enumerates the underlying files providing the <paramref name="searchPattern"/>
        /// </summary>
        /// <param name="target">The target location of this operation</param>
        /// <param name="searchPattern">A string representing a pattern to match</param>
        /// <param name="searchOptions">Defines how the underlying <see cref="IStorageProvider{PathDescriptor,FileDescriptor,FileMetaData,TIterator}"/> handles the search</param>
        /// <returns>An <see cref="StorageIterator"/> instance enumerating the result of this lookup</returns>
        StorageIterator GetFiles(string target, string searchPattern, SearchOption searchOptions);
        /// <summary>
        /// Enumerates the underlying files
        /// </summary>
        /// <param name="target">The target location of this operation</param>
        /// <returns>An <see cref="StorageIterator"/> instance enumerating the result of this lookup</returns>
        StorageIterator GetFiles(string target);
        
        /// <summary>
        /// Removes the directory this object is pointing to
        /// </summary>
        /// <param name="target">The target location of this operation</param>
        /// <param name="recursive">Defines if the directory should remove all subdirectories in the same turn</param>
        void Remove(string target, bool recursive);
    }
}