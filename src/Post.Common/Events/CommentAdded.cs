using CQRS.Core.Messages;

namespace Post.Common.Events
{
    public record CommentAdded : BaseEvent
    {
        public CommentAdded() : base()
        {
            
        }
        public required Guid CommentId { get; set; }
        public required string Comment { get; set; }
        public required string Username { get; set; }
        public required DateTime CommentDate { get; set; }
    }
}
