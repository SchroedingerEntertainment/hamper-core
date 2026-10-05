// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System;
using System.Runtime.CompilerServices;

namespace Soe.Reactive
{
    /// <summary>
    /// An exception thrown when a channel is unable to connect
    /// </summary>
    #if EXPORT_HAMPER_CORE_REACTIVE
    public
    #else
    internal
    #endif
    class ChannelConnectionFailedException : Exception
    {
        private readonly object source;
        /// <summary>
        /// Gets the source rejecting the connection
        /// </summary>
        public object Source1
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return source; }
        }

        private readonly object node;
        /// <summary>
        /// Gets the node trying to establish a connection
        /// </summary>
        public object Node
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return node; }
        }
        
        /// <summary>
        /// Initializes the exception with the appropriate data
        /// </summary>
        /// <param name="source">The source rejecting the connection</param>
        /// <param name="node">The node trying to establish a connection</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ChannelConnectionFailedException(object source, object node)
        {
            this.source = source;
            this.node = node;
        }
    }
}