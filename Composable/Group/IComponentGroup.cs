// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Reflection.Metadata;

namespace Soe.Composable
{
    #if EXPORT_HAMPER_CORE_COMPOSITION
    public
    #else
    internal
    #endif
    interface IComponentGroup : IDisposable
    {
        int Count
        {
            get;
        }
        
        void ComponentAdded<T>(EntityId entity, ref int index)
            where T : struct;
        
        void ComponentRemoved<T>(EntityId entity, ref int index)
            where T : struct;
        
        bool TryGetEntity(EntityId entity, out int index);
    }
}