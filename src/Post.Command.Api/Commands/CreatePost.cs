using CQRS.Core.Commands;

namespace Post.Command.Api.Commands
{
    public record CreatePost : BaseCommand
    {
        public required string Author { get; set; }
        public required string Message { get; set; }
    }
}
