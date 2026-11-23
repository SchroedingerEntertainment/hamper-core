// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.IO;

namespace System
{
    /// <summary>
    /// 32 bit Non-Cryptographic Hash Algorithm
    /// </summary>
    #if EXPORT_HAMPER_CORE_SHARP
    public
    #else
    internal
    #endif
    class FastHash
    {
        [InlineArray(16)]
        private struct HashMem
        {
            private const int DefaultCapacity = 16;
            private byte element0;
            
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Span<byte> AsSpan()
            {
                return MemoryMarshal.CreateSpan(ref element0, DefaultCapacity);
            }
        }
        
        private struct FastHashState
        {
            public UInt64 length;
            public UInt32 seed;
            public UInt32 v1;
            public UInt32 v2;
            public UInt32 v3;
            public UInt32 v4;
            public int memSize;
            public HashMem memory;
        };

        private const UInt32 Prime32_1 = 2654435761U;
        private const UInt32 Prime32_2 = 2246822519U;
        private const UInt32 Prime2_3 = 3266489917U;
        private const UInt32 Prime32_4 = 668265263U;
        private const UInt32 Prime32_5 = 374761393U;
        private FastHashState state;

        /// <summary>
        /// Creates a new initial hash value instance
        /// </summary>
        public FastHash()
        {
            Initialize();
        }
        void Initialize(UInt32 seed = 0)
        {
            state.seed = seed;
            state.v1 = seed + Prime32_1 + Prime32_2;
            state.v2 = seed + Prime32_2;
            state.v3 = seed + 0;
            state.v4 = seed - Prime32_1;
            state.length = 0;
            state.memSize = 0;
            state.memory = default;
        }

        /// <summary>
        /// Hashes certain block of data into current state hash
        /// </summary>
        /// <param name="data">A block of data to create hash values from</param>
        public bool AddHash(ReadOnlySpan<byte> data)
        {
            int index = 0;

            state.length += (UInt32)data.Length;
            if (state.memSize + data.Length < 16)
            {
                data.CopyTo(state.memory.AsSpan().Slice(state.memSize));
                state.memSize += data.Length;

                return true;
            }
            if (state.memSize > 0)
            {
                data.Slice(0, 16 - state.memSize)
                    .CopyTo(state.memory.AsSpan().Slice(state.memSize));

                state.v1 = Hash(state.v1, state.memory, index);
                index += 4;
                state.v2 = Hash(state.v2, state.memory, index);
                index += 4;
                state.v3 = Hash(state.v3, state.memory, index);
                index += 4;
                state.v4 = Hash(state.v4, state.memory, index);
                index = 0;

                state.memSize = 0;
            }
            if (index <= data.Length - 16)
            {
                int limit = data.Length - 16;
                UInt32 v1 = state.v1;
                UInt32 v2 = state.v2;
                UInt32 v3 = state.v3;
                UInt32 v4 = state.v4;
                do
                {
                    v1 = Hash(v1, data, index);
                    index += 4;
                    v2 = Hash(v2, data, index);
                    index += 4;
                    v3 = Hash(v3, data, index);
                    index += 4;
                    v4 = Hash(v4, data, index);
                    index += 4;
                }
                while (index <= limit);
                state.v1 = v1;
                state.v2 = v2;
                state.v3 = v3;
                state.v4 = v4;
            }
            if (index < data.Length)
            {
                data.Slice(index, data.Length - index)
                    .CopyTo(state.memory.AsSpan());
                state.memSize = data.Length - index;
            }
            return true;
        }
        /// <summary>
        /// Completes current state hash and returns the final result
        /// </summary>
        /// <returns>The final 32-bit hash value</returns>
        public UInt32 FinalizeHash()
        {
            UInt32 h32;

            int index = 0;
            if (state.length >= 16) h32 = RotateLeft(state.v1, 1) + RotateLeft(state.v2, 7) + RotateLeft(state.v3, 12) + RotateLeft(state.v4, 18);
            else h32 = state.seed + Prime32_5;

            h32 += (UInt32)state.length;
            while (index <= state.memSize - 4)
            {
                h32 += BitConverter.ToUInt32(state.memory.AsSpan().Slice(index)) * Prime2_3;
                h32 = RotateLeft(h32, 17) * Prime32_4;
                index += 4;
            }
            while (index < state.memSize)
            {
                h32 += state.memory[index] * Prime32_5;
                h32 = RotateLeft(h32, 11) * Prime32_1;
                index++;
            }
            h32 ^= h32 >> 15;
            h32 *= Prime32_2;
            h32 ^= h32 >> 13;
            h32 *= Prime2_3;
            h32 ^= h32 >> 16;
            return h32;
        }

