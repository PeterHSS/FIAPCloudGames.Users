using FIAPCloudGames.Users.Api.Commom.Interfaces;
using FIAPCloudGames.Users.Api.Features.Users.Models;
using FIAPCloudGames.Users.Api.Features.Users.Repositories;
using FIAPCloudGames.Users.Api.Infrastructure.Persistence.Context;
using FIAPCloudGames.Users.Api.Infrastructure.Persistence.Repositories;
using FIAPCloudGames.Users.Api.Infrastructure.Providers;
using FIAPCloudGames.Users.Api.Infrastructure.Services;
using FIAPCloudGames.Users.Api.Infrastructure.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace FIAPCloudGames.Users.Api.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddSettings(configuration)
            .AddRepositories(configuration)
            .AddProviders()
            .AddServices();

        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<UserDbContext>(options => options.UseNpgsql(configuration.GetConnectionString(nameof(User).ToString())));

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }

    private static IServiceCollection AddSettings(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));

        services.AddSingleton(serviceProvider => serviceProvider.GetRequiredService<IOptions<JwtSettings>>().Value);

        return services;
    }

    private static IServiceCollection AddProviders(this IServiceCollection services)
    {
        services.AddScoped<IPasswordHasherProvider, PasswordHasherProvider>();

        services.AddScoped<IJwtProvider, JwtProvider>();

        return services;
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();

        services.AddScoped<ICurrentUserService, CurrentUserService>();

        return services;
    }
}
