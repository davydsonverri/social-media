using CQRS.Core.Commands;

namespace Post.Command.Api.Commands
{
    public record DeleteComment: BaseCommand
    {
        public required Did CommentId { get; set; }
        public required string Username { get; set; }
    }
}
