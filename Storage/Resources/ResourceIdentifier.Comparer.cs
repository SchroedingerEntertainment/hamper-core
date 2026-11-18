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
        /// Represents a case-insensitive <see cref="Char"/> comparsion
        /// </summary>
        public sealed class OrdnialComparer : IEqualityComparer<char>
        {
            private static readonly OrdnialComparer comparer;
            /// <summary>
            /// Gets the default instance of this comparer
            /// </summary>
            public static OrdnialComparer Default
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get { return comparer; }
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            static OrdnialComparer()
            {
                comparer = new OrdnialComparer();
            }
            
            /// <inheritdoc/>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            bool IEqualityComparer<char>.Equals(char x, char y)
            {
                return (char.ToLower(x) == char.ToLower(y));
            }

            /// <inheritdoc/>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public int GetHashCode(char obj)
            {
                return char.ToLower(obj).GetHashCode();
            }
        }
    }
}