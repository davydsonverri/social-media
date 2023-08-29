using CQRS.Core.Commands;

namespace Post.Command.Api.Commands
{
    public record UpdatePost : BaseCommand
    {
        public string Message { get; set; }
    }
}
