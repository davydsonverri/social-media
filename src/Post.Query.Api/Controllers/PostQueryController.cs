using CQRS.Core.Infra;
using Microsoft.AspNetCore.Mvc;
using Post.Common.DTOs;
using Post.Query.Api.DTOs;
using Post.Query.Api.Queries;
using Post.Query.Domain.Entities;

namespace Post.Query.Api.Controllers
{
    [ApiController]
    [Route("api/v1/post")]
    public class PostQueryController : ControllerBase
    {
        private readonly ILogger<PostQueryController> _logger;
        private readonly IQueryDispatcher<PostEntity> _queryDispatcher;

        public PostQueryController(ILogger<PostQueryController> logger, IQueryDispatcher<PostEntity> queryDispatcher)
        {
            _logger = logger;
            _queryDispatcher = queryDispatcher;
        }

        [HttpGet]
        public async Task<ActionResult> GetPosts()
        {
            try
            {
                var posts = await _queryDispatcher.SendAsync(new ListAllPostsQuery());

                if (posts == null || posts.Count == 0)
                {
                    return NoContent();
                }

                return Ok(new PostQueryResponse
                {
                    Posts = posts,
                    Message = $"Record count {posts.Count}"
                });
            } catch (Exception ex)
            {
                string errorMessage = $"Error while processing {typeof(ListAllPostsQuery)}";
                _logger.LogError(ex, errorMessage);

                return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse
                {
                    Message = errorMessage
                });
            }
        }

        [HttpGet("{postId}")]
        public async Task<ActionResult> GetPosts(Guid postId)
        {
            try
            {
                var posts = await _queryDispatcher.SendAsync(new FindPostByIdQuery() { Id = postId });

                if (posts == null || posts.Count == 0)
                {
                    return NoContent();
                }

                return Ok(new PostQueryResponse
                {
                    Posts = posts,
                    Message = $"Record count {posts.Count}"
                });
            } catch (Exception ex)
            {
                string errorMessage = $"Error while processing {typeof(FindPostByIdQuery)}";
                _logger.LogError(ex, errorMessage);

                return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse
                {
                    Message = errorMessage
                });
            }
        }

        [HttpGet("authors/{author}")]
        public async Task<ActionResult> FindPostByAuthorQuery(string author)
        {
            try
            {
                var posts = await _queryDispatcher.SendAsync(new FindPostByAuthorQuery() { Author = author });

                if (posts == null || posts.Count == 0)
                {
                    return NoContent();
                }

                return Ok(new PostQueryResponse
                {
                    Posts = posts,
                    Message = $"Record count {posts.Count}"
                });
            } catch (Exception ex)
            {
                string errorMessage = $"Error while processing {typeof(FindPostByAuthorQuery)}";
                _logger.LogError(ex, errorMessage);

                return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse
                {
                    Message = errorMessage
                });
            }
        }
    }
}
