// Copyright (C) 2017 Schroedinger Entertainment
// Distributed under the Schroedinger Entertainment EULA (See EULA.md for details)

using System.Runtime.CompilerServices;
using System.Text;
using Soe.Collections.HashSet;
using Soe.Collections.Inline;

namespace Soe.Runtime
{
    #if EXPORT_HAMPER_CORE_RUNTIME_COMMANDS
    public
    #else
    internal
    #endif
    static class InfoBuilder
    {
        private const int Margin = 2; 
        private const int Gap = 3;
        
        private struct OptionParameter : IHashContainer<string>
        {
            private readonly int hash;
            /// <inheritdoc/>
            public int Hash
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get { return hash; }
            }

            private SmallArray<string?, SmallArray2<string?>> names;
            private int count;
            
            /// <inheritdoc/>
            public string Key
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get { return names[0] ?? string.Empty; }
            }

            private string? description;
            /// <summary>
            /// Gets or sets this options description
            /// </summary>
            public string? Description
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get { return description; }
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                set { description = value; }
            }

            private OptionFlags flags;
            /// <summary>
            /// Gets or sets this options parameter flags
            /// </summary>
            public OptionFlags Flags
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get { return flags; }
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                set { flags = value; }
            }
            
            private int length;
            /// <summary>
            /// Gets the length of this options name
            /// </summary>
            public int Length
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get { return length; }
            }
            
            /// <inheritdoc/>
            public bool IsValid
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get { return (hash != 0 && count > 0); }
            }

            /// <summary>
            /// Initializes the parameter
            /// </summary>
            /// <param name="hash">The hash code of this parameter</param>
            /// <param name="name">The primary name of this parameter</param>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public OptionParameter(int hash, string name)
            {
                this.hash = hash;
                Append(name);
            }

            /// <summary>
            /// Gets a collection of names from this option
            /// </summary>
            /// <returns>The collection of names</returns>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Span<string?> AsSpan()
            {
                return names.AsSpan()
                    .Slice(0, count);
            }
            
            /// <summary>
            /// Adds the provided alias to this option
            /// </summary>
            /// <param name="alias">The alias to be added to this option</param>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void Append(string alias)
            {
                if (count > 0)
                {
                    length += 2;
                }
                EnsureCapacity(++count);
                names[count - 1] = alias;
                
                length += (alias.Length + (alias.Length == 1 ? 1 : 2));
            }
            
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            void EnsureCapacity(int capacity)
            {
                capacity = Math.Max(names.Length, capacity);
                if (names.Length < capacity)
                {
                    names.Resize(capacity.NextPowerOfTwo());
                }
            }
        }

        /// <summary>
        /// Creates an info text based on the provided command line options
        /// </summary>
        /// <param name="options">A collection of command line options to add</param>
        /// <param name="header">Determines if a header should be generated</param>
        /// <param name="headerPostfix">If generating a header, adds a postfix to the end of the line</param>
        /// <param name="terminalWidth">The space of the command window</param>
        /// <returns>The generated info text</returns>
        public static string Generate(ReadOnlySpan<Option> options, bool header = true, string? headerPostfix = null, int terminalWidth = 0)
        {
            HashSet<string, HeapArray<OptionParameter>, OptionParameter> commands = new HashSet<string, HeapArray<OptionParameter>, OptionParameter>(EqualityComparer<string>.Default);
            ConvertOptions(options, ref commands);
            
            StringBuilder result = new StringBuilder();
            if (header)
            {
                CreateParameterHeader(ref commands, result);
                if (!string.IsNullOrWhiteSpace(headerPostfix))
                {
                    if (result.Length > 0)
                    {
                        result.Append(' ');
                    }
                    result.Append(headerPostfix);
                }
                result.Append(Environment.NewLine);
            }
            CreateParameterList(ref commands, result, Math.Max(60, terminalWidth));
            return result.ToString();
        }

        static void ConvertOptions(ReadOnlySpan<Option> options, ref HashSet<string, HeapArray<OptionParameter>, OptionParameter> commands)
        {
            foreach(Option option in options)
            {
                string key; if (!option.FlagSet(OptionFlags.Alias))
                {
                    key = option.Name;
                }
                else if (option.Parameter != null)
                {
                    key = option.Parameter!;
                }
                else continue;
                
                int hash = key.GetHashCode();
                if (!commands.Find(key, hash, out int index, out int distance, out Ref<OptionParameter> parameter))
                {
                    parameter = new Ref<OptionParameter>(ref commands.Emplace(key, hash, index, distance, commands.Version));
                    if (!parameter.Value.IsValid)
                    {
                        parameter.Value = new OptionParameter(hash, key);
                    }
                }

                if (!option.FlagSet(OptionFlags.Alias))
                {
                    parameter.Value.Description = option.Description;
                    parameter.Value.Flags = option.Flags;
                }
                else parameter.Value.Append(option.Name);
            }
        }

        static void CreateParameterHeader(ref HashSet<string, HeapArray<OptionParameter>, OptionParameter> commands, StringBuilder result)
        {
            // Hash lookup isn't needed anymore, we can safely sort here
            commands.AsSpan().Sort(HeaderComparer);

            foreach (OptionParameter parameter in commands.AsSpan())
            {
                if (parameter.IsValid && (parameter.Flags & OptionFlags.NoHeader) == 0)
                {
                    if (result.Length > 0)
                    {
                        result.Append(' ');
                    }
                    if ((parameter.Flags & OptionFlags.Required) == 0 && (parameter.Flags & OptionFlags.Optional) != 0)
                    {
                        result.Append('[');
                    }
                    else if ((parameter.Flags & OptionFlags.Optional) == 0 && (parameter.Flags & OptionFlags.Required) != 0)
                    {
                        result.Append('<');
                    }
                    
                    // Name lookup isn't needed anymore, we can safely sort here
                    parameter.AsSpan().Sort(NameComparer);
                    Span<string?> names = parameter.AsSpan();
                    for (int i = 0; i < names.Length; i++)
                    {
                        if (i > 0)
                        {
                            result.Append('|');
                        }
                        if((parameter.Flags & OptionFlags.Verb) == 0)
                        {
                            result.Append(FormatName(names[i]!));
                        }
                        else result.Append(names[i]!);
                    }
                    if((parameter.Flags & OptionFlags.Verb) == 0)
                    {
                        if ((parameter.Flags & OptionFlags.Flag) == 0 &&
                            (parameter.Flags & OptionFlags.RequiresParameter) != 0)
                        {
                            result.Append(" :<value>");
                        }
                        else if ((parameter.Flags & OptionFlags.Flag) == 0)
                        {
                            result.Append(" [:value]");
                        }
                    }
                    if ((parameter.Flags & OptionFlags.Required) == 0 && (parameter.Flags & OptionFlags.Optional) != 0)
                    {
                        result.Append(']');
                    }
                    else if ((parameter.Flags & OptionFlags.Optional) == 0 && (parameter.Flags & OptionFlags.Required) != 0)
                    {
                        result.Append('>');
                    }
                }
            }
        }
        static void CreateParameterList(ref HashSet<string, HeapArray<OptionParameter>, OptionParameter> commands, StringBuilder result, int terminalWidth)
        {
            int leftWidth = 10;
            foreach (OptionParameter parameter in commands.AsSpan())
            {
                // Determines the max width of the parameter column content
                if (parameter.IsValid)
                {
                    int length = parameter.Length;
                    if ((parameter.Flags & OptionFlags.Flag) == 0 && (parameter.Flags & OptionFlags.RequiresParameter) != 0)
                    {
                        // _<value>
                        length += 8;
                    }
                    else if ((parameter.Flags & OptionFlags.Flag) == 0)
                    {
                        // _[value]
                        length += 8;
                    }

                    leftWidth = Math.Max(leftWidth, length);
                }
            }
            
            // Calculate the column widths
            leftWidth = Math.Min(leftWidth, Math.Max(10, (terminalWidth - Margin - Gap) / 2));
            int rightWidth = Math.Max(10, terminalWidth - Margin - Gap - leftWidth);

            SmallArray<string, SmallArray4<string>> left = default;
            SmallArray<string, SmallArray4<string>> right = default;
            StringBuilder builder = new StringBuilder();
            
            // Hash lookup isn't needed anymore, we can safely sort here
            commands.AsSpan().Sort(ListComparer);
            foreach (OptionParameter parameter in commands.AsSpan())
            {
                if(parameter.IsValid)
                {
                    builder.Clear();
                    
                    // Name lookup isn't needed anymore, we can safely sort here
                    parameter.AsSpan().Sort(NameComparer);
                    foreach (string? name in parameter.AsSpan())
                    {
                        // Build the parameter line
                        if (builder.Length > 0)
                        {
                            builder.Append(", ");
                        }
                        if((parameter.Flags & OptionFlags.Verb) == 0)
                        {
                            builder.Append(FormatName(name!));
                        }
                        else builder.Append(name!);
                    }
                    if ((parameter.Flags & OptionFlags.Verb) == 0)
                    {
                        if ((parameter.Flags & OptionFlags.Flag) == 0 && (parameter.Flags & OptionFlags.RequiresParameter) != 0)
                        {
                            builder.Append(" <value>");
                        }
                        else if ((parameter.Flags & OptionFlags.Flag) == 0)
                        {
                            builder.Append(" [value]");
                        }
                    }

                    // Wrap column contents to fit the provided space
                    int leftLines = WrapText(ref left, builder, builder.ToString(), leftWidth);
                    int rightLines = WrapText(ref right, builder, parameter.Description, rightWidth);
                    for (int i = 0, count = Math.Max(leftLines, rightLines); i < count; i++)
                    {
                        // Build columns
                        string item = (i < leftLines ? left[i] : string.Empty);
                        result.Append(' ', Margin);
                        result.Append(item.PadRight(leftWidth));

                        item = (i < rightLines ? right[i] : string.Empty);
                        result.Append(' ', Gap);
                        result.AppendLine(item);
                    }
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static int HeaderComparer(OptionParameter x, OptionParameter y)
        {
            if((x.Flags & OptionFlags.Verb) == (y.Flags & OptionFlags.Verb))
            {
                return Comparer<string>.Default.Compare(x.Key, y.Key);
            }
            else if ((x.Flags & OptionFlags.Verb) != 0)
            {
                return 1;
            }
            else return -1;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static int ListComparer(OptionParameter x, OptionParameter y)
        {
            if((x.Flags & OptionFlags.Verb) == (y.Flags & OptionFlags.Verb))
            {
                return Comparer<string>.Default.Compare(x.Key, y.Key);
            }
            else if ((x.Flags & OptionFlags.Verb) != 0)
            {
                return -1;
            }
            else return 1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static int NameComparer(string? x, string? y)
        {
            return Comparer<string>.Default.Compare(x, y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static string FormatName(string name)
        {
            return (name.Length == 1 ? string.Concat("-", name) : string.Concat("--", name));
        }

        static int WrapText<StringBuffer>(ref StringBuffer buffer, StringBuilder builder, string? text, int maxWidth)
            where StringBuffer : struct, IArrayAccessor<string>
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                buffer[0] = string.Empty;
                return 1;
            }
            else
            {
                int count = 0;
                maxWidth = Math.Max(1, maxWidth);
                foreach (string paragraph in text.Split('\n'))
                {
                    builder.Clear();
                    foreach (string word in paragraph.Split(' ', StringSplitOptions.RemoveEmptyEntries))
                    {
                        if (builder.Length > 0 && builder.Length + 1 + word.Length > maxWidth)
                        {
                            Append(ref buffer, builder.ToString(), ref count);
                            builder.Clear();
                        }
                        if (builder.Length > 0)
                        {
                            builder.Append(' ');
                        }
                        builder.Append(word);
                        while (builder.Length > maxWidth)
                        {
                            Append(ref buffer, builder.ToString(0, maxWidth), ref count);
                            builder.Remove(0, maxWidth);
                        }
                    }
                    if (builder.Length > 0)
                    {
                        Append(ref buffer, builder.ToString(), ref count);
                    }
                }
                return count;
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void Append<StringBuffer>(ref StringBuffer buffer, string value, ref int count)
            where StringBuffer : struct, IArrayAccessor<string>
        {
            EnsureCapacity(ref buffer, ++count);
            buffer[count - 1] = value;
        }
            
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void EnsureCapacity<StringBuffer>(ref StringBuffer buffer, int capacity)
            where StringBuffer : struct, IArrayAccessor<string>
        {
            capacity = Math.Max(buffer.Length, capacity);
            if (buffer.Length < capacity)
            {
                buffer.Resize(capacity.NextPowerOfTwo());
            }
        }
    }
}