using System.Buffers;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Domain.Identity.ULID
{
    public class DidJsonConverter : JsonConverter<Did>
    {
        //
        // Summary:
        //     Read a Did value represented by a string from JSON.
        public override Did Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            try
            {
                if (reader.TokenType != JsonTokenType.String)
                {
                    throw new JsonException("Expected string");
                }

                if (reader.HasValueSequence)
                {
                    ReadOnlySequence<byte> source = reader.ValueSequence;
                    if (source.Length != 26)
                    {
                        throw new JsonException("Did invalid: length must be 26");
                    }

                    Span<byte> span = stackalloc byte[26];
                    source.CopyTo(span);
                    Did.TryParse(span, out var did);
                    return did;
                }

                ReadOnlySpan<byte> valueSpan = reader.ValueSpan;
                if (valueSpan.Length != 26)
                {
                    throw new JsonException("Did invalid: length must be 26");
                }

                Did.TryParse(valueSpan, out var did2);
                return did2;
            } catch (IndexOutOfRangeException innerException)
            {
                throw new JsonException("Did invalid: length must be 26", innerException);
            } catch (OverflowException innerException2)
            {
                throw new JsonException("Did invalid: invalid character", innerException2);
            }
        }

        public override void Write(Utf8JsonWriter writer, Did value, JsonSerializerOptions options)
        {
            Span<byte> span = stackalloc byte[26];
            value.TryWriteStringify(span);
            writer.WriteStringValue(span);
        }
    }

}


