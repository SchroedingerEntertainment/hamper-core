// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System;
using System.Runtime.CompilerServices;

namespace Soe.Reactive
{
    /// <summary>
    /// An exception thrown when a channel is unable to accept more connections
    /// </summary>
    #if EXPORT_HAMPER_CORE_REACTIVE
    public
    #else
    internal
    #endif
    class ChannelLimitReachedException : Exception
    {
        private static readonly ChannelLimitReachedException instance;
        
        /// <summary>
        /// An instance of the exception thrown when a channel is unable to accept more connections
        /// </summary>
        public static ChannelLimitReachedException Instance
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return instance; }
        }

        static ChannelLimitReachedException()
        {
            instance = new ChannelLimitReachedException();
        }
    }
}