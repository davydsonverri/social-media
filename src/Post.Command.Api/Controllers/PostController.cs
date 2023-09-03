using CQRS.Core.Exceptions;
using CQRS.Core.Infra;
using Microsoft.AspNetCore.Mvc;
using Post.Command.Api.Commands;
using Post.Command.Api.DTOs;
using Post.Common.DTOs;

namespace Post.Command.Api.Controllers
{
    [Route("api/v1/posts")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly ILogger<PostController> _logger;
        private readonly ICommandDispatcher _commandDispatcher;

        public PostController(ILogger<PostController> logger, ICommandDispatcher commandDispatcher)
        {
            _logger = logger;
            _commandDispatcher = commandDispatcher;
        }

        [HttpPost]
        public async Task<ActionResult> Post(CreatePost command)
        {
            var id = Guid.NewGuid();

            try
            {
                command.Id = id;
                await _commandDispatcher.SendAsync(command);

                return StatusCode(StatusCodes.Status201Created, new CreatePostResponse
                {
                    PostId = id,
                    Message = "Post creation request completed successfuly"
                });
            } catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Client made a bad request");
                return BadRequest(new BaseResponse()
                {
                    Message = ex.Message
                });
            } catch (Exception ex)
            {
                const string errorMessage = "Error while processing request to create a post";
                _logger.LogError(ex, errorMessage);
                return BadRequest(new CreatePostResponse()
                {
                    PostId = id,
                    Message = errorMessage
                });
            }
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult> Patch(Guid id, UpdatePost command)
        {
            try
            {
                command.Id = id;
                await _commandDispatcher.SendAsync(command);

                return StatusCode(StatusCodes.Status201Created, new BaseResponse
                {
                    Message = "Post creation request completed successfuly"
                });
            } catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Client made a bad request");
                return BadRequest(new BaseResponse()
                {
                    Message = ex.Message
                });
            } catch (AggregateNotFoundException ex)
            {
                _logger.LogWarning(ex, "Unable to find post with id {postId}", command.Id);
                return BadRequest(new BaseResponse()
                {
                    Message = ex.Message
                });
            } catch (Exception ex)
            {
                const string errorMessage = "Error while processing request to edit a post";
                _logger.LogError(ex, errorMessage);
                return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse()
                {
                    Message = errorMessage
                });
            }
        }

        [HttpPost("{id}/like")]
        public async Task<ActionResult> Like(Guid id)
        {
            try
            {
                await _commandDispatcher.SendAsync(new LikePost { Id = id });

                return StatusCode(StatusCodes.Status201Created, new BaseResponse
                {
                    Message = "Like post request completed successfuly"
                });
            } catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Client made a bad request");
                return BadRequest(new BaseResponse()
                {
                    Message = ex.Message
                });
            } catch (AggregateNotFoundException ex)
            {
                _logger.LogWarning(ex, "Unable to find post with id {postId}", id);
                return BadRequest(new BaseResponse()
                {
                    Message = ex.Message
                });
            } catch (Exception ex)
            {
                const string errorMessage = "Error while processing request to like a post";
                _logger.LogError(ex, errorMessage);
                return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse()
                {
                    Message = errorMessage
                });
            }
        }

        [HttpPost("{id}/comments")]
        public async Task<ActionResult> PostComments(Guid id, CommentPost command)
        {
            try
            {
                command.Id = id;
                await _commandDispatcher.SendAsync(command);

                return StatusCode(StatusCodes.Status201Created, new BaseResponse
                {
                    Message = "Comment creation request completed successfuly"
                });
            } catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Client made a bad request");
                return BadRequest(new BaseResponse()
                {
                    Message = ex.Message
                });
            } catch (AggregateNotFoundException ex)
            {
                _logger.LogWarning(ex, "Unable to find post with id {postId}", command.Id);
                return BadRequest(new BaseResponse()
                {
                    Message = ex.Message
                });
            } catch (Exception ex)
            {
                const string errorMessage = "Error while processing request to comment a post";
                _logger.LogError(ex, errorMessage);
                return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse()
                {
                    Message = errorMessage
                });
            }
        }

    }
}
