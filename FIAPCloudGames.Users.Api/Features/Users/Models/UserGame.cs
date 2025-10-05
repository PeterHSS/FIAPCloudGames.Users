namespace FIAPCloudGames.Users.Api.Features.Users.Models;

public sealed class UserGame
{
    public UserGame(Guid id, Guid gameId, Guid userId, DateTime purchasedAt)
    {
        Id = id;
        GameId = gameId;
        UserId = userId;
        PurchasedAt = purchasedAt;
    }

    private UserGame() { }


    public Guid Id { get; init; }
    public Guid GameId { get; init; }
    public Guid UserId { get; init; }
    public DateTime PurchasedAt { get; init; }
    public User User { get; init; }
}