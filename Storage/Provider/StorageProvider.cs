// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;

namespace Soe.Storage
{
    /// <summary>
    /// Represents a storage provider over the default filesystem
    /// </summary>
    #if EXPORT_HAMPER_CORE_STORAGE
    public
    #else
    internal
    #endif
    sealed partial class StorageProvider : IStorageProvider, IPathProvider, IFileProvider, IContextHandler, IFileMetaDataProvider
    {
        private static readonly StorageProvider instance;
        /// <summary>
        /// Gets the default instance of this storage provider
        /// </summary>
        public static StorageProvider Default
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return instance; }
        }

        private readonly ResourceIdentifier root;
        /// <inheritdoc/>
        public ResourceIdentifier Root
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return root; }
        }

        /// <inheritdoc/>
        public IPathProvider Directories
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get{ return this; }
        }

        /// <inheritdoc/>
        public IFileProvider Files
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get{ return this; }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static StorageProvider()
        {
            instance = new StorageProvider(ResourceIdentifier.Root);
        }
        /// <summary>
        /// Initializes this provider to the provided <see cref="ResourceIdentifier"/> location
        /// </summary>
        /// <param name="root">The <see cref="ResourceIdentifier"/> pointing to the root location in the default filesystem</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public StorageProvider(ResourceIdentifier root)
        {
            this.root = root;
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Dispose()
        { }
        
        #region Directory
        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PathDescriptor GetDirectory(ReadOnlySpanTuple<char, char, char, char, char, char> target)
        {
            string str = ResourceIdentifier.Normalize(ReadOnlySpanTuple.Create(root.Target.FullName, target), NormalizationFlags.SystemSeparatorChar | NormalizationFlags.AbsolutePath);
            return new PathDescriptor(this, new ResourceIdentifier(str, ResourceIdentifier.ParseFast(str)));
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PathDescriptor GetDirectory(ReadOnlySpanTuple<char, char, char, char, char> target)
        {
            string str = ResourceIdentifier.Normalize(ReadOnlySpanTuple.Create(root.Target.FullName, target), NormalizationFlags.SystemSeparatorChar | NormalizationFlags.AbsolutePath);
            return new PathDescriptor(this, new ResourceIdentifier(str, ResourceIdentifier.ParseFast(str)));
        }
        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PathDescriptor GetDirectory(ReadOnlySpanTuple<char, char, char, char> target)
        {
            string str = ResourceIdentifier.Normalize(ReadOnlySpanTuple.Create(root.Target.FullName, target), NormalizationFlags.SystemSeparatorChar | NormalizationFlags.AbsolutePath);
            return new PathDescriptor(this, new ResourceIdentifier(str, ResourceIdentifier.ParseFast(str)));
        }
        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PathDescriptor GetDirectory(ReadOnlySpanTuple<char, char, char> target)
        {
            string str = ResourceIdentifier.Normalize(ReadOnlySpanTuple.Create(root.Target.FullName, target), NormalizationFlags.SystemSeparatorChar | NormalizationFlags.AbsolutePath);
            return new PathDescriptor(this, new ResourceIdentifier(str, ResourceIdentifier.ParseFast(str)));
        }
        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PathDescriptor GetDirectory(ReadOnlySpanTuple<char, char> target)
        {
            string str = ResourceIdentifier.Normalize(ReadOnlySpanTuple.Create(root.Target.FullName, target), NormalizationFlags.SystemSeparatorChar | NormalizationFlags.AbsolutePath);
            return new PathDescriptor(this, new ResourceIdentifier(str, ResourceIdentifier.ParseFast(str)));
        }
        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PathDescriptor GetDirectory(ReadOnlySpan<char> target)
        {
            string str = ResourceIdentifier.Normalize(ReadOnlySpanTuple.Create(root.Target.FullName, target), NormalizationFlags.SystemSeparatorChar | NormalizationFlags.AbsolutePath);
            return new PathDescriptor(this, new ResourceIdentifier(str, ResourceIdentifier.ParseFast(str)));
        }
        #endregion
        
        #region File
        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public FileDescriptor GetFile(ReadOnlySpanTuple<char, char, char, char, char, char> target)
        {
            string str = ResourceIdentifier.Normalize(ReadOnlySpanTuple.Create(root.Target.FullName, target), NormalizationFlags.SystemSeparatorChar | NormalizationFlags.AbsolutePath);
            return new FileDescriptor(this, new ResourceIdentifier(str, ResourceIdentifier.ParseFast(str)));
        }
        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public FileDescriptor GetFile(ReadOnlySpanTuple<char, char, char, char, char> target)
        {
            string str = ResourceIdentifier.Normalize(ReadOnlySpanTuple.Create(root.Target.FullName, target), NormalizationFlags.SystemSeparatorChar | NormalizationFlags.AbsolutePath);
            return new FileDescriptor(this, new ResourceIdentifier(str, ResourceIdentifier.ParseFast(str)));
        }
        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public FileDescriptor GetFile(ReadOnlySpanTuple<char, char, char, char> target)
        {
            string str = ResourceIdentifier.Normalize(ReadOnlySpanTuple.Create(root.Target.FullName, target), NormalizationFlags.SystemSeparatorChar | NormalizationFlags.AbsolutePath);
            return new FileDescriptor(this, new ResourceIdentifier(str, ResourceIdentifier.ParseFast(str)));
        }
        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public FileDescriptor GetFile(ReadOnlySpanTuple<char, char, char> target)
        {
            string str = ResourceIdentifier.Normalize(ReadOnlySpanTuple.Create(root.Target.FullName, target), NormalizationFlags.SystemSeparatorChar | NormalizationFlags.AbsolutePath);
            return new FileDescriptor(this, new ResourceIdentifier(str, ResourceIdentifier.ParseFast(str)));
        }
        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public FileDescriptor GetFile(ReadOnlySpanTuple<char, char> target)
        {
            string str = ResourceIdentifier.Normalize(ReadOnlySpanTuple.Create(root.Target.FullName, target), NormalizationFlags.SystemSeparatorChar | NormalizationFlags.AbsolutePath);
            return new FileDescriptor(this, new ResourceIdentifier(str, ResourceIdentifier.ParseFast(str)));
        }
        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public FileDescriptor GetFile(ReadOnlySpan<char> target)
        {
            string str = ResourceIdentifier.Normalize(ReadOnlySpanTuple.Create(root.Target.FullName, target), NormalizationFlags.SystemSeparatorChar | NormalizationFlags.AbsolutePath);
            return new FileDescriptor(this, new ResourceIdentifier(str, ResourceIdentifier.ParseFast(str)));
        }
        #endregion
        
        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsValid(ReadOnlySpan<char> target)
        {
            return target.StartsWith(root.Target.FullName, StringComparison.OrdinalIgnoreCase);
        }
    }
}