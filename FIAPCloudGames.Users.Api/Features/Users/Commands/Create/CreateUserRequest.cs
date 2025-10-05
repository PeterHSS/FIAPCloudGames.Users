namespace FIAPCloudGames.Users.Api.Features.Users.Commands.Create;

public record CreateUserRequest(string Name, string Email, string Password, string Nickname, string Document, DateTime BirthDate);
