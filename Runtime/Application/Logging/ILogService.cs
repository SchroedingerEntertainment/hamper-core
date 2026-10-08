// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

namespace Soe.Runtime
{
    /// <summary>
    /// Provides and manages different output channels for log messaging
    /// </summary>
    #if EXPORT_HAMPER_CORE_RUNTIME_APPLICATION
    public
    #else
    internal
    #endif
    interface ILogService
    {
        /// <summary>
        /// Gets or sets the current message filtering
        /// </summary>
        SeverityFlags LogLevel
        {
            get;
            set;
        }
        
        /// <summary>
        /// Writes the specified string value to the declared output stream
        /// </summary>
        /// <param name="messageType">Determines the target output stream</param>
        /// <param name="messageLevel">Classifies the importance of the message</param>
        /// <param name="message">The message to write</param>
        void Message(VerbosityFlags messageType, SeverityFlags messageLevel, string message);
        /// <summary>
        /// Writes the text representation of the specified object to the declared output stream using the specified format information
        /// </summary>
        /// <param name="messageType">Determines the target output stream</param>
        /// <param name="messageLevel">Classifies the importance of the message</param>
        /// <param name="format">A composite format string</param>
        /// <param name="arg0">The first object to write using format</param>
        void Message(VerbosityFlags messageType, SeverityFlags messageLevel, string format, object? arg0);
        /// <summary>
        /// Writes the text representation of the specified objects to the declared output stream using the specified format information
        /// </summary>
        /// <param name="messageType">Determines the target output stream</param>
        /// <param name="messageLevel">Classifies the importance of the message</param>
        /// <param name="format">A composite format string</param>
        /// <param name="arg0">The first object to write using format</param>
        /// <param name="arg1">The second object to write using format</param>
        void Message(VerbosityFlags messageType, SeverityFlags messageLevel, string format, object? arg0, object? arg1);
        /// <summary>
        /// Writes the text representation of the specified objects to the declared output stream using the specified format information
        /// </summary>
        /// <param name="messageType">Determines the target output stream</param>
        /// <param name="messageLevel">Classifies the importance of the message</param>
        /// <param name="format">A composite format string</param>
        /// <param name="arg">A collection of objects to write using format</param>
        void Message(VerbosityFlags messageType, SeverityFlags messageLevel, string format, params object?[]? arg);
    }
}