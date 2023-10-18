using CQRS.Core.Domain;
using Post.Common.Events;

namespace Post.Command.Domain.Aggregates
{
    public class PostAggregate : AggregateRoot
    {
                
        public string Author { get; private set; } = string.Empty;
        public string Message { get; private set; } = string.Empty;
        public bool Active { get; private set; }
        public int Likes { get; private set; }
        public Dictionary<Did, Tuple<string, string>> Comments { get; private set; } = new();
        
        public PostAggregate()
        {            
        }

        public PostAggregate(Did id, string author, string message)
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
            Active = true;
            Author = @event.Author;
            Message = @event.Message;
        }

        public void UpdateMessage(string message)
        {
            if(!Active)
            {
                throw new InvalidOperationException("Unable to edit inactive post.");
            }

            if (string.IsNullOrWhiteSpace(message))
            {
                throw new InvalidOperationException("The value of message cannot be empty.");
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
            Message = @event.Message;
        }

        public void LikePost()
        {
            if (!Active)
            {
                throw new InvalidOperationException("Unable to like deleted post.");
            }

            RaiseEvent(new PostLiked
            {
                Id = _id
            });
        }

        public void Apply(PostLiked @event)
        {
            _id = @event.Id;
            Likes += 1;
        }

        public void AddComment(Did commentId, string comment, string username)
        {
            if (!Active)
            {
                throw new InvalidOperationException("Unable to comment to an inactive post.");
            }

            if (string.IsNullOrWhiteSpace(comment))
            {
                throw new InvalidOperationException("The value of comment cannot be empty.");
            }

            RaiseEvent(new CommentAdded
            {
                Id = _id,
                CommentId = commentId,
                Comment = comment,
                Username = username,
                CommentDate = DateTime.Now
            });
        }

        public void Apply(CommentAdded @event)
        {
            _id = @event.Id;
            Comments.Add(@event.CommentId, new Tuple<string, string>(@event.Comment, @event.Username));
        }

        public void UpdateComment(Did commentId, string comment, string username)
        {
            if (!Active)
            {
                throw new InvalidOperationException("Unable to edit comment of an inactive post.");
            }

            if (!Comments[commentId].Item2.Equals(username, StringComparison.CurrentCultureIgnoreCase))
            {
                throw new InvalidOperationException("You are not allowed to edit a comment that was made by another user.");
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
            Comments[@event.CommentId] = new Tuple<string, string>(@event.Comment, @event.Username);
        }

        public void DeleteComment(Did commentId, string username)
        {
            if (!Active)
            {
                throw new InvalidOperationException("Unable to remove a comment from an inactive post.");
            }

            if (!Comments[commentId].Item2.Equals(username, StringComparison.CurrentCultureIgnoreCase))
            {
                throw new InvalidOperationException("You are not allowed to remove a comment that was made by another user.");
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
            Comments.Remove(@event.CommentId);
        }

        public void DeletePost(string username)
        {
            if (!Active)
            {
                throw new InvalidOperationException("The post has already been removed.");
            }

            if (!Author.Equals(username, StringComparison.CurrentCultureIgnoreCase))
            {
                throw new InvalidOperationException("You are not allowed to delete a post from someone else.");
            }

            RaiseEvent(new PostDeleted
            {
                Id = _id
            });
        }

        public void Apply(PostDeleted @event)
        {
            _id = @event.Id;
            Active = false;
        }
    }
}
