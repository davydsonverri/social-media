using Bogus;
using Post.Command.Domain.Aggregates;

namespace Post.Command.Domain.Tests.Fixture
{
    [CollectionDefinition(nameof(PostCollection))]
    public class PostCollection : ICollectionFixture<PostAggregateFixture> { }

    public class PostAggregateFixture : IDisposable
    {
        private readonly Faker _faker;

        public PostAggregateFixture()
        {
            _faker = new Faker(locale: "pt_BR");
        }

        public PostAggregate BuildValidPost()
        {
            var id = _faker.Random.Guid();
            string author = _faker.Person.FullName;
            string message = string.Join(" ", _faker.Lorem.Words(_faker.Random.Int(3, 50)));
            var aggregate = new PostAggregate(id, author, message);
            return aggregate;
        }

        public PostAggregate BuildDeletedPost()
        {
            var aggregate = BuildValidPost();
            aggregate.DeletePost(aggregate.Author);
            return aggregate;
        }

        public void Dispose()
        {

        }
    }
}
