using CQRS.Core.Commands;

namespace Post.Command.Api.Commands
{
    public record DeletePost: BaseCommand
    {
        public Guid CommentId { get; set; }
        public string Username { get; set; }
    }
}
