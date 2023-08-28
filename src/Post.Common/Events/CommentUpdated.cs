using CQRS.Core.Messages;

namespace Post.Common.Events
{
    public record CommentUpdated : BaseEvent
    {
        public CommentUpdated() : base()
        {
            
        }
        public Guid CommentId { get; set; }
        public string Comment { get; set; }
        public string Username { get; set; }
        public DateTime CommentUpdateDate { get; set; }
    }
}
