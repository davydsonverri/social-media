using CQRS.Core.Commands;

namespace Post.Command.Api.Commands
{
    public record RestoreReadDb : BaseCommand
    {
        public RestoreReadDb() 
        { 
        }
    }
}
