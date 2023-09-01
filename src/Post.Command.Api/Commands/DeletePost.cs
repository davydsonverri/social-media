using CQRS.Core.Commands;

namespace Post.Command.Api.Commands
{
    public record DeletePost: BaseCommand
    {
        public required Guid CommentId { get; set; }
        public required string Username { get; set; }
    }
}
