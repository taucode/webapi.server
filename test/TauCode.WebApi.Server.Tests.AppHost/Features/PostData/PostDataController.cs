using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using TauCode.Cqrs.Commands;

namespace TauCode.WebApi.Server.Tests.AppHost.Features.PostData
{
    [ApiController]
    public class PostDataController : ControllerBase
    {
        private readonly ICommandDispatcher _commandDispatcher;

        public PostDataController(ICommandDispatcher commandDispatcher)
        {
            _commandDispatcher = commandDispatcher;
        }

        [HttpPost]
        [Route("api/misc/post-data")]
        public IActionResult PostData([FromBody] PostDataCommand command)
        {
            _commandDispatcher.Dispatch(command);
            var response = new PostDataResponse
            {
                Greeting = command.GetResult(),
            };
            return this.Ok(response);
        }

        [HttpPost]
        [Route("api/misc/post-data-async")]
        public async Task<IActionResult> PostDataAsync([FromBody] PostDataCommand command)
        {
            await _commandDispatcher.DispatchAsync(command, CancellationToken.None);
            var response = new PostDataResponse
            {
                Greeting = command.GetResult(),
            };
            return this.Ok(response);
        }
    }
}
