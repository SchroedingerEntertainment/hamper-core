// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;

namespace Soe.Reactive
{
    /// <summary>
    /// Provides a representation of a conditional data flow controller over a certain data source
    /// </summary>
    /// <typeparam name="T">The object that provides notification information</typeparam>
    /// <typeparam name="MutatorT">A data flow controller</typeparam>
    /// <typeparam name="ObservableT">An observable data source to modify</typeparam>
    #if EXPORT_HAMPER_CORE_REACTIVE
    public
    #else
    internal
    #endif
    readonly ref struct Mutator<T, ObservableT, MutatorT>
        where ObservableT : class
        where MutatorT : struct, IGatingMutator<T>
    {
        private readonly ObservableT source;
        /// <summary>
        /// Gets the data source instance this controller is based on
        /// </summary>
        public ObservableT Source
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return source; }
        }

        private readonly MutatorT instance;
        /// <summary>
        /// Gets the instance of the underlying data flow controller
        /// </summary>
        public MutatorT Instance
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return instance; }
        }

        /// <summary>
        /// Initializes the conditional data flow controller representation
        /// </summary>
        /// <param name="source">The data source instance this controller is based on</param>
        /// <param name="instance">The instance of the underlying data flow controller</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Mutator(ObservableT source, MutatorT instance)
        {
            this.source = source;
            this.instance = instance;
        }
    }
    
    /// <summary>
    /// Provides a representation of a data transformation unit over a certain data source
    /// </summary>
    /// <typeparam name="TIn">The object that provides notification information</typeparam>
    /// <typeparam name="TOut">The transformed object that provides notification information</typeparam>
    /// <typeparam name="MutatorT">A data transformation unit</typeparam>
    /// <typeparam name="ObservableT">An observable data source to modify</typeparam>
    #if EXPORT_HAMPER_CORE_REACTIVE
    public
    #else
    internal
    #endif
    readonly ref struct Mutator<TIn, TOut, ObservableT, MutatorT>
        where ObservableT : class
        where MutatorT : struct, IProjectionMutator<TIn, TOut>
    {
        private readonly ObservableT source;
        /// <summary>
        /// Gets the data source instance this controller is based on
        /// </summary>
        public ObservableT Source
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return source; }
        }
        
        private readonly MutatorT instance;
        /// <summary>
        /// Gets the instance of the underlying data transformation unit
        /// </summary>
        public MutatorT Instance
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return instance; }
        }

        /// <summary>
        /// Initializes the conditional data transformation representation
        /// </summary>
        /// <param name="source">The data source instance this controller is based on</param>
        /// <param name="instance">The instance of the underlying data transformation unit</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Mutator(ObservableT source, MutatorT instance)
        {
            this.source = source;
            this.instance = instance;
        }
    }
    
    /// <summary>
    /// Provides a representation of a conditional data transformation unit over a certain data source
    /// </summary>
    /// <typeparam name="TIn">The object that provides notification information</typeparam>
    /// <typeparam name="TKey">The object that provides information about the target subscriptions</typeparam>
    /// <typeparam name="TValue">The transformed object that provides notification information</typeparam>
    /// <typeparam name="MutatorT">A data conditional transformation unit</typeparam>
    /// <typeparam name="ObservableT">An observable data source to modify</typeparam>
    #if EXPORT_HAMPER_CORE_REACTIVE
    public
    #else
    internal
    #endif
    readonly ref struct Mutator<TIn, TKey, TValue, ObservableT, MutatorT>
        where ObservableT : class
        where MutatorT : struct, IProjectionMutator<TIn, TKey, TValue>
    {
        private readonly ObservableT source;
        /// <summary>
        /// Gets the data source instance this controller is based on
        /// </summary>
        public ObservableT Source
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return source; }
        }
        
        private readonly MutatorT instance;
        /// <summary>
        /// Gets the instance of the underlying data transformation unit
        /// </summary>
        public MutatorT Instance
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return instance; }
        }

        /// <summary>
        /// Initializes the conditional data transformation representation
        /// </summary>
        /// <param name="source">The data source instance this controller is based on</param>
        /// <param name="instance">The instance of the underlying data transformation unit</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Mutator(ObservableT source, MutatorT instance)
        {
            this.source = source;
            this.instance = instance;
        }
    }
}