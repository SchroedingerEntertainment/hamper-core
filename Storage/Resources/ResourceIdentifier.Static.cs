// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;

namespace Soe.Storage
{
    #if EXPORT_HAMPER_CORE_STORAGE
    public
    #else
    internal
    #endif
    readonly partial struct ResourceIdentifier
    {
        /// <summary>
        /// Computes the corresponding metadata for a unified resource identifier 
        /// </summary>
        /// <param name="source">The source to compute the metadata from</param>
        /// <param name="normalize">Determines if <paramref name="source"/> should be normalized into default path separation</param>
        /// <returns>The computed metadata corresponding to the provided source</returns>
        /// <remarks>The unified resource identifier uses UNIX path separation by default</remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ResourceMetaData Parse(Span<char> source, bool normalize)
        {
            ResourceMetaData result = default;
            bool hasExtension = false;
            for (int i = 0, state = 0; i < source.Length; i++)
            {
                if (normalize && source[i] != Character.PathSeparatorAlternative)
                {
                    // Normalize Windows \ to UNIX /
                    source[i] = Character.PathSeparator;
                }
                Parse(ref result, ref hasExtension, ref state, ref i, source);
            }
            
            Fixup(ref result, source.Length);
            return result;
        }
        /// <summary>
        /// Computes the corresponding metadata for a unified resource identifier 
        /// </summary>
        /// <param name="source">The source to compute the metadata from</param>
        /// <returns>The computed metadata corresponding to the provided source</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ResourceMetaData Parse(ReadOnlySpan<char> source)
        {
            ResourceMetaData result = default;
            bool hasExtension = false;
            for (int i = 0, state = 0; i < source.Length; i++)
            {
                Parse(ref result, ref hasExtension, ref state, ref i, source);
            }
            
            Fixup(ref result, source.Length);
            return result;
        }
        /// <summary>
        /// Computes the corresponding metadata for a unified resource identifier 
        /// </summary>
        /// <param name="source">The source to compute the metadata from</param>
        /// <returns>The computed metadata corresponding to the provided source</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ResourceMetaData Parse(string source)
        {
            return Parse(source.AsSpan());
        }
        static void Parse(ref ResourceMetaData result, ref bool hasExtension, ref int state, ref int index, ReadOnlySpan<char> source)
        {
            switch (state)
            {
                #region Checked
                case 0: switch (source[index])
                    {
                        #region Remote path
                        case Character.PathSeparator:
                        case Character.PathSeparatorAlternative: if (source.Length > index + 1 && source[index + 1] == source[index])
                            {
                                #region UNC path
                                if (++index == 1)
                                {
                                    if(index <= byte.MaxValue)
                                    {
                                        result = new ResourceMetaData(result.Flags | IdentifierFlags.IsUnc, index + 1, index + 1, index + 1, index + 1, index + 1);
                                        state = 1;
                                    }
                                    else ThrowPathTooLong(byte.MaxValue);
                                }
                                #endregion

                                #region URI
                                else if(index + 1 <= byte.MaxValue)
                                {
                                    result = new ResourceMetaData(result.Flags | IdentifierFlags.IsUri, index + 1, index + 1, index + 1, index + 1, index + 1);
                                    state = 1;
                                }
                                else ThrowPathTooLong(byte.MaxValue);
                                #endregion
                            }
                        #endregion
                                
                        #region Local path
                            else
                            {
                                result = new ResourceMetaData(index > 0 ? (result.Flags | IdentifierFlags.IsRelative) : result.Flags, 0, 0, index + 1, index + 1, index + 1);
                                state = 2;
                            }
                            break;
                        #endregion
                            
                        #region Relative path
                        case Character.PathShiftDelimiter:
                            {
                                index--;
                                state = 2;
                            }
                            break;
                        #endregion
                            
                        #region Absolute path
                        case Character.DriveDelimiter: if(source.Length > index + 1)
                            {
                                switch (source[++index])
                                {
                                    #region Drive Identifier
                                    case Character.PathSeparator:
                                    case Character.PathSeparatorAlternative:
                                        if (source.Length <= index + 1 || source[index + 1] != source[index])
                                        {
                                            result = new ResourceMetaData(result.Flags, 0, index, index, index, index);
                                            state = 2;
                                        }
                                        else index--;
                                        break;
                                    #endregion
                                        
                                    #region Error
                                    default: throw new FormatException(string.Concat("Unexpected ':' character at ", index));
                                    #endregion
                                }
                            } 
                            break;
                        #endregion
                    }
                    break;
                #endregion
                    
                #region Before Path
                case 1: switch (source[index])
                    {
                        #region Path
                        case Character.PathSeparator:
                        case Character.PathSeparatorAlternative: if (index <= byte.MaxValue)
                            {
                                result = new ResourceMetaData(result.Flags, result.Authority, index, index + 1, index + 1, index + 1);
                                state = 2;
                            }
                            else ThrowPathTooLong(byte.MaxValue);
                            break;
                        #endregion
                            
                        #region Arguments
                        case Character.ArgumentMarker: if (index <= byte.MaxValue)
                            {
                                result = new ResourceMetaData(result.Flags, result.Authority, index, index, index, index);
                                index = source.Length;
                            }
                            else ThrowPathTooLong(byte.MaxValue);
                            break;
                        #endregion
                    }
                    break;
                #endregion
                    
                #region Path
                case 2: switch (source[index])
                    {
                        #region Path segment
                        case Character.PathSeparator: 
                        case Character.PathSeparatorAlternative: if(index + 1 <= UInt16.MaxValue)
                            {
                                if(index > 0 && index + 1 <  source.Length)
                                {
                                    result = new ResourceMetaData(result.Flags, result.Authority, result.Path, index + 1, index + 1, index + 1);
                                }
                            }
                            else ThrowPathTooLong(UInt16.MaxValue);
                            break;
                        #endregion

                        #region Shift
                        case Character.PathShiftDelimiter: if(source.Length > index + 1)
                            {
                                switch (source[index + 1])
                                {
                                    case Character.PathShiftDelimiter: if (source.Length > index + 2 && (source[index + 2] == Character.PathSeparator || source[index + 2] == Character.PathSeparatorAlternative))
                                        {
                                            #region ../ ..\
                                            index += 2;
                                            if(index <= UInt16.MaxValue)
                                            {
                                                result = new ResourceMetaData(result.Flags | IdentifierFlags.IsRelative, result.Authority, result.Path, index + 1, index + 1, index + 1);
                                            }
                                            else ThrowPathTooLong(UInt16.MaxValue);
                                            #endregion
                                        }
                                        else
                                        {
                                            #region .
                                            if(++index - result.Name <= byte.MaxValue)
                                            {
                                                result = new ResourceMetaData(result.Flags, result.Authority, result.Path, result.Name, index - result.Name, index + 1);
                                                hasExtension = true;
                                            }
                                            else ThrowPathTooLong(byte.MaxValue);
                                            #endregion
                                        }
                                        break;

                                    #region ./ .\
                                    case Character.PathSeparator:
                                    case Character.PathSeparatorAlternative: if (++index <= UInt16.MaxValue)
                                        {
                                            result = new ResourceMetaData(result.Flags | IdentifierFlags.IsRelative, result.Authority, result.Path, index + 1, index + 1, index + 1);
                                        }
                                        else ThrowPathTooLong(UInt16.MaxValue);
                                        break;
                                    #endregion
                                        
                                    #region Anything else
                                    default: if(index - result.Name <= byte.MaxValue)
                                        {
                                            result = new ResourceMetaData(result.Flags, result.Authority, result.Path, result.Name, index - result.Name, index + 1);
                                            hasExtension = true;
                                        }
                                        else ThrowPathTooLong(byte.MaxValue);
                                        break;
                                    #endregion
                                }
                            }
                        #endregion
                                
                        #region Extension
                            else if(index - result.Name <= byte.MaxValue)
                            {
                                result = new ResourceMetaData(result.Flags, result.Authority, result.Path, result.Name, index - result.Name, index + 1);
                                hasExtension = true;
                            }
                            else ThrowPathTooLong(byte.MaxValue);
                            break;
                        #endregion

                        #region Arguments
                        case Character.ArgumentMarker:
                        {
                            result = new ResourceMetaData(result.Flags, result.Authority, result.Path, result.Name, result.Extension, index);
                            index = source.Length;
                        }
                            break;
                        #endregion
                        
                        #region Anything else
                        default: if(index - result.Name <= byte.MaxValue)
                            {
                                result = new ResourceMetaData(result.Flags, result.Authority, result.Path, result.Name, hasExtension ? result.Extension : index + 1, index + 1);
                            }
                            else ThrowPathTooLong(byte.MaxValue);
                            break;
                        #endregion
                    }
                    break;
                #endregion
            }
        }

        static void Fixup(ref ResourceMetaData result, int length)
        {
            if (result.Path < result.Authority)
            {
                result = new ResourceMetaData(result.Flags, result.Authority, length, result.Name, result.Extension, result.Arguments);
            }
            if (result.Name < result.Path)
            {
                result = new ResourceMetaData(result.Flags, result.Authority, result.Path, result.Path, result.Extension, result.Arguments);
            }
            if (result.Arguments < result.Name)
            {
                result = new ResourceMetaData(result.Flags, result.Authority, result.Path, result.Name, result.Extension, length);
            }
            if (result.Extension > result.Arguments - result.Name)
            {
                result = new ResourceMetaData(result.Flags, result.Authority, result.Path, result.Name, result.Arguments - result.Name, result.Arguments);
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void ThrowPathTooLong(int length)
        {
            throw new PathTooLongException(string.Concat("Identifier segment is larger than ", length, " characters"));
        }
    }
}