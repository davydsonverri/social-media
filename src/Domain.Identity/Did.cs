using Domain.Identity.ULID;
using System.Buffers;
using System.Buffers.Binary;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;

namespace System
{
    //
    //     Summary:
    //     Represents a Universally Unique Lexicographically Sortable Identifier (ULID).
    //     Spec: https://github.com/ulid/spec
    //
    [StructLayout(LayoutKind.Explicit, Size = 16)]
    [DebuggerDisplay("{ToString(),nq}")]
    [TypeConverter(typeof(DidTypeConverter))]
    [JsonConverter(typeof(DidJsonConverter))]
    public readonly struct Did : IEquatable<Did>, IComparable<Did>, IComparable, ISpanFormattable, ISpanParsable<Did>
    {
        private static readonly char[] Base32Text = "0123456789ABCDEFGHJKMNPQRSTVWXYZ".ToCharArray();

        private static readonly byte[] Base32Bytes = Encoding.UTF8.GetBytes(Base32Text);

        private static readonly byte[] CharToBase32 = new byte[123]
        {
            255, 255, 255, 255, 255, 255, 255, 255, 255, 255,
            255, 255, 255, 255, 255, 255, 255, 255, 255, 255,
            255, 255, 255, 255, 255, 255, 255, 255, 255, 255,
            255, 255, 255, 255, 255, 255, 255, 255, 255, 255,
            255, 255, 255, 255, 255, 255, 255, 255, 0, 1,
            2, 3, 4, 5, 6, 7, 8, 9, 255, 255,
            255, 255, 255, 255, 255, 10, 11, 12, 13, 14,
            15, 16, 17, 255, 18, 19, 255, 20, 21, 255,
            22, 23, 24, 25, 26, 255, 27, 28, 29, 30,
            31, 255, 255, 255, 255, 255, 255, 10, 11, 12,
            13, 14, 15, 16, 17, 255, 18, 19, 255, 20,
            21, 255, 22, 23, 24, 25, 26, 255, 27, 28,
            29, 30, 31
        };

        private static readonly DateTimeOffset UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static readonly Did MinValue = new Did(UnixEpoch.ToUnixTimeMilliseconds(), new byte[10] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });

        public static readonly Did MaxValue = new Did(DateTimeOffset.MaxValue.ToUnixTimeMilliseconds(), new byte[10] { 255, 255, 255, 255, 255, 255, 255, 255, 255, 255 });

        public static readonly Did Empty = default(Did);

        [FieldOffset(0)]
        private readonly byte timestamp0;

        [FieldOffset(1)]
        private readonly byte timestamp1;

        [FieldOffset(2)]
        private readonly byte timestamp2;

        [FieldOffset(3)]
        private readonly byte timestamp3;

        [FieldOffset(4)]
        private readonly byte timestamp4;

        [FieldOffset(5)]
        private readonly byte timestamp5;

        [FieldOffset(6)]
        private readonly byte randomness0;

        [FieldOffset(7)]
        private readonly byte randomness1;

        [FieldOffset(8)]
        private readonly byte randomness2;

        [FieldOffset(9)]
        private readonly byte randomness3;

        [FieldOffset(10)]
        private readonly byte randomness4;

        [FieldOffset(11)]
        private readonly byte randomness5;

        [FieldOffset(12)]
        private readonly byte randomness6;

        [FieldOffset(13)]
        private readonly byte randomness7;

        [FieldOffset(14)]
        private readonly byte randomness8;

        [FieldOffset(15)]
        private readonly byte randomness9;

        [IgnoreDataMember]
        public byte[] Random => new byte[10] { randomness0, randomness1, randomness2, randomness3, randomness4, randomness5, randomness6, randomness7, randomness8, randomness9 };

