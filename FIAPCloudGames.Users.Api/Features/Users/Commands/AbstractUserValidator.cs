using System.Linq.Expressions;
using FluentValidation;

namespace FIAPCloudGames.Users.Api.Features.Users.Commands;

internal abstract class AbstractUserValidator<T> : AbstractValidator<T>
{
    protected void AddNameRule(Expression<Func<T, string>> selector)
    {
        RuleFor(selector)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(200).WithMessage("Name can have a maximum of 200 characters.");
    }

    protected void AddNicknameRule(Expression<Func<T, string>> selector)
    {
        RuleFor(selector)
            .NotEmpty().WithMessage("Nickname is required.")
            .MaximumLength(100).WithMessage("Nickname can have a maximum of 100 characters.");
    }
}
