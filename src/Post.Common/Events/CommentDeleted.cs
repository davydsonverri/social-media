using CQRS.Core.Messages;

namespace Post.Common.Events
{
    public record CommentDeleted : BaseEvent
    {
        public CommentDeleted() : base()
        {
            
        }
        public required Guid CommentId { get; set; }
        
    }
}