        [IgnoreDataMember]
        public DateTimeOffset Time
        {
            get
            {
                if (BitConverter.IsLittleEndian)
                {
                    uint value = Unsafe.As<byte, uint>(ref Unsafe.AsRef(in timestamp0));
                    return DateTimeOffset.FromUnixTimeMilliseconds((long)(BinaryPrimitives.ReverseEndianness(Unsafe.As<byte, ushort>(ref Unsafe.AsRef(in timestamp4))) + ((ulong)BinaryPrimitives.ReverseEndianness(value) << 16)));
                }

                uint num = Unsafe.As<byte, uint>(ref Unsafe.AsRef(in timestamp0));
                ushort num2 = Unsafe.As<byte, ushort>(ref Unsafe.AsRef(in timestamp4));
                return DateTimeOffset.FromUnixTimeMilliseconds((long)(((ulong)num << 16) + num2));
            }
        }

        private static bool IsVector128Supported => Vector128.IsHardwareAccelerated;

        internal Did(long timestampMilliseconds, XorShift64 random)
        {
            this = default(Did);
            ref byte source = ref Unsafe.As<long, byte>(ref timestampMilliseconds);
            if (BitConverter.IsLittleEndian)
            {
                timestamp0 = Unsafe.Add(ref source, 5);
                timestamp1 = Unsafe.Add(ref source, 4);
                timestamp2 = Unsafe.Add(ref source, 3);
                timestamp3 = Unsafe.Add(ref source, 2);
                timestamp4 = Unsafe.Add(ref source, 1);
                timestamp5 = Unsafe.Add(ref source, 0);
            } else
            {
                timestamp0 = Unsafe.Add(ref source, 2);
                timestamp1 = Unsafe.Add(ref source, 3);
                timestamp2 = Unsafe.Add(ref source, 4);
                timestamp3 = Unsafe.Add(ref source, 5);
                timestamp4 = Unsafe.Add(ref source, 6);
                timestamp5 = Unsafe.Add(ref source, 7);
            }

            Unsafe.WriteUnaligned(ref randomness0, random.Next());
            Unsafe.WriteUnaligned(ref randomness2, random.Next());
        }

        internal Did(long timestampMilliseconds, ReadOnlySpan<byte> randomness)
        {
            this = default(Did);
            ref byte source = ref Unsafe.As<long, byte>(ref timestampMilliseconds);
            if (BitConverter.IsLittleEndian)
            {
                timestamp0 = Unsafe.Add(ref source, 5);
                timestamp1 = Unsafe.Add(ref source, 4);
                timestamp2 = Unsafe.Add(ref source, 3);
                timestamp3 = Unsafe.Add(ref source, 2);
                timestamp4 = Unsafe.Add(ref source, 1);
                timestamp5 = Unsafe.Add(ref source, 0);
            } else
            {
                timestamp0 = Unsafe.Add(ref source, 2);
                timestamp1 = Unsafe.Add(ref source, 3);
                timestamp2 = Unsafe.Add(ref source, 4);
                timestamp3 = Unsafe.Add(ref source, 5);
                timestamp4 = Unsafe.Add(ref source, 6);
                timestamp5 = Unsafe.Add(ref source, 7);
            }

            ref byte reference = ref MemoryMarshal.GetReference(randomness);
            randomness0 = randomness[0];
            randomness1 = randomness[1];
            Unsafe.WriteUnaligned(ref randomness2, Unsafe.As<byte, ulong>(ref Unsafe.Add(ref reference, 2)));
        }

        public Did(ReadOnlySpan<byte> bytes)
        {
            this = default(Did);
            if (bytes.Length != 16)
            {
                throw new ArgumentException("invalid bytes length, length:" + bytes.Length);
            }

            ref byte reference = ref MemoryMarshal.GetReference(bytes);
            Unsafe.WriteUnaligned(ref timestamp0, Unsafe.As<byte, ulong>(ref reference));
            Unsafe.WriteUnaligned(ref randomness2, Unsafe.As<byte, ulong>(ref Unsafe.Add(ref reference, 8)));
        }

