// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;

namespace Soe.Storage
{
    #if EXPORT_HAMPER_CORE_STORAGE
    public
    #else
    internal
    #endif
    sealed partial class StorageProvider
    {
        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        ReadOnlySpan<char> IContextHandler.GetCurrent(object? context, ulong parameter)
        {
            if (context is IEnumerator<string> enumerator)
            {
                return enumerator.Current.AsSpan((int)parameter);
            }
            else return ReadOnlySpan<char>.Empty;
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void IContextHandler.Dispose(object? context, ref ulong parameter)
        {
            (context as IEnumerator<string>)?.Dispose();
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        bool IContextHandler.MoveNext(object? context, ref ulong parameter)
        {
            return ((context as IEnumerator<string>)?.MoveNext() ?? false);
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void IContextHandler.Reset(object? context, ref ulong parameter)
        {
            (context as IEnumerator<string>)?.Reset();
        }
    }
}