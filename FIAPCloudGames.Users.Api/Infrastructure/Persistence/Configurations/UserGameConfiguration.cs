using FIAPCloudGames.Users.Api.Features.Users.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FIAPCloudGames.Users.Api.Infrastructure.Persistence.Configurations;

public class UserGameConfiguration : IEntityTypeConfiguration<UserGame>
{
    public void Configure(EntityTypeBuilder<UserGame> builder)
    {
        builder.ToTable("UserGame");

        builder.HasKey(userGame => userGame.Id).HasName("PK_UserGame");

        builder.Property(userGame => userGame.Id).IsRequired();

        builder.Property(userGame => userGame.GameId).IsRequired();

        builder.Property(userGame => userGame.UserId).IsRequired();

        builder.Property(userGame => userGame.PurchasedAt).IsRequired();

        builder.HasOne(userGame => userGame.User)
            .WithMany(user => user.Games)
            .HasForeignKey(userGame => userGame.UserId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_UserGame_Users_UserId");
    }
}
