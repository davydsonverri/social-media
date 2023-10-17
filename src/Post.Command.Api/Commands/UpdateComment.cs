using CQRS.Core.Commands;

namespace Post.Command.Api.Commands
{
    public record UpdateComment: BaseCommand
    {
        public required Did CommentId { get; set; }
        public required string Comment { get; set; }
        public required string Username { get; set; }
    }
}
