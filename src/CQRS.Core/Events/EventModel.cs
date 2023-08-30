using CQRS.Core.Messages;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS.Core.Events
{
    public class EventModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public required string Id { get; set; }
        public required DateTime TimeStamp { get; set; }
        public required Guid AggregateId { get; set; }
        public required string AggregateType { get; set; }
        public required int Version { get; set; }
        public required string EventType { get; set; }
        public required BaseEvent EventData { get; set; }
    }
}
