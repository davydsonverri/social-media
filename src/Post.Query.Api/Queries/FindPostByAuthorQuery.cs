using CQRS.Core.Queries;

namespace Post.Query.Api.Queries
{
    public record FindPostByAuthorQuery : BaseQuery
    {
        public required string Author { get; set; }
    }
}
