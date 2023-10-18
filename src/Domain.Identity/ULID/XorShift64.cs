using System.Runtime.CompilerServices;

namespace Domain.Identity.ULID
{
    internal class XorShift64
    {
        private ulong x = 88172645463325252uL;

        public XorShift64(ulong seed)
        {
            if (seed != 0L)
            {
                x = seed;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ulong Next()
        {
            x ^= x << 7;
            return x ^= x >> 9;
        }
    }
}
