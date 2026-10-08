// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace System
{
    /// <summary>
    /// Provides runtime unique ID numbers
    /// </summary>
    #if EXPORT_HAMPER_CORE_SHARP
    public
    #else
    internal
    #endif
    static class UniqueId
    {
        /// <summary>
        /// An ID generator using the Snowflake algorithm
        /// https://en.wikipedia.org/wiki/Snowflake_ID
        /// </summary>
        private struct IdGenerator(Int64 generatorId)
        {
            private readonly Int64 timeMask = GetBitMask(timestampBits);
            private readonly Int64 sequenceMask = GetBitMask(sequenceBits);
            private readonly int idUniqueBits = threadIdBits + sequenceBits;
            
            Int64 currentTimestamp = -1;
            int currentSequence = 0;

            /// <summary>
            /// Gets if the generator is properly initialized
            /// </summary>
            public bool IsValid
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get { return timeMask != 0 && sequenceMask != 0; }
            }
            
            /// <summary>
            /// Attempts to get the next unique ID
            /// </summary>
            /// <param name="id">The unique ID generated</param>
            /// <returns>True if a new unique ID was created, false otherwise</returns>
            public bool TryNext(out Int64 id)
            {
                Int64 ticks = clockCallback();
                Int64 timestamp = ticks & timeMask;
                if (timestamp < currentTimestamp || ticks < 0)
                {
                    id = 0;
                    return false;
                }
                else if (timestamp == currentTimestamp)
                {
                    if (currentSequence >= sequenceMask)
                    {
                        SpinWait wait = new SpinWait();
                        do
                        {
                            wait.SpinOnce();
                        }
                        while(currentTimestamp == clockCallback());
                    }
                    currentSequence++;
                }
                else
                {
                    currentSequence = 0;
                    currentTimestamp = timestamp;
                }
                unchecked
                {
                    id = (timestamp << idUniqueBits) + (generatorId << sequenceBits) + currentSequence;
                }
                return true;
            }
        }

        [ThreadStatic]
        private static IdGenerator generator;
        private static int nextId;

        private static byte timestampBits = 41;
        /// <summary>
        /// Gets or sets the number of bits used to store the timestamp
        /// </summary>
        public static byte TimestampBits
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return timestampBits; } 
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set { timestampBits = value; }
        }
        /// <summary>
        /// Gets the limit of intervals the generator can run
        /// </summary>
        public static Int64 MaxIntervals
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return (1L << timestampBits); }
        }

        private static byte threadIdBits = 10;
        /// <summary>
        /// Gets or sets the number of bits used to store the thread ID
        /// </summary>
        public static byte ThreadIdBits
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return threadIdBits; } 
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set { threadIdBits = value; }
        }
        /// <summary>
        /// Gets the limit of threads which can query IDs
        /// </summary>
        public static int MaxThreads
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return (1 << threadIdBits); }
        }

        private static byte sequenceBits = 12;
        /// <summary>
        /// Gets or sets the number of bits used to store the ID sequence
        /// </summary>
        public static byte SequenceBits
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return sequenceBits; } 
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set { sequenceBits = value; }
        }
        /// <summary>
        /// Gets the limit of sequential IDs for a determined time period
        /// </summary>
        public static int MaxSequences
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return (1 << sequenceBits); }
        }

        private static Func<Int64> clockCallback = Stopwatch.GetTimestamp;
        /// <summary>
        /// Gets or sets the tick clock callback used to obtain the timestamp needed to generate unique IDs
        /// </summary>
        public static Func<Int64> ClockCallback
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return clockCallback; } 
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set { clockCallback = value; }
        }

        /// <summary>
        /// Tries to obtain a new unique ID from the generator
        /// </summary>
        /// <returns>True if the attempt was successful, false otherwise</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryNext64(out Int64 id)
        {
            if (!generator.IsValid)
            {
                generator = new IdGenerator(Interlocked.Increment(ref nextId));
            }
            return generator.TryNext(out id);
        }
        /// <summary>
        /// Obtains a new unique ID from the generator
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int64 Next64()
        {
            if (TryNext64(out Int64 id))
            {
                throw new ArgumentOutOfRangeException();
            }
            else return id;
        }
        
        /// <summary>
        /// Tries to obtain a new unique ID from the generator
        /// </summary>
        /// <returns>True if the attempt was successful, false otherwise</returns>
        public static bool TryNext32(out int id)
        {
            if (TryNext64(out Int64 tmp))
            {
                id = (int)tmp.Fnv32();
                return true;
            }
            else
            {
                id = 0;
                return false;
            }
        }
        /// <summary>
        /// Obtains a new unique ID from the generator
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Next32()
        {
            if (!TryNext32(out int id))
            {
                throw new ArgumentOutOfRangeException();
            }
            else return id;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static Int64 GetBitMask(byte bits)
        {
            return (1L << bits) - 1;
        }
    }
}