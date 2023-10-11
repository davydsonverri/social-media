namespace Post.Command.Api.Commands
{
    public interface ICommandHandler
    {
        Task HandleAsync(CreatePost command);
        Task HandleAsync(UpdatePost command);
        Task HandleAsync(DeletePost command);
        Task HandleAsync(LikePost command);
        Task HandleAsync(CommentPost command);
        Task HandleAsync(UpdateComment command);
        Task HandleAsync(DeleteComment command);
        Task HandleAsync(RestoreReadDb command);
    }
}
