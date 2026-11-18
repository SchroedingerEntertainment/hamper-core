// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;

namespace System
{
    #if EXPORT_HAMPER_CORE_SHARP
    public
    #else
    internal
    #endif
    static partial class SpanExtension
    {
        [method: MethodImpl(MethodImplOptions.AggressiveInlining)]
        readonly ref struct ParameterTuple(Span<char> destination, Ref<int> offset, char separator)
        {
            private readonly Span<char> destination = destination;
            /// <summary>
            /// Gets the string buffer to fill
            /// </summary>
            public Span<char> Destination
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get { return destination; }
            }

            private readonly Ref<int> offset = offset;
            /// <summary>
            /// Gets the current offset within the buffer
            /// </summary>
            public ref int Offset
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get { return ref offset.Value; }
            }

            /// <summary>
            /// Gets the separator to add between segments
            /// </summary>
            public char Separator
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get { return separator; }
            }
        }
        
        // ReSharper disable InvalidXmlDocComment
        
        /// <summary>
        /// Returns the string representation of this object
        /// </summary>
        /// <returns>The string representation of this object</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string Concat(this ReadOnlySpanTuple<char> tuple)
        {
            return tuple.Item1.ToString();
        }
        /// <summary>
        /// Returns the string representation of this object
        /// </summary>
        /// <returns>The string representation of this object</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string Concat(this ReadOnlySpanTuple<char, char> tuple)
        {
            return string.Create(tuple.Length, tuple, Create);
            void Create(Span<char> destination, ReadOnlySpanTuple<char, char> t)
            {
                int offset = 0;
                t.ForEach(FillString, new ParameterTuple(destination, new Ref<int>(ref offset), '\0'));
            }
        }
        /// <summary>
        /// Returns the string representation of this object
        /// </summary>
        /// <param name="separator">A separator to add between each segment</param>
        /// <returns>The string representation of this object</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string Concat(this ReadOnlySpanTuple<char, char> tuple, char separator)
        {
            int length = 0;
            tuple.ForEach(GetFinalLength, new Ref<int>(ref length));
            
            return string.Create(length, tuple, Create);
            void Create(Span<char> destination, ReadOnlySpanTuple<char, char> t)
            {
                int offset = 0;
                t.ForEach(FillString, new ParameterTuple(destination, new Ref<int>(ref offset), separator));
            }
        }
        /// <summary>
        /// Returns the string representation of this object
        /// </summary>
        /// <returns>The string representation of this object</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string Concat(this ReadOnlySpanTuple<char, char, char> tuple)
        {
            return string.Create(tuple.Length, tuple, Create);
            void Create(Span<char> destination, ReadOnlySpanTuple<char, char, char> t)
            {
                int offset = 0;
                t.ForEach(FillString, new ParameterTuple(destination, new Ref<int>(ref offset), '\0'));
            }
        }
        /// <summary>
        /// Returns the string representation of this object
        /// </summary>
        /// <param name="separator">A separator to add between each segment</param>
        /// <returns>The string representation of this object</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string Concat(this ReadOnlySpanTuple<char, char, char> tuple, char separator)
        {
            int length = 0;
            tuple.ForEach(GetFinalLength, new Ref<int>(ref length));
            
            return string.Create(length, tuple, Create);
            void Create(Span<char> destination, ReadOnlySpanTuple<char, char, char> t)
            {
                int offset = 0;
                t.ForEach(FillString, new ParameterTuple(destination, new Ref<int>(ref offset), separator));
            }
        }
        /// <summary>
        /// Returns the string representation of this object
        /// </summary>
        /// <returns>The string representation of this object</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string Concat(this ReadOnlySpanTuple<char, char, char, char> tuple)
        {
            return string.Create(tuple.Length, tuple, Create);
            void Create(Span<char> destination, ReadOnlySpanTuple<char, char, char, char> t)
            {
                int offset = 0;
                t.ForEach(FillString, new ParameterTuple(destination, new Ref<int>(ref offset), '\0'));
            }
        }
        /// <summary>
        /// Returns the string representation of this object
        /// </summary>
        /// <param name="separator">A separator to add between each segment</param>
        /// <returns>The string representation of this object</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string Concat(this ReadOnlySpanTuple<char, char, char, char> tuple, char separator)
        {
            int length = 0;
            tuple.ForEach(GetFinalLength, new Ref<int>(ref length));
            
            return string.Create(length, tuple, Create);
            void Create(Span<char> destination, ReadOnlySpanTuple<char, char, char, char> t)
            {
                int offset = 0;
                t.ForEach(FillString, new ParameterTuple(destination, new Ref<int>(ref offset), separator));
            }
        }
        /// <summary>
        /// Returns the string representation of this object
        /// </summary>
        /// <returns>The string representation of this object</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string Concat(this ReadOnlySpanTuple<char, char, char, char, char> tuple)
        {
            return string.Create(tuple.Length, tuple, Create);
            void Create(Span<char> destination, ReadOnlySpanTuple<char, char, char, char, char> t)
            {
                int offset = 0;
                t.ForEach(FillString, new ParameterTuple(destination, new Ref<int>(ref offset), '\0'));
            }
        }
        /// <summary>
        /// Returns the string representation of this object
        /// </summary>
        /// <param name="separator">A separator to add between each segment</param>
        /// <returns>The string representation of this object</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string Concat(this ReadOnlySpanTuple<char, char, char, char, char> tuple, char separator)
        {
            int length = 0;
            tuple.ForEach(GetFinalLength, new Ref<int>(ref length));
            
            return string.Create(length, tuple, Create);
            void Create(Span<char> destination, ReadOnlySpanTuple<char, char, char, char, char> t)
            {
                int offset = 0;
                t.ForEach(FillString, new ParameterTuple(destination, new Ref<int>(ref offset), separator));
            }
        }
        /// <summary>
        /// Returns the string representation of this object
        /// </summary>
        /// <returns>The string representation of this object</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string Concat(this ReadOnlySpanTuple<char, char, char, char, char, char> tuple)
        {
            return string.Create(tuple.Length, tuple, Create);
            void Create(Span<char> destination, ReadOnlySpanTuple<char, char, char, char, char, char> t)
            {
                int offset = 0;
                t.ForEach(FillString, new ParameterTuple(destination, new Ref<int>(ref offset), '\0'));
            }
        }
        /// <summary>
        /// Returns the string representation of this object
        /// </summary>
        /// <param name="separator">A separator to add between each segment</param>
        /// <returns>The string representation of this object</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string Concat(this ReadOnlySpanTuple<char, char, char, char, char, char> tuple, char separator)
        {
            int length = 0;
            tuple.ForEach(GetFinalLength, new Ref<int>(ref length));
            
            return string.Create(length, tuple, Create);
            void Create(Span<char> destination, ReadOnlySpanTuple<char, char, char, char, char, char> t)
            {
                int offset = 0;
                t.ForEach(FillString, new ParameterTuple(destination, new Ref<int>(ref offset), separator));
            }
        }
        /// <summary>
        /// Returns the string representation of this object
        /// </summary>
        /// <returns>The string representation of this object</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string Concat(this ReadOnlySpanTuple<char, char, char, char, char, char, char> tuple)
        {
            return string.Create(tuple.Length, tuple, Create);
            void Create(Span<char> destination, ReadOnlySpanTuple<char, char, char, char, char, char, char> t)
            {
                int offset = 0;
                t.ForEach(FillString, new ParameterTuple(destination, new Ref<int>(ref offset), '\0'));
            }
        }
        /// <summary>
        /// Returns the string representation of this object
        /// </summary>
        /// <param name="separator">A separator to add between each segment</param>
        /// <returns>The string representation of this object</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string Concat(this ReadOnlySpanTuple<char, char, char, char, char, char, char> tuple, char separator)
        {
            int length = 0;
            tuple.ForEach(GetFinalLength, new Ref<int>(ref length));
            
            return string.Create(length, tuple, Create);
            void Create(Span<char> destination, ReadOnlySpanTuple<char, char, char, char, char, char, char> t)
            {
                int offset = 0;
                t.ForEach(FillString, new ParameterTuple(destination, new Ref<int>(ref offset), separator));
            }
        }

        // ReSharper restore InvalidXmlDocComment

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void GetFinalLength(ReadOnlySpan<char> item, ReadOnlySpan<char> next, int index, Ref<int> param)
        {
            param.Value += item.Length;
            if (param.Value > 0 && !next.IsEmpty)
            {
                param.Value++;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void FillString(ReadOnlySpan<char> item, ReadOnlySpan<char> next, int index, ParameterTuple param)
        {
            item.CopyTo(param.Destination.Slice(param.Offset));
            param.Offset += item.Length;

            if (param.Separator != 0 && param.Offset > 0 && !next.IsEmpty)
            {
                param.Destination[param.Offset++] = param.Separator;
            }
        }
    }
}