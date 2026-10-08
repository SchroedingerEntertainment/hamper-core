// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;
using System.Text;
using Soe.Pooling;

namespace Soe.Storage
{
    #if EXPORT_HAMPER_CORE_STORAGE
    public
    #else
    internal
    #endif
    readonly partial struct ResourceIdentifier
    {
        public readonly ref partial struct TargetComponent
        {
            /// <summary>
            /// Gets the parent of this target
            /// </summary>
            public ReadOnlySpan<char> Parent
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get { return GetParent(FullName); }
            }
            
            /// <summary>
            /// Returns a string that represents the current <see cref="TargetComponent.FullName"/> property
            /// </summary>
            /// <param name="flags">Normalization settings for the resulting string</param>
            /// <returns>The normalized string representation of the current <see cref="TargetComponent.FullName"/> property</returns>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public string ToString(NormalizationFlags flags)
            {
                return Normalize(FullName, flags);
            }
        }
        
        /// <summary>
        /// Returns a string that is the logical parent of <paramref name="source"/>
        /// </summary>
        /// <param name="source">The source to get the parent for</param>
        /// <returns>The target that is the logical parent of <paramref name="source"/>, or <see cref="ReadOnlySpan{T}.Empty"/></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpan<char> GetParent(ReadOnlySpan<char> source)
        {
            for (int i = source.Length - 1; i >= 0; i--)
            {
                if (source[i] == Character.PathSeparator || source[i]  == Character.PathSeparatorAlternative)
                {
                    return source.Slice(0, i);
                }
            }
            return ReadOnlySpan<char>.Empty;
        }
        
        /// <summary>
        /// Normalizes the provided source by removing any occurrences of specific separator chars and relative segments
        /// </summary>
        /// <param name="source">The source to be normalized</param>
        /// <param name="flags">Normalization settings for the resulting string</param>
        /// <returns>The normalized string representation of <paramref name="source"/></returns>
        public static string Normalize(ReadOnlySpan<char> source, NormalizationFlags flags)
        {
            char pathSeparator = flags.FlagSet(NormalizationFlags.SystemSeparatorChar) ? System.IO.Path.DirectorySeparatorChar : Character.PathSeparator;
            StringBuilder sb = StringBuilderPool.Shared.Rent();
            try
            {
                bool isRooted = (source.Length > 0 && (source[0] == Character.PathSeparator || source[0] == Character.PathSeparatorAlternative));
                if (isRooted)
                {
                    sb.Append(pathSeparator);
                }
                for (int segmentStart = 0, length = source.Length, normalizedStart = sb.Length; segmentStart < length;)
                {
                    while (segmentStart < length && (source[segmentStart] == Character.PathSeparator || source[segmentStart] == Character.PathSeparatorAlternative))
                    {
                        segmentStart++;
                    }
                    if (segmentStart < length)
                    {
                        int segmentEnd = segmentStart;
                        while (segmentEnd < length && source[segmentEnd] != Character.PathSeparator && source[segmentEnd] != Character.PathSeparatorAlternative)
                        {
                            segmentEnd++;
                        }
                        
                        ReadOnlySpan<char> segment = source.Slice(segmentStart, segmentEnd - segmentStart);
                        if (segment.SequenceEqual("."))
                        {
                            // Ignore current directory
                        }
                        else if (segment.SequenceEqual(".."))
                        {
                            if (sb.Length > normalizedStart)
                            {
                                int lastSeparator = -1;
                                for (int i = sb.Length - 1; i >= normalizedStart; i--)
                                {
                                    if (sb[i] == pathSeparator)
                                    {
                                        lastSeparator = i;
                                        break;
                                    }
                                }
                                if (lastSeparator >= normalizedStart)
                                {
                                    sb.Length = lastSeparator;
                                }
                                else
                                {
                                    sb.Length = normalizedStart;
                                }
                            }
                            else if (!isRooted && !flags.FlagSet(NormalizationFlags.AbsolutePath))
                            {
                                if (sb.Length > 0 && sb[sb.Length - 1] != pathSeparator)
                                {
                                    sb.Append(pathSeparator);
                                }
                                sb.Append("..");
                            }
                        }
                        else
                        {
                            if (sb.Length > 0 && sb[sb.Length - 1] != pathSeparator)
                            {
                                sb.Append(pathSeparator);
                            }
                            for (int i = 0; i < segment.Length; i++)
                            {
                                int offset = Expand(segment.Slice(i), sb);
                                if (offset == 0)
                                {
                                    sb.Append(segment[i]);
                                }
                                else i += (offset - 1);
                            }
                        }
                        segmentStart = segmentEnd;
                    }
                    else break;
                }
                return sb.ToString();
            }
            finally
            {
                StringBuilderPool.Shared.Return(sb);
            }
        }
        
        static int Expand(ReadOnlySpan<char> input, StringBuilder sb)
        {
            if (input[0] == '~' && (input.Length == 1 || input[1] == Character.PathSeparator || input[1] == Character.PathSeparatorAlternative))
            {
                // Home directory: ~, ~/ or ~\
                
                string value = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                if (!string.IsNullOrEmpty(value))
                {
                    sb.Append(value);
                    return 1;
                }
            }
            else if (input[0] == '%')
            {
                // Windows: %VARIABLE%
                
                int end = input.Slice(1).IndexOf('%');
                if (end >= 0)
                {
                    ReadOnlySpan<char> name = input.Slice(1, end);
                    if (!name.IsEmpty)
                    {
                        string? value = Environment.GetEnvironmentVariable(name.ToString());
                        if (!string.IsNullOrEmpty(value))
                        {
                            sb.Append(value);
                            return end + 2;
                        }
                    }
                }
            }
            else if (input[0] == '$')
            {
                // Unix: $VARIABLE, ${VARIABLE}, ${VARIABLE:-default}
                
                ReadOnlySpan<char> name;
                ReadOnlySpan<char> defaultValue = default;
                bool hasDefault = false;
                int consumed;

                if (input.Length > 1 && input[1] == '{')
                {
                    int end = input.Slice(2).IndexOf('}');
                    if (end > 0)
                    {
                        ReadOnlySpan<char> expression = input.Slice(2, end);
                        consumed = end + 3;

                        int separator = expression.IndexOf(":-");
                        if (separator >= 0)
                        {
                            name = expression.Slice(0, separator);
                            defaultValue = expression.Slice(separator + 2, expression.Length - separator - 2);
                            hasDefault = true;
                        }
                        else name = expression;
                    }
                    else return 0;
                }
                else
                {
                    int end = 1;
                    while (end < input.Length && IsVariableNameChar(input[end]))
                    {
                        end++;
                    }
                    if (end > 1)
                    {
                        name = input.Slice(1, end - 1);
                        consumed = end;
                    }
                    else return 0;
                }
                if (!name.IsEmpty)
                {
                    string? value = Environment.GetEnvironmentVariable(name.ToString());
                    if (string.IsNullOrEmpty(value) && hasDefault)
                    {
                        sb.Append(defaultValue);
                        return consumed;
                    }
                    else if (!string.IsNullOrEmpty(value))
                    {
                        sb.Append(value);
                        return consumed;
                    }
                }
            }
            return 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool IsVariableNameChar(char c)
        {
            return char.IsAsciiLetterOrDigit(c) || c == '_';
        }
    }
}