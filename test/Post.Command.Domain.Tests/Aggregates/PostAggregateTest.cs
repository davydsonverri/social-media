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
    }
}