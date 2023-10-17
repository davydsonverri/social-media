using CQRS.Core.Infra;
using CQRS.Core.Messages;
using CQRS.Core.Producers;
using Domain.Identity;
using Domain.Identity.ULID;
using FluentAssertions;
using Moq;
using Post.Command.Infra.Handlers;

namespace Post.Command.Infra.UnitTests.Handler
{
    public class EventSourcingHandlerTest
    {
        private readonly IDomainIdentity _domainIdentity = new UlidGenerator();

        [Fact]
        public async void EventSourcingHandler_GetByIdAsync_MustReturnLoadedAggregate()
        {
            var expectedId = _domainIdentity.NewId();
            var eventStoreMock = new Mock<IEventStore>();
            var eventProducerMock = new Mock<IEventProducer>();
            var expectedEvents = new List<BaseEvent>();
            expectedEvents.Add(new SampleAggregateEvent() { Id = expectedId });
            eventStoreMock.Setup(x => x.GetEventsAsync(expectedId)).Returns(() => { return Task.FromResult(expectedEvents); });

            var sut = new EventSourcingHandler<SampleAggregate>(eventStoreMock.Object, eventProducerMock.Object);            
            var aggregate = await sut.GetByIdAsync(expectedId);

            aggregate.Id.Should().Be(expectedId);
            aggregate.Version.Should().Be(0);
            aggregate.EventWasApplied.Should().BeTrue();
        }
    }
}
