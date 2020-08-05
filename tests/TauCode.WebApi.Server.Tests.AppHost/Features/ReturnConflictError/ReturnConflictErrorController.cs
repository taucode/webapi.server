using System;
using Microsoft.AspNetCore.Mvc;

namespace TauCode.WebApi.Server.Tests.AppHost.Features.ReturnConflictError
{
    [ApiController]
    public class ReturnConflictErrorController : ControllerBase
    {
        [HttpGet]
        [Route("api/misc/conflict")]
        public IActionResult ReturnConflictError()
        {
            var ex = new InvalidOperationException("Bad action!");
            return this.ConflictError(ex);
        }
    }
}
