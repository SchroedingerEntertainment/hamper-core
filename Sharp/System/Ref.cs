// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;

namespace System
{
    /// <summary>
    /// A reference to a value of type <typeparamref name="T"/>
    /// </summary>
    [Serializable]
    #if EXPORT_HAMPER_CORE_SHARP
    public
    #else
    internal
    #endif
    readonly ref struct Ref<T>
    {
        private readonly ref T value;
        /// <summary>
        /// Gets the reference value
        /// </summary>
        public ref T Value
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return ref value; }
        }

        /// <summary>
        /// Initializes a reference
        /// </summary>
        /// <param name="value">The reference to a value of type <typeparamref name="T"/></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Ref(ref T value)
        {
            this.value = ref value;
        }

        /// <summary>
        /// Returns an empty reference
        /// </summary>
        /// <returns>An empty reference instance</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Ref<T> CreateEmpty()
        {
            return default;
        }
    }
}