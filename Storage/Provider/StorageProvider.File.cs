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
        Stream IFileProvider.Create(string target, int length)
        {
            if (IsValid(target))
            {
                return File.Create(target, length);
            }
            else throw new UnauthorizedAccessException();
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void IFileProvider.CopyTo(string target, string destination, bool overwrite)
        {
            if (IsValid(target) && IsValid(destination))
            {
                File.Copy(target, destination, overwrite);
            }
            else throw new UnauthorizedAccessException();
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void IFileProvider.CopyTo(string target, string destination)
        {
            if (IsValid(target) && IsValid(destination))
            {
                File.Copy(target, destination);
            }
            else throw new UnauthorizedAccessException();
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void IFileProvider.Delete(string target)
        {
            if (IsValid(target))
            {
                File.Delete(target);
            }
            else throw new UnauthorizedAccessException();
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        bool IFileProvider.Exists(string target)
        {
            if (IsValid(target))
            {
                return File.Exists(target);
            }
            else throw new UnauthorizedAccessException();
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        FileMetaData IFileProvider.GetMetaData(string target)
        {
            if (IsValid(target))
            {
                return new FileMetaData(this, target);
            }
            else throw new UnauthorizedAccessException();
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void IFileProvider.MoveTo(string target, string destination, bool overwrite)
        {
            if (IsValid(target) && IsValid(destination))
            {
                File.Move(target, destination, overwrite);
            }
            else throw new UnauthorizedAccessException();
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void IFileProvider.MoveTo(string target, string destination)
        {
            if (IsValid(target) && IsValid(destination))
            {
                File.Move(target, destination);
            }
            else throw new UnauthorizedAccessException();
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        Stream IFileProvider.Open(string target, FileMode mode, FileAccess access, FileShare share)
        {
            if (IsValid(target))
            {
                return File.Open(target, mode, access, share);
            }
            else throw new UnauthorizedAccessException();
        }
        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        Stream IFileProvider.Open(string target, FileMode mode, FileAccess access)
        {
            if (IsValid(target))
            {
                return File.Open(target, mode, access);
            }
            else throw new UnauthorizedAccessException();
        }
    }
}