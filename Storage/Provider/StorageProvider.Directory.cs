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
        void IPathProvider.Clear(string target, bool recursive)
        {
            if (IsValid(target))
            {
                foreach (string file in Directory.GetFiles(target))
                {
                    File.Delete(file);
                }

                if (recursive)
                {
                    foreach (string directory in Directory.GetDirectories(target))
                    {
                        Directory.Delete(directory, true);
                    }
                }
            }
            else throw new UnauthorizedAccessException();
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void IPathProvider.Create(string target)
        {
            if (IsValid(target))
            {
                Directory.CreateDirectory(target);
            }
            else throw new UnauthorizedAccessException();
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        bool IPathProvider.Exists(string target, string searchPattern, SearchOption searchOptions)
        {
            return Directories.GetDirectories(target, searchPattern, searchOptions).MoveNext();
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        bool IPathProvider.Exists(string target)
        {
            if (IsValid(target))
            {
                return Directory.Exists(target);
            }
            else throw new UnauthorizedAccessException();
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        StorageIterator IPathProvider.GetDirectories(string target, string searchPattern, SearchOption searchOptions)
        {
            if (IsValid(target))
            {
                return new StorageIterator
                (
                    this,
                    Directory.EnumerateDirectories
                    (
                        target,
                        searchPattern,
                        searchOptions

                    ).GetEnumerator()
                );
            }
            else throw new UnauthorizedAccessException();
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        StorageIterator IPathProvider.GetDirectories(string target)
        {
            if (IsValid(target))
            {
                return new StorageIterator
                (
                    this,
                    Directory.EnumerateDirectories
                    (
                        target

                    ).GetEnumerator()
                );
            }
            else throw new UnauthorizedAccessException();
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        StorageIterator IPathProvider.GetFiles(string target, string searchPattern, SearchOption searchOptions)
        {
            if (IsValid(target))
            {
                return new StorageIterator
                (
                    this,
                    Directory.EnumerateFiles
                    (
                        target,
                        searchPattern,
                        searchOptions

                    ).GetEnumerator()
                );
            }
            else throw new UnauthorizedAccessException();
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        StorageIterator IPathProvider.GetFiles(string target)
        {
            if (IsValid(target))
            {
                return new StorageIterator
                (
                    this,
                    Directory.EnumerateFiles
                    (
                        target

                    ).GetEnumerator()
                );
            }
            else throw new UnauthorizedAccessException();
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void IPathProvider.Remove(string target, bool recursive)
        {
            if (IsValid(target))
            {
                Directory.Delete(target, recursive);
            }
            else throw new UnauthorizedAccessException();
        }
    }
}