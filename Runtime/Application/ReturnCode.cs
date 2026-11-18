// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Soe.Runtime
{
    /// <summary>
    /// Represents an exit code determining the status of an application
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    #if EXPORT_HAMPER_CORE_RUNTIME_APPLICATION
    public
    #else
    internal
    #endif
    readonly struct ReturnCode : IEquatable<ReturnCode>
    {
        /// <summary>
        /// The action or operation lead to a generic error
        /// </summary>
        public static readonly ReturnCode Error = new ReturnCode(-1);
        /// <summary>
        /// The action or operation was successful
        /// </summary>
        public static readonly ReturnCode Success = new ReturnCode(0);
        
        [FieldOffset(0)]
        private readonly int value;
        
        [FieldOffset(0)]
        private readonly UInt16 statusCode;
        /// <summary>
        /// Gets the status code of the corresponding category
        /// </summary>
        public int StatusCode
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return statusCode; }
        }
        
        [FieldOffset(3)]
        private readonly byte categoryId;
        /// <summary>
        /// Gets the error category of the return code
        /// </summary>
        public int CategoryId
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return categoryId; }
        }
        
        /// <summary>
        /// Initializes this return code from the provided values
        /// </summary>
        /// <param name="categoryId">The error category of the return code</param>
        /// <param name="statusCode">The status code of the corresponding category</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReturnCode(int categoryId, int statusCode)
        {
            this.statusCode = (UInt16)statusCode;
            this.categoryId = (byte)categoryId;
        }
        /// <summary>
        /// Initializes this return code with the provided value
        /// </summary>
        /// <param name="value">This return codes value</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReturnCode(int value)
        {
            this.value = value;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator ReturnCode(int value)
        {
            return new ReturnCode(value);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator int(ReturnCode entity)
        {
            return entity.value;
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(ReturnCode other)
        {
            return value == other.value;
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object? obj)
        {
            return obj is ReturnCode other && Equals(other);
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode()
        {
            return value.GetHashCode();
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString()
        {
            return $"{{Category: {categoryId}, Status: {statusCode}}}";
        }
    }
}