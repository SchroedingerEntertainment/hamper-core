// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;
using System.Text;
using Soe.Parsing;
using ValueType = Soe.Parsing.ValueType;

namespace Soe.Runtime
{
    /// <summary>
    /// A command parser fragment
    /// </summary>
    #if EXPORT_HAMPER_CORE_RUNTIME_COMMANDS
    public
    #else
    internal
    #endif
    readonly struct CommandFragment : IParserFragment<TextBuffer>
    {
        private readonly ITextProvider<TextBuffer> provider;
        
        private readonly CommandToken type;
        /// <summary>
        /// Gets the type of this fragment
        /// </summary>
        public CommandToken Type
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return type; }
        }

        private readonly int key;
        /// <inheritdoc/>
        public TextSource<TextBuffer> Key
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return provider.GetSource(0, key); }
        }

        private readonly int value;
        /// <inheritdoc/>
        public TextSource<TextBuffer> Value
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return provider.GetSource(key, value); }
        }

        /// <summary>
        /// Initializes this fragment from the provided values
        /// </summary>
        /// <param name="token">The token implementing this fragment</param>
        /// <param name="provider">The root provider of this fragment</param>
        /// <param name="key">The length of the fragment name</param>
        /// <param name="value">The length of the fragment value</param>
        internal CommandFragment(CommandToken token, ITextProvider<TextBuffer> provider, int key, int value)
        {
            this.type = token;
            this.provider = provider;
            this.key = key;
            this.value = value;
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)] 
        public bool IsOfType(ValueType valueType)
        {
            return true;
        }

        /// <summary>
        /// Converts this fragments value to <see cref="String"/>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
