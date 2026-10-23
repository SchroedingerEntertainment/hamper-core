// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

namespace Soe.Composable
{
    /// <summary>
    /// Stores pure data structures defining specific properties of an entity in a component pool 
    /// </summary>
    #if EXPORT_HAMPER_CORE_COMPOSITION
    public
    #else
    internal
    #endif
    interface IComponent
    {
        /// <summary>
        /// Gets the maximum number of components that can be stored
        /// </summary>
        public int Capacity
        {
            get;
        }
        
        /// <summary>
        /// Gets the current number of components stored
        /// </summary>
        public int Count
        {
            get;
        }
        
        /// <summary>
        /// Removes all components currently stored
        /// </summary>
        public void Clear();
    }
}