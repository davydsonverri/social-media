using CQRS.Core.Commands;

namespace Post.Command.Api.Commands
{
    public class DeleteComment: BaseCommand
    {
        public Guid CommentId { get; set; }
        public string Username { get; set; }
    }
}
