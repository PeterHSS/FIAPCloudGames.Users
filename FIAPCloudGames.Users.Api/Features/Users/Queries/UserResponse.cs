using FIAPCloudGames.Users.Api.Features.Users.Models;

namespace FIAPCloudGames.Users.Api.Features.Users.Queries;

public record UserResponse(
    Guid Id,
    string Name,
    string Email,
    string Nickname,
    string Document,
    DateTime BirthDate,
    IEnumerable<GameResponse> Games)
{
    public static UserResponse Create(User user, IEnumerable<GameResponse> games)
    {
        return new UserResponse(
            user.Id,
            user.Name,
            user.Email,
            user.Nickname,
            user.Document,
            user.BirthDate,
            games);
    }

    public static UserResponse Create(User user)
    {
        return new UserResponse(
            user.Id,
            user.Name,
            user.Email,
            user.Nickname,
            user.Document,
            user.BirthDate,
            []);
    }
}