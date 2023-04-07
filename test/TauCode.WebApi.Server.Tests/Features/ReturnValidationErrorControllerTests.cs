using NUnit.Framework;
using System.Net;

namespace TauCode.WebApi.Server.Tests.Features;

[Ignore("todo: Temporary. remove in prod")]
[TestFixture]
public class ReturnValidationErrorControllerTests : AppTestBase
{
    [Test]
    public void GetValidationException_NoArguments_ReturnsExpectedValidationErrorDto()
    {
        // Arrange

        // Act
        var response = this.HttpClient.GetAsync($"api/misc/validation-exception").Result;

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        Assert.That(response.Headers.GetValues("X-Payload-Type").Single(), Is.EqualTo("Error.Validation"));
        var validationError = response.ReadAsValidationError();
        Assert.That(validationError.Code, Is.EqualTo("ValidationError"));
        Assert.That(validationError.Message, Is.EqualTo("Couldn't validate"));
        validationError.ShouldHaveFailureNumber(2)
            .ShouldContainFailure("name", "NameValidator", "Too long")
            .ShouldContainFailure("birthday", "DateValidator", "Too young");
    }

    [Test]
    public void GetValidationResult_NoArguments_ReturnsExpectedValidationErrorDto()
    {
        // Arrange

        // Act
        var response = this.HttpClient.GetAsync($"api/misc/validation-result").Result;

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        Assert.That(response.Headers.GetValues("X-Payload-Type").Single(), Is.EqualTo("Error.Validation"));
        var validationError = response.ReadAsValidationError();
        Assert.That(validationError.Code, Is.EqualTo("ValidationError"));
        Assert.That(validationError.Message, Is.EqualTo("The request is invalid."));
        validationError.ShouldHaveFailureNumber(2)
            .ShouldContainFailure("pork", "FoodValidator", "Too fat")
            .ShouldContainFailure("house", "HomeValidator", "Too far");
    }
}