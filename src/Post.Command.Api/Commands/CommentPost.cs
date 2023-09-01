using CQRS.Core.Commands;

namespace Post.Command.Api.Commands
{
    public record CommentPost: BaseCommand
    {
        public required string Comment { get; set; }
        public required string Username { get; set; }
    }
}
