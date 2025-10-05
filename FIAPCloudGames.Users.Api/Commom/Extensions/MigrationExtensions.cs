using FIAPCloudGames.Users.Api.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace FIAPCloudGames.Users.Api.Commom.ExtensionMethods;

public static class MigrationExtensions
{
    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();

        UserDbContext dbContext = scope.ServiceProvider.GetRequiredService<UserDbContext>();

        dbContext.Database.Migrate();
    }
}
