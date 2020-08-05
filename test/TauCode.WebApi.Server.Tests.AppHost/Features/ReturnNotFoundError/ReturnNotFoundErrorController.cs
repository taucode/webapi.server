using System.Globalization;
using Microsoft.AspNetCore.Mvc;

namespace TauCode.WebApi.Server.Tests.AppHost.Features.ReturnNotFoundError
{
    [ApiController]
    public class ReturnNotFoundErrorController : ControllerBase
    {
        [HttpGet]
        [Route("api/misc/not-found")]
        public IActionResult ReturnNotFoundError()
        {
            var ex = new CultureNotFoundException("Bez kultur net multur.");
            return this.NotFoundError(ex);
        }
    }
}