        internal Did(ReadOnlySpan<char> base32)
        {
            randomness9 = (byte)((CharToBase32[(uint)base32[24]] << 5) | CharToBase32[(uint)base32[25]]);
            timestamp0 = (byte)((CharToBase32[(uint)base32[0]] << 5) | CharToBase32[(uint)base32[1]]);
            timestamp1 = (byte)((CharToBase32[(uint)base32[2]] << 3) | (CharToBase32[(uint)base32[3]] >> 2));
            timestamp2 = (byte)((CharToBase32[(uint)base32[3]] << 6) | (CharToBase32[(uint)base32[4]] << 1) | (CharToBase32[(uint)base32[5]] >> 4));
            timestamp3 = (byte)((CharToBase32[(uint)base32[5]] << 4) | (CharToBase32[(uint)base32[6]] >> 1));
            timestamp4 = (byte)((CharToBase32[(uint)base32[6]] << 7) | (CharToBase32[(uint)base32[7]] << 2) | (CharToBase32[(uint)base32[8]] >> 3));
            timestamp5 = (byte)((CharToBase32[(uint)base32[8]] << 5) | CharToBase32[(uint)base32[9]]);
            randomness0 = (byte)((CharToBase32[(uint)base32[10]] << 3) | (CharToBase32[(uint)base32[11]] >> 2));
            randomness1 = (byte)((CharToBase32[(uint)base32[11]] << 6) | (CharToBase32[(uint)base32[12]] << 1) | (CharToBase32[(uint)base32[13]] >> 4));
            randomness2 = (byte)((CharToBase32[(uint)base32[13]] << 4) | (CharToBase32[(uint)base32[14]] >> 1));
            randomness3 = (byte)((CharToBase32[(uint)base32[14]] << 7) | (CharToBase32[(uint)base32[15]] << 2) | (CharToBase32[(uint)base32[16]] >> 3));
            randomness4 = (byte)((CharToBase32[(uint)base32[16]] << 5) | CharToBase32[(uint)base32[17]]);
            randomness5 = (byte)((CharToBase32[(uint)base32[18]] << 3) | (CharToBase32[(uint)base32[19]] >> 2));
            randomness6 = (byte)((CharToBase32[(uint)base32[19]] << 6) | (CharToBase32[(uint)base32[20]] << 1) | (CharToBase32[(uint)base32[21]] >> 4));
            randomness7 = (byte)((CharToBase32[(uint)base32[21]] << 4) | (CharToBase32[(uint)base32[22]] >> 1));
            randomness8 = (byte)((CharToBase32[(uint)base32[22]] << 7) | (CharToBase32[(uint)base32[23]] << 2) | (CharToBase32[(uint)base32[24]] >> 3));
        }

        public Did(Guid guid)
        {
            if (IsVector128Supported && BitConverter.IsLittleEndian)
            {
                Vector128<byte> source = Shuffle(Unsafe.As<Guid, Vector128<byte>>(ref guid), Vector128.Create((byte)3, (byte)2, (byte)1, (byte)0, (byte)5, (byte)4, (byte)7, (byte)6, (byte)8, (byte)9, (byte)10, (byte)11, (byte)12, (byte)13, (byte)14, (byte)15));
                this = Unsafe.As<Vector128<byte>, Did>(ref source);
                return;
            }

            Span<byte> span = stackalloc byte[16];
            if (BitConverter.IsLittleEndian)
            {
                ref uint reference = ref Unsafe.As<Guid, uint>(ref guid);
                uint value = BinaryPrimitives.ReverseEndianness(reference);
                MemoryMarshal.Write(span, ref value);
                reference = ref Unsafe.Add(ref reference, 1);
                uint value2 = ((reference & 0xFF00FF) << 8) | ((reference & 0xFF00FF00u) >> 8);
                MemoryMarshal.Write(span.Slice(4), ref value2);
                MemoryMarshal.Write(value: ref Unsafe.As<uint, ulong>(ref Unsafe.Add(ref reference, 1)), destination: span.Slice(8));
            } else
            {
                MemoryMarshal.Write(span, ref guid);
            }

            this = MemoryMarshal.Read<Did>(span);
        }

        public static Did NewDid()
        {
            return new Did(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(), RandomProvider.GetXorShift64());
        }

