using CQRS.Core.Messages;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CQRS.Core.Events
{
    public class EventModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = default!;
        public required DateTime TimeStamp { get; set; }
        public required Did AggregateId { get; set; }
        public required string AggregateType { get; set; }
        public required int Version { get; set; }
        public required string EventType { get; set; }
        public required BaseEvent EventData { get; set; }
    }
}
