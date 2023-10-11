using CQRS.Core.Commands;
using CQRS.Core.Exceptions;
using CQRS.Core.Infra;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Post.Command.Api.Commands;
using Post.Common.DTOs;

namespace Post.Command.Api.Controllers
{
    [Route("api/v1/events/")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly ILogger<EventsController> _logger;
        private readonly ICommandDispatcher _commandDispatcher;

        public EventsController(ILogger<EventsController> logger, ICommandDispatcher commandDispatcher)
        {
            _logger = logger;
            _commandDispatcher = commandDispatcher;
        }

        [HttpPost("restore-read-db")]
        public async Task<ActionResult> RestoreReadDbAsync()
        {
            try
            {
                await _commandDispatcher.SendAsync(new RestoreReadDb());

                return StatusCode(StatusCodes.Status201Created, new BaseResponse
                {
                    Message = "Read database restore request completed successfully!"
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
                const string errorMessage = "Error while processing request to restore read database";
                _logger.LogError(ex, errorMessage);
                return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse()
                {
                    Message = errorMessage
                });
            }
        }

    }
}
