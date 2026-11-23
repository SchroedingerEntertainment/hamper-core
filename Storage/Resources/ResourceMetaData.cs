// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Soe.Storage
{
    /// <summary>
    /// Declares metadata of a unified resource identifier
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    #if EXPORT_HAMPER_CORE_STORAGE
    public
    #else
    internal
    #endif
    readonly struct ResourceMetaData : IEquatable<ResourceMetaData>
    {
        [FieldOffset(0)]
        private readonly UInt64 value;
        
        [FieldOffset(0)]
        private readonly IdentifierFlags flags;
        /// <summary>
        /// Gets the flags of the corresponding resource identifier
        /// </summary>
        public IdentifierFlags Flags
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return flags; }
        }
        
        [FieldOffset(1)]
        private readonly byte authority;
        /// <summary>
        /// Gets the index of first character in the authority portion of the corresponding resource identifier
        /// </summary>
        public int Authority
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return authority; }
        }
        
        [FieldOffset(2)]
        private readonly byte path;
        /// <summary>
        /// Gets the index of first character in the path of the corresponding resource identifier
        /// </summary>
        public int Path
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return path; }
        }
        
        [FieldOffset(3)]
        private readonly UInt16 name;
        /// <summary>
        /// Gets the index of first character in the name of the corresponding resource identifier
        /// </summary>
        public int Name
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return name; }
        }
        
        [FieldOffset(5)]
        private readonly byte extension;
        /// <summary>
        /// Gets the index of first character in the file extension of the corresponding resource identifier
        /// </summary>
        public int Extension
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return extension; }
        }
        
        [FieldOffset(6)]
        private readonly UInt16 arguments;
        /// <summary>
        /// Gets the index of first character in the argument portion of the corresponding resource identifier
        /// </summary>
        public int Arguments
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return arguments; }
        }

        /// <summary>
        /// Initializes a new metadata instance from the provided values
        /// </summary>
        /// <param name="flags">The flags of the corresponding resource identifier</param>
        /// <param name="authority">The index of first character in the authority portion of the corresponding resource identifier</param>
        /// <param name="path">The index of first character in the path of the corresponding resource identifier</param>
        /// <param name="name">The index of first character in the name of the corresponding resource identifier</param>
        /// <param name="extension">The index of first character in the file extension portion of the corresponding resource identifier</param>
        /// <param name="arguments">The index of first character in the argument portion of the corresponding resource identifier</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ResourceMetaData(IdentifierFlags flags, int authority, int path, int name, int extension, int arguments)
        {
            this.value = 0;
            this.flags = flags;
            this.authority = (byte)authority;
            this.path = (byte)path;
            this.name = (UInt16)name;
            this.extension = (byte)extension;
            this.arguments = (UInt16)arguments;
        }
        /// <summary>
        /// Initializes a new metadata instance from the provided value
        /// </summary>
        /// <param name="value">The packed metadata instance to copy</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ResourceMetaData(UInt64 value)
        {
            this.value = value;
        }
        
        /*[MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void Shift(int length)
        {
            path += (byte)length;
            name += (Int16)length;
            extension += (Int16)length;
            arguments += (Int16)length;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void Extend(int length)
        {
            name += (Int16)length;
            extension += (Int16)length;
            arguments += (Int16)length;
        }*/

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(ResourceMetaData other)
        {
            return (value == other.value);
        }
        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object? obj)
        {
            return obj is ResourceMetaData other && Equals(other);
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode()
        {
            return value.GetHashCode();
        }
    }
}