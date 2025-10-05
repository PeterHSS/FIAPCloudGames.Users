using Carter;
using FIAPCloudGames.Users.Api.Commom;

namespace FIAPCloudGames.Users.Api.Features.Users.Queries.GetAll;

public sealed class GetAllUsersEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("users",
            async (GetAllUsersUseCase useCase, CancellationToken cancellationToken) =>
            {
                IEnumerable<UserResponse> response = await useCase.HandleAsync(cancellationToken);

                return Results.Ok(response);
            })
            .WithTags(Tags.Users)
            .RequireAuthorization(AuthorizationPolicies.AdministratorOnly);
    }
}
