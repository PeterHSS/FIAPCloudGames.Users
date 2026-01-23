namespace FIAPCloudGames.Users.Api.Features.Users.Commands.Create;

public record UserCreatedEvent(Guid Id, string Name, string Email);
