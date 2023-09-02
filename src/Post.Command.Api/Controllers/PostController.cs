﻿using CQRS.Core.Infra;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Post.Command.Api.Commands;
using Post.Command.Api.DTOs;
using Post.Common.DTOs;

namespace Post.Command.Api.Controllers
{
    [Route("api/v1/[controller]")]
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
    }
}
