using FIAPCloudGames.Users.Api.Commom.Interfaces;
using FIAPCloudGames.Users.Api.Features.Users.Models;
using FIAPCloudGames.Users.Api.Features.Users.Repositories;
using Serilog;

namespace FIAPCloudGames.Users.Api.Features.Users.Commands.Update;

public sealed class UpdateUserUseCase(IUserRepository userRepository, ICurrentUserService currentUserService, IUnitOfWork unitOfWork)
{
    public async Task HandleAsync(Guid id, UpdateUserRequest request, CancellationToken cancellationToken = default)
    {
        Log.Information("Start updating user. {@UserId} {@Request}", id, request);

        if (id != currentUserService.UserId)
        {
            Log.Warning("Unauthorized update attempt. {@CurrentUserId} tried to update {@TargetUserId}", currentUserService.UserId, id);

            throw new UnauthorizedAccessException("You are not allowed to update this user.");
        }

        User? user = await userRepository.GetByIdAsync(id, cancellationToken);

        if (user is null)
        {
            Log.Warning("User not found for update. {@UserId}", id);

            throw new KeyNotFoundException($"User with ID {id} not found.");
        }

        user.UpdateInformation(request.Name, request.Nickname);

        userRepository.Update(user);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        Log.Information("User updated successfully. {@UserId} {@UpdatedFields}", id, new { request.Name, request.Nickname });
    }
}
