using CQRS.Core.Domain;
using Post.Common.Events;

namespace Post.Command.Infra.UnitTests
{
    public class SampleAggregate : AggregateRoot
    {
        public bool EventWasApplied { get; private set; }

        public void Apply(SampleAggregateEvent @event)
        {
            _id = @event.Id;
            EventWasApplied = true;                        
        }
    }
}
