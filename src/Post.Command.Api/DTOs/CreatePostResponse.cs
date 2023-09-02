using Post.Common.DTOs;

namespace Post.Command.Api.DTOs
{
    public class CreatePostResponse : BaseResponse
    {
        public required Guid PostId { get; set; }
    }
}
