// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.IO;
using System.Runtime.CompilerServices;
using Soe.Collections.HashSet;

namespace Soe.Storage
{
    /// <summary>
    /// Stores volume information from the underlying operating system
    /// </summary>
    #if EXPORT_HAMPER_CORE_STORAGE
    public
    #else
    internal
    #endif
    static partial class VolumeManager
    {
        private static HashSet<string, HeapArray<VolumeInfo>, VolumeInfo> volumes;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static VolumeManager()
        {
            volumes = new HashSet<string, HeapArray<VolumeInfo>, VolumeInfo>(StringComparer.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// Gets the collection of stored volumes
        /// </summary>
        /// <returns>An in-memory representation of the sequence</returns>
        public static ReadOnlySpan<VolumeInfo> AsReadOnlySpan()
        {
            return volumes.AsSpan();
        }
        
        /// <summary>
        /// Updates stored volume information from the current state of the underlying operating system
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Initialize()
        {
            volumes.Clear();
            if (OperatingSystem.IsLinux())
            {
                InitializeLinux();
            }
            else if (OperatingSystem.IsWindows())
            {
                InitializeWindows();
            }
        }

        static void InitializeLinux()
        {
            const string mountFile = "/proc/self/mounts";
            if (File.Exists(mountFile))
            {
                foreach (string line in File.ReadLines(mountFile))
                {
                    int mount = FindWhitespace(line, 0) + 1;
                    int fs = FindWhitespace(line, mount) + 1;

                    if (fs >= 0 && (fs - mount) - 1 > 0 && !line.AsSpan().StartsWith(line.AsSpan(fs, Math.Max(FindWhitespace(line, fs) - fs, 0))))
                    {
                        ReadOnlySpan<char> mountPoint = line.AsSpan(mount, (fs - mount) - 1);
                        
                        if ((mountPoint.Length == 1 || !line.AsSpan().StartsWith(mountPoint)) && Path.IsPathFullyQualified(mountPoint))
                        {
                            ReadOnlySpan<char> name = (mountPoint.Length == 1 ? "root" : mountPoint.Slice(mountPoint.LastIndexOf(Path.DirectorySeparatorChar) + 1));
                            ReadOnlySpan<char> volumeName = (line.StartsWith("/dev/") ? line.AsSpan(5, mount - 6) : ReadOnlySpan<char>.Empty);

                            if (!volumeName.IsEmpty)
                            {
                                if(mountPoint.Length > 1)
                                {
                                    foreach (VolumeInfo volume in volumes)
                                    {
                                        ReadOnlySpan<char> path = volume.Location.Target.FullName;
                                        if (path.Length > 1 && mountPoint.StartsWith(path))
                                        {
                                            // Skip mounts that are part of another mount
                                            goto End;
                                        }
                                        else if (path.Length > 1 && path.StartsWith(mountPoint))
                                        {
                                            // Remove mounts that are part of another mount
                                            volumes.Remove(volume.Key, volume.Hash);
                                            break;
                                        }
                                    }
                                }
                                Register(name.ToString(), new ResourceIdentifier(mountPoint.ToString(), ResourceIdentifier.Parse(mountPoint)), volumeName.ToString());
                            }
                        }
                    }
                End:;
                }
            }
        }
        static void InitializeWindows()
        {
            foreach (DriveInfo drive in DriveInfo.GetDrives())
            {
                try
                {
                    string identifier = drive.RootDirectory.FullName.TrimEnd(':', '\\', '/');
                    if (!string.IsNullOrWhiteSpace(identifier))
                    {
                        Register(identifier, new ResourceIdentifier(drive.RootDirectory.FullName, ResourceIdentifier.Parse(drive.RootDirectory.FullName)), null);
                    }
                }
                catch (IOException)
                { }
                catch (UnauthorizedAccessException)
                { }
            }
        }

        static int FindWhitespace(string str, int index)
        {
            for (int length = str.Length; index < length; index++)
            {
                if (Char.IsWhiteSpace(str[index]))
                {
                    return index;
                }
            }
            return 0;
        }

        static string Register(string identifier, ResourceIdentifier location, string? volumeName)
        {
            if(ValidateString(identifier))
            {
                int hash = identifier.GetHashCode();
                if(volumes.Find(identifier, hash, out int index, out int distance, out _))
                {
                    identifier = (!string.IsNullOrWhiteSpace(volumeName) ? string.Concat(volumeName, ">", identifier) : identifier);
                    for (int suffix = 0; ;suffix++)
                    {
                        string key = (suffix == 0 ? identifier : string.Concat(identifier, "~", suffix));
                        hash = identifier.GetHashCode();
                        if(!volumes.Find(identifier, hash, out index, out distance, out _))
                        {
                            identifier = key;
                            break;
                        }
                    }
                }
                volumes.Emplace(identifier, hash, index, distance, volumes.Version) = new VolumeInfo(identifier, location);
                return identifier;
            }
            else throw new ArgumentOutOfRangeException(nameof(identifier));
        }
        /// <summary>
        /// Adds a new volume information to the manager
        /// </summary>
        /// <param name="identifier">An identifier the volume is associated with</param>
        /// <param name="location">A unified resource identifier associated with the volume</param>
        /// <returns>The resulting <paramref name="identifier"/> the volume is associated with</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="identifier"/> is null or empty</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string Register(string identifier, ResourceIdentifier location)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(identifier);
            return Register(identifier, location, null);
        }

        /// <summary>
        /// Attempts to get the volume associated with <paramref name="identifier"/>
        /// </summary>
        /// <param name="identifier">An identifier the volume is associated with</param>
        /// <param name="location">The unified resource identifier associated with the volume</param>
        /// <returns>True if a volume is associated with <paramref name="identifier"/>, false otherwise</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryGetVolume(string identifier, out ResourceIdentifier location)
        {
            if (volumes.Find(identifier, identifier.GetHashCode(), out _, out _, out Ref<VolumeInfo> volume))
            {
                location = volume.Value.Location;
                return true;
            }
            else
            {
                location = default;
                return false;
            }
        }
        
        static bool ValidateString(string str)
        {
            for (int index = 0, length = str.Length; index < length; index++)
            {
                if (!Char.IsAsciiLetterOrDigit(str[index]))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Returns an iterator that iterates through the collection of volumes
        /// </summary>
        /// <returns>The iterator handling the underlying collection</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlyIterator<VolumeInfo, HashSet.ReadOnlyIteratorStrategy<string, VolumeInfo>> GetEnumerator()
        {
            return new ReadOnlyIterator<VolumeInfo, HashSet.ReadOnlyIteratorStrategy<string, VolumeInfo>>(volumes.AsSpan());
        }
    }
}