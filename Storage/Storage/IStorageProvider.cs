// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

namespace Soe.Storage
{
    /// <summary>
    /// Represents a storage provider instance on an underlying data source
    /// </summary>
    #if EXPORT_HAMPER_CORE_STORAGE
    public
    #else
    internal
    #endif
    interface IStorageProvider : IDisposable
    {
        /// <summary>
        /// Gets the root of this storage provider
        /// </summary>
        ResourceIdentifier Root
        {
            get;
        }

        /// <summary>
        /// Gets the corresponding <see cref="IPathProvider"/> instance
        /// </summary>
        IPathProvider Directories
        {
            get;
        }
        
        /// <summary>
        /// Gets the corresponding <see cref="IFileProvider"/> instance
        /// </summary>
        IFileProvider Files
        {
            get;
        }
        
        /// <summary>
        /// Converts the provided target into a valid <see cref="PathDescriptor"/> object
        /// </summary>
        /// <param name="target">The target relative to the <see cref="IStorageProvider.Root"/> location</param>
        /// <returns>The resulting <see cref="PathDescriptor"/> object</returns>
        PathDescriptor GetDirectory(ReadOnlySpanTuple<char, char, char, char, char, char> target);
        /// <summary>
        /// Converts the provided target into a valid <see cref="PathDescriptor"/> object
        /// </summary>
        /// <param name="target">The target relative to the <see cref="IStorageProvider.Root"/> location</param>
        /// <returns>The resulting <see cref="PathDescriptor"/> object</returns>
        PathDescriptor GetDirectory(ReadOnlySpanTuple<char, char, char, char, char> target);
        /// <summary>
        /// Converts the provided target into a valid <see cref="PathDescriptor"/> object
        /// </summary>
        /// <param name="target">The target relative to the <see cref="IStorageProvider.Root"/> location</param>
        /// <returns>The resulting <see cref="PathDescriptor"/> object</returns>
        PathDescriptor GetDirectory(ReadOnlySpanTuple<char, char, char, char> target);
        /// <summary>
        /// Converts the provided target into a valid <see cref="PathDescriptor"/> object
        /// </summary>
        /// <param name="target">The target relative to the <see cref="IStorageProvider.Root"/> location</param>
        /// <returns>The resulting <see cref="PathDescriptor"/> object</returns>
        PathDescriptor GetDirectory(ReadOnlySpanTuple<char, char, char> target);
        /// <summary>
        /// Converts the provided target into a valid <see cref="PathDescriptor"/> object
        /// </summary>
        /// <param name="target">The target relative to the <see cref="IStorageProvider.Root"/> location</param>
        /// <returns>The resulting <see cref="PathDescriptor"/> object</returns>
        PathDescriptor GetDirectory(ReadOnlySpanTuple<char, char> target);
        /// <summary>
        /// Converts the provided target into a valid <see cref="PathDescriptor"/> object
        /// </summary>
        /// <param name="target">The target relative to the <see cref="IStorageProvider.Root"/> location</param>
        /// <returns>The resulting <see cref="PathDescriptor"/> object</returns>
        PathDescriptor GetDirectory(ReadOnlySpan<char> target);
        
        /// <summary>
        /// Converts the provided target into a valid <see cref="FileDescriptor"/> object managed by this provider
        /// </summary>
        /// <param name="target">The target relative to the <see cref="IStorageProvider.Root"/> location</param>
        /// <returns>The resulting <see cref="FileDescriptor"/> object managed by this provider</returns>
        FileDescriptor GetFile(ReadOnlySpanTuple<char, char, char, char, char, char> target);
        /// <summary>
        /// Converts the provided target into a valid <see cref="FileDescriptor"/> object managed by this provider
        /// </summary>
        /// <param name="target">The target relative to the <see cref="IStorageProvider.Root"/> location</param>
        /// <returns>The resulting <see cref="FileDescriptor"/> object managed by this provider</returns>
        FileDescriptor GetFile(ReadOnlySpanTuple<char, char, char, char, char> target);
        /// <summary>
        /// Converts the provided target into a valid <see cref="FileDescriptor"/> object managed by this provider
        /// </summary>
        /// <param name="target">The target relative to the <see cref="IStorageProvider.Root"/> location</param>
        /// <returns>The resulting <see cref="FileDescriptor"/> object managed by this provider</returns>
        FileDescriptor GetFile(ReadOnlySpanTuple<char, char, char, char> target);
        /// <summary>
        /// Converts the provided target into a valid <see cref="FileDescriptor"/> object managed by this provider
        /// </summary>
        /// <param name="target">The target relative to the <see cref="IStorageProvider.Root"/> location</param>
        /// <returns>The resulting <see cref="FileDescriptor"/> object managed by this provider</returns>
        FileDescriptor GetFile(ReadOnlySpanTuple<char, char, char> target);
        /// <summary>
        /// Converts the provided target into a valid <see cref="FileDescriptor"/> object managed by this provider
        /// </summary>
        /// <param name="target">The target relative to the <see cref="IStorageProvider.Root"/> location</param>
        /// <returns>The resulting <see cref="FileDescriptor"/> object managed by this provider</returns>
        FileDescriptor GetFile(ReadOnlySpanTuple<char, char> target);
        /// <summary>
        /// Converts the provided target into a valid <see cref="FileDescriptor"/> object managed by this provider
        /// </summary>
        /// <param name="target">The target relative to the <see cref="IStorageProvider.Root"/> location</param>
        /// <returns>The resulting <see cref="FileDescriptor"/> object managed by this provider</returns>
        FileDescriptor GetFile(ReadOnlySpan<char> target);

        /// <summary>
        /// Gets if the provided target is a valid target, relative to the <see cref="IStorageProvider.Root"/> location
        /// </summary>
        /// <param name="target">The target to validate</param>
        /// <returns>True if the provided target is within the <see cref="IStorageProvider.Root"/> location</returns>
        bool IsValid(ReadOnlySpan<char>  target);
    }
}