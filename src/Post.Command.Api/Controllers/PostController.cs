using CQRS.Core.Commands;
using CQRS.Core.Exceptions;
using CQRS.Core.Infra;
using Domain.Identity;
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
        private readonly IDomainIdentity _domainIdentity;

        public PostController(ILogger<PostController> logger, ICommandDispatcher commandDispatcher, IDomainIdentity domainIdentity)
        {
            _logger = logger;
            _commandDispatcher = commandDispatcher;
            _domainIdentity = domainIdentity;
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
            command.Id = _domainIdentity.NewId();
            return await DispatchCommand(command);
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult> PatchPosts(Did id, UpdatePost command)
        {
            command.Id = id;
            return await DispatchCommand(command);
        }

        [HttpDelete("{postId}")]
        public async Task<ActionResult> DeletePosts(Did postId, DeletePost command)
        {
            command.Id = postId;
            return await DispatchCommand(command);
        }

        [HttpPost("{id}/like")]
        public async Task<ActionResult> Like(Did id)
        {
            return await DispatchCommand(new LikePost { Id = id });
        }

        [HttpPost("{id}/comments")]
        public async Task<ActionResult> PostComments(Did id, CommentPost command)
        {
            command.Id = id;
            return await DispatchCommand(command);            
        }

        [HttpPatch("{postId}/comments/{commentId}")]
        public async Task<ActionResult> PatchComments(Did postId, Did commentId,UpdateComment command)
        {
            command.Id = postId;
            command.CommentId = commentId;
            return await DispatchCommand(command);
        }

        [HttpDelete("{postId}/comments/{commentId}")]
        public async Task<ActionResult> DeleteComments(Did postId, Did commentId, DeleteComment command)
        {
            command.Id = postId;
            command.CommentId = commentId;
            return await DispatchCommand(command);
        }
    }
}
