using CQRS.Core.Commands;

namespace Post.Command.Api.Commands
{
    public record DeleteComment : BaseCommand
    {
        public Guid CommentId { get; set; }
        public string Username { get; set; }
    }
}
