using CQRS.Core.Messages;

namespace Post.Common.Events
{
    public record PostDeleted : BaseEvent
    {
        public PostDeleted() : base()
        {
            
        }
    }
}
