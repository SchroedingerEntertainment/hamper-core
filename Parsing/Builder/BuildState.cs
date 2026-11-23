// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;
using Soe.Collections.Embedded;

namespace Soe.Parsing
{
    /// <summary>
    /// Provides stackable state transitions to be used in a parser
    /// </summary>
    #if EXPORT_HAMPER_CORE_PARSING
    public
    #else
    internal
    #endif
    struct BuildState<StateId> : IIterable<StateId, Iterator<StateId>.DefaultStrategy>, ISequence<StateId>, IDisposable
        where StateId : struct, IConvertible, IComparable
    {
        private EmbeddedStack<StateId, PoolArray<StateId>> stack;
        readonly StateId defaultState;

        /// <summary>
        /// Returns the amount of states currently maintained
        /// </summary>
        public int Count
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return stack.Count; }
        }

        /// <summary>
        /// Gets or sets current state in exclusive mode
        /// </summary>
        public StateId Current
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                if (stack.Count == 0)
                {
                    return defaultState;
                }
                else return stack.Peek();
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set { Set(value); }
        }

        /// <summary>
        /// Creates a new stackable state
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BuildState(StateId defaultState)
        {
            this.stack = new EmbeddedStack<StateId, PoolArray<StateId>>();
            this.defaultState = defaultState;
        }
        /// <summary>
        /// Creates a new stackable state
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BuildState()
            :this(default(StateId))
        { }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator StateId(BuildState<StateId> state)
        {
            return state.Current;
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Span<StateId> AsSpan()
        {
            return stack.AsSpan();
        }

        /// <summary>
        /// Removes all active states from the stack
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Clear()
        {
            stack.Clear();
        }
        
        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Dispose()
        {
            stack.Dispose();
        }
        
        /// <summary>
        /// Sets the provided state in exclusive mode
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Set(StateId value)
        {
            if (stack.Count > 0)
            {
                stack.Clear();
            }
            stack.Push(value);
        }
        /// <summary>
        /// Adds the provided state in inclusive mode and makes it the
        /// current active state. Anything else is preserved
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add(StateId value)
        {
            stack.Push(value);
        }

        /// <summary>
        /// Modifies current top most state to provided one
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Change(StateId value)
        {
            if (stack.Count > 0)
            {
                stack.Pop();
            }
            stack.Push(value);
        }

        /// <summary>
        /// Resets current states to default
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Reset()
        {
            stack.Clear();
        }

        /// <summary>
        /// Removes the top stacked state
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public StateId Remove()
        {
            if (stack.Count == 0)
            {
                return defaultState;
            }
            else return stack.Pop();
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Iterator<StateId, Iterator<StateId>.DefaultStrategy> GetEnumerator()
        {
            return stack.GetEnumerator();
        }
    }
}
