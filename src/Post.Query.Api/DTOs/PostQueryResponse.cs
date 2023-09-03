using Post.Common.DTOs;
using Post.Query.Domain.Entities;

namespace Post.Query.Api.DTOs
{
    public class PostQueryResponse : BaseResponse
    {
        public required List<PostEntity> Posts { get; set; }
    }
}
