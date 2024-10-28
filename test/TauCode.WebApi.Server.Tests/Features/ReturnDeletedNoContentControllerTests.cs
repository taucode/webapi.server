using NUnit.Framework;
using System.Net;

namespace TauCode.WebApi.Server.Tests.Features;

[Ignore("todo: Temporary. remove in prod")]
[TestFixture]
public class ReturnDeletedNoContentControllerTests : AppTestBase
{
    [Test]
    public void GetResponse_NoArguments_ReturnsExpectedDeletedNoContentResponse()
    {
        // Arrange

        // Act
        var response = this.HttpClient.GetAsync($"api/misc/deleted-no-content").Result;

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
        Assert.That(response.Headers.GetValues("X-Deleted-Instance-Id").Single(), Is.EqualTo("deleted-id"));
    }
}