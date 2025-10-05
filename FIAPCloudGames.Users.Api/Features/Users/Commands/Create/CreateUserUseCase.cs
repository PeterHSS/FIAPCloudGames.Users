using FIAPCloudGames.Users.Api.Commom;
using FIAPCloudGames.Users.Api.Commom.Interfaces;
using FIAPCloudGames.Users.Api.Features.Users.Models;
using FIAPCloudGames.Users.Api.Features.Users.Repositories;
using Serilog;

namespace FIAPCloudGames.Users.Api.Features.Users.Commands.Create;

public sealed class CreateUserUseCase(IUserRepository userRepository, IPasswordHasherProvider passwordHasher, IUnitOfWork unitOfWork)
{
    public async Task HandleAsync(CreateUserRequest request, CancellationToken cancellationToken = default)
    {
        Log.Information("Creating user with email: {Email}", request.Email);

        if (!await userRepository.IsUniqueEmail(request.Email.ToLower(), cancellationToken))
        {
            Log.Warning("Attempt to create user with existing email: {Email}", request.Email);

            throw new ArgumentException("Email already exists.");
        }

        if (!await userRepository.IsUniqueDocument(request.Document.OnlyNumbers(), cancellationToken))
        {
            Log.Warning("Attempt to create user with existing document: {Document}", request.Document);

            throw new ArgumentException("Document already exists.");
        }

        string hashedPassword = passwordHasher.Hash(request.Password);

        User user = User.Create(request.Name, request.Email, hashedPassword, request.Nickname, request.Document.OnlyNumbers(), request.BirthDate);

        await userRepository.AddAsync(user, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        Log.Information("User created successfully with ID: {UserId}", user.Id);
    }
}
