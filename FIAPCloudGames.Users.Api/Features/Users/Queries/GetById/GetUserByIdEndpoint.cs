using Carter;
using FIAPCloudGames.Users.Api.Commom;

namespace FIAPCloudGames.Users.Api.Features.Users.Queries.GetById;

public sealed class GetUserByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("users/{id:guid}",
            async (Guid id, GetUserByIdUseCase useCase, CancellationToken cancellationToken) =>
            {
                UserResponse response = await useCase.HandleAsync(id, cancellationToken);

                return Results.Ok(response);
            })
            .WithTags(Tags.Users)
            .RequireAuthorization(AuthorizationPolicies.AdministratorOnly);
    }
}