        public static Did NewDid(DateTimeOffset timestamp)
        {
            return new Did(timestamp.ToUnixTimeMilliseconds(), RandomProvider.GetXorShift64());
        }

        public static Did NewDid(DateTimeOffset timestamp, ReadOnlySpan<byte> randomness)
        {
            if (randomness.Length != 10)
            {
                throw new ArgumentException("invalid randomness length, length:" + randomness.Length);
            }

            return new Did(timestamp.ToUnixTimeMilliseconds(), randomness);
        }

        public static Did Parse(string base32)
        {
            return Parse(base32.AsSpan());
        }

        public static Did Parse(ReadOnlySpan<char> base32)
        {
            if (base32.Length != 26)
            {
                throw new ArgumentException("invalid base32 length, length:" + base32.Length);
            }

            return new Did(base32);
        }

        public static Did Parse(ReadOnlySpan<byte> base32)
        {
            if (!TryParse(base32, out var did))
            {
                throw new ArgumentException("invalid base32 length, length:" + base32.Length);
            }

            return did;
        }

        public static bool TryParse(string base32, out Did did)
        {
            return TryParse(base32.AsSpan(), out did);
        }

        public static bool TryParse(ReadOnlySpan<char> base32, out Did did)
        {
            if (base32.Length != 26)
            {
                did = default(Did);
                return false;
            }

            try
            {
                did = new Did(base32);
                return true;
            } catch
            {
                did = default(Did);
                return false;
            }
        }

        public static bool TryParse(ReadOnlySpan<byte> base32, out Did did)
        {
            if (base32.Length != 26)
            {
                did = default(Did);
                return false;
            }

            try
            {
                did = ParseCore(base32);
                return true;
            } catch
            {
                did = default(Did);
                return false;
            }
        }

        private static Did ParseCore(ReadOnlySpan<byte> base32)
        {
            if (base32.Length != 26)
            {
                throw new ArgumentException("invalid base32 length, length:" + base32.Length);
            }

            Did source = default(Did);
            Unsafe.Add(ref Unsafe.As<Did, byte>(ref source), 15) = (byte)((CharToBase32[base32[24]] << 5) | CharToBase32[base32[25]]);
            Unsafe.Add(ref Unsafe.As<Did, byte>(ref source), 0) = (byte)((CharToBase32[base32[0]] << 5) | CharToBase32[base32[1]]);
            Unsafe.Add(ref Unsafe.As<Did, byte>(ref source), 1) = (byte)((CharToBase32[base32[2]] << 3) | (CharToBase32[base32[3]] >> 2));
            Unsafe.Add(ref Unsafe.As<Did, byte>(ref source), 2) = (byte)((CharToBase32[base32[3]] << 6) | (CharToBase32[base32[4]] << 1) | (CharToBase32[base32[5]] >> 4));
            Unsafe.Add(ref Unsafe.As<Did, byte>(ref source), 3) = (byte)((CharToBase32[base32[5]] << 4) | (CharToBase32[base32[6]] >> 1));
            Unsafe.Add(ref Unsafe.As<Did, byte>(ref source), 4) = (byte)((CharToBase32[base32[6]] << 7) | (CharToBase32[base32[7]] << 2) | (CharToBase32[base32[8]] >> 3));
            Unsafe.Add(ref Unsafe.As<Did, byte>(ref source), 5) = (byte)((CharToBase32[base32[8]] << 5) | CharToBase32[base32[9]]);
            Unsafe.Add(ref Unsafe.As<Did, byte>(ref source), 6) = (byte)((CharToBase32[base32[10]] << 3) | (CharToBase32[base32[11]] >> 2));
            Unsafe.Add(ref Unsafe.As<Did, byte>(ref source), 7) = (byte)((CharToBase32[base32[11]] << 6) | (CharToBase32[base32[12]] << 1) | (CharToBase32[base32[13]] >> 4));
            Unsafe.Add(ref Unsafe.As<Did, byte>(ref source), 8) = (byte)((CharToBase32[base32[13]] << 4) | (CharToBase32[base32[14]] >> 1));
            Unsafe.Add(ref Unsafe.As<Did, byte>(ref source), 9) = (byte)((CharToBase32[base32[14]] << 7) | (CharToBase32[base32[15]] << 2) | (CharToBase32[base32[16]] >> 3));
            Unsafe.Add(ref Unsafe.As<Did, byte>(ref source), 10) = (byte)((CharToBase32[base32[16]] << 5) | CharToBase32[base32[17]]);
            Unsafe.Add(ref Unsafe.As<Did, byte>(ref source), 11) = (byte)((CharToBase32[base32[18]] << 3) | (CharToBase32[base32[19]] >> 2));
            Unsafe.Add(ref Unsafe.As<Did, byte>(ref source), 12) = (byte)((CharToBase32[base32[19]] << 6) | (CharToBase32[base32[20]] << 1) | (CharToBase32[base32[21]] >> 4));
            Unsafe.Add(ref Unsafe.As<Did, byte>(ref source), 13) = (byte)((CharToBase32[base32[21]] << 4) | (CharToBase32[base32[22]] >> 1));
            Unsafe.Add(ref Unsafe.As<Did, byte>(ref source), 14) = (byte)((CharToBase32[base32[22]] << 7) | (CharToBase32[base32[23]] << 2) | (CharToBase32[base32[24]] >> 3));
            return source;
        }

