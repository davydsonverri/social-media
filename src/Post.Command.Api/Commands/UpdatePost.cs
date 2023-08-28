using CQRS.Core.Commands;

namespace Post.Command.Api.Commands
{
    public class UpdatePost : BaseCommand
    {
        public string Message { get; set; }
    }
}
