// Licensed to Schroedinger Entertainment (SOE) under the terms of the AGPLv3
// Licensed to you by SOE under the terms of the AGPLv3 or another OSI-approved license 

namespace System
{
    #if EXPORT_HAMPER_CORE_SHARP
    public
    #else
    internal
    #endif
    static partial class MathExtension
    {
        private static readonly int[] SmallPrimes =
        [
            2,
            3,
            5,
            7,
            11,
            13,
            17,
            19,
            23,
            29
        ];
        private static readonly int[] Indices =
        [
            1,
            7,
            11,
            13,
            17,
            19,
            23,
            29
        ];

        /// <summary>
        /// Processes next prime number greater than or equal to this value
        /// </summary>
        public static Int32 NextPrime(this Int32 i)
        {
            int lookupCount = SmallPrimes.Length;
            if (i <= SmallPrimes[lookupCount - 1])
            {
                for (int j = lookupCount - 1; j >= 0; j--)
                {
                    if (i > SmallPrimes[j])
                    {
                        return SmallPrimes[j + 1];
                    }
                }
            }

            lookupCount = Indices.Length;
            int l = 30;

            int k0 = i / l;
            int index = Indices.Length - 1;
            for (int j = 0; j < Indices.Length; j++)
            {
                if (Indices[j] >= i - k0 * l)
                {
                    index = j;
                    break;
                }
            }
            i = l * k0 + Indices[index];

            while (!i.IsPrime())
            {
                if (++index == lookupCount)
                {
                    ++k0;
                    index = 0;
                }
                i = l * k0 + Indices[index];
            }
            return i;
        }
        /// <summary>
        /// Processes next prime number greater than or equal to this value
        /// </summary>
        public static UInt32 NextPrime(this UInt32 i)
        {
            int lookupCount = SmallPrimes.Length;
            if (i <= SmallPrimes[lookupCount - 1])
            {
                for (int j = lookupCount - 1; j >= 0; j--)
                {
                    if (i > SmallPrimes[j])
                    {
                        return (UInt32)SmallPrimes[j + 1];
                    }
                }
            }

            lookupCount = Indices.Length;
            int l = 30;

            long k0 = i / l;
            int index = Indices.Length - 1;
            for (int j = 0; j < Indices.Length; j++)
            {
                if (Indices[j] >= i - k0 * l)
                {
                    index = j;
                    break;
                }
            }
            i = (UInt32)(l * k0 + Indices[index]);

            while (!i.IsPrime())
            {
                if (++index == lookupCount)
                {
                    ++k0;
                    index = 0;
                }
                i = (UInt32)(l * k0 + Indices[index]);
            }
            return i;
        }

        /// <summary>
        /// Gets if this value is a prime number
        /// </summary>
        public static bool IsPrime(this Int32 i)
        {
            for (int j = 3; j < SmallPrimes.Length; j++)
            {
                int p = SmallPrimes[j];
                int q = i / p;
                if (q < p)
                {
                    return true;
                }
                else if (i == q * p)
                {
                    return false;
                }
            }
            for (int j = 31;;)
            {
                int q = i / j;
                if (q < j)
                {
                    return true;
                }
                else if (i == q * j)
                {
                    return false;
                }
                j += 6;

                q = i / j;
                if (q < j)
                {
                    return true;
                }
                else if (i == q * j)
                {
                    return false;
                }
                j += 4;

                q = i / j;
                if (q < j)
                {
                    return true;
                }
                else if (i == q * j)
                {
                    return false;
                }
                j += 2;

                q = i / j;
                if (q < j)
                {
                    return true;
                }
                else if (i == q * j)
                {
                    return false;
                }
                j += 4;

                q = i / j;
                if (q < j)
                {
                    return true;
                }
                else if (i == q * j)
                {
                    return false;
                }
                j += 2;

                q = i / j;
                if (q < j)
                {
                    return true;
                }
                else if (i == q * j)
                {
                    return false;
                }
                j += 4;

                q = i / j;
                if (q < j)
                {
                    return true;
                }
                else if (i == q * j)
                {
                    return false;
                }
                j += 6;

                q = i / j;
                if (q < j)
                {
                    return true;
                }
                else if (i == q * j)
                {
                    return false;
                }
                j += 2;
            }
        }
        /// <summary>
        /// Gets if this value is a prime number
        /// </summary>
        public static bool IsPrime(this UInt32 i)
        {
            for (int j = 3; j < SmallPrimes.Length; j++)
            {
                int p = SmallPrimes[j];
                long q = i / p;
                if (q < p)
                {
                    return true;
                }
                else if (i == q * p)
                {
                    return false;
                }
            }
            for (int j = 31; ;)
            {
                long q = i / j;
                if (q < j)
                {
                    return true;
                }
                else if (i == q * j)
                {
                    return false;
                }
                j += 6;

                q = i / j;
                if (q < j)
                {
                    return true;
                }
                else if (i == q * j)
                {
                    return false;
                }
                j += 4;

                q = i / j;
                if (q < j)
                {
                    return true;
                }
                else if (i == q * j)
                {
                    return false;
                }
                j += 2;

                q = i / j;
                if (q < j)
                {
                    return true;
                }
                else if (i == q * j)
                {
                    return false;
                }
                j += 4;

                q = i / j;
                if (q < j)
                {
                    return true;
                }
                else if (i == q * j)
                {
                    return false;
                }
                j += 2;

                q = i / j;
                if (q < j)
                {
                    return true;
                }
                else if (i == q * j)
                {
                    return false;
                }
                j += 4;

                q = i / j;
                if (q < j)
                {
                    return true;
                }
                else if (i == q * j)
                {
                    return false;
                }
                j += 6;

                q = i / j;
                if (q < j)
                {
                    return true;
                }
                else if (i == q * j)
                {
                    return false;
                }
                j += 2;
            }
        }
    }
}
