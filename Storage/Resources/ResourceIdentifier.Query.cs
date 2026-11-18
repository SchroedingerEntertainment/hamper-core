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
        public readonly ref partial struct QueryComponent
        {
            /// <summary>
            /// Returns an iterator that iterates through the collection of volumes
            /// </summary>
            /// <returns>The iterator handling the underlying collection</returns>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public ReadOnlyQueryIterator GetEnumerator()
            {
                return new ReadOnlyQueryIterator(Args);
            }
        }
        
        /// <summary>
        /// Handles iteration over a collection of read-only arguments
        /// </summary>
        /// <param name="source">The argument string to parse</param>
        [method: MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref struct ReadOnlyQueryIterator(ReadOnlySpan<char> source) : IReadOnlyIterator<ReadOnlySpanTuple<char, char>>
        {
            private readonly ReadOnlySpan<char> source = source;
            private Range property;
            private Range value;
            private int index;
            
            // ReSharper disable MemberHidesStaticFromOuterClass
            
            /// <summary>
            /// Gets the element in the collection at the current position of this iterator
            /// </summary>
            public ReadOnlySpanTuple<char, char> Current
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get { return ReadOnlySpanTuple.Create(source.Slice(property.Start.Value, property.End.Value),
                                                      source.Slice(value.Start.Value, value.End.Value)); }
            }
            
            // ReSharper restore MemberHidesStaticFromOuterClass

            /// <summary>
            /// Advances the iterator to the next element of the collection
            /// </summary>
            /// <returns>True if the iterator was successfully advanced to the next element, false otherwise</returns>
            public bool MoveNext()
            {
                if(index < source.Length)
                {
                    int propertyIndex = index;
                    int valueIndex = index;

                    for (; index < source.Length; index++)
                    {
                        switch (source[index])
                        {
                            #region Value Separator
                            case Character.ArgumentValueMarker:
                                if (valueIndex == propertyIndex)
                                {
                                    valueIndex = index;
                                }

                                break;
                            #endregion

                            #region Property Separator
                            case Character.ArgumentSeparator:
                                if (valueIndex > propertyIndex)
                                {
                                    property = new Range(propertyIndex, valueIndex - propertyIndex);
                                    value = new Range(valueIndex + 1, index - valueIndex - 1);
                                }
                                else
                                {
                                    property = new Range(propertyIndex, index - propertyIndex);
                                    value = new Range(0, 0);
                                }
                                return true;
                            #endregion
                        }
                    }

                    #region Last
                    if (valueIndex > propertyIndex)
                    {
                        property = new Range(propertyIndex, valueIndex - propertyIndex);
                        value = new Range(valueIndex + 1, source.Length - valueIndex - 1);
                    }
                    else
                    {
                        property = new Range(propertyIndex, source.Length - propertyIndex);
                        value = new Range(0, 0);
                    }

                    #endregion
                    
                    return true;
                }
                else return false;
            }

            /// <summary>
            /// Sets the iterator to its initial position
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void Reset()
            {
                index = 0;
            }
        }
    }
}