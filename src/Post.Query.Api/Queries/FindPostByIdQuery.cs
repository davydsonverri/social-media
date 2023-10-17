using CQRS.Core.Queries;

namespace Post.Query.Api.Queries
{
    public record FindPostByIdQuery : BaseQuery
    {
        public required Did Id { get; set; }
    }
}
