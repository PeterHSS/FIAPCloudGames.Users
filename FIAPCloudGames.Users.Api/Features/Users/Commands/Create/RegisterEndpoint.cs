using Carter;
using FIAPCloudGames.Users.Api.Commom;
using FluentValidation;

namespace FIAPCloudGames.Users.Api.Features.Users.Commands.Create;

public sealed class RegisterEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("users/register",
            async (CreateUserRequest request, CreateUserUseCase useCase, IValidator<CreateUserRequest> validator, CancellationToken cancellationToken) =>
            {
                validator.ValidateAndThrow(request);

                await useCase.HandleAsync(request, cancellationToken);

                return Results.Created();
            })
            .WithTags(Tags.Users);
    }
}
