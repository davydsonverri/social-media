using CQRS.Core.Commands;

namespace Post.Command.Api.Commands
{
    public class CreatePost : BaseCommand
    {
        public string Author { get; set; }
        public string Message { get; set; }
    }
}
