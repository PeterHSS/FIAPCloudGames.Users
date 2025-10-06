using FIAPCloudGames.Users.Api.Commom.Interfaces;
using FIAPCloudGames.Users.Api.Features.Users.Models;
using FIAPCloudGames.Users.Api.Features.Users.Queries;
using FIAPCloudGames.Users.Api.Features.Users.Repositories;
using Serilog;

namespace FIAPCloudGames.Users.Api.Features.Users.Commands.Purchase;

public sealed class PurchaseGamesUseCase(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork,
    ICurrentUserService currentUserService,
    IGameService gameService,
    IOrderService orderService)
{
    public async Task HandleAsync(UserPurchaseRequest request, CancellationToken cancellationToken = default)
    {
        Log.Information("Processing purchase for user with ID {UserId} for games with IDs {@GameId}.", currentUserService.UserId, request);

        if (!request.gamesIds.Any())
        {
            Log.Warning("No game IDs provided for purchase by user with ID {UserId}.", currentUserService.UserId);

            throw new ArgumentException("At least one game ID must be provided for purchase.");
        }


        User? user = await userRepository.GetByIdAsync(currentUserService.UserId, cancellationToken);

        if (user is null)
        {
            Log.Warning("User with ID {UserId} not found.", currentUserService.UserId);

            throw new KeyNotFoundException($"User with ID {currentUserService} not found.");
        }

        IEnumerable<GameResponse> gamesToPurchase = await gameService.GetGameByIds(request.gamesIds);

        if (!gamesToPurchase.Any())
        {
            Log.Warning("No games found for purchase with IDs {@GameIds}.", request.gamesIds);

            throw new KeyNotFoundException($"No games found for the provided IDs: {string.Join(", ", request.gamesIds)}.");
        }

        IEnumerable<Guid> missingGameIds = request.gamesIds.Except(gamesToPurchase.Select(game => game.Id));

        if (missingGameIds.Any())
        {
            Log.Warning("Some game IDs were not found: {@MissingGameIds}.", missingGameIds);

            throw new KeyNotFoundException($"The following game IDs were not found: {string.Join(", ", missingGameIds)}.");
        }

        bool orderCreated = await orderService.CreateOrderAsync(user.Id, gamesToPurchase.Select(game => game.Id));

        if (!orderCreated)
        {
            Log.Error("Failed to create order for user with ID {UserId}.", user.Id);
         
            throw new InvalidOperationException("Failed to create order. Please try again later.");
        }

        foreach (var game in gamesToPurchase)
        {
            if (user.HasPurchasedGame(game.Id))
            {
                Log.Warning("User with ID {UserId} has already purchased game with ID {GameId}.", user.Id, game.Id);

                continue;
            }

            user.PurchaseGame(game.Id);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