        public byte[] ToByteArray()
        {
            byte[] array = new byte[16];
            Unsafe.WriteUnaligned(ref array[0], this);
            return array;
        }

        public bool TryWriteBytes(Span<byte> destination)
        {
            if (destination.Length < 16)
            {
                return false;
            }

            Unsafe.WriteUnaligned(ref MemoryMarshal.GetReference(destination), this);
            return true;
        }

        public string ToBase64(Base64FormattingOptions options = Base64FormattingOptions.None)
        {
            byte[] array = ArrayPool<byte>.Shared.Rent(16);
            try
            {
                TryWriteBytes(array);
                return Convert.ToBase64String(array, options);
            } finally
            {
                ArrayPool<byte>.Shared.Return(array);
            }
        }

        public bool TryWriteStringify(Span<byte> span)
        {
            if (span.Length < 26)
            {
                return false;
            }

            span[25] = Base32Bytes[randomness9 & 0x1F];
            span[0] = Base32Bytes[(timestamp0 & 0xE0) >> 5];
            span[1] = Base32Bytes[timestamp0 & 0x1F];
            span[2] = Base32Bytes[(timestamp1 & 0xF8) >> 3];
            span[3] = Base32Bytes[((timestamp1 & 7) << 2) | ((timestamp2 & 0xC0) >> 6)];
            span[4] = Base32Bytes[(timestamp2 & 0x3E) >> 1];
            span[5] = Base32Bytes[((timestamp2 & 1) << 4) | ((timestamp3 & 0xF0) >> 4)];
            span[6] = Base32Bytes[((timestamp3 & 0xF) << 1) | ((timestamp4 & 0x80) >> 7)];
            span[7] = Base32Bytes[(timestamp4 & 0x7C) >> 2];
            span[8] = Base32Bytes[((timestamp4 & 3) << 3) | ((timestamp5 & 0xE0) >> 5)];
            span[9] = Base32Bytes[timestamp5 & 0x1F];
            span[10] = Base32Bytes[(randomness0 & 0xF8) >> 3];
            span[11] = Base32Bytes[((randomness0 & 7) << 2) | ((randomness1 & 0xC0) >> 6)];
            span[12] = Base32Bytes[(randomness1 & 0x3E) >> 1];
            span[13] = Base32Bytes[((randomness1 & 1) << 4) | ((randomness2 & 0xF0) >> 4)];
            span[14] = Base32Bytes[((randomness2 & 0xF) << 1) | ((randomness3 & 0x80) >> 7)];
            span[15] = Base32Bytes[(randomness3 & 0x7C) >> 2];
            span[16] = Base32Bytes[((randomness3 & 3) << 3) | ((randomness4 & 0xE0) >> 5)];
            span[17] = Base32Bytes[randomness4 & 0x1F];
            span[18] = Base32Bytes[(randomness5 & 0xF8) >> 3];
            span[19] = Base32Bytes[((randomness5 & 7) << 2) | ((randomness6 & 0xC0) >> 6)];
            span[20] = Base32Bytes[(randomness6 & 0x3E) >> 1];
            span[21] = Base32Bytes[((randomness6 & 1) << 4) | ((randomness7 & 0xF0) >> 4)];
            span[22] = Base32Bytes[((randomness7 & 0xF) << 1) | ((randomness8 & 0x80) >> 7)];
            span[23] = Base32Bytes[(randomness8 & 0x7C) >> 2];
            span[24] = Base32Bytes[((randomness8 & 3) << 3) | ((randomness9 & 0xE0) >> 5)];
            return true;
        }

