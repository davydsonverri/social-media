using CQRS.Core.Domain;
using CQRS.Core.Events;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Post.Command.Infra.Config;

namespace Post.Command.Infra.Repositories
{
    public class EventStoreRepository : IEventStoreRepository
    {
        private readonly IMongoCollection<EventModel> _eventStoreCollection;

        public EventStoreRepository(IOptions<MongoDbConfig> config)
        {
            var mongoClient = new MongoClient(config.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(config.Value.DatabaseName);

            _eventStoreCollection = mongoDatabase.GetCollection<EventModel>(config.Value.Collection);
        }

        public async Task SaveAsync(EventModel @event)
        {
            await _eventStoreCollection.InsertOneAsync(@event);
        }

        public async Task<List<EventModel>> FindByAggregateId(Guid aggregateId)
        {
            return await _eventStoreCollection.Find(x => x.AggregateId == aggregateId).ToListAsync();
        }
    }
}
