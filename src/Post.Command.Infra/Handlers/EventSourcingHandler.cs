using CQRS.Core.Domain;
using CQRS.Core.Handlers;
using CQRS.Core.Infra;
using CQRS.Core.Producers;
using Post.Command.Domain.Aggregates;

namespace Post.Command.Infra.Handlers
{
    public class EventSourcingHandler : IEventSourcingHandler<PostAggregate>
    {
        private readonly IEventStore _eventStore;
        private readonly IEventProducer _eventProducer;
        public EventSourcingHandler(IEventStore eventStore, IEventProducer eventProducer)
        {
            _eventStore = eventStore;
            _eventProducer = eventProducer;
        }

        public async Task<PostAggregate> GetByIdAsync(Guid aggregateId)
        {
            var aggregate = new PostAggregate();
            var events = await _eventStore.GetEventsAsync(aggregateId);

            if (events == null || events.Count == 0) { return aggregate; }

            aggregate.ReplayEvents(events);
            aggregate.Version = events.Select(e => e.Version).Max();

            return aggregate;
        }

        public async Task SaveAsync(AggregateRoot aggregate)
        {
            await _eventStore.SaveEventAsync(aggregate.Id, aggregate.GetUncommittedEvents(), aggregate.Version);
            aggregate.MarkEventsAsCommited();
        }

        public async Task RepublishEventsAsync()
        {
            var aggregateIds = await _eventStore.GetAggregateIdsAsync();

            if (aggregateIds == null || !aggregateIds.Any()) return;

            foreach (var aggregateId in aggregateIds)
            {
                var aggregate = await GetByIdAsync(aggregateId);

                if (aggregate == null || !aggregate.Active) continue;

                var events = await _eventStore.GetEventsAsync(aggregateId);

                foreach(var @event in events)
                {
                    var topic = Environment.GetEnvironmentVariable("KAFKA_TOPIC")!;
                    await _eventProducer.ProduceAsync(topic, @event);
                }
            }
        }
    }
}
