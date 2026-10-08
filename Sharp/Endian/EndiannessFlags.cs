// Copyright (C) 2017 Schroedinger Entertainment
// Distributed under the Schroedinger Entertainment EULA (See EULA.md for details)

namespace System
{
    /// <summary>
    /// Endianness is primarily expressed as big-endian or little-endian.
    /// A big-endian system stores the most significant byte of a word at
    /// the smallest memory address and the least significant byte at the
    /// largest
    /// </summary>
    #if EXPORT_SOE_SHARP_COMPATIBILITY
    public
    #else
    internal
    #endif
    enum EndiannessFlags
    {
        BigEndian,
        LittleEndian
    }
}
