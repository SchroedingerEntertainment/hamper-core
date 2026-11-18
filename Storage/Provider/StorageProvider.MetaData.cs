// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.IO;
using System.Runtime.CompilerServices;

namespace Soe.Storage
{
    #if EXPORT_HAMPER_CORE_STORAGE
    public
    #else
    internal
    #endif
    sealed partial class StorageProvider
    {
        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        long IFileMetaDataProvider.GetLength(string target)
        {
            if (IsValid(target))
            {
                return new FileInfo(target).Length;
            }
            else throw new UnauthorizedAccessException();
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        DateTime IFileMetaDataProvider.GetCreationTime(string target)
        {
            if (IsValid(target))
            {
                return File.GetCreationTimeUtc(target);
            }
            else throw new UnauthorizedAccessException();
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        DateTime IFileMetaDataProvider.GetLastAccessTime(string target)
        {
            if (IsValid(target))
            {
                return File.GetLastAccessTimeUtc(target);
            }
            else throw new UnauthorizedAccessException();
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        DateTime IFileMetaDataProvider.GetLastWriteTime(string target)
        {
            if (IsValid(target))
            {
                return File.GetLastWriteTimeUtc(target);
            }
            else throw new UnauthorizedAccessException();
        }
    }
}