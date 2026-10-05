// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

namespace Soe.Threading
{
    /// <summary>
    /// An enum of order operational flags returned by <see cref="IAccessPolicy"/>
    /// </summary>
    [Flags]
    #if EXPORT_HAMPER_CORE_THREADING
    public
    #else
    internal
    #endif
    enum PolicyResolutionFlags : int
    {
        /// <summary>
        /// An empty bit. The task scheduler will not handle the potential dependency
        /// </summary>
        None = 0,
        /// <summary>
        /// Tells the task scheduler to treat this dependency as this tasks parent
        /// </summary>
        Wait = 0x1,
        /// <summary>
        /// Tells the task scheduler to not probe any potential dependencies beyond the current one
        /// </summary>
        Barrier = 0x2
    }
}