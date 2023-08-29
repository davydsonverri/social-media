using CQRS.Core.Commands;

namespace Post.Command.Api.Commands
{
    public record CommentPost: BaseCommand
    {
        public string Comment { get; set; }
        public string Username { get; set; }
    }
}
