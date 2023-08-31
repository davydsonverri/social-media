using Post.Common.Events;

namespace Post.Query.Infra.Handlers
{
    public interface IEventHandler
    {
        Task On(PostCreated @event);
        Task On(PostUpdated @event);
        Task On(PostLiked @event);
        Task On(PostDeleted @event);
        Task On(CommentAdded @event);
        Task On(CommentUpdated @event);
        Task On(CommentDeleted @event);        
    }
}
