using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace TauCode.WebApi.Server.Tests.AppHost.Features.ReturnValidationError;

[ApiController]
public class ReturnValidationErrorController : ControllerBase
{
    [HttpGet]
    [Route("api/misc/validation-exception")]
    public IActionResult ReturnValidationErrorException()
    {
        var ex = new ValidationException(
            "Couldn't validate",
            new List<ValidationFailure>
            {
                new ValidationFailure("name", "Too long")
                {
                    ErrorCode = "NameValidator",
                },
                new ValidationFailure("birthday", "Too young")
                {
                    ErrorCode = "DateValidator",
                },
            });

        return this.ValidationError(ex);
    }

    [HttpGet]
    [Route("api/misc/validation-result")]
    public IActionResult ReturnValidationResult()
    {
        var result = new ValidationResult(new List<ValidationFailure>
        {
            new ValidationFailure("pork", "Too fat")
            {
                ErrorCode = "FoodValidator",
            },
            new ValidationFailure("house", "Too far")
            {
                ErrorCode = "HomeValidator",
            },
        });

        return this.ValidationError(result);
    }

}