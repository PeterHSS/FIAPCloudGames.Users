using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;

namespace FIAPCloudGames.Users.Api.Commom.Extensions;

public static class SwaggerGenExtensions
{
    public static IServiceCollection AddSwaggerGenWithAuth(this IServiceCollection services)
    {
        services.AddSwaggerGen(setupAction =>
        {
            setupAction.CustomSchemaIds(type => type.FullName);

            OpenApiSecurityScheme openApiSecurityScheme = new OpenApiSecurityScheme
            {
                Name = "JWT Authentication",
                Description = "Enter your JWT token in this field",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = JwtBearerDefaults.AuthenticationScheme,
                BearerFormat = "JWT"
            };

            setupAction.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, openApiSecurityScheme);

            OpenApiSecurityRequirement openApiSecurityRequirement = new OpenApiSecurityRequirement
            {
                {
                     new OpenApiSecurityScheme
                     {
                         Reference = new OpenApiReference
                         {
                             Type = ReferenceType.SecurityScheme,
                             Id = JwtBearerDefaults.AuthenticationScheme
                         }
                     },
                     []
                }
            };

            setupAction.AddSecurityRequirement(openApiSecurityRequirement);
        });

        return services;
    }
}

