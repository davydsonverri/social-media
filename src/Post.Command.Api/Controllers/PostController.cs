using CQRS.Core.Commands;
using CQRS.Core.Exceptions;
using CQRS.Core.Identity;
using CQRS.Core.Infra;
using Microsoft.AspNetCore.Mvc;
using Post.Command.Api.Commands;
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

        private async Task<ActionResult> DispatchCommand(BaseCommand command)
        {
            try
            {
                await _commandDispatcher.SendAsync(command);

                return StatusCode(StatusCodes.Status201Created, new BaseResponse
                {
                    Message = $"{command.GetType().Name} command completed successfuly"
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
                _logger.LogWarning(ex, "Unable to find aggregate with id {id}", command.Id);
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

        [HttpPost]
        public async Task<ActionResult> PostPosts(CreatePost command)
        {
            command.Id = IdGenerator.NewId();
            return await DispatchCommand(command);
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult> PatchPosts(Guid id, UpdatePost command)
        {
            command.Id = id;
            return await DispatchCommand(command);
        }

        [HttpDelete("{postId}")]
        public async Task<ActionResult> DeletePosts(Guid postId, DeletePost command)
        {
            command.Id = postId;
            return await DispatchCommand(command);
        }

        [HttpPost("{id}/like")]
        public async Task<ActionResult> Like(Guid id)
        {
            return await DispatchCommand(new LikePost { Id = id });
        }

        [HttpPost("{id}/comments")]
        public async Task<ActionResult> PostComments(Guid id, CommentPost command)
        {
            command.Id = id;
            return await DispatchCommand(command);            
        }

        [HttpPatch("{postId}/comments/{commentId}")]
        public async Task<ActionResult> PatchComments(Guid postId, Guid commentId,UpdateComment command)
        {
            command.Id = postId;
            command.CommentId = commentId;
            return await DispatchCommand(command);
        }

        [HttpDelete("{postId}/comments/{commentId}")]
        public async Task<ActionResult> DeleteComments(Guid postId, Guid commentId, DeleteComment command)
        {
            command.Id = postId;
            command.CommentId = commentId;
            return await DispatchCommand(command);
        }
    }
}
