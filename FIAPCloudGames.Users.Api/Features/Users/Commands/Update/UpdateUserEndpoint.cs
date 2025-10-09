using Carter;
using FIAPCloudGames.Users.Api.Commom;
using FluentValidation;

namespace FIAPCloudGames.Users.Api.Features.Users.Commands.Update;

public sealed class UpdateUserEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("users/{id:guid}",
            async (Guid id, UpdateUserRequest request, UpdateUserUseCase useCase, IValidator<UpdateUserRequest> validator, CancellationToken cancellationToken) =>
            {
                validator.ValidateAndThrow(request);

                await useCase.HandleAsync(id, request, cancellationToken);

                return Results.NoContent();
            })
            .WithTags(Tags.Users);
    }
}
