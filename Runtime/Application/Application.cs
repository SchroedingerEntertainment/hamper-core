// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;

namespace Soe.Runtime
{
    /// <summary>
    /// Manages application specific settings and properties
    /// </summary>
    #if EXPORT_HAMPER_CORE_RUNTIME_APPLICATION
    public
    #else
    internal
    #endif
    static partial class Application
    {
        private static int exitCode;
        /// <summary>
        /// Gets or sets this applications exit code
        /// </summary>
        public static ReturnCode ExitCode
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return Volatile.Read(ref exitCode); }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set { Volatile.Write(ref exitCode, (int)value); }
        }
        
        private static ILogService log;
        /// <summary>
        /// Gets or sets the current active log service for this application
        /// </summary>
        public static ILogService LogService
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return Volatile.Read(ref log); }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set { Volatile.Write(ref log, value); }
        }
        
        /// <summary>
        /// Gets the target of this application
        /// </summary>
        public static string BaseDirectory
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return AppContext.BaseDirectory; }
        }

        /// <summary>
        /// Gets or sets the target of the current active directory
        /// </summary>
        public static string LocalDirectory
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return Environment.CurrentDirectory; }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set { Environment.CurrentDirectory = value; }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static Application()
        {
            log = new ConsoleLogService();
        }
        
        /// <summary>
        /// Writes the specified string value to the declared output stream
        /// </summary>
        /// <param name="messageType">Determines the target output stream</param>
        /// <param name="messageLevel">Classifies the importance of the message</param>
        /// <param name="message">The message to write</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Log(VerbosityFlags messageType, SeverityFlags messageLevel, string message)
        {
            if (messageType == VerbosityFlags.Error)
            {
                Interlocked.CompareExchange(ref exitCode, 1, 0);
            }
            LogService.Message(messageType, messageLevel, message);
        }
        /// <summary>
        /// Writes the text representation of the specified object to the declared output stream using the specified format information
        /// </summary>
        /// <param name="messageType">Determines the target output stream</param>
        /// <param name="messageLevel">Classifies the importance of the message</param>
        /// <param name="format">A composite format string</param>
        /// <param name="arg0">The first object to write using format</param>
        public static void Log(VerbosityFlags messageType, SeverityFlags messageLevel, string format, object? arg0)
        {
            if (messageType == VerbosityFlags.Error)
            {
                Interlocked.CompareExchange(ref exitCode, 1, 0);
            }
            LogService.Message(messageType, messageLevel,  format, arg0);
        }
        /// <summary>
        /// Writes the text representation of the specified objects to the declared output stream using the specified format information
        /// </summary>
        /// <param name="messageType">Determines the target output stream</param>
        /// <param name="messageLevel">Classifies the importance of the message</param>
        /// <param name="format">A composite format string</param>
        /// <param name="arg0">The first object to write using format</param>
        /// <param name="arg1">The second object to write using format</param>
        public static void Log(VerbosityFlags messageType, SeverityFlags messageLevel, string format, object? arg0, object? arg1)
        {
            if (messageType == VerbosityFlags.Error)
            {
                Interlocked.CompareExchange(ref exitCode, 1, 0);
            }
            LogService.Message(messageType, messageLevel, format, arg0, arg1);
        }
        /// <summary>
        /// Writes the text representation of the specified objects to the declared output stream using the specified format information
        /// </summary>
        /// <param name="messageType">Determines the target output stream</param>
        /// <param name="messageLevel">Classifies the importance of the message</param>
        /// <param name="format">A composite format string</param>
        /// <param name="arg">A collection of objects to write using format</param>
        public static void Log(VerbosityFlags messageType, SeverityFlags messageLevel, string format, params object?[]? arg)
        {
            if (messageType == VerbosityFlags.Error)
            {
                Interlocked.CompareExchange(ref exitCode, 1, 0);
            }
            LogService.Message(messageType, messageLevel, format, arg);
        }

        /// <summary>
        /// Prompts the user and awaits an input
        /// </summary>
        /// <param name="localization">The localization object for this prompt</param>
        /// <param name="settings">The settings for this prompt</param>
        /// <param name="message">The message to write</param>
        /// <returns>The result of the user prompt</returns>
        public static bool Prompt(PromptLocalisation localization, PromptSettings settings, string message)
        {
            char trueKey = (settings.FlagSet(PromptSettings.InvertDefault) ? char.ToUpperInvariant(localization.TrueKey) : char.ToLowerInvariant(localization.TrueKey));
            char falseKey = (settings.FlagSet(PromptSettings.InvertDefault) ? char.ToLowerInvariant(localization.FalseKey) : char.ToUpperInvariant(localization.FalseKey));
            for(; ;)
            {
                string? result = LogService.Prompt(string.Concat(message, " [",  trueKey, "/", falseKey, "] "));
                if (string.IsNullOrEmpty(result))
                {
                    return settings.FlagSet(PromptSettings.InvertDefault);
                }
                else if (result.Length == 1)
                {
                    char key = char.ToLowerInvariant(result[0]);
                    if (key == char.ToLowerInvariant(localization.TrueKey))
                    {
                        return true;
                    }
                    else if(key == char.ToLowerInvariant(localization.FalseKey))
                    {
                        return false;
                    }
                }
                if(settings.FlagSet(PromptSettings.ValidateInput))
                {
                    LogService.Message(VerbosityFlags.Error, SeverityFlags.Silent, localization.ErrorMessage, localization.TrueKey, localization.FalseKey);
                }
                else return settings.FlagSet(PromptSettings.InvertDefault);
            }
        }
        /// <summary>
        /// Prompts the user and awaits an input
        /// </summary>
        /// <param name="localization">The localization object for this prompt</param>
        /// <param name="settings">The settings for this prompt</param>
        /// <param name="format">A composite format string</param>
        /// <param name="arg0">The first object to write using format</param>
        /// <returns>The result of the user prompt</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Prompt(PromptLocalisation localization, PromptSettings settings, string format, object? arg0)
        {
            return Prompt(localization, settings, string.Format(format, arg0));
        }
        /// <summary>
        /// Prompts the user and awaits an input
        /// </summary>
        /// <param name="localization">The localization object for this prompt</param>
        /// <param name="settings">The settings for this prompt</param>
        /// <param name="format">A composite format string</param>
        /// <param name="arg0">The first object to write using format</param>
        /// <param name="arg1">The second object to write using format</param>
        /// <returns>The result of the user prompt</returns>
        public static bool Prompt(PromptLocalisation localization, PromptSettings settings, string format, object? arg0, object? arg1)
        {
            return Prompt(localization, settings, string.Format(format, arg0, arg1));
        }
    }
}