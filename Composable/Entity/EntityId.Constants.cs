// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

namespace Soe.Composable
{
    #if EXPORT_HAMPER_CORE_COMPOSITION
    public
    #else
    internal
    #endif
    readonly partial struct EntityId
    {
        /// <summary>
        /// Represents an uninitialized object identity
        /// </summary>
        public static readonly EntityId Null = new EntityId(Int32.MaxValue, 0, 0, EntityFlags.None);

        /// <summary>
        /// Represents an object marked for deletion
        /// </summary>
        public static readonly EntityId Reserved = new EntityId(0, 0, 0, EntityFlags.Reserved);

        /// <summary>
        /// Represents an invalid object identity
        /// </summary>
        public static readonly EntityId Invalid = Null | Reserved;
    }
}