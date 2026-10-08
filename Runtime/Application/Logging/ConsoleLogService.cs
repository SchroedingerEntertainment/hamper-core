// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;
using Soe.Threading;

namespace Soe.Runtime
{
    /// <summary>
    /// Sends log messages to stdout and stderr
    /// </summary>
    /// <remarks>This class is thread-safe</remarks>
    #if EXPORT_HAMPER_CORE_RUNTIME_APPLICATION
    public
    #else
    internal
    #endif
    sealed class ConsoleLogService : ILogService
    {
        private UInt32 lockVariable;
        
        private int logLevel;
        /// <inheritdoc/>
        public SeverityFlags LogLevel
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return (SeverityFlags)Volatile.Read(ref logLevel); }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set { Volatile.Write(ref logLevel, (int)value); }
        }
        
        /// <inheritdoc/>
        public void Message(VerbosityFlags messageType, SeverityFlags messageLevel, string message)
        {
            if (messageLevel <= LogLevel)
            {
                using (ScopedDisposable.Acquire<UInt32, FastSpinLock.LockOperation>(ref lockVariable))
                {
                    if (messageType == VerbosityFlags.Message)
                    {
                        Console.WriteLine(message);
                    }
                    else
                    {
                        ProcessPrefix(messageType, !Console.IsErrorRedirected);
                        Console.Error.WriteLine(message);
                    }
                }
            }
        }
        /// <inheritdoc/>
        public void Message(VerbosityFlags messageType, SeverityFlags messageLevel, string format, object? arg0)
        {
            if (messageLevel <= LogLevel)
            {
                using (ScopedDisposable.Acquire<UInt32, FastSpinLock.LockOperation>(ref lockVariable))
                {
                    if (messageType == VerbosityFlags.Message)
                    {
                        Console.WriteLine(format, arg0);
                    }
                    else
                    {
                        ProcessPrefix(messageType, !Console.IsErrorRedirected);
                        Console.Error.WriteLine(format, arg0);
                    }
                }
            }
        }
        /// <inheritdoc/>
        public void Message(VerbosityFlags messageType, SeverityFlags messageLevel, string format, object? arg0, object? arg1)
        {
            if (messageLevel <= LogLevel)
            {
                using (ScopedDisposable.Acquire<UInt32, FastSpinLock.LockOperation>(ref lockVariable))
                {
                    if (messageType == VerbosityFlags.Message)
                    {
                        Console.WriteLine(format, arg0, arg1);
                    }
                    else
                    {
                        ProcessPrefix(messageType, !Console.IsErrorRedirected);
                        Console.Error.WriteLine(format, arg0, arg1);
                    }
                }
            }
        }
        /// <inheritdoc/>
        public void Message(VerbosityFlags messageType, SeverityFlags messageLevel, string format, params object?[]? arg)
        {
            if (messageLevel <= LogLevel)
            {
                using (ScopedDisposable.Acquire<UInt32, FastSpinLock.LockOperation>(ref lockVariable))
                {
                    if (messageType == VerbosityFlags.Message)
                    {
                        Console.WriteLine(format, arg);
                    }
                    else
                    {
                        ProcessPrefix(messageType, !Console.IsErrorRedirected);
                        Console.Error.WriteLine(format, arg!);
                    }
                }
            }
        }

        static void ProcessPrefix(VerbosityFlags messageType, bool withColor)
        {
            if (messageType == VerbosityFlags.Warning)
            {
                if (withColor)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                }
                Console.Error.Write("WARNING: ");
                if (withColor)
                {
                    Console.ResetColor();
                }
            }
            else if (messageType == VerbosityFlags.Error)
            {
                if (withColor)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }
                Console.Error.Write("ERROR: ");
                if (withColor)
                {
                    Console.ResetColor();
                }
            }
        }
    }
}