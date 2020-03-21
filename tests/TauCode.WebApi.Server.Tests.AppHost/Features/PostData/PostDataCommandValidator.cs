using FluentValidation;
using System;

namespace TauCode.WebApi.Server.Tests.AppHost.Features.PostData
{
    public class PostDataCommandValidator : AbstractValidator<PostDataCommand>
    {
        private static readonly DateTime MinDate = new DateTime(1970, 1, 1);

        public PostDataCommandValidator()
        {
            this.RuleFor(x => x.UserName)
                .NotEmpty();

            this.RuleFor(x => x.Birthday)
                .GreaterThan(MinDate)
                .WithMessage("Too old :(.");
        }
    }
}
