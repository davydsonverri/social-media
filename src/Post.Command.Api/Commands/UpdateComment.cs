using CQRS.Core.Commands;

namespace Post.Command.Api.Commands
{
    public class UpdateComment: BaseCommand
    {
        public Guid CommentId { get; set; }
        public string Comment { get; set; }
        public string Username { get; set; }
    }
}
