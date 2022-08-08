using FluentValidation.Results;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Text;

// todo clean
namespace TauCode.WebApi.Server.Tests;

internal static class TestExtensions
{
    internal static T ReadAs<T>(this HttpResponseMessage message)
    {
        var json = message.Content.ReadAsStringAsync().Result;
        var result = JsonConvert.DeserializeObject<T>(json);
        return result;
    }

    internal static ErrorDto ReadAsError(this HttpResponseMessage message)
    {
        return message.ReadAs<ErrorDto>();
    }

    internal static ValidationErrorDto ReadAsValidationError(this HttpResponseMessage message)
    {
        return message.ReadAs<ValidationErrorDto>();
    }

    internal static string BuildQueryString(this IDictionary<string, string> parameterDictionary)
    {
        var sb = new StringBuilder();
        var added = false;

        foreach (var pair in parameterDictionary)
        {
            if (pair.Value == null)
            {
                continue;
            }

            if (added)
            {
                sb.Append("&");
            }

            added = true;

            sb.Append($"{pair.Key}={pair.Value}");
        }

        return sb.ToString();
    }

    //internal static IdDto ToIdDto(this string s)
    //{
    //    if (s == null)
    //    {
    //        return null;
    //    }

    //    return new IdDto(s);
    //}

    //internal static IdDto ToIdDto(this IdBase id)
    //{
    //    return new IdDto(id.Id);
    //}

    internal static ValidationResult ShouldBeValid(this ValidationResult validationResult)
    {
        Assert.That(validationResult.IsValid, Is.True);
        return validationResult;
    }

    internal static ValidationResult ShouldBeInvalid(this ValidationResult validationResult, int expectedErrorCount)
    {
        Assert.That(validationResult.IsValid, Is.False);
        Assert.That(validationResult.Errors, Has.Count.EqualTo(expectedErrorCount));

        return validationResult;
    }

    internal static ValidationResult ShouldHaveError(
        this ValidationResult validationResult,
        int errorIndex,
        string propertyName,
        string expectedErrorCode,
        string expectedErrorMessage)
    {
        Assert.That(validationResult.Errors[errorIndex].PropertyName, Is.EqualTo(propertyName));
        Assert.That(validationResult.Errors[errorIndex].ErrorCode, Is.EqualTo(expectedErrorCode));
        Assert.That(validationResult.Errors[errorIndex].ErrorMessage, Is.EqualTo(expectedErrorMessage));

        return validationResult;
    }

    internal static ValidationErrorDto ShouldHaveFailureNumber(
        this ValidationErrorDto validationError,
        int failureNumber)
    {
        Assert.That(validationError.Failures, Has.Count.EqualTo(failureNumber));
        return validationError;
    }

    internal static ValidationErrorDto ShouldContainFailure(
        this ValidationErrorDto validationError,
        string key,
        string code,
        string message)
    {
        Assert.That(validationError.Failures, Does.ContainKey(key));
        var failure = validationError.Failures[key];
        Assert.That(failure.Code, Is.EqualTo(code));
        Assert.That(failure.Message, Is.EqualTo(message));

        return validationError;
    }
}