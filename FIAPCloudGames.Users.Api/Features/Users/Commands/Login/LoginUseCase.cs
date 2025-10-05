using FIAPCloudGames.Users.Api.Commom.Interfaces;
using FIAPCloudGames.Users.Api.Features.Users.Models;
using FIAPCloudGames.Users.Api.Features.Users.Repositories;
using Serilog;

namespace FIAPCloudGames.Users.Api.Features.Users.Commands.Login;

public sealed class LoginUseCase(IUserRepository userRepository, IPasswordHasherProvider passwordHasher, IJwtProvider jwtProvider)
{
    public async Task<LoginResponse> HandleAsync(LoginRequest request, CancellationToken cancellation = default)
    {
        Log.Information("Attempting to log in user with email: {Email}", request.Email);

        User? user = await userRepository.GetByEmailAsync(request.Email, cancellation);

        if (user is null)
        {
            Log.Warning("Login failed for email: {Email} - User not found", request.Email);
         
            throw new KeyNotFoundException("Invalid email or password.");
        }

        bool verified = passwordHasher.Verify(request.Password, user.Password);

        if (!verified)
        {
            Log.Warning("Login failed for email: {Email} - Invalid password", request.Email);
         
            throw new KeyNotFoundException("Invalid email or password.");
        }

        string token = jwtProvider.Create(user);
        
        Log.Information("User {Email} logged in successfully", request.Email);

        return new LoginResponse(token);
    }
}
