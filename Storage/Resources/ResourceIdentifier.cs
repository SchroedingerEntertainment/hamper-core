// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;

namespace Soe.Storage
{
    /// <summary>
    /// Provides an object representation of a unified resource identifier and it's various properties
    /// </summary>
    /// <param name="identifier">The source of this identifier</param>
    /// <param name="meta">The metadata of the identifier</param>
    [method: MethodImpl(MethodImplOptions.AggressiveInlining)]
    #if EXPORT_HAMPER_CORE_STORAGE
    public
    #else
    internal
    #endif
    readonly partial struct ResourceIdentifier(string identifier, ResourceMetaData meta)
        : IEquatable<ResourceIdentifier>
    {
        private readonly string identifier = identifier;
        private readonly ResourceMetaData meta = meta;

        /// <summary>
        /// Gets if this unified resource identifier points to a certain location
        /// </summary>
        public bool IsLocation
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return (meta.Path > 0); }
        }

        /// <summary>
        /// Gets the location component of this unified resource identifier
        /// </summary>
        public LocationComponent Location
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new LocationComponent(identifier, meta); }
        }
        
        /// <summary>
        /// Gets if this unified resource identifier points to a full qualified target
        /// </summary>
        public bool IsTarget
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return ((meta.Name - meta.Path) > 0); }
        }
        
        /// <summary>
        /// Gets the target component of this unified resource identifier
        /// </summary>
        public TargetComponent Target
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new TargetComponent(identifier, meta); }
        }
        
        /// <summary>
        /// Gets if this unified resource identifier contains a query string
        /// </summary>
        public bool IsQuery
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return (meta.Arguments + 1 < identifier.Length); }
        }
        
        /// <summary>
        /// Gets the query component of this unified resource identifier
        /// </summary>
        public QueryComponent Query
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new QueryComponent(identifier, meta); }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator ResourceMetaData(ResourceIdentifier identifier)
        {
            return identifier.meta;
        }
        
        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(ResourceIdentifier other)
        {
            return string.Equals(identifier, other.identifier, StringComparison.InvariantCulture);
        }
        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object? obj)
        {
            return obj is ResourceIdentifier other && Equals(other);
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode()
        {
            return identifier.GetHashCode();
        }
    }
}