using NUnit.Framework;
using System.Net;

namespace TauCode.WebApi.Server.Tests.Features;

[TestFixture]
public class ReturnNotFoundErrorControllerTests : AppTestBase
{
    [Test]
    public void GetResponse_NoArguments_ReturnsExpectedConflictError()
    {
        // Arrange

        // Act
        var response = this.HttpClient.GetAsync($"api/misc/not-found").Result;

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        Assert.That(response.Headers.GetValues("X-Payload-Type").Single(), Is.EqualTo("Error"));
        var error = response.ReadAsError();
        Assert.That(error.Code, Is.EqualTo("System.Globalization.CultureNotFoundException"));
        Assert.That(error.Message, Is.EqualTo("Bez kultur net multur."));
    }
}