// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

namespace Soe.Reactive
{
    /// <summary>
    /// Represents control flags managing the behavior of the dispatcher
    /// </summary>
    #if EXPORT_HAMPER_CORE_REACTIVE
    public
    #else
    internal
    #endif
    enum NotificationResult
    {
        /// <summary>
        /// The notification was handled successful
        /// </summary>
        Ok = 0,
        /// <summary>
        /// The dispatcher should break
        /// </summary>
        Abort,
        /// <summary>
        /// An error occured while the notification was handled
        /// </summary>
        Error
    }
}