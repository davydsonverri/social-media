using System.Runtime.CompilerServices;
using System.Security.Cryptography;

namespace Domain.Identity.ULID
{
    internal static class RandomProvider
    {
        [ThreadStatic]
        private static Random random;

        [ThreadStatic]
        private static XorShift64 xorShift;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Random GetRandom()
        {
            if (random == null)
            {
                random = CreateRandom();
            }

            return random;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static Random CreateRandom()
        {
            using RandomNumberGenerator randomNumberGenerator = RandomNumberGenerator.Create();
            byte[] array = new byte[4];
            randomNumberGenerator.GetBytes(array);
            return new Random(BitConverter.ToInt32(array, 0));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static XorShift64 GetXorShift64()
        {
            if (xorShift == null)
            {
                xorShift = CreateXorShift64();
            }

            return xorShift;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static XorShift64 CreateXorShift64()
        {
            using RandomNumberGenerator randomNumberGenerator = RandomNumberGenerator.Create();
            byte[] array = new byte[8];
            randomNumberGenerator.GetBytes(array);
            return new XorShift64(BitConverter.ToUInt64(array, 0));
        }
    }
}
