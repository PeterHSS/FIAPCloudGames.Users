namespace FIAPCloudGames.Users.Api.Features.Users.Commands.Update;

internal sealed class UpdateUserValidator : AbstractUserValidator<UpdateUserRequest>
{
    public UpdateUserValidator()
    {
        AddNameRule(user => user.Name);

        AddNicknameRule(user => user.Nickname);
    }
}
