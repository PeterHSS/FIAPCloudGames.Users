namespace FIAPCloudGames.Users.Api.Features.Users.Models;

public class User 
{
    private User() { }
    public Guid Id { get; init; }
    public DateTime CreatedAt { get; init; }
    public string Name { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string Password { get; private set; } = string.Empty;
    public string Nickname { get; private set; } = string.Empty;
    public string Document { get; private set; } = string.Empty;    
    public DateTime BirthDate { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public Role Role { get; private set; }
    public ICollection<UserGame> Games { get; private set; }

    public static User Create(string name, string email, string password, string nickname, string document, DateTime birthDate)
    {
        return new User
        {
            Id = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow,
            Name = name,
            Email = email.ToLower(),
            Password = password,
            Nickname = nickname,
            Document = document,
            BirthDate = birthDate,
            Role = Role.User,
        };
    }

    public bool HasPurchasedGame(Guid gameId) 
        => Games.Any(game => game.GameId == gameId);

    public void PurchaseGame(Guid gameId)
    {
        if (HasPurchasedGame(gameId))
            return;

        Games.Add(new UserGame(Guid.NewGuid(), gameId, Id, DateTime.UtcNow));
    }

    public void UpdateInformation(string name, string nickname)
    {
        Name = name;
        Nickname = nickname;
        UpdatedAt = DateTime.UtcNow;
    }
}
