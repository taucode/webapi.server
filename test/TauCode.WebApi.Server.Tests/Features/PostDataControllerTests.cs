using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Net;
using System.Net.Http;
using TauCode.WebApi.Server.Tests.AppHost.Features.PostData;

namespace TauCode.WebApi.Server.Tests.Features
{
    [TestFixture]
    public class PostDataControllerTests : AppTestBase
    {
        [Test]
        public void PostData_ValidRequest_ReturnsGreeting()
        {
            // Arrange
            var command = new PostDataCommand
            {
                UserName = "ak",
                Birthday = new DateTime(1978, 7, 5),
            };

            // Act
            var response = this.HttpClient.PostAsJsonAsync($"api/misc/post-data", command).Result;

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var json = response.Content.ReadAsStringAsync().Result;
            var responsePayload = JsonConvert.DeserializeObject<PostDataResponse>(json);
            Assert.That(responsePayload.Greeting, Is.EqualTo("Hello, ak! Your birthday is 1978-07-05."));
        }

        [Test]
        public void PostData_InvalidRequest_ReturnsValidationResult()
        {
            // Arrange
            var badCommand = this.CreateBadCommand();

            // Act
            var response = this.HttpClient.PostAsJsonAsync($"api/misc/post-data", badCommand).Result;

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            var json = response.Content.ReadAsStringAsync().Result;
            var validationError = JsonConvert.DeserializeObject<ValidationErrorDto>(json);
            Assert.That(validationError.Code, Is.EqualTo("ValidationError"));
            Assert.That(validationError.Message, Is.EqualTo("The request is invalid."));

            Assert.That(validationError.Failures, Has.Count.EqualTo(2));

            var failure = validationError.Failures["userName"];
            Assert.That(failure.Code, Is.EqualTo("NotEmptyValidator"));
            Assert.That(failure.Message, Is.EqualTo("'User Name' must not be empty."));

            failure = validationError.Failures["birthday"];
            Assert.That(failure.Code, Is.EqualTo("GreaterThanValidator"));
            Assert.That(failure.Message, Is.EqualTo("Too old :(."));
        }

        [Test]
        public void PostDataAsync_InvalidRequest_ReturnsValidationResult()
        {
            // Arrange
            var badCommand = this.CreateBadCommand();

            // Act
            var response = this.HttpClient.PostAsJsonAsync($"api/misc/post-data-async", badCommand).Result;

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            var json = response.Content.ReadAsStringAsync().Result;
            var validationError = JsonConvert.DeserializeObject<ValidationErrorDto>(json);
            Assert.That(validationError.Code, Is.EqualTo("ValidationError"));
            Assert.That(validationError.Message, Is.EqualTo("The request is invalid."));

            Assert.That(validationError.Failures, Has.Count.EqualTo(2));

            var failure = validationError.Failures["userName"];
            Assert.That(failure.Code, Is.EqualTo("NotEmptyValidator"));
            Assert.That(failure.Message, Is.EqualTo("'User Name' must not be empty."));

            failure = validationError.Failures["birthday"];
            Assert.That(failure.Code, Is.EqualTo("GreaterThanValidator"));
            Assert.That(failure.Message, Is.EqualTo("Too old :(."));
        }

        private PostDataCommand CreateBadCommand()
        {
            return new PostDataCommand
            {
                UserName = "",
                Birthday = new DateTime(1954, 11, 11),
            };
        }
    }
}
