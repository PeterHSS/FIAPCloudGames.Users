using FIAPCloudGames.Users.Api.Features.Users.Models;
using Microsoft.EntityFrameworkCore;

namespace FIAPCloudGames.Users.Api.Infrastructure.Persistence.Context;

public sealed class UserDbContext : DbContext
{
    public DbSet<User> Users { get; set; }

    public UserDbContext(DbContextOptions options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserDbContext).Assembly);
    }
}
