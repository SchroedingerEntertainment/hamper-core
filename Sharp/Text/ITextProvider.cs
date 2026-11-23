// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

namespace System.Text
{
    /// <summary>
    /// Represents a text buffer providing <see cref="TextSource{Source}"/>
    /// </summary>
    #if EXPORT_HAMPER_CORE_SHARP
    public
    #else
    internal
    #endif
    interface ITextProvider<Source>
        where Source : ITextSource
    {
        /// <summary>
        /// Generates a <see cref="TextSource{Source}"/> from the internal buffer
        /// </summary>
        TextSource<Source> GetSource(int startIndex, int length);
    }
}