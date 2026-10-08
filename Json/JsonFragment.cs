// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;
using System.Text;
using Soe.Parsing;
using ValueType = Soe.Parsing.ValueType;

namespace Soe.Json
{
    /// <summary>
    /// A JSON parser fragment
    /// </summary>
    #if EXPORT_HAMPER_CORE_JSON
    public
    #else
    internal
    #endif
    readonly struct JsonFragment : IParserFragment<TextBuffer>
    {
        private readonly ITextProvider<TextBuffer> provider;
        
        private readonly JsonToken type;
        /// <summary>
        /// Gets the type of this fragment
        /// </summary>
        /// <remarks>
        /// Must be one of (BeginObject, EndObject, BeginArray, EndArray, Null, Boolean, Numeric, String)
        /// </remarks>
        public JsonToken Type
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
        /// <param name="token">One of (BeginObject, EndObject, BeginArray, EndArray, Null, Boolean, Numeric, String)</param>
        /// <param name="provider">The root provider of this fragment</param>
        /// <param name="key">The length of the fragment name</param>
        /// <param name="value">The length of the fragment value</param>
        internal JsonFragment(JsonToken token, ITextProvider<TextBuffer> provider, int key, int value)
        {
            switch (token)
            {
                case JsonToken.BeginObject:
                case JsonToken.EndObject:
                case JsonToken.BeginArray:
                case JsonToken.EndArray:
                case JsonToken.Null:
                case JsonToken.Boolean:
                case JsonToken.Numeric:
                case JsonToken.String:
                    {
                        this.type = token;
                        this.provider = provider;
                        this.key = key;
                        this.value = value;
                    }
                    break;
                default: throw new ArgumentOutOfRangeException(nameof(token));
            }
        }
        
        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)] 
        public bool IsOfType(ValueType valueType)
                 {
                     switch (valueType)
                     {
                         case ValueType.Boolean: return (type == JsonToken.Boolean);
                         case ValueType.Numeric: return (type == JsonToken.Numeric);
                         default: return true;
                     }
                 }

        /// <summary>
        /// Converts this fragments value to <see cref="String"/>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString()
        {
            switch(type)
            {
                case JsonToken.Boolean:
                case JsonToken.Numeric:
                case JsonToken.String: return Value.ToString();
                default: return string.Empty;
            }
        }
    }
}
