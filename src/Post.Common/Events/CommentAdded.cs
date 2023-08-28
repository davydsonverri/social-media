using CQRS.Core.Messages;

namespace Post.Common.Events
{
    public record CommentAdded : BaseEvent
    {
        public CommentAdded() : base()
        {
            
        }
        public Guid CommentId { get; set; }
        public string Comment { get; set; }
        public string Username { get; set; }
        public DateTime CommentDate { get; set; }
    }
}
