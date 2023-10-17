using Bogus;
using Domain.Identity.ULID;
using Post.Command.Domain.Aggregates;

namespace Post.Command.Domain.Tests.Fixture
{
    [CollectionDefinition(nameof(PostCollection))]
    public class PostCollection : ICollectionFixture<PostAggregateFixture> { }

    public class PostAggregateFixture : IDisposable
    {
        private readonly Faker _faker;
        private readonly UlidGenerator _ulidGenerator = new();

        public PostAggregateFixture()
        {
            _faker = new Faker(locale: "pt_BR");
        }

        public PostAggregate BuildValidPost()
        {
            var postId = _ulidGenerator.NewId();
            string postAuthor = _faker.Person.FullName;
            string postMessage = string.Join(" ", _faker.Lorem.Words(_faker.Random.Int(3, 50)));
            var aggregate = new PostAggregate(postId, postAuthor, postMessage);
            return aggregate;
        }

        public PostAggregate BuildValidPostWithComment()
        {
            var postId = _ulidGenerator.NewId();
            string postAuthor = _faker.Person.FullName;
            string postMessage = string.Join(" ", _faker.Lorem.Words(_faker.Random.Int(3, 50)));
            var aggregate = new PostAggregate(postId, postAuthor, postMessage);
            
            var postComment = string.Join(" ", _faker.Lorem.Words(_faker.Random.Int(3, 50)));
            var postCommentId = _ulidGenerator.NewId();
            var postUsername = _faker.Internet.UserName();
            aggregate.AddComment(postCommentId, postComment, postUsername);

            return aggregate;
        }

        public PostAggregate BuildDeletedPost()
        {
            var aggregate = BuildValidPost();
            aggregate.DeletePost(aggregate.Author);
            return aggregate;
        }

        public PostAggregate BuildDeletedPostWithComments()
        {
            var aggregate = BuildValidPostWithComment();
            aggregate.DeletePost(aggregate.Author);
            return aggregate;
        }

        public void Dispose()
        {

        }
    }
}
