using CQRS.Core.Messages;

namespace Post.Common.Events
{
    public record PostLiked: BaseEvent
    {
        public PostLiked() : base()
        {
            
        }
    }
}
