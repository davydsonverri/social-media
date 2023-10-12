using Bogus;
using FluentAssertions;
using Post.Command.Domain.Aggregates;
using Post.Command.Domain.Tests.Fixture;

namespace Post.Command.Domain.Tests.Aggregates
{
    [Collection(nameof(PostCollection))]
    public class PostAggregateTest
    {
        private readonly Faker _faker;
        private readonly PostAggregateFixture _postAggregateFixture;

        public PostAggregateTest(PostAggregateFixture postAggregateFixture)
        {
            _faker = new Faker("pt_BR");
            _postAggregateFixture = postAggregateFixture;
        }

        [Fact(DisplayName = "New 'Post' must be in VALID state and parameters on the right place")]
        [Trait("Aggregate", "Post")]
        public void Post_NewPost_MustBeValid()
        {
            var expectedPost = new
            {
                Id = _faker.Random.Guid(),
                Author = _faker.Person.FullName,
                Message = string.Join(" ", _faker.Lorem.Words(_faker.Random.Int(3, 50))),
                Active = true
            };

            var sut = new PostAggregate(expectedPost.Id, expectedPost.Author, expectedPost.Message);

            sut.Should().BeEquivalentTo(expectedPost);
        }

        [Fact(DisplayName = "Must be unable to update 'Post' message when 'Post' is deleted")]
        [Trait("Aggregate", "Post")]
        public void UpdatePostMessage_WhenPostIsDeleted_MustThrowInvalidOperationException()
        {
            var newMessage = string.Join(" ", _faker.Lorem.Words(_faker.Random.Int(3, 50)));
            var sut = _postAggregateFixture.BuildDeletedPost();

            sut.Invoking(x => x.UpdateMessage(newMessage))
               .Should().Throw<InvalidOperationException>()
               .WithMessage("Unable to edit inactive post.");
        }

        [Fact(DisplayName = "Must be unable to update 'Post' with empty message")]
        [Trait("Aggregate", "Post")]
        public void UpdatePostMessage_WithEmptyMessage_MustThrowInvalidOperationException()
        {
            var sut = _postAggregateFixture.BuildValidPost();

            sut.Invoking(x => x.UpdateMessage(string.Empty))
               .Should().Throw<InvalidOperationException>()
               .WithMessage("The value of message cannot be empty.");
        }

        [Fact(DisplayName = "Must be able to like 'Post'")]
        [Trait("Aggregate", "Post")]
        public void Post_Like_MustIncrementLikes()
        {
            var sut = _postAggregateFixture.BuildValidPost();
            var currentLikes = sut.Likes;

            sut.LikePost();

            sut.Likes.Should<int>().Be(currentLikes + 1);
        }

        [Fact(DisplayName = "Must be unable to like 'Post' when 'Post' is deleted")]
        [Trait("Aggregate", "Post")]
        public void LikePost_WhenPostIsDeleted_MustThrowInvalidOperationException()
        {
            var sut = _postAggregateFixture.BuildDeletedPost();

            sut.Invoking(x => x.LikePost())
               .Should().Throw<InvalidOperationException>()
               .WithMessage("Unable to like deleted post.");
        }

        [Fact(DisplayName = "Must be able to add comment to 'Post'")]
        [Trait("Aggregate", "Post")]
        public void Post_Comment_MustAddNewComment()
        {
            var expectedUserName = _faker.Internet.UserName();
            var expectedComment = new Tuple<string, string>(expectedUserName, string.Join(" ", _faker.Lorem.Words(_faker.Random.Int(3, 50))));
            var sut = _postAggregateFixture.BuildValidPost();

            sut.AddComment(expectedComment.Item1, expectedComment.Item2);

            sut.Comments.First().Value.Should().Be(expectedComment);
        }

        [Fact(DisplayName = "Must be unable to add comment to 'Post' when 'Post' is deleted")]
        [Trait("Aggregate", "Post")]
        public void AddComment_WhenPostIsDeleted_MustThrowInvalidOperationException()
        {
            var expectedUserName = _faker.Internet.UserName();
            var expectedComment = string.Join(" ", _faker.Lorem.Words(_faker.Random.Int(3, 50)));
            var sut = _postAggregateFixture.BuildDeletedPost();

            sut.Invoking(x => x.AddComment(expectedUserName, expectedUserName))
               .Should().Throw<InvalidOperationException>()
               .WithMessage("Unable to comment to an inactive post.");
        }

