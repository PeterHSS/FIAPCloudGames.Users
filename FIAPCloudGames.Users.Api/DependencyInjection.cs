using System.Reflection;
using Asp.Versioning;
using Carter;
using FIAPCloudGames.Promotions.Api.Infrastructure.Services;
using FIAPCloudGames.Users.Api.Commom.Extensions;
using FIAPCloudGames.Users.Api.Commom.Interfaces;
using FIAPCloudGames.Users.Api.Commom.Middlewares;
using FIAPCloudGames.Users.Api.Features.Users.Commands.Create;
using FIAPCloudGames.Users.Api.Features.Users.Commands.Login;
using FIAPCloudGames.Users.Api.Features.Users.Commands.Purchase;
using FIAPCloudGames.Users.Api.Features.Users.Commands.Update;
using FIAPCloudGames.Users.Api.Features.Users.Models;
using FIAPCloudGames.Users.Api.Features.Users.Queries.GetAll;
using FIAPCloudGames.Users.Api.Features.Users.Queries.GetById;
using FIAPCloudGames.Users.Api.Features.Users.Repositories;
using FIAPCloudGames.Users.Api.Infrastructure.Persistence.Context;
using FIAPCloudGames.Users.Api.Infrastructure.Persistence.Repositories;
using FIAPCloudGames.Users.Api.Infrastructure.Providers;
using FIAPCloudGames.Users.Api.Infrastructure.Services;
using FIAPCloudGames.Users.Api.Infrastructure.Settings;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace FIAPCloudGames.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddDependencyInjection(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddPresentation(configuration)
            .AddInfrastructure(configuration)
            .AddUseCases()
            .AddValidators()
            .AddHttpClients(configuration);

        return services;
    }

    private static IServiceCollection AddValidators(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly(), ServiceLifetime.Scoped, includeInternalTypes: true);

        return services;
    }

    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddSettings(configuration)
            .AddRepositories(configuration)
            .AddProviders()
            .AddServices();

        return services;
    }

    public static IServiceCollection AddPresentation(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddEndpointsApiExplorer();

        services.AddSwaggerGenWithAuth();

        services.AddCarter();

        services.AddJwtAuthenticationAndAuthorization(configuration);

        services.AddProblemDetails(configure =>
        {
            configure.CustomizeProblemDetails = (context) =>
            {
                context.ProblemDetails.Extensions.TryAdd("requestId", context.HttpContext.TraceIdentifier);
            };
        });

        services.AddExceptionHandler<ValidationExceptionHandlerMiddleware>();

        services.AddExceptionHandler<GlobalExceptionHandlerMiddleware>();

        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<UserDbContext>(options => options.UseNpgsql(configuration.GetConnectionString(nameof(User))));

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

    public static IServiceCollection AddUseCases(this IServiceCollection services)
    {
        services.AddScoped<CreateUserUseCase>();

        services.AddScoped<LoginUseCase>();

        services.AddScoped<PurchaseGamesUseCase>();

        services.AddScoped<UpdateUserUseCase>();

        services.AddScoped<GetAllUsersUseCase>();

        services.AddScoped<GetUserByIdUseCase>();

        return services;
    }

    public static IServiceCollection AddHttpClients(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient<IGameService, GameService>(client =>
        {
            string gameBaseUrl = configuration.GetValue<string>("GameApi:BaseAddress")!;

            client.BaseAddress = new Uri(gameBaseUrl);
        });

        services.AddHttpClient<IOrderService, OrderService>(client =>
        {
            string gameBaseUrl = configuration.GetValue<string>("OrderApi:BaseAddress")!;

            client.BaseAddress = new Uri(gameBaseUrl);
        });

        return services;
    }
}