        public bool TryWriteStringify(Span<char> span)
        {
            if (span.Length < 26)
            {
                return false;
            }

            span[25] = Base32Text[randomness9 & 0x1F];
            span[0] = Base32Text[(timestamp0 & 0xE0) >> 5];
            span[1] = Base32Text[timestamp0 & 0x1F];
            span[2] = Base32Text[(timestamp1 & 0xF8) >> 3];
            span[3] = Base32Text[((timestamp1 & 7) << 2) | ((timestamp2 & 0xC0) >> 6)];
            span[4] = Base32Text[(timestamp2 & 0x3E) >> 1];
            span[5] = Base32Text[((timestamp2 & 1) << 4) | ((timestamp3 & 0xF0) >> 4)];
            span[6] = Base32Text[((timestamp3 & 0xF) << 1) | ((timestamp4 & 0x80) >> 7)];
            span[7] = Base32Text[(timestamp4 & 0x7C) >> 2];
            span[8] = Base32Text[((timestamp4 & 3) << 3) | ((timestamp5 & 0xE0) >> 5)];
            span[9] = Base32Text[timestamp5 & 0x1F];
            span[10] = Base32Text[(randomness0 & 0xF8) >> 3];
            span[11] = Base32Text[((randomness0 & 7) << 2) | ((randomness1 & 0xC0) >> 6)];
            span[12] = Base32Text[(randomness1 & 0x3E) >> 1];
            span[13] = Base32Text[((randomness1 & 1) << 4) | ((randomness2 & 0xF0) >> 4)];
            span[14] = Base32Text[((randomness2 & 0xF) << 1) | ((randomness3 & 0x80) >> 7)];
            span[15] = Base32Text[(randomness3 & 0x7C) >> 2];
            span[16] = Base32Text[((randomness3 & 3) << 3) | ((randomness4 & 0xE0) >> 5)];
            span[17] = Base32Text[randomness4 & 0x1F];
            span[18] = Base32Text[(randomness5 & 0xF8) >> 3];
            span[19] = Base32Text[((randomness5 & 7) << 2) | ((randomness6 & 0xC0) >> 6)];
            span[20] = Base32Text[(randomness6 & 0x3E) >> 1];
            span[21] = Base32Text[((randomness6 & 1) << 4) | ((randomness7 & 0xF0) >> 4)];
            span[22] = Base32Text[((randomness7 & 0xF) << 1) | ((randomness8 & 0x80) >> 7)];
            span[23] = Base32Text[(randomness8 & 0x7C) >> 2];
            span[24] = Base32Text[((randomness8 & 3) << 3) | ((randomness9 & 0xE0) >> 5)];
            return true;
        }

        public override string ToString()
        {
            return string.Create(26, this, delegate (Span<char> span, Did state)
            {
                state.TryWriteStringify(span);
            });
        }

