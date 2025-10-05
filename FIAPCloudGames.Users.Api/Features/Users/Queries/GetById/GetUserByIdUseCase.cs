using FIAPCloudGames.Users.Api.Commom.Interfaces;
using FIAPCloudGames.Users.Api.Features.Users.Models;
using FIAPCloudGames.Users.Api.Features.Users.Repositories;
using Serilog;

namespace FIAPCloudGames.Users.Api.Features.Users.Queries.GetById;

public sealed class GetUserByIdUseCase(IUserRepository userRepository, IGameService gameService)
{
    public async Task<UserResponse> HandleAsync(Guid id, CancellationToken cancellationToken = default)
    {
        Log.Information("Retrieving user with ID {UserId}", id);

        User? user = await userRepository.GetByIdAsync(id, cancellationToken);

        if (user is null)
        {
            Log.Warning("User with ID {UserId} not found", id);

            throw new KeyNotFoundException($"User with ID {id} not found.");
        }

        IEnumerable<GameResponse> games = await gameService.GetGameByIds(user.Games.Select(game => game.GameId).ToList());

        Log.Information("User with ID {UserId} retrieved successfully.", id);

        return UserResponse.Create(user, games);
    }
}
