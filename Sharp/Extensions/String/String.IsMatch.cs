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
    static partial class StringExtension
    {
        // ReSharper disable InvalidXmlDocComment
        
        /// <summary>
        /// Provides the character used to perform wildcard character checks
        /// </summary>
        public const char WildcardCharacterDelimiter = '?';
        /// <summary>
        /// Provides the character used to perform wildcard literal checks
        /// </summary>
        public const char WildcardLiteralDelimiter = '*';
        /// <summary>
        /// Provides the character used to perform character escaping
        /// </summary>
        public const char EscapeDelimiter = '\\';
        
        /// <summary>
        /// Indicates whether the specified expression pattern matches this input string
        /// </summary>
        /// <param name="pattern">A simple expression pattern to match</param>
        /// <param name="flag">An enumeration value that controls expression pattern matching</param>
        /// <returns>True if the expression pattern matches, false otherwise</returns>
        /// <remarks>The expression pattern allows for wildcard characters ? (single match) and * (matches a range). Those
        /// characters need to be escaped in order to match as a single character instead.
        /// 
        /// Pattern must be all lower case in order to allow MatchingFlags.IgnoreCase and MatchingFlags.Invariant
        /// to work</remarks>
        public static bool IsMatch(this string s, string pattern, MatchingFlags flag)
        {
            for (int stringIndex = 0, stringPointer = 0, stringLength = s.Length, patternIndex = 0, patternPointer = 0, patternLength = pattern.Length; patternIndex < patternLength || stringIndex < stringLength;)
            {
                if (patternIndex < patternLength)
                {
                    int controlChar = pattern[patternIndex];
                    switch (controlChar)
                    {
                        #region Wildcard character
                        case WildcardCharacterDelimiter: if (stringIndex < stringLength)
                            {
                                patternIndex++;
                                stringIndex++;
                                continue;
                            }
                            else break;
                        #endregion

                        #region Wildcard literal
                        case WildcardLiteralDelimiter: if (patternIndex + 1 < patternLength)
                            {
                                patternPointer = patternIndex++;
                                stringPointer = stringIndex + 1;
                                continue;
                            }
                            else return true;
                        #endregion

                        #region Escape character
                        case EscapeDelimiter: if (patternIndex + 1 < patternLength && (controlChar == WildcardCharacterDelimiter || controlChar == WildcardLiteralDelimiter || controlChar == EscapeDelimiter))
                            {
                                patternIndex++;
                            }
                            goto default;
                        #endregion

                        #region Anything else
                        default:
                            {
                                char current;
                                switch (flag)
                                {
                                    case MatchingFlags.IgnoreCase: current = Char.ToLower(s[stringIndex]); break;
                                    case MatchingFlags.Invariant: current = Char.ToLowerInvariant(s[stringIndex]); break;
                                    default: current = s[stringIndex]; break;
                                }
                                if (stringIndex < stringLength && current == controlChar)
                                {
                                    patternIndex++;
                                    stringIndex++;
                                    continue;
                                }
                                else break;
                            }
                        #endregion
                    }
                }

                #region Error checking
                if (stringPointer > 0 && stringPointer <= stringLength)
                {
                    patternIndex = patternPointer;
                    stringIndex = stringPointer;
                }
                else return false;
                #endregion
            }
            return true;
        }
        /// <summary>
        /// Indicates whether the specified expression pattern matches this input string
        /// </summary>
        /// <param name="pattern">A simple expression pattern to match</param>
        /// <returns>True if the expression pattern matches, false otherwise</returns>
        /// <remarks>The expression pattern allows for wildcard characters ? (single match) and * (matches a range). Those
        /// characters need to be escaped in order to match as a single character instead</remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsMatch(this string s, string pattern)
        {
            return  s.IsMatch(pattern, MatchingFlags.Default);
        }
    }
}
