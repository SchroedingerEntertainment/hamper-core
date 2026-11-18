// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;

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
    readonly ref struct FileDescriptor
    {
        private readonly ResourceIdentifier identifier;
        private readonly IFileProvider provider;

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
        /// Gets the parent location of the descriptor object
        /// </summary>
        public ReadOnlySpan<char> Parent
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return identifier.Target.Parent; }
        }
        
        /// <summary>
        /// Gets a <see cref="FileMetaData"/> object of this file
        /// </summary>
        public FileMetaData MetaData
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return provider.GetMetaData(identifier); }
        }

        /// <summary>
        /// Gets if the provided file exists
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
        public FileDescriptor(IFileProvider provider, ResourceIdentifier identifier)
        {
            if (!identifier.IsLocation && !identifier.IsQuery)
            {
                this.provider = provider;
                this.identifier = identifier;
            }
            else throw new ArgumentException(nameof(identifier));
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator ResourceIdentifier(FileDescriptor descriptor)
        {
            return descriptor.identifier;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator string(FileDescriptor descriptor)
        {
            return descriptor.identifier;
        }
        
        /// <summary>
        /// Copies the provided file to the destination specified
        /// </summary>
        /// <param name="destination">The destination to copy the file to</param>
        /// <param name="overwrite">Sets if the file should override any existing files</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void CopyTo(string destination, bool overwrite)
        {
            provider.CopyTo(identifier, destination, overwrite);
        }
        /// <summary>
        /// Copies the provided file to the destination specified
        /// </summary>
        /// <param name="destination">The destination to copy the file to</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void CopyTo(string destination)
        {
            provider.CopyTo(identifier, destination);
        }

        /// <summary>
        /// Creates a new file this object is pointing to
        /// </summary>
        /// <param name="length">The initial memory in bytes</param>
        /// <returns>A <see cref="Stream"/> object pointing to the file's memory</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        Stream Create(int length)
        {
            return provider.Create(identifier, length);
        }

        /// <summary>
        /// Removes the file this object is pointing to
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void Delete()
        {
            provider.Delete(identifier);
        }

        /// <summary>
        /// Moves the provided file to the destination specified
        /// </summary>
        /// <param name="destination">The destination to move this file to</param>
        /// <param name="overwrite">Sets if the file should override any existing files</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void MoveTo(string destination, bool overwrite)
        {
            provider.MoveTo(identifier, destination, overwrite);
        }
        /// <summary>
        /// Moves the provided file to the destination specified
        /// </summary>
        /// <param name="destination">The destination to move this file to</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void MoveTo(string destination)
        {
            provider.MoveTo(identifier, destination);
        }

        /// <summary>
        /// Provides access to the file's underlying data managed by the <see cref="IStorageProvider"/>
        /// </summary>
        /// <param name="mode">A mode value that specifies whether a file is created if one does not exist,
        /// and determines whether the contents of existing files are retained or overwritten</param>
        /// <param name="access">An access value that specifies the operations that can be performed on the file</param>
        /// <param name="share">A share value specifying the type of access other threads have to the file</param>
        /// <returns>A <see cref="Stream"/> object pointing to the file's memory</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        Stream Open(FileMode mode, FileAccess access, FileShare share)
        {
            return provider.Open(identifier, mode, access, share);
        }
        /// <summary>
        /// Provides access to the file's underlying data managed by the <see cref="IStorageProvider"/>
        /// </summary>
        /// <param name="mode">A mode value that specifies whether a file is created if one does not exist,
        /// and determines whether the contents of existing files are retained or overwritten</param>
        /// <param name="access">An access value that specifies the operations that can be performed on the file</param>
        /// <returns>A <see cref="Stream"/> object pointing to the file's memory</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        Stream Open(FileMode mode, FileAccess access)
        {
            return provider.Open(identifier, mode, access);
        }
    }
}