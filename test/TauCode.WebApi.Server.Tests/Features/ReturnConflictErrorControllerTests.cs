using NUnit.Framework;
using System.Linq;
using System.Net;

namespace TauCode.WebApi.Server.Tests.Features
{
    [TestFixture]
    public class ReturnConflictErrorControllerTests : AppTestBase
    {
        [Test]
        public void GetResponse_NoArguments_ReturnsExpectedConflictError()
        {
            // Arrange

            // Act
            var response = this.HttpClient.GetAsync($"api/misc/conflict").Result;

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Conflict));
            Assert.That(response.Headers.GetValues("X-Payload-Type").Single(), Is.EqualTo("Error"));
            var error = response.ReadAsError();
            Assert.That(error.Code, Is.EqualTo("System.InvalidOperationException"));
            Assert.That(error.Message, Is.EqualTo("Bad action!"));
        }
    }
}
