using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using TauCode.Cqrs.Commands;
using TauCode.Validation;

namespace TauCode.WebApi.Server;

public class ValidationFilterAttribute : ActionFilterAttribute
{
    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        ValidationErrorDto? validationError = null;
        var commandValidatorSource = this.GetCommandValidatorSource(context);

        // Verify that the model state is valid before running the argument value specific validators.
        if (!context.ModelState.IsValid)
        {
            // Prepare the validation error response
            validationError = ValidationErrorDto.CreateStandard();

            foreach (var fieldState in context.ModelState)
            {
                // Make sure that all the property names are camel cased and remove all model prefixes
                var fieldName = WebApiHostHelper.EnsurePropertyNameIsCamelCase(
                    Regex.Replace(fieldState.Key, @"^(.*?\.)(.*)", "$2"));

                // Get only the first error, the rest will be skipped
                var error = fieldState.Value.Errors.First();

                // Create the error message
                var errorMessage = "Unknown error.";
                if (!string.IsNullOrEmpty(error.ErrorMessage))
                {
                    errorMessage = error.ErrorMessage;
                }
                else if (!string.IsNullOrEmpty(error.Exception?.Message))
                {
                    errorMessage = error.Exception.Message;
                }

                // Add the error to the response, with an empty error code, since this is an unspecific error
                validationError.AddFailure(fieldName, "MvcModelError", errorMessage);
            }
        }

        // Validate all the arguments for the current action
        foreach (var argument in context.ActionDescriptor.Parameters)
        {
            // Skip all arguments without a registered validator

            // todo clean
            //var validatorRecord = _validatorTypes.GetValueOrDefault(argument.ParameterType);
            //if (validatorRecord == null)
            //{
            //    continue;
            //}

            // Get the registered validator
            //var validator = context.HttpContext.RequestServices.GetService(validatorRecord.ValidatorType);

            //if (validator == null)
            //{
            //    continue; // could not resolve validator
            //}

            var validator = commandValidatorSource.CreateCommandValidator(
                context.HttpContext.RequestServices, argument.ParameterType);

            if (validator == null)
            {
                continue;
            }

            // Inject the action arguments into the validator, so that they can be used in the validation
            // This is a "hack" to, amongst other, support unique validation on the update commands where the resource id is needed to exclude itself from the unique check.
            if (validator is IParameterValidator parameterValidator)
            {
                parameterValidator.Parameters = context.ActionArguments;
            }

            // Validate the argument
            var argumentValue = context.ActionArguments[argument.Name];

            if (argumentValue == null)
            {
                validationError = ValidationErrorDto.CreateStandard($"Argument '{argument.Name}' is null.");
                break;
            }

            // todo clean
            //var method = validatorRecord.ValidateMethod;
            //var validationResult = (ValidationResult)method.Invoke(validator, new[] { argumentValue });

            var validationResult = await this.ValidateAsync(validator, argumentValue);

            // Return if the argument value was valid
            if (validationResult.IsValid)
            {
                continue;
            }

            // Create an validation error response, if it does not already exist
            if (validationError == null)
            {
                validationError = ValidationErrorDto.CreateStandard();
            }

            // Add every field specific error to validation error response
            foreach (var validationFailure in validationResult.Errors)
            {
                // Make sure that all the property names are camel cased
                var propertyName = WebApiHostHelper.EnsurePropertyNameIsCamelCase(validationFailure.PropertyName);

                // Only add the first validation message for a property
                if (!validationError.Failures.ContainsKey(propertyName))
                {
                    validationError.AddFailure(propertyName, validationFailure.ErrorCode, validationFailure.ErrorMessage);
                }
            }
        }

        if (validationError != null)
        {
            context.Result = new ContentResult
            {
                StatusCode = StatusCodes.Status400BadRequest,
                ContentType = "application/json",
                Content = JsonConvert.SerializeObject(validationError),
            };

            // Set the action response to a 400 Bad Request, with the validation error response as content
            context.HttpContext.Response.Headers.Add(DtoHelper.PayloadTypeHeaderName, DtoHelper.ValidationErrorPayloadType);
        }
        else
        {
            this.OnActionExecuted(await next());
        }
    }

    private Task<ValidationResult> ValidateAsync(object validator, object argumentValue)
    {
        // todo: keep method in record
        var method = validator.GetType()
            .GetMethod(
                nameof(AbstractValidator<ICommand>.ValidateAsync),
                new Type[] { argumentValue.GetType(), typeof(CancellationToken) });

        if (method == null)
        {
            throw new NotImplementedException();
        }

        var task = (Task<ValidationResult>)method.Invoke(
            validator,
            new object[]
            {
                argumentValue,
                default(CancellationToken)
            })!;

        return task!;
    }

    protected virtual ICommandValidatorSource GetCommandValidatorSource(ActionExecutingContext context)
    {
        var commandValidatorSource =
            (ICommandValidatorSource?)context.HttpContext.RequestServices.GetService(
                typeof(ICommandValidatorSource));

        if (commandValidatorSource == null)
        {
            throw new NotImplementedException();
        }

        return commandValidatorSource;
    }
}