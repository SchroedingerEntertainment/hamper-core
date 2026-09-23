// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

namespace Soe.Composable
{
    /// <summary>
    /// Specifies constraints of how an entity behaves in either <see cref="Entities"/> or <see cref="Component{T}"/> pools
    /// </summary>
    [Flags]
    #if EXPORT_HAMPER_CORE_COMPOSITION
    public
    #else
    internal
    #endif
    enum EntityFlags : byte
    {
        /// <summary>
        /// No specific behavior set for this entity
        /// </summary>
        None = 0,
        
        /// <summary>
        /// The entity might not change it's current composition
        /// </summary>
        Locked = 0x4,
        /// <summary>
        /// The entity is reserved for deletion
        /// </summary>
        Reserved = 0x80
    }
}
