using CQRS.Core.Messages;

namespace Post.Common.Events
{
    public record CommentUpdated : BaseEvent
    {
        public CommentUpdated() : base()
        {
            
        }
        public required Guid CommentId { get; set; }
        public required string Comment { get; set; }
        public required string Username { get; set; }
        public required DateTime CommentUpdateDate { get; set; }
    }
}
