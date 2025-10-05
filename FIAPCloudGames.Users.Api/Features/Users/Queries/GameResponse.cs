namespace FIAPCloudGames.Users.Api.Features.Users.Queries;

public record GameResponse(Guid Id, string Name, string Description, DateTime ReleasedAt, string Genre);