        [Fact(DisplayName = "Must be unable to add comment to 'Post' with empty message")]
        [Trait("Aggregate", "Post")]
        public void AddComment_WithEmptyMessage_MustThrowInvalidOperationException()
        {
            var expectedUserName = _faker.Internet.UserName();
            var sut = _postAggregateFixture.BuildValidPost();

            sut.Invoking(x => x.AddComment(string.Empty, expectedUserName))
               .Should().Throw<InvalidOperationException>()
               .WithMessage("The value of comment cannot be empty.");
        }

        [Fact(DisplayName = "Must be able to update 'Post' comment")]
        [Trait("Aggregate", "Post")]
        public void Post_Comment_MustUpdateComment()
        {
            var sut = _postAggregateFixture.BuildValidPostWithComment();
            var existingCommentId = sut.Comments.First().Key;
            var expectedUserName = sut.Comments.First().Value.Item2;
            var expectedComment = string.Join(" ", _faker.Lorem.Words(_faker.Random.Int(3, 50)));

            sut.UpdateComment(existingCommentId, expectedComment, expectedUserName);

            sut.Comments.First().Value.Item1.Should().Be(expectedComment);
        }

        [Fact(DisplayName = "Must be unable to update 'Post' comment when 'Post' is deleted")]
        [Trait("Aggregate", "Post")]
        public void UpdateComment_WhenPostIsDeleted_MustThrowInvalidOperationException()
        {
            var sut = _postAggregateFixture.BuildDeletedPostWithComments();
            var existingCommentId = sut.Comments.First().Key;
            var expectedUserName = sut.Comments.First().Value.Item2;
            var expectedComment = string.Join(" ", _faker.Lorem.Words(_faker.Random.Int(3, 50)));

            sut.Invoking(x => x.UpdateComment(existingCommentId, expectedComment, expectedUserName))
               .Should().Throw<InvalidOperationException>()
               .WithMessage("Unable to edit comment of an inactive post.");
        }

        [Fact(DisplayName = "Must be unable to update 'Post' comment from someone else")]
        [Trait("Aggregate", "Post")]
        public void UpdateComment_WhenPostIsFromSomeoneElse_MustThrowInvalidOperationException()
        {
            var sut = _postAggregateFixture.BuildValidPostWithComment();
            var existingCommentId = sut.Comments.First().Key;
            var expectedUserName = _faker.Internet.UserName();
            var expectedComment = string.Join(" ", _faker.Lorem.Words(_faker.Random.Int(3, 50)));

            sut.Invoking(x => x.UpdateComment(existingCommentId, expectedComment, expectedUserName))
               .Should().Throw<InvalidOperationException>()
               .WithMessage("You are not allowed to edit a comment that was made by another user.");
        }

        [Fact(DisplayName = "Must be able to delete a 'Post'")]
        [Trait("Aggregate", "Post")]
        public void Post_Delete_MustDeletePost()
        {
            var sut = _postAggregateFixture.BuildValidPostWithComment();

            sut.DeletePost(sut.Author);

            sut.Active.Should().BeFalse();
        }

        [Fact(DisplayName = "Must be unable to delete a deleted 'Post'")]
        [Trait("Aggregate", "Post")]
        public void DeletePost_WhenPostIsDeleted_MustThrowInvalidOperationException()
        {
            var sut = _postAggregateFixture.BuildDeletedPostWithComments();            

            sut.Invoking(x => x.DeletePost(sut.Author))
               .Should().Throw<InvalidOperationException>()
               .WithMessage("The post has already been removed.");
        }

        [Fact(DisplayName = "Must be unable to delete a deleted 'Post'")]
        [Trait("Aggregate", "Post")]
        public void DeletePost_WhenPostIsFromSomeoneElse_MustThrowInvalidOperationException()
        {
            var sut = _postAggregateFixture.BuildValidPostWithComment();
            var otherUser = _faker.Internet.UserName();

            sut.Invoking(x => x.DeletePost(otherUser))
               .Should().Throw<InvalidOperationException>()
               .WithMessage("You are not allowed to delete a post from someone else.");
        }
    }
}