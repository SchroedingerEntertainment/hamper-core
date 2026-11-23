// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;
using System.Text;
using Soe.Parsing;
using Soe.Pooling;

namespace Soe.Runtime
{
    /// <summary>
    /// A stream reader specialized to process text into arguments
    /// </summary>
    #if EXPORT_HAMPER_CORE_RUNTIME_COMMANDS
    public
    #else
    internal
    #endif
    partial class CommandReader<Source> : TokenReader<CommandToken, Source>, ITextProvider<TextBuffer>
        where Source : struct, IStreamSource
    {
        enum CommandProcessingFlags : byte
        {
            BeforeCommand = 0,
            BeforeValue,
        }

        private CommandToken preserved;
        private BuildState<CommandProcessingFlags> state;
        private TextBuffer buffer;

        private readonly CommandReaderOptions options;
        /// <summary>
        /// Gets the configuration of the argument reader
        /// </summary>
        public CommandReaderOptions Options
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return options; }
        }
        
        private CommandFragment fragment;
        /// <summary>
        /// Returns the current parser fragment
        /// </summary>
        public CommandFragment Fragment
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return fragment; }
        }

        /// <summary>
        /// Creates a new command reader from the given stream
        /// </summary>
        /// <param name="source">The stream reader used to process the data</param>
        /// <param name="options">Configures the behavior of the argument reader</param>
        public CommandReader(Source source, CommandReaderOptions options = CommandReaderOptions.AllowVerbValues | CommandReaderOptions.IgnoreMatch)
            : base(source)
        {
            this.options = options;
            this.state = new BuildState<CommandProcessingFlags>();
            this.buffer = new TextBuffer(StringBuilderPool.Shared.Rent());
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Initialize(Source source)
        {
            base.Initialize(source);
            this.preserved = CommandToken.EndOfStream;
            this.buffer.Clear(true);
            this.state.Clear();
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Dispose()
        {
            StringBuilderPool.Shared.Return(buffer.Buffer);
            buffer = default;
        }
        
        bool Evaluate(ReadOnlySpan<Option> commandOptions)
        {
            if (commandOptions.TryFindOption(fragment.Key, out Option option))
            {
                if (option.FlagSet(OptionFlags.Verb))
                {
                    // Option is a verb, ignore
                }
                else if (option.FlagSet(OptionFlags.Alias))
                {
                    if (option.Parameter != null)
                    {
                        buffer.Buffer.Remove(0, buffer.Offset);
                        buffer.Buffer.Insert(0, option.Parameter);
                        buffer.Offset = option.Parameter.Length;
                        fragment = new CommandFragment(fragment.Type, this, buffer.Offset, 0);
                        
                        return Evaluate(commandOptions);
                    }
                    else fragment = new CommandFragment(CommandToken.Unknown, this, buffer.Offset, 0);
                    return true;
                }
                else if (!option.FlagSet(OptionFlags.RequiresParameter) || option.FlagSet(OptionFlags.Flag))
                {
                    return true;
                }
            }
            if (!options.HasFlag(CommandReaderOptions.IgnoreMatch))
            {
                fragment = new CommandFragment(CommandToken.Unknown, this, buffer.Offset, 0);
            }
            return false;
        }
        
        /// <summary>
        /// Generates a <see cref="TextSource{Source}"/> from the internal buffer
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TextSource<TextBuffer> GetCharacterSource(int startIndex, int length)
        {
            return new TextSource<TextBuffer>(ref buffer, startIndex, length);
        }
        
        /// <summary>
        /// Reads the next token from the input stream and advances it's position by one
        /// </summary>
        /// <param name="commandOptions">A sorted collection of options passed to the parser</param>
        /// <returns>True if a token could be read properly, false otherwise</returns>
        /// <remarks>The collection provided via <paramref name="commandOptions"/> must be sorted in ASCII byte order: [0-9][A-Z][a-z][punctuations],
        /// in order to prevent performance impact while resolving parsed commands into the corresponding option</remarks>
        public bool Read(ReadOnlySpan<Option> commandOptions)
        {
            if (preserved != CommandToken.EndOfStream)
            {
                buffer.Buffer.Remove(0, buffer.Offset);
                buffer.Offset = 0;
            }
            else buffer.Clear(true);
            
        Next:
            switch ((CommandProcessingFlags)state)
            {
                #region BeforeCommand
                case CommandProcessingFlags.BeforeCommand:
                {
                    CommandToken token = ReadNext();
                    switch (token)
                    {
                        #region Argument
                        case CommandToken.ArgumentName:
                            {
                                if (options.HasFlag(CommandReaderOptions.IgnoreCase))
                                {
                                    for(int i = 0, length = buffer.Length; i < length; i++)
                                        buffer[i] = char.ToLowerInvariant(buffer[i]);
                                }
                                buffer.Preserve();
                                fragment = new CommandFragment(CommandToken.Command, this, buffer.Offset, 0);
                                if (!Evaluate(commandOptions))
                                {
                                    state.Set(CommandProcessingFlags.BeforeValue);
                                }
                                else return true;
                            }
                            goto Next;
                        #endregion
                        
                        #region Compound
                        case CommandToken.CompoundArgument:
                            {
                                bool hasCommandsLeft;
                                
                                if(buffer.Length > 1)
                                {
                                    preserved = CommandToken.CompoundArgument;
                                    hasCommandsLeft = true;
                                }
                                else hasCommandsLeft = false;
                                if (options.HasFlag(CommandReaderOptions.IgnoreCase))
                                {
                                    buffer[0] = char.ToLowerInvariant(buffer[0]);
                                }
                                buffer.Offset++;
                                fragment = new CommandFragment(CommandToken.Command, this, buffer.Offset, 0);
                                if (!Evaluate(commandOptions) && !hasCommandsLeft)
                                {
                                    state.Set(CommandProcessingFlags.BeforeValue);
                                }
                                else return true;
                            }
                            goto Next;
                        #endregion
                        
                        #region Value
                        case CommandToken.ArgumentValue:
                            {
                                fragment = new CommandFragment(CommandToken.Unknown, this, 0, buffer.Length);
                            }
                            return true;
                        #endregion
                        
                        #region Verb
                        case CommandToken.FloatingValue:
                            {
                                fragment = new CommandFragment(options.FlagSet(CommandReaderOptions.AllowVerbValues) ? CommandToken.Verb : CommandToken.Unknown, this, 0, buffer.Length);
                            }
                            return true;
                        #endregion

                        #region EOF
                        case CommandToken.EndOfStream: return false;
                        #endregion
                        
                        #region Invalid
                        default: goto Next;
                        #endregion
                    }
                }
                #endregion
                
                #region BeforeValue
                case CommandProcessingFlags.BeforeValue:
                {
                    CommandToken token = ReadNext();
                    switch (token)
                    {
                        #region Value
                        case CommandToken.ArgumentValue:
                        case CommandToken.FloatingValue:
                            {
                                fragment = new CommandFragment(fragment.Type, this, buffer.Offset, buffer.Length -  buffer.Offset);
                                state.Reset();
                            }
                            return true;
                        #endregion
                        
                        #region Invalid
                        default:
                            {
                                preserved = token;
                                state.Reset();
                            }
                            return true;
                        #endregion
                    }
                }
                #endregion
            }
            throw new NotImplementedException();
        }
        /// <summary>
        /// Reads the next token from the input stream and advances it's position by one
        /// </summary>
        /// <returns>True if a token could be read properly, false otherwise</returns>
        public new bool Read()
        {
            return Read(Array.Empty<Option>());
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        CommandToken ReadNext()
        {
            if (preserved != CommandToken.EndOfStream)
            {
                CommandToken token = preserved;
                preserved = CommandToken.EndOfStream;
                
                return token;
            }
            else return base.Read();
        }

        #region ITextProvider Members
        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        TextSource<TextBuffer> ITextProvider<TextBuffer>.GetSource(int startIndex, int length)
        {
            return new TextSource<TextBuffer>(ref buffer, startIndex, length);
        }
        #endregion
    }
}
