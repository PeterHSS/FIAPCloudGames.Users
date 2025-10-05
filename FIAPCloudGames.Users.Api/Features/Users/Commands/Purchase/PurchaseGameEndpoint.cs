using Carter;
using FIAPCloudGames.Users.Api.Commom;

namespace FIAPCloudGames.Users.Api.Features.Users.Commands.Purchase;

public sealed class PurchaseGameEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("users/purchases",
            async (UserPurchaseRequest request, PurchaseGamesUseCase useCase, CancellationToken cancellationToken) =>
            {
                await useCase.HandleAsync(request, cancellationToken);

                return Results.NoContent();
            })
            .WithTags(Tags.Users)
            .RequireAuthorization();
    }
}
