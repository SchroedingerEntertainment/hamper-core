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
        /// Combines two target components into a single target
        /// </summary>
        /// <param name="target1">The first component to combine</param>
        /// <param name="target2">The second component to combine</param>
        /// <returns>An n-value tuple representing the combined target</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpanTuple<char, char> Combine(TargetComponent target1, TargetComponent target2)
        {
            if (target2.IsRooted)
            {
                return ReadOnlySpanTuple.Create(target1.FullName, target2.FullName.Slice(1));
            }
            else return ReadOnlySpanTuple.Create(target1.FullName, target2.FullName);
        }
        /// <summary>
        /// Combines two target components into a single target
        /// </summary>
        /// <param name="target1">The first component to combine</param>
        /// <param name="target2">The second component to combine</param>
        /// <returns>An n-value tuple representing the combined target</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpanTuple<char, char> Combine(ReadOnlySpan<char> target1, TargetComponent target2)
        {
            if (target2.IsRooted)
            {
                return ReadOnlySpanTuple.Create(target1, target2.FullName.Slice(1));
            }
            else return ReadOnlySpanTuple.Create(target1, target2.FullName);
        }
        /// <summary>
        /// Appends a target component to the provided n-value tuple
        /// </summary>
        /// <param name="tuple">The tuple to append the target component to</param>
        /// <param name="target">The target component to append</param>
        /// <returns>An n-value tuple representing the combined target</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpanTuple<char, char, char> Combine(ReadOnlySpanTuple<char, char> tuple, TargetComponent target)
        {
            if (target.IsRooted)
            {
                return ReadOnlySpanTuple.Create(tuple.Item1, tuple.Item2, target.FullName.Slice(1));
            }
            else return ReadOnlySpanTuple.Create(tuple.Item1, tuple.Item2, target.FullName);
        }
        /// <summary>
        /// Appends a target component to the provided n-value tuple
        /// </summary>
        /// <param name="tuple">The tuple to append the target component to</param>
        /// <param name="target">The target component to append</param>
        /// <returns>An n-value tuple representing the combined target</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpanTuple<char, char, char, char> Combine(ReadOnlySpanTuple<char, char, char> tuple, TargetComponent target)
        {
            if (target.IsRooted)
            {
                return ReadOnlySpanTuple.Create(tuple.Item1, tuple.Item2, tuple.Item3, target.FullName.Slice(1));
            }
            else return ReadOnlySpanTuple.Create(tuple.Item1, tuple.Item2, tuple.Item3, target.FullName);
        }
        /// <summary>
        /// Appends a target component to the provided n-value tuple
        /// </summary>
        /// <param name="tuple">The tuple to append the target component to</param>
        /// <param name="target">The target component to append</param>
        /// <returns>An n-value tuple representing the combined target</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpanTuple<char, char, char, char, char> Combine(ReadOnlySpanTuple<char, char, char, char> tuple, TargetComponent target)
        {
            if (target.IsRooted)
            {
                return ReadOnlySpanTuple.Create(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, target.FullName.Slice(1));
            }
            else return ReadOnlySpanTuple.Create(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, target.FullName);
        }
        /// <summary>
        /// Appends a target component to the provided n-value tuple
        /// </summary>
        /// <param name="tuple">The tuple to append the target component to</param>
        /// <param name="target">The target component to append</param>
        /// <returns>An n-value tuple representing the combined target</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpanTuple<char, char, char, char, char, char> Combine(ReadOnlySpanTuple<char, char, char, char, char> tuple, TargetComponent target)
        {
            if (target.IsRooted)
            {
                return ReadOnlySpanTuple.Create(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, target.FullName.Slice(1));
            }
            else return ReadOnlySpanTuple.Create(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, target.FullName);
        }
        /// <summary>
        /// Appends a target component to the provided n-value tuple
        /// </summary>
        /// <param name="tuple">The tuple to append the target component to</param>
        /// <param name="target">The target component to append</param>
        /// <returns>An n-value tuple representing the combined target</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpanTuple<char, char, char, char, char, char, char> Combine(ReadOnlySpanTuple<char, char, char, char, char, char> tuple, TargetComponent target)
        {
            if (target.IsRooted)
            {
                return ReadOnlySpanTuple.Create(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, target.FullName.Slice(1));
            }
            else return ReadOnlySpanTuple.Create(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, target.FullName);
        }
        /// <summary>
        /// Combines three target components into a single target
        /// </summary>
        /// <param name="target1">The first component to combine</param>
        /// <param name="target2">The second component to combine</param>
        /// <param name="target3">The third component to combine</param>
        /// <returns>An n-value tuple representing the combined target</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpanTuple<char, char, char> Combine(TargetComponent target1, TargetComponent target2, TargetComponent target3)
        {
            ReadOnlySpan<char> segment2 = target2.FullName;
            if (target2.IsRooted)
            {
                segment2 = segment2.Slice(1);
            }
            ReadOnlySpan<char> segment3 = target3.FullName;
            if (target3.IsRooted)
            {
                segment3 = segment3.Slice(1);
            }
            return ReadOnlySpanTuple.Create(target1.FullName, segment2, segment3);
        }
        /// <summary>
        /// Combines three target components into a single target
        /// </summary>
        /// <param name="target1">The first component to combine</param>
        /// <param name="target2">The second component to combine</param>
        /// <param name="target3">The third component to combine</param>
        /// <returns>An n-value tuple representing the combined target</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpanTuple<char, char, char> Combine(ReadOnlySpan<char> target1, TargetComponent target2, TargetComponent target3)
        {
            ReadOnlySpan<char> segment2 = target2.FullName;
            if (target2.IsRooted)
            {
                segment2 = segment2.Slice(1);
            }
            ReadOnlySpan<char> segment3 = target3.FullName;
            if (target3.IsRooted)
            {
                segment3 = segment3.Slice(1);
            }
            return ReadOnlySpanTuple.Create(target1, segment2, segment3);
        }
        /// <summary>
        /// Combines four target components into a single target
        /// </summary>
        /// <param name="target1">The first component to combine</param>
        /// <param name="target2">The second component to combine</param>
        /// <param name="target3">The third component to combine</param>
        /// <param name="target4">The fourth component to combine</param>
        /// <returns>An n-value tuple representing the combined target</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpanTuple<char, char, char, char> Combine(TargetComponent target1, TargetComponent target2, TargetComponent target3, TargetComponent target4)
        {
            ReadOnlySpan<char> segment2 = target2.FullName;
            if (target2.IsRooted)
            {
                segment2 = segment2.Slice(1);
            }
            ReadOnlySpan<char> segment3 = target3.FullName;
            if (target3.IsRooted)
            {
                segment3 = segment3.Slice(1);
            }
            ReadOnlySpan<char> segment4 = target4.FullName;
            if (target4.IsRooted)
            {
                segment4 = segment4.Slice(1);
            }
            return ReadOnlySpanTuple.Create(target1.FullName, segment2, segment3, segment4);
        }
        /// <summary>
        /// Combines four target components into a single target
        /// </summary>
        /// <param name="target1">The first component to combine</param>
        /// <param name="target2">The second component to combine</param>
        /// <param name="target3">The third component to combine</param>
        /// <param name="target4">The fourth component to combine</param>
        /// <returns>An n-value tuple representing the combined target</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpanTuple<char, char, char, char> Combine(ReadOnlySpan<char> target1, TargetComponent target2, TargetComponent target3, TargetComponent target4)
        {
            ReadOnlySpan<char> segment2 = target2.FullName;
            if (target2.IsRooted)
            {
                segment2 = segment2.Slice(1);
            }
            ReadOnlySpan<char> segment3 = target3.FullName;
            if (target3.IsRooted)
            {
                segment3 = segment3.Slice(1);
            }
            ReadOnlySpan<char> segment4 = target4.FullName;
            if (target4.IsRooted)
            {
                segment4 = segment4.Slice(1);
            }
            return ReadOnlySpanTuple.Create(target1, segment2, segment3, segment4);
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string Normalize(ReadOnlySpan<char> source, NormalizationFlags flags)
        {
            char pathSeparator = flags.FlagSet(NormalizationFlags.SystemSeparatorChar) ? System.IO.Path.DirectorySeparatorChar : Character.PathSeparator;
            StringBuilder sb = StringBuilderPool.Shared.Rent();
            try
            {
                Normalize(source,sb, pathSeparator, source.Length > 0 && (source[0] == Character.PathSeparator || source[0] == Character.PathSeparatorAlternative), flags);
                return sb.ToString();
            }
            finally
            {
                StringBuilderPool.Shared.Return(sb);
            }
        }
        /// <summary>
        /// Normalizes the provided source by removing any occurrences of specific separator chars and relative segments
        /// </summary>
        /// <param name="source">The source to be normalized</param>
        /// <param name="flags">Normalization settings for the resulting string</param>
        /// <returns>The normalized string representation of <paramref name="source"/></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string Normalize(ReadOnlySpanTuple<char, char> source, NormalizationFlags flags)
        {
            char pathSeparator = flags.FlagSet(NormalizationFlags.SystemSeparatorChar) ? System.IO.Path.DirectorySeparatorChar : Character.PathSeparator;
            StringBuilder sb = StringBuilderPool.Shared.Rent();
            try
            {
                source.ForEach(Normalize, (sb, pathSeparator, flags));
                return sb.ToString();
            }
            finally
            {
                StringBuilderPool.Shared.Return(sb);
            }
        }
        /// <summary>
        /// Normalizes the provided source by removing any occurrences of specific separator chars and relative segments
        /// </summary>
        /// <param name="source">The source to be normalized</param>
        /// <param name="flags">Normalization settings for the resulting string</param>
        /// <returns>The normalized string representation of <paramref name="source"/></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string Normalize(ReadOnlySpanTuple<char, char, char> source, NormalizationFlags flags)
        {
            char pathSeparator = flags.FlagSet(NormalizationFlags.SystemSeparatorChar) ? System.IO.Path.DirectorySeparatorChar : Character.PathSeparator;
            StringBuilder sb = StringBuilderPool.Shared.Rent();
            try
            {
                source.ForEach(Normalize, (sb, pathSeparator, flags));
                return sb.ToString();
            }
            finally
            {
                StringBuilderPool.Shared.Return(sb);
            }
        }
        /// <summary>
        /// Normalizes the provided source by removing any occurrences of specific separator chars and relative segments
        /// </summary>
        /// <param name="source">The source to be normalized</param>
        /// <param name="flags">Normalization settings for the resulting string</param>
        /// <returns>The normalized string representation of <paramref name="source"/></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string Normalize(ReadOnlySpanTuple<char, char, char, char> source, NormalizationFlags flags)
        {
            char pathSeparator = flags.FlagSet(NormalizationFlags.SystemSeparatorChar) ? System.IO.Path.DirectorySeparatorChar : Character.PathSeparator;
            StringBuilder sb = StringBuilderPool.Shared.Rent();
            try
            {
                source.ForEach(Normalize, (sb, pathSeparator, flags));
                return sb.ToString();
            }
            finally
            {
                StringBuilderPool.Shared.Return(sb);
            }
        }
        /// <summary>
        /// Normalizes the provided source by removing any occurrences of specific separator chars and relative segments
        /// </summary>
        /// <param name="source">The source to be normalized</param>
        /// <param name="flags">Normalization settings for the resulting string</param>
        /// <returns>The normalized string representation of <paramref name="source"/></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string Normalize(ReadOnlySpanTuple<char, char, char, char, char> source, NormalizationFlags flags)
        {
            char pathSeparator = flags.FlagSet(NormalizationFlags.SystemSeparatorChar) ? System.IO.Path.DirectorySeparatorChar : Character.PathSeparator;
            StringBuilder sb = StringBuilderPool.Shared.Rent();
            try
            {
                source.ForEach(Normalize, (sb, pathSeparator, flags));
                return sb.ToString();
            }
            finally
            {
                StringBuilderPool.Shared.Return(sb);
            }
        }
        /// <summary>
        /// Normalizes the provided source by removing any occurrences of specific separator chars and relative segments
        /// </summary>
        /// <param name="source">The source to be normalized</param>
        /// <param name="flags">Normalization settings for the resulting string</param>
        /// <returns>The normalized string representation of <paramref name="source"/></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string Normalize(ReadOnlySpanTuple<char, char, char, char, char, char> source, NormalizationFlags flags)
        {
            char pathSeparator = flags.FlagSet(NormalizationFlags.SystemSeparatorChar) ? System.IO.Path.DirectorySeparatorChar : Character.PathSeparator;
            StringBuilder sb = StringBuilderPool.Shared.Rent();
            try
            {
                source.ForEach(Normalize, (sb, pathSeparator, flags));
                return sb.ToString();
            }
            finally
            {
                StringBuilderPool.Shared.Return(sb);
            }
        }
        /// <summary>
        /// Normalizes the provided source by removing any occurrences of specific separator chars and relative segments
        /// </summary>
        /// <param name="source">The source to be normalized</param>
        /// <param name="flags">Normalization settings for the resulting string</param>
        /// <returns>The normalized string representation of <paramref name="source"/></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string Normalize(ReadOnlySpanTuple<char, char, char, char, char, char, char> source, NormalizationFlags flags)
        {
            char pathSeparator = flags.FlagSet(NormalizationFlags.SystemSeparatorChar) ? System.IO.Path.DirectorySeparatorChar : Character.PathSeparator;
            StringBuilder sb = StringBuilderPool.Shared.Rent();
            try
            {
                source.ForEach(Normalize, (sb, pathSeparator, flags));
                return sb.ToString();
            }
            finally
            {
                StringBuilderPool.Shared.Return(sb);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void Normalize(ReadOnlySpan<char> item, ReadOnlySpan<char> next, int index, ValueTuple<StringBuilder, char, NormalizationFlags> parameter)
        {
            Normalize(item, parameter.Item1, parameter.Item2, index == 0 && item.Length > 0 && (item[0] == Character.PathSeparator || item[0] == Character.PathSeparatorAlternative), parameter.Item3);
            if(parameter.Item1.Length > 0 && !next.IsEmpty && parameter.Item1[parameter.Item1.Length - 1] != parameter.Item2)
            {
                parameter.Item1.Append(parameter.Item2);
            }
        }
        static void Normalize(ReadOnlySpan<char> source, StringBuilder sb, char pathSeparator, bool isRooted, NormalizationFlags flags)
        {
            if (isRooted)
            {
                sb.Append(pathSeparator);
            }
            for (int segmentStart = 0, length = source.Length, normalizedStart = (flags.FlagSet(NormalizationFlags.AbsolutePath) ? 0 : sb.Length); segmentStart < length;)
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
                                sb.Length = lastSeparator + 1;
                            }
                            else sb.Length = normalizedStart;
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
            if (sb.Length > 1 && sb[sb.Length - 1] == pathSeparator)
            {
                sb.Length--;
            }
        }

        static int Expand(ReadOnlySpan<char> input, StringBuilder sb)
        {
            if (sb.Length == 0 && input[0] == '~')
            {
                int end = 1;
                while (end < input.Length && input[end] != Character.PathSeparator && input[end] != Character.PathSeparatorAlternative)
                {
                    end++;
                }
                
                // Home directory: ~, ~username, ~/ or ~\
                
                string homeDirectory = GetUserHomeDirectory(input.Slice(1, end - 1));
                if (!string.IsNullOrEmpty(homeDirectory))
                {
                    sb.Append(homeDirectory);
                    return end;
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
        static string GetUserHomeDirectory(ReadOnlySpan<char> username)
        {
            if (username.IsEmpty || username.SequenceEqual(Environment.GetEnvironmentVariable("USER"), OrdnialComparer.Default))
            {
                return Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            }
            else return string.Empty;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool IsVariableNameChar(char c)
        {
            return char.IsAsciiLetterOrDigit(c) || c == '_';
        }
    }
}