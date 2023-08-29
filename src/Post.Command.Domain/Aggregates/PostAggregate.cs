using CQRS.Core.Domain;
using CQRS.Core.Messages;
using Post.Common.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Post.Command.Domain.Aggregates
{
    public class PostAggregate : AggregateRoot
    {
        private bool _active;
        private string _author;
        private readonly Dictionary<Guid, Tuple<string, string>> _comments = new();

        public bool Active
        {
            get => _active; set => _active = value;
        }

        public PostAggregate()
        {
            
        }

        public PostAggregate(Guid id, string author, string message)
        {
            RaiseEvent(new PostCreated
            {
                Id = id,
                Author = author,
                Message = message,
                PostDate = DateTime.Now
            });
        }

        public void Apply(PostCreated @event)
        {
            _id = @event.Id;
            _active = true;
            _author = @event.Author;
        }

        public void EditMessage(string message)
        {
            if(!_active)
            {
                throw new InvalidOperationException("Unable to edit inactive post");
            }

            if (string.IsNullOrWhiteSpace(message))
            {
                throw new InvalidOperationException($"The value of {nameof(message)} cannot be empty.");
            }

            RaiseEvent(new PostUpdated
            {
                Id= _id,
                Message = message
            });
        }

        public void Apply(PostUpdated @event)
        {
            _id = @event.Id;
        }

        public void LikePost()
        {
            if (!_active)
            {
                throw new InvalidOperationException("Unable to like inactive post");
            }

            RaiseEvent(new PostLiked
            {
                Id = _id
            });
        }

        public void Apply(PostLiked @event)
        {
            _id = @event.Id;
        }

        public void AddComment(string comment, string username)
        {
            if (!_active)
            {
                throw new InvalidOperationException("Unable to comment to an inactive post");
            }

            if (string.IsNullOrWhiteSpace(comment))
            {
                throw new InvalidOperationException($"The value of {nameof(comment)} cannot be empty.");
            }

            RaiseEvent(new CommentAdded
            {
                Id = _id,
                CommentId = Guid.NewGuid(),
                Comment = comment,
                Username = username,
                CommentDate = DateTime.Now
            });
        }

        public void Apply(CommentAdded @event)
        {
            _id = @event.Id;
            _comments.Add(@event.CommentId, new Tuple<string, string>(@event.Comment, @event.Username));
        }

        public void UpdateComment(Guid commentId, string comment, string username)
        {
            if (!_active)
            {
                throw new InvalidOperationException("Unable to edit comment of an inactive post");
            }

            if (!_comments[commentId].Item2.Equals(username, StringComparison.CurrentCultureIgnoreCase))
            {
                throw new InvalidOperationException("You are not allowed to edit a comment that was made by another user");
            }

            RaiseEvent(new CommentUpdated { 
                Id = _id, 
                CommentId = commentId,
                Comment = comment,
                Username = username,
                CommentUpdateDate = DateTime.Now
            });
        }

        public void Apply(CommentUpdated @event)
        {
            _id = @event.Id;
            _comments[@event.CommentId] = new Tuple<string, string>(@event.Comment, @event.Username);
        }

        public void DeleteComment(Guid commentId, string username)
        {
            if (!_active)
            {
                throw new InvalidOperationException("Unable to remove a comment from an inactive post");
            }

            if (!_comments[commentId].Item2.Equals(username, StringComparison.CurrentCultureIgnoreCase))
            {
                throw new InvalidOperationException("You are not allowed to remove a comment that was made by another user");
            }

            RaiseEvent(new CommentDeleted
            {
                Id = _id,
                CommentId = commentId,
            });
        }

        public void Apply(CommentDeleted @event)
        {
            _id = @event.Id;
            _comments.Remove(@event.CommentId);
        }

        public void DeletePost(string username)
        {
            if (!_active)
            {
                throw new InvalidOperationException("The post has already been removed");
            }

            if (!_author.Equals(username, StringComparison.CurrentCultureIgnoreCase))
            {
                throw new InvalidOperationException("You are not allowed to delete a post from someone else!");
            }

            RaiseEvent(new PostDeleted
            {
                Id = _id
            });
        }

        public void Apply(PostDeleted @event)
        {
            _id = @event.Id;
            _active = false;
        }
    }
}
