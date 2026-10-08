// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

namespace Soe.Runtime
{
    /// <summary>
    /// Declares different log levels used by <see cref="ILogService"/> 
    /// </summary>
    #if EXPORT_HAMPER_CORE_RUNTIME_APPLICATION
    public
    #else
    internal
    #endif
    enum SeverityFlags
    {
        /// <summary>
        /// Most messages are suppressed application wide
        /// </summary>
        None = 0,
        /// <summary>
        /// Most messages are suppressed application wide
        /// </summary>
        Silent = None,
        /// <summary>
        /// Only critical messages are displayed
        /// </summary>
        Minimal,
        /// <summary>
        /// Every message is displayed
        /// </summary>
        Full
    }
}
