// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;

namespace Soe.Storage
{
    /// <summary>
    /// Represents a provider instance operating on a single directory of the underlying data source
    /// </summary>
    #if EXPORT_HAMPER_CORE_STORAGE
    public
    #else
    internal
    #endif
    readonly ref struct PathDescriptor
    {
        private readonly ResourceIdentifier identifier;
        private readonly IPathProvider provider;

        /// <summary>
        /// Gets the name of the descriptor object
        /// </summary>
        public ReadOnlySpan<char> Name
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return identifier.Target.Name; }
        }
        
        /// <summary>
        /// Gets the full qualified name of the descriptor object
        /// </summary>
        public ReadOnlySpan<char> FullName
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return identifier.Target.FullName; }
        }

        /// <summary>
        /// Gets the extension of the descriptor object
        /// </summary>
        public ReadOnlySpan<char> Extension
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return identifier.Target.Extension; }
        }
        
        /// <summary>
        /// Gets the parent location of the descriptor object
        /// </summary>
        public ReadOnlySpan<char> Parent
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return identifier.Target.Parent; }
        }
        
        /// <summary>
        /// Gets if the provided directory exists
        /// </summary>
        public bool Exists
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return provider.Exists(identifier); }
        }
        
        /// <summary>
        /// Initializes this descriptor object with the provided target
        /// </summary>
        /// <param name="provider">The <see cref="IFileProvider"/> instance this descriptor belongs to</param>
        /// <param name="identifier">The target location of the descriptor</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="identifier"/> contains location and/or query components</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PathDescriptor(IPathProvider provider, ResourceIdentifier identifier)
        {
            if (!identifier.IsLocation && !identifier.IsQuery)
            {
                this.provider = provider;
                this.identifier = identifier;
            }
            else throw new ArgumentException(nameof(identifier));
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator ResourceIdentifier(PathDescriptor descriptor)
        {
            return descriptor.identifier;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator string(PathDescriptor descriptor)
        {
            return descriptor.identifier;
        }

        /// <summary>
        /// Clears the contents of this directory
        /// </summary>
        /// <param name="recursive">Defines if the directory should clear all subdirectories in the same turn</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Clear(bool recursive)
        {
            provider.Clear(identifier, recursive);
        }

        /// <summary>
        /// Creates a new directory this object is pointing to
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Create()
        {
            provider.Create(identifier);
        }

        /// <summary>
        /// Enumerates the underlying directories providing the <paramref name="searchPattern"/>
        /// </summary>
        /// <param name="searchPattern">A string representing a pattern to match</param>
        /// <param name="searchOptions">Defines how the underlying <see cref="StorageProvider"/> handles the search</param>
        /// <returns>An <see cref="StorageIterator"/> instance enumerating the result of this lookup</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public StorageIterator GetDirectories(string searchPattern, SearchOption searchOptions)
        {
            return provider.GetDirectories(identifier, searchPattern, searchOptions);
        }
        /// <summary>
        /// Enumerates the underlying directories
        /// </summary>
        /// <returns>An <see cref="StorageIterator"/> instance enumerating the result of this lookup</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public StorageIterator GetDirectories()
        {
            return provider.GetDirectories(identifier);
        }

        /// <summary>
        /// Enumerates the underlying files providing the <paramref name="searchPattern"/>
        /// </summary>
        /// <param name="searchPattern">A string representing a pattern to match</param>
        /// <param name="searchOptions">Defines how the underlying <see cref="IStorageProvider"/> handles the search</param>
        /// <returns>An <see cref="StorageIterator"/> instance enumerating the result of this lookup</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public StorageIterator GetFiles(string searchPattern, SearchOption searchOptions)
        {
            return provider.GetFiles(identifier, searchPattern, searchOptions);
        }
        /// <summary>
        /// Enumerates the underlying files
        /// </summary>
        /// <returns>An <see cref="StorageIterator"/> instance enumerating the result of this lookup</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public StorageIterator GetFiles()
        {
            return provider.GetFiles(identifier);
        }

        /// <summary>
        /// Removes the directory this object is pointing to
        /// </summary>
        /// <param name="recursive">Defines if the directory should remove all subdirectories in the same turn</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Remove(bool recursive)
        {
            provider.Remove(identifier, recursive);
        }
    }
}