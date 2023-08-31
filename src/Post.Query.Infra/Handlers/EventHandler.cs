using Post.Common.Events;
using Post.Query.Domain.Entities;
using Post.Query.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Post.Query.Infra.Handlers
{
    public class EventHandler : IEventHandler
    {
        private readonly IPostRepository _postRepository;
        private readonly ICommentRepository _commentRepository;

        public EventHandler(IPostRepository postRepository, ICommentRepository commentRepository)
        {
            _postRepository = postRepository;
            _commentRepository = commentRepository;
        }

        public async Task On(PostCreated @event)
        {
            var post = new PostEntity
            {
                Id = @event.Id,
                Author = @event.Author,
                PostDate = @event.PostDate,
                Message = @event.Message
            };

            await _postRepository.CreateAsync(post);
        }

        public async Task On(PostUpdated @event)
        {
            var post = await _postRepository.GetByIdAsync(@event.Id);

            if (post == null) return;

            post.Message = @event.Message;
            await _postRepository.UpdateAsync(post);
        }

        public async Task On(PostLiked @event)
        {
            var post = await _postRepository.GetByIdAsync(@event.Id);

            if (post == null) return;

            post.Likes++;
            await _postRepository.UpdateAsync(post);
        }

        public async Task On(PostDeleted @event)
        {
            await _postRepository.DeleteAsync(@event.Id);
        }

        public async Task On(CommentAdded @event)
        {
            var comment = new CommentEntity
            {
                PostId = @event.Id,
                Id = @event.CommentId,
                CommentDate = @event.CommentDate,
                Comment = @event.Comment,
                Username = @event.Username,
                Edited = false
            };

            await _commentRepository.CreateAsync(comment);
        }

        public async Task On(CommentUpdated @event)
        {
            var comment = await _commentRepository.GetByIdAsync(@event.Id);

            if (comment == null) return;

            comment.Comment = @event.Comment;
            comment.Edited = true;
            comment.CommentDate = @event.CommentUpdateDate;

            await _commentRepository.UpdateAsync(comment);
        }

        public async Task On(CommentDeleted @event)
        {
            await _commentRepository.DeleteAsync(@event.CommentId);
        }
    }
}
