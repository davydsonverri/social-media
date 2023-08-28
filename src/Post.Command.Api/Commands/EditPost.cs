using CQRS.Core.Commands;

namespace Post.Command.Api.Commands
{
    public class EditPost : BaseCommand
    {
        public string Message { get; set; }
    }
}
