using Carter;
using FIAPCloudGames.Users.Api.Commom;

namespace FIAPCloudGames.Users.Api.Features.Users.Commands.Login;

public sealed class LoginEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("users/login",
            async (LoginRequest request, LoginUseCase useCase, CancellationToken cancellation) =>
            {
                LoginResponse response = await useCase.HandleAsync(request, cancellation);

                return Results.Ok(response);
            })
            .WithTags(Tags.Users);
    }
}
