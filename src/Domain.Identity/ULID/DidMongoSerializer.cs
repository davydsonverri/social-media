using MongoDB.Bson.Serialization;

namespace Domain.Identity.ULID
{
    public class DidMongoSerializer : IBsonSerializer<Did>
    {
        public Type ValueType => typeof(Did);

        public Did Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            return Did.Parse(BsonSerializer.Deserialize<string>(context.Reader));
        }

        public void Serialize(BsonSerializationContext context, BsonSerializationArgs args, Did value)
        {
            BsonSerializer.Serialize(context.Writer, value.ToString());
        }

        public void Serialize(BsonSerializationContext context, BsonSerializationArgs args, object value)
        {
            BsonSerializer.Serialize(context.Writer, value.ToString());
        }

        object IBsonSerializer.Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            return Did.Parse(BsonSerializer.Deserialize<string>(context.Reader));
        }
    }
}
