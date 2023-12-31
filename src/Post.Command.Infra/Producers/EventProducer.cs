﻿using Confluent.Kafka;
using CQRS.Core.Exceptions;
using CQRS.Core.Messages;
using CQRS.Core.Producers;
using Domain.Identity;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Post.Command.Infra.Producers
{
    public class EventProducer : IEventProducer
    {
        private readonly ProducerConfig _config;
        private readonly IDomainIdentity _domainIdentity;

        public EventProducer(IOptions<ProducerConfig> config, IDomainIdentity domainIdentity)
        {
            _config = config.Value;
            _domainIdentity = domainIdentity;
        }
        public async Task ProduceAsync<T>(string topic, T @event) where T : BaseEvent
        {
            using var producer = new ProducerBuilder<string, string>(_config)
                .SetKeySerializer(Serializers.Utf8)
                .SetValueSerializer(Serializers.Utf8)
                .Build();

            var eventMessage = new Message<string, string>
            {
                Key = _domainIdentity.NewId().ToString(),
                Value = JsonSerializer.Serialize(@event, @event.GetType())
            };

            var deliveryResult = await producer.ProduceAsync(topic, eventMessage);

            if(deliveryResult.Status == PersistenceStatus.NotPersisted) 
            {
                throw new BrokerException($"Unable to produce {@event.GetType().Name} message on topic {topic} due to: {deliveryResult.Message}");
            }
        }
    }
}
