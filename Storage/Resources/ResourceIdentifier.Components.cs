// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;

namespace Soe.Storage
{
    #if EXPORT_HAMPER_CORE_STORAGE
    public
    #else
    internal
    #endif
    readonly partial struct ResourceIdentifier
    {
        /// <summary>
        /// The location component of a unified resource identifier
        /// </summary>
        public readonly ref partial struct LocationComponent(ReadOnlySpan<char> identifier, ResourceMetaData meta)
        {
            private readonly ReadOnlySpan<char> identifier = identifier;
            private readonly ResourceMetaData meta = meta;
            
            /// <summary>
            /// Gets if this unified resource identifier has an authority component set. This is true
            /// for network paths or remote locations
            /// </summary>
            public bool IsUnc
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get { return meta.Flags.FlagSet(IdentifierFlags.IsUnc); }
            }
        
            /// <summary>
            /// Gets if this unified resource identifier has an authority component set. This is true
            /// for network paths or remote locations
            /// </summary>
            public bool IsUri
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get { return meta.Flags.FlagSet(IdentifierFlags.IsUri); }
            }
            
            /// <summary>
            /// Gets the scheme of the authority handling the location this unified resource identifier points to
            /// </summary>
            public ReadOnlySpan<char> Scheme
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get
                {
                    if(!IsUnc)
                    {
                        ReadOnlySpan<char> scheme = identifier.Slice(0, meta.Authority);
                        if (IsUri)
                        {
                            return scheme.Slice(0, scheme.Length - 3);
                        }
                        else return scheme;
                    }
                    else return ReadOnlySpan<char>.Empty;
                }
            }
            
            /// <summary>
            /// Gets the name of an authority handling the location this unified resource identifier points to
            /// </summary>
            public ReadOnlySpan<char> Authority
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get
                {
                    ReadOnlySpan<char> authority = identifier.Slice(meta.Authority, meta.Path - meta.Authority);
                    if (!authority.IsEmpty && authority[authority.Length - 1] == Character.DriveDelimiter)
                    {
                        return authority.Slice(0, authority.Length - 1);
                    }
                    else return authority;
                }
            }
        }
        
        /// <summary>
        /// The target component of a unified resource identifier
        /// </summary>
        public readonly ref partial struct TargetComponent(ReadOnlySpan<char> identifier, ResourceMetaData meta)
        {
            private readonly ReadOnlySpan<char> identifier = identifier;
            private readonly ResourceMetaData meta = meta;
            
            /// <summary>
            /// Gets if the target is absolute
            /// </summary>
            public bool IsAbsolute
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get { return !IsRelative; }
            }

            /// <summary>
            /// Gets if the target is relative to a certain root
            /// </summary>
            public bool IsRelative
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get { return meta.Flags.FlagSet(IdentifierFlags.IsRelative); }
            }

            /// <summary>
            /// Gets if the first segment points to the root node
            /// </summary>
            public bool IsRooted
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get
                {
                    ReadOnlySpan<char> path = Path;
                    return (path.Length > 0 && (path[0] == Character.PathSeparator || path[0] == Character.PathSeparatorAlternative));
                }
            }
        
            /// <summary>
            /// Gets the full qualified path or location the target points to
            /// </summary>
            public ReadOnlySpan<char> Path
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get
                {
                    ReadOnlySpan<char> path = identifier.Slice(meta.Path, meta.Name - meta.Path);
                    if (CanTrim(path))
                    {
                        return path.Slice(0, path.Length - 1);
                    }
                    else return path;
                }
            }
            
            /// <summary>
            /// Gets the name of the target resource identifier
            /// </summary>
            public ReadOnlySpan<char> Name
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get { return identifier.Slice(meta.Name, meta.Arguments - meta.Name); }
            }
        
            /// <summary>
            /// Gets the full qualified location the target points to
            /// </summary>
            public ReadOnlySpan<char> FullName
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get
                {
                    ReadOnlySpan<char> path = identifier.Slice(meta.Path, meta.Arguments - meta.Path);
                    if (CanTrim(path))
                    {
                        return path.Slice(0, path.Length - 1);
                    }
                    else return path;
                }
            }
            
            /// <summary>
            /// Gets the extension of the resource identifier
            /// </summary>
            public ReadOnlySpan<char> Extension
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get
                {
                    ReadOnlySpan<char> name = Name;
                    return (meta.Extension < name.Length ? Name.Slice(meta.Extension + 1) : ReadOnlySpan<char>.Empty);
                }
            }

            static bool CanTrim(ReadOnlySpan<char> path)
            {
                if (path.Length > 1 && (path[path.Length - 1] == Character.PathSeparator || path[path.Length - 1] == Character.PathSeparatorAlternative))
                {
                    if (path.EndsWith("../") || path.EndsWith("..\\"))
                    {
                        if (path.Length > 3 && path[path.Length - 4] != Character.PathSeparator && path[path.Length - 4] != Character.PathSeparatorAlternative)
                        {
                            return true;
                        }
                    }
                    else if (path.EndsWith("./") || path.EndsWith(".\\"))
                    {
                        if (path.Length > 2 && path[path.Length - 3] != Character.PathSeparator && path[path.Length - 3] != Character.PathSeparatorAlternative)
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
        }
        
        /// <summary>
        /// The query component of a unified resource identifier
        /// </summary>
        public readonly ref partial struct QueryComponent(ReadOnlySpan<char> identifier, ResourceMetaData meta)
        {
            private readonly ReadOnlySpan<char> identifier = identifier;
            private readonly ResourceMetaData meta = meta;
            
            ReadOnlySpan<char> Args
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get { return identifier.Slice(meta.Arguments < identifier.Length ? meta.Arguments + 1 : meta.Arguments); }
            }
        }
    }
}