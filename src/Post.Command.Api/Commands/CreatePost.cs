using CQRS.Core.Commands;

namespace Post.Command.Api.Commands
{
    public record CreatePost : BaseCommand
    {
        public string Author { get; set; }
        public string Message { get; set; }
    }
}
