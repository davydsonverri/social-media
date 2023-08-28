using CQRS.Core.Messages;

namespace Post.Common.Events
{
    public record CommentDeleted : BaseEvent
    {
        public CommentDeleted() : base()
        {
            
        }
        public Guid CommentId { get; set; }
        
    }
}
