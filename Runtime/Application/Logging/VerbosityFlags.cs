// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

namespace Soe.Runtime
{
    /// <summary>
    /// Declares different output channels used by <see cref="ILogService"/> 
    /// </summary>
    #if EXPORT_HAMPER_CORE_RUNTIME_APPLICATION
    public
    #else
    internal
    #endif
    enum VerbosityFlags
    {
        /// <summary>
        /// Redirects the message to the standard output stream
        /// </summary>
        Message,
        /// <summary>
        /// Redirects the message to the error output stream, depicting a warning
        /// </summary>
        Warning,
        /// <summary>
        /// Redirects the message to the error output stream
        /// </summary>
        Error
    }
}
