using CQRS.Core.Commands;

namespace Post.Command.Api.Commands
{
    public record UpdatePost : BaseCommand
    {
        public required string Message { get; set; }
    }
}
