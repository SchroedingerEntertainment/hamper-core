// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;
using Soe.Collections.HashSet;

namespace Soe.Storage
{
    #if EXPORT_HAMPER_CORE_STORAGE
    public
    #else
    internal
    #endif
    static partial class VolumeManager
    {
        /// <summary>
        /// Provides access to information on a logical volume
        /// </summary>
        /// <param name="identifier">An identifier the volume is associated with</param>
        /// <param name="location">A unified resource identifier associated with the volume</param>
        public readonly struct VolumeInfo(string identifier, ResourceIdentifier location) : IHashContainer<string>
        {
            /// <inheritdoc/>
            public string Key
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get { return identifier; }
            }
            
            /// <inheritdoc/>
            public int Hash
            {
                get { return identifier.GetHashCode(); }
            }
            
            /// <summary>
            /// Gets the unified resource identifier this Volume points to
            /// </summary>
            public ResourceIdentifier Location
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get { return location; }
            }
            
            /// <inheritdoc/>
            public bool IsValid
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get { return !string.IsNullOrWhiteSpace(identifier); }
            }
        }
    }
}