        /// <summary>
        /// Computes a hash value for the given block of data
        /// </summary>
        /// <param name="data">A block of data to create a hash value from</param>
        /// <param name="seed">A 32-bit number to act as the seed for this hash</param>
        /// <returns>The final 32-bit hash value</returns>
        public static UInt32 Hash(ReadOnlySpan<byte> data, UInt32 seed = 0)
        {
            UInt32 h32;

            int index = 0;
            if (data.Length >= 16)
            {
                int limit = data.Length - 16;
                UInt32 v1 = seed + Prime32_1 + Prime32_2;
                UInt32 v2 = seed + Prime32_2;
                UInt32 v3 = seed + 0;
                UInt32 v4 = seed - Prime32_1;
                do
                {
                    v1 = Hash(v1, data, index);
                    index += 4;
                    v2 = Hash(v2, data, index);
                    index += 4;
                    v3 = Hash(v3, data, index);
                    index += 4;
                    v4 = Hash(v4, data, index);
                    index += 4;
                }
                while (index <= limit);
                h32 = RotateLeft(v1, 1) + RotateLeft(v2, 7) + RotateLeft(v3, 12) + RotateLeft(v4, 18);
            }
            else h32 = seed + Prime32_5;
            h32 += (UInt32)data.Length;

            while (index <= data.Length - 4)
            {
                h32 += BitConverter.ToUInt32(data.Slice(index)) * Prime2_3;
                h32 = RotateLeft(h32, 17) * Prime32_4;
                index += 4;
            }
            while (index < data.Length)
            {
                h32 += data[index] * Prime32_5;
                h32 = RotateLeft(h32, 11) * Prime32_1;
                index++;
            }
            h32 ^= h32 >> 15;
            h32 *= Prime32_2;
            h32 ^= h32 >> 13;
            h32 *= Prime2_3;
            h32 ^= h32 >> 16;
            return h32;
        }
        /// <summary>
        /// Computes a hash value for the given block of data
        /// </summary>
        /// <param name="stream">A stream to create a hash value from</param>
        /// <param name="size">The size of data to be processed</param>
        /// <param name="seed">A 32-bit number to act as the seed for this hash</param>
        /// <returns>The final 32-bit hash value</returns>
        public static UInt32 Hash(Stream stream, int size, UInt32 seed = 0)
        {
            UInt32 h32;

            int index = 0;
            Span<byte> buffer = stackalloc byte[16];
            if (size >= 16)
            {
                int limit = size - 16;
                UInt32 v1 = seed + Prime32_1 + Prime32_2;
                UInt32 v2 = seed + Prime32_2;
                UInt32 v3 = seed + 0;
                UInt32 v4 = seed - Prime32_1;
                do
                {
                    int loopIndex = 0;
                    ReadOnlySpan<byte> bytesRead = buffer.Slice(0, stream.Read(buffer));

                    v1 = Hash(v1, bytesRead, loopIndex);
                    loopIndex += 4;
                    v2 = Hash(v2, bytesRead, loopIndex);
                    loopIndex += 4;
                    v3 = Hash(v3, bytesRead, loopIndex);
                    loopIndex += 4;
                    v4 = Hash(v4, bytesRead, loopIndex);
                    loopIndex += 4;

                    index += loopIndex;
                }
                while (index <= limit);
                h32 = RotateLeft(v1, 1) + RotateLeft(v2, 7) + RotateLeft(v3, 12) + RotateLeft(v4, 18);
            }
            else h32 = seed + Prime32_5;
            h32 += (UInt32)size;

            buffer = buffer.Slice(0, 4);
            while (index <= size - 4)
            {
                ReadOnlySpan<byte> bytesRead = buffer.Slice(0, stream.Read(buffer));
                
                h32 += BitConverter.ToUInt32(bytesRead) * Prime2_3;
                h32 = RotateLeft(h32, 17) * Prime32_4;
                index += 4;
            }

            buffer = buffer.Slice(0, 1);
            while (index < size)
            {
                ReadOnlySpan<byte> bytesRead = buffer.Slice(0, stream.Read(buffer));
                
                h32 += bytesRead[0] * Prime32_5;
                h32 = RotateLeft(h32, 11) * Prime32_1;
                index++;
            }
            h32 ^= h32 >> 15;
            h32 *= Prime32_2;
            h32 ^= h32 >> 13;
            h32 *= Prime2_3;
            h32 ^= h32 >> 16;
            return h32;
        }
        /// <summary>
        /// Computes a hash value for the given block of data
        /// </summary>
        /// <param name="stream">A stream to create a hash value from</param>
        /// <param name="seed">A 32-bit number to act as the seed for this hash</param>
        /// <returns>The final 32-bit hash value</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt32 Hash(Stream stream, UInt32 seed = 0)
        {
            return Hash(stream, (int)stream.Length, seed);
        }
        static UInt32 Hash(UInt32 value, ReadOnlySpan<byte> data, int index)
        {
            UInt32 tmp = BitConverter.ToUInt32(data.Slice(index));
            value += tmp * Prime32_2;
            value = RotateLeft(value, 13);
            value *= Prime32_1;

            return value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static UInt32 RotateLeft(UInt32 value, int count)
        {
            return (value << count) | (value >> (32 - count));
        }
    }
}