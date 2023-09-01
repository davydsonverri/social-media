using CQRS.Core.Messages;
using Post.Common.Events;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Post.Query.Infra.Converter
{
    public class EventJsonConverter : JsonConverter<BaseEvent>
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert.IsAssignableFrom(typeof(BaseEvent));
        }

        public override BaseEvent? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if(!JsonDocument.TryParseValue(ref reader, out var doc))
            {
                throw new JsonException($"Failed to parse {nameof(JsonDocument)}");
            }

            if(!doc.RootElement.TryGetProperty("Type", out var type))
            {
                throw new JsonException($"Could not identify the Type property");
            }

            var typeName = type.GetString();
            var json = doc.RootElement.GetRawText();

            return typeName switch
            {
                nameof(PostCreated) => JsonSerializer.Deserialize<PostCreated>(json, options),
                nameof(PostUpdated) => JsonSerializer.Deserialize<PostUpdated>(json, options),
                nameof(PostLiked) => JsonSerializer.Deserialize<PostLiked>(json, options),
                nameof(PostDeleted) => JsonSerializer.Deserialize<PostDeleted>(json, options),
                nameof(CommentAdded) => JsonSerializer.Deserialize<CommentAdded>(json, options),
                nameof(CommentUpdated) => JsonSerializer.Deserialize<CommentUpdated>(json, options),
                nameof(CommentDeleted) => JsonSerializer.Deserialize<CommentDeleted>(json, options),
                _ => throw new JsonException($"{typeName} is not supported yet!")
            };
        }

        public override void Write(Utf8JsonWriter writer, BaseEvent value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
