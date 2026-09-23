// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using Soe.Threading;

namespace Soe.Composable
{
    /// <summary>
    /// Aligns component pools so that iterating over specific component combinations runs at maximum speed
    /// </summary>
    #if EXPORT_HAMPER_CORE_COMPOSITION
    public
    #else
    internal
    #endif
    interface IComponentGroup : IDisposable, IReadOnlySequence<EntityId>
    {
        /// <summary>
        /// Gets the current number of entities belonging to this group
        /// </summary>
        int Count
        {
            get;
        }
        
        /// <summary>
        /// Called whenever a component is added to a specific component pool
        /// </summary>
        /// <param name="entity">The entity the added component belongs to</param>
        /// <param name="index">The index at which the component was added into the pool</param>
        /// <typeparam name="T">The component type added</typeparam>
        void ComponentAdded<T>(EntityId entity, ref int index)
            where T : struct;
        
        /// <summary>
        /// Called whenever a component is removed from a specific component pool
        /// </summary>
        /// <param name="entity">The entity the removed component belongs to</param>
        /// <param name="index">The index at which the component is currently in the pool</param>
        /// <typeparam name="T">The component type removed</typeparam>
        void ComponentRemoved<T>(EntityId entity, ref int index)
            where T : struct;
        
        /// <summary>
        /// Called whenever a borrow request targets one or more component pools belonging to this group
        /// </summary>
        /// <param name="resolver">The dependency resolver belonging to the request</param>
        /// <param name="uniqueId">The instance id of the component pool</param>
        /// <typeparam name="T">The component type requested</typeparam>
        /// <returns>True if the request was altered, false otherwise</returns>
        bool OnRequest<T>(AccessManager.DependencyTreeResolver resolver, UInt32 uniqueId)
            where T : struct;
        
        /// <summary>
        /// Gets if a given entity resides in this group
        /// </summary>
        /// <param name="entity">The entity to get the index for</param>
        /// <param name="index">The zero-based index at which the entity was found</param>
        /// <returns>True if the entity is part of this group, false otherwise</returns>
        bool TryGetEntity(EntityId entity, out int index);
    }
}