        public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider provider)
        {
            if (TryWriteStringify(destination))
            {
                charsWritten = 26;
                return true;
            }

            charsWritten = 0;
            return false;
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            return ToString();
        }

        public static Did Parse(string s, IFormatProvider provider)
        {
            return Parse(s);
        }

        static Did IParsable<Did>.Parse(string s, IFormatProvider provider)
        {
            //ILSpy generated this explicit interface implementation from .override directive in Parse
            return Parse(s, provider!);
        }

        public static bool TryParse(string s, IFormatProvider? provider, out Did result)
        {
            return TryParse(s, out result);
        }

        static bool IParsable<Did>.TryParse(string s, IFormatProvider provider, out Did result)
        {
            //ILSpy generated this explicit interface implementation from .override directive in TryParse
            return TryParse(s, provider, out result);
        }

        public static Did Parse(ReadOnlySpan<char> s, IFormatProvider provider)
        {
            return Parse(s);
        }

        static Did ISpanParsable<Did>.Parse(ReadOnlySpan<char> s, IFormatProvider provider)
        {
            //ILSpy generated this explicit interface implementation from .override directive in Parse
            return Parse(s, provider);
        }

        public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider provider, out Did result)
        {
            return TryParse(s, out result);
        }

        static bool ISpanParsable<Did>.TryParse(ReadOnlySpan<char> s, IFormatProvider provider, out Did result)
        {
            //ILSpy generated this explicit interface implementation from .override directive in TryParse
            return TryParse(s, provider, out result);
        }

        public bool TryFormat(Span<byte> destination, out int bytesWritten, ReadOnlySpan<char> format, IFormatProvider provider)
        {
            if (TryWriteStringify(destination))
            {
                bytesWritten = 26;
                return true;
            }

            bytesWritten = 0;
            return false;
        }

        public override int GetHashCode()
        {
            ref int reference = ref Unsafe.As<Did, int>(ref Unsafe.AsRef(in this));
            return reference ^ Unsafe.Add(ref reference, 1) ^ Unsafe.Add(ref reference, 2) ^ Unsafe.Add(ref reference, 3);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool EqualsCore(in Did left, in Did right)
        {
            if (Vector128.IsHardwareAccelerated)
            {
                return Unsafe.As<Did, Vector128<byte>>(ref Unsafe.AsRef(in left)) == Unsafe.As<Did, Vector128<byte>>(ref Unsafe.AsRef(in right));
            }

            if (Sse2.IsSupported)
            {
                Vector128<byte> left2 = Unsafe.As<Did, Vector128<byte>>(ref Unsafe.AsRef(in left));
                Vector128<byte> right2 = Unsafe.As<Did, Vector128<byte>>(ref Unsafe.AsRef(in right));
                return Sse2.MoveMask(Sse2.CompareEqual(left2, right2)) == 65535;
            }

            ref long reference = ref Unsafe.As<Did, long>(ref Unsafe.AsRef(in left));
            ref long reference2 = ref Unsafe.As<Did, long>(ref Unsafe.AsRef(in right));
            if (reference == reference2)
            {
                return Unsafe.Add(ref reference, 1) == Unsafe.Add(ref reference2, 1);
            }

            return false;
        }

        public bool Equals(Did other)
        {
            return EqualsCore(in this, in other);
        }

        public override bool Equals(object obj)
        {
            if (obj is Did)
            {
                Did right = (Did)obj;
                return EqualsCore(in this, in right);
            }

            return false;
        }

        public static bool operator ==(Did a, Did b)
        {
            return EqualsCore(in a, in b);
        }

        public static bool operator !=(Did a, Did b)
        {
            return !EqualsCore(in a, in b);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int GetResult(byte me, byte them)
        {
            if (me > them)
            {
                return 1;
            }
            return -1;
        }

        public int CompareTo(Did other)
        {
            if (timestamp0 != other.timestamp0)
            {
                return GetResult(timestamp0, other.timestamp0);
            }

            if (timestamp1 != other.timestamp1)
            {
                return GetResult(timestamp1, other.timestamp1);
            }

            if (timestamp2 != other.timestamp2)
            {
                return GetResult(timestamp2, other.timestamp2);
            }

            if (timestamp3 != other.timestamp3)
            {
                return GetResult(timestamp3, other.timestamp3);
            }

            if (timestamp4 != other.timestamp4)
            {
                return GetResult(timestamp4, other.timestamp4);
            }

            if (timestamp5 != other.timestamp5)
            {
                return GetResult(timestamp5, other.timestamp5);
            }

            if (randomness0 != other.randomness0)
            {
                return GetResult(randomness0, other.randomness0);
            }

            if (randomness1 != other.randomness1)
            {
                return GetResult(randomness1, other.randomness1);
            }

            if (randomness2 != other.randomness2)
            {
                return GetResult(randomness2, other.randomness2);
            }

            if (randomness3 != other.randomness3)
            {
                return GetResult(randomness3, other.randomness3);
            }

            if (randomness4 != other.randomness4)
            {
                return GetResult(randomness4, other.randomness4);
            }

            if (randomness5 != other.randomness5)
            {
                return GetResult(randomness5, other.randomness5);
            }

            if (randomness6 != other.randomness6)
            {
                return GetResult(randomness6, other.randomness6);
            }

            if (randomness7 != other.randomness7)
            {
                return GetResult(randomness7, other.randomness7);
            }

            if (randomness8 != other.randomness8)
            {
                return GetResult(randomness8, other.randomness8);
            }

            if (randomness9 != other.randomness9)
            {
                return GetResult(randomness9, other.randomness9);
            }

            return 0;
        }

        public int CompareTo(object value)
        {
            if (value == null)
            {
                return 1;
            }

            if (value is Did)
            {
                Did other = (Did)value;
                return CompareTo(other);
            }

            throw new ArgumentException("Object must be of type ULID.", "value");
        }

        public static explicit operator Guid(Did _this)
        {
            return _this.ToGuid();
        }

        public static bool operator <(Did left, Did right)
        {
            return left.CompareTo(right) == -1;
        }

        public static bool operator >(Did left, Did right)
        {
            return left.CompareTo(right) == 1;
        }

        public static bool operator >=(Did left, Did right)
        {
            return left.CompareTo(right) >= 0;
        }

        public static bool operator <=(Did left, Did right)
        {
            return left.CompareTo(right) <= 0;
        }

        //
        // Summary:
        //     Convert this Did value to a Guid value with the same comparability.
        //
        // Returns:
        //     The converted Guid value
        //
        // Remarks:
        //     The byte arrangement between Did and Guid is not preserved.
        public Guid ToGuid()
        {
            if (IsVector128Supported && BitConverter.IsLittleEndian)
            {
                Vector128<byte> source = Shuffle(Unsafe.As<Did, Vector128<byte>>(ref Unsafe.AsRef(in this)), Vector128.Create((byte)3, (byte)2, (byte)1, (byte)0, (byte)5, (byte)4, (byte)7, (byte)6, (byte)8, (byte)9, (byte)10, (byte)11, (byte)12, (byte)13, (byte)14, (byte)15));
                return Unsafe.As<Vector128<byte>, Guid>(ref source);
            }

            Span<byte> span = stackalloc byte[16];
            if (BitConverter.IsLittleEndian)
            {
                ref uint reference = ref Unsafe.As<Did, uint>(ref Unsafe.AsRef(in this));
                uint value = BinaryPrimitives.ReverseEndianness(reference);
                MemoryMarshal.Write(span, ref value);
                reference = ref Unsafe.Add(ref reference, 1);
                uint value2 = ((reference & 0xFF00FF) << 8) | ((reference & 0xFF00FF00u) >> 8);
                MemoryMarshal.Write(span.Slice(4), ref value2);
                MemoryMarshal.Write(value: ref Unsafe.As<uint, ulong>(ref Unsafe.Add(ref reference, 1)), destination: span.Slice(8));
            } else
            {
                MemoryMarshal.Write(span, ref Unsafe.AsRef(in this));
            }

            return MemoryMarshal.Read<Guid>(span);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Vector128<byte> Shuffle(Vector128<byte> value, Vector128<byte> mask)
        {
            if (Vector128.IsHardwareAccelerated)
            {
                return Vector128.Shuffle(value, mask);
            }

            if (Ssse3.IsSupported)
            {
                return Ssse3.Shuffle(value, mask);
            }

            throw new NotImplementedException();
        }
    }
}