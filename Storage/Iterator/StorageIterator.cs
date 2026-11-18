// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;

namespace Soe.Storage
{
    /// <summary>
    /// Handles iteration over a collection of file system entries
    /// </summary>
    /// <param name="handler">A context handler controlling the behavior of the iterator</param>
    /// <param name="context">An optional context object</param>
    /// <param name="parameter">An optional parameter</param>
    [method: MethodImpl(MethodImplOptions.AggressiveInlining)]
    #if EXPORT_HAMPER_CORE_STORAGE
    public
    #else
    internal
    #endif
    ref struct StorageIterator(IContextHandler handler, object? context = null, UInt64 parameter = 0) : IReadOnlyIterator<ReadOnlySpan<char>>, IDisposable
    {
        private UInt64 parameter = parameter;
        
        /// <inheritdoc/>
        public ReadOnlySpan<char> Current
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return handler.GetCurrent(context, parameter); }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Dispose()
        {
            handler.Dispose(context, ref parameter);
        }
        
        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool MoveNext()
        {
            return handler.MoveNext(context, ref parameter);
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Reset()
        {
            handler.Reset(context, ref parameter);
        }
    }
}