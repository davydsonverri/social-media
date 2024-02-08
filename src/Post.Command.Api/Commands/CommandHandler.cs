using CQRS.Core.Handlers;
using Domain.Identity;
using Post.Command.Domain.Aggregates;

namespace Post.Command.Api.Commands
{
    public class CommandHandler : ICommandHandler
    {
        private readonly IEventSourcingHandler<PostAggregate> _eventSourcingHandler;
        private readonly IDomainIdentity _domainIdentity;

        public CommandHandler(IEventSourcingHandler<PostAggregate> eventSourcingHandler, IDomainIdentity domainIdentity)
        {
            _eventSourcingHandler = eventSourcingHandler;
            _domainIdentity = domainIdentity;
            // testes
        }

        public async Task HandleAsync(CreatePost command)
        {
            var aggregate = new PostAggregate(command.Id, command.Author, command.Message);
            await _eventSourcingHandler.SaveAsync(aggregate);
        }

        public async Task HandleAsync(UpdatePost command)
        {
            var aggregate = await _eventSourcingHandler.GetByIdAsync(command.Id);
            aggregate.UpdateMessage(command.Message);
            await _eventSourcingHandler.SaveAsync(aggregate);
        }

        public async Task HandleAsync(DeletePost command)
        {
            var aggregate = await _eventSourcingHandler.GetByIdAsync(command.Id);
            aggregate.DeletePost(command.Username);
            await _eventSourcingHandler.SaveAsync(aggregate);
        }

        public async Task HandleAsync(LikePost command)
        {
            var aggregate = await _eventSourcingHandler.GetByIdAsync(command.Id);
            aggregate.LikePost();
            await _eventSourcingHandler.SaveAsync(aggregate);
        }

        public async Task HandleAsync(CommentPost command)
        {
            var aggregate = await _eventSourcingHandler.GetByIdAsync(command.Id);
            var commentId = _domainIdentity.NewId();
            aggregate.AddComment(commentId, command.Comment, command.Username);
            await _eventSourcingHandler.SaveAsync(aggregate);
        }

        public async Task HandleAsync(UpdateComment command)
        {
            var aggregate = await _eventSourcingHandler.GetByIdAsync(command.Id);
            aggregate.UpdateComment(command.CommentId, command.Comment, command.Username);
            await _eventSourcingHandler.SaveAsync(aggregate);
        }

        public async Task HandleAsync(DeleteComment command)
        {
            var aggregate = await _eventSourcingHandler.GetByIdAsync(command.Id);
            aggregate.DeleteComment(command.CommentId, command.Username);
            await _eventSourcingHandler.SaveAsync(aggregate);
        }

        public async Task HandleAsync(RestoreReadDb command)
        {
            await _eventSourcingHandler.RepublishEventsAsync();
        }
    }
}
