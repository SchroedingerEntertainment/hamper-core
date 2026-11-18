// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;
using System.Text;

namespace Soe.Parsing
{
    #if EXPORT_HAMPER_CORE_PARSING
    public
    #else
    internal
    #endif
    static partial class FragmentExtension
    {
        // ReSharper disable InvalidXmlDocComment
        
        /// <summary>
        /// Converts this fragment into the desired value type if applicable
        /// </summary>
        /// <param name="result">The converted value</param>
        /// <returns>True if conversion was successful, false otherwise</returns>
        public static bool ToBoolean<Source, TypedFragment>(this TypedFragment fragment, out bool result)
            where Source : ITextSource
            where TypedFragment : struct, IParserFragment<Source>
        {
            TextSource<Source> value = fragment.Value;
            if (value.Length >= 4 && value.Length <= 5)
            {
                Span<char> rawValue = stackalloc char[value.Length];
                value.CopyTo(rawValue, value.Length);
                
                if (!fragment.IsOfType(ValueType.Boolean) || !bool.TryParse(rawValue, out result))
                {
                    result = false;
                    return false;
                }
                else return true;
            }
            else
            {
                result = false;
                return false;
            }
        }
        
        /// <summary>
        /// Converts this fragment into the desired value type if applicable
        /// </summary>
        /// <param name="result">The converted value</param>
        /// <returns>True if conversion was successful, false otherwise</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ToInteger<Source, TypedFragment>(this TypedFragment fragment, out Int16 result)
            where Source : ITextSource
            where TypedFragment : struct, IParserFragment<Source>
        {
            if (ToDecimal<Source, TypedFragment>(fragment, out Decimal tmp))
            {
                result = Decimal.ToInt16(tmp);
                return true;
            }
            else
            {
                result = 0;
                return false;
            }
        }
        /// <summary>
        /// Converts this fragment into the desired value type if applicable
        /// </summary>
        /// <param name="result">The converted value</param>
        /// <returns>True if conversion was successful, false otherwise</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ToInteger<Source, TypedFragment>(this TypedFragment fragment, out UInt16 result)
            where Source : ITextSource
            where TypedFragment : struct, IParserFragment<Source>
        {
            if (ToDecimal<Source, TypedFragment>(fragment, out Decimal tmp))
            {
                result = Decimal.ToUInt16(tmp);
                return true;
            }
            else
            {
                result = 0;
                return false;
            }
        }
        /// <summary>
        /// Converts this fragment into the desired value type if applicable
        /// </summary>
        /// <param name="result">The converted value</param>
        /// <returns>True if conversion was successful, false otherwise</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ToInteger<Source, TypedFragment>(this TypedFragment fragment, out Int32 result)
            where Source : ITextSource
            where TypedFragment : struct, IParserFragment<Source>
        {
            if (ToDecimal<Source, TypedFragment>(fragment, out Decimal tmp))
            {
                result = Decimal.ToInt32(tmp);
                return true;
            }
            else
            {
                result = 0;
                return false;
            }
        }
        /// <summary>
        /// Converts this fragment into the desired value type if applicable
        /// </summary>
        /// <param name="result">The converted value</param>
        /// <returns>True if conversion was successful, false otherwise</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ToInteger<Source, TypedFragment>(this TypedFragment fragment, out UInt32 result)
            where Source : ITextSource
            where TypedFragment : struct, IParserFragment<Source>
        {
            if (ToDecimal<Source, TypedFragment>(fragment, out Decimal tmp))
            {
                result = Decimal.ToUInt32(tmp);
                return true;
            }
            else
            {
                result = 0;
                return false;
            }
        }
        /// <summary>
        /// Converts this fragment into the desired value type if applicable
        /// </summary>
        /// <param name="result">The converted value</param>
        /// <returns>True if conversion was successful, false otherwise</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ToInteger<Source, TypedFragment>(this TypedFragment fragment, out Int64 result)
            where Source : ITextSource
            where TypedFragment : struct, IParserFragment<Source>
        {
            if (ToDecimal<Source, TypedFragment>(fragment, out Decimal tmp))
            {
                result = Decimal.ToInt64(tmp);
                return true;
            }
            else
            {
                result = 0;
                return false;
            }
        }
        /// <summary>
        /// Converts this fragment into the desired value type if applicable
        /// </summary>
        /// <param name="result">The converted value</param>
        /// <returns>True if conversion was successful, false otherwise</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ToInteger<Source, TypedFragment>(this TypedFragment fragment, out UInt64 result)
            where Source : ITextSource
            where TypedFragment : struct, IParserFragment<Source>
        {
            if (ToDecimal<Source, TypedFragment>(fragment, out Decimal tmp))
            {
                result = Decimal.ToUInt64(tmp);
                return true;
            }
            else
            {
                result = 0;
                return false;
            }
        }
        /// <summary>
        /// Converts this fragment into the desired value type if applicable
        /// </summary>
        /// <param name="result">The converted value</param>
        /// <returns>True if conversion was successful, false otherwise</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ToInteger<Source, TypedFragment>(this TypedFragment fragment, out Decimal result)
            where Source : ITextSource
            where TypedFragment : struct, IParserFragment<Source>
        {
            if (ToDecimal<Source, TypedFragment>(fragment, out result))
            {
                result = Decimal.Truncate(result);
                return true;
            }
            else return false;
        }
        
        /// <summary>
        /// Converts this fragment into the desired value type if applicable
        /// </summary>
        /// <param name="result">The converted value</param>
        /// <returns>True if conversion was successful, false otherwise</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ToSingle<Source, TypedFragment>(this TypedFragment fragment, out float result)
            where Source : ITextSource
            where TypedFragment : struct, IParserFragment<Source>
        {
            if (ToDecimal<Source, TypedFragment>(fragment, out Decimal tmp))
            {
                result = Decimal.ToSingle(tmp);
                return true;
            }
            else
            {
                result = 0;
                return false;
            }
        }
        
        /// <summary>
        /// Converts this fragment into the desired value type if applicable
        /// </summary>
        /// <param name="result">The converted value</param>
        /// <returns>True if conversion was successful, false otherwise</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ToDouble<Source, TypedFragment>(this TypedFragment fragment, out double result)
            where Source : ITextSource
            where TypedFragment : struct, IParserFragment<Source>
        {
            if (ToDecimal<Source, TypedFragment>(fragment, out Decimal tmp))
            {
                result = Decimal.ToDouble(tmp);
                return true;
            }
            else
            {
                result = 0;
                return false;
            }
        }
        
        /// <summary>
        /// Converts this fragment into the desired value type if applicable
        /// </summary>
        /// <param name="result">The converted value</param>
        /// <returns>True if conversion was successful, false otherwise</returns>
        public static bool ToDecimal<Source, TypedFragment>(this TypedFragment fragment, out Decimal result)
            where Source : ITextSource
            where TypedFragment : struct, IParserFragment<Source>
        {
            TextSource<Source> value = fragment.Value;
            if (value.Length >= 0 && value.Length <= 32)
            {
                Span<char> rawValue = stackalloc char[value.Length];
                value.CopyTo(rawValue, value.Length);
                
                if (!fragment.IsOfType(ValueType.Numeric) || !Decimal.TryParse(rawValue, out result))
                {
                    result = 0;
                    return false;
                }
                else return true;
            }
            else
            {
                result = 0;
                return false;
            }
        }
        
        // ReSharper restore InvalidXmlDocComment
    }
}