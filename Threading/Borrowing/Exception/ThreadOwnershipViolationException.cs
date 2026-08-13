// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System;

namespace Soe.Threading
{
    /// <summary>
    /// An exception thrown when a non-owning thread attempts to perform an owned operation or vice versa
    /// </summary>
    #if EXPORT_HAMPER_CORE_THREADING
    public
    #else
    internal
    #endif
    class ThreadOwnershipViolationException : Exception
    { }
}