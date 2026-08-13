// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

namespace Soe.Composable
{
    #if EXPORT_HAMPER_CORE_COMPOSITION
    public
    #else
    internal
    #endif
    interface IComponent
    {
        public int Capacity
        {
            get;
        }
        
        public int Count
        {
            get;
        }
        
        public void Clear();
    }
}