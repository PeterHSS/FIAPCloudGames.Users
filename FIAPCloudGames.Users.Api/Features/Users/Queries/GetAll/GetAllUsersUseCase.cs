using FIAPCloudGames.Users.Api.Features.Users.Models;
using FIAPCloudGames.Users.Api.Features.Users.Repositories;
using Serilog;

namespace FIAPCloudGames.Users.Api.Features.Users.Queries.GetAll;

public sealed class GetAllUsersUseCase(IUserRepository userRepository)
{
    public async Task<IEnumerable<UserResponse>> HandleAsync(CancellationToken cancellationToken = default)
    {
        Log.Information("Retrieving all users...");

        IEnumerable<User> users = await userRepository.GetAllAsync(cancellationToken);

        Log.Information("Retrieved {Count} users.", users.Count());

        return users.Select(UserResponse.Create);
    }
}
