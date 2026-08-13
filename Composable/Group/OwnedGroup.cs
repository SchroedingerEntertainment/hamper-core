// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;
using Soe.Threading;

namespace Soe.Composable
{
    #if EXPORT_HAMPER_CORE_COMPOSITION
    public
    #else
    internal
    #endif
    class OwnedGroup<T1, T2> : IComponentGroup
        where T1 : struct
        where T2 : struct
    {
        private Component<T1>? component1;
        private Component<T2>? component2;

        private int count;
        /// <inheritdoc/>
        public int Count
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return count; }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private OwnedGroup()
        {
            this.count = 0;
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Dispose()
        {
            component1?.ReleaseGroup(this);
            component2?.ReleaseGroup(this);
        }

        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void ConditionalMove<T>(Component<T> component, int index)
            where T : struct
        {
            if (index > count)
            {
                if (component.Swap(index, count))
                {
                    index = count;
                }
                else throw new InvalidOperationException();
            }
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGetEntity(EntityId entity, out int index)
        {
            index = component1?.IndexOf(entity) ?? -1;
            return (index < count);
        }

        #region IComponentGroup Members

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void IComponentGroup.ComponentAdded<T>(EntityId entity, ref int index)
        {
            if (typeof(T) == typeof(T1))
            {
                
            }
            else if (typeof(T) == typeof(T2))
            {
                
            }
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void IComponentGroup.ComponentRemoved<T>(EntityId entity, ref int index)
        {
            throw new NotImplementedException();
        }
        #endregion

        public static OwnedGroup<T1, T2> Initialize(Component<T1> component1, Component<T2> component2)
        {
            if (component1.ShardId == component2.ShardId)
            {
                OwnedGroup<T1, T2> result = new OwnedGroup<T1, T2>();
                if (!component1.AttachGroup(result))
                {
                    throw new ArgumentException(nameof(component1));
                }
                else if (!component2.AttachGroup(result))
                {
                    component2.ReleaseGroup(result);
                    throw new ArgumentException(nameof(component2));
                }
                else
                {
                    result.component1 = component1;
                    result.component2 = component2;
                    return result;
                }
            }
            else throw new AccessViolationException();
        }
    }
}