using FluentValidation;

namespace FIAPCloudGames.Users.Api.Features.Users.Commands.Create;

internal sealed class CreateUserValidator : AbstractUserValidator<CreateUserRequest>
{
    public CreateUserValidator()
    {
        AddNameRule(user => user.Name);

        AddNicknameRule(user => user.Nickname);

        RuleFor(user => user.Email)
            .NotEmpty().WithMessage("Email is required.")
            .MaximumLength(254).WithMessage("Email can have a maximum of 254 characters.")
            .EmailAddress().WithMessage("Email format is invalid.");

        RuleFor(user => user.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
            .Must(BeAValidPassword).WithMessage("Password must contain letters, numbers, and special characters.");

        RuleFor(user => user.Document)
            .NotEmpty().WithMessage("Document is required.")
            .MaximumLength(14).WithMessage("Document can have a maximum of 14 characters.")
            .Must(BeAValidDocument).WithMessage("Document is not valid.");

        RuleFor(user => user.BirthDate)
            .NotEmpty().WithMessage("BirthDate is required.")
            .Must(BeInThePast).WithMessage("BirthDate must be in the past.");
    }

    private bool BeAValidDocument(string document)
    {
        string digits = new(document.Where(char.IsDigit).ToArray());

        if (digits.Length == 11)
            return DocumentValidator.IsValidCpf(digits);

        if (digits.Length == 14)
            return DocumentValidator.IsValidCnpj(digits);

        return false;
    }

    private static bool BeInThePast(DateTime dateTime)
         => dateTime.Date < DateTime.UtcNow.Date;

    private static bool BeAValidPassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
            return false;

        return password.Any(char.IsLetter) &&
               password.Any(char.IsDigit) &&
               password.Any(c => !char.IsLetterOrDigit(c));
    }
}
