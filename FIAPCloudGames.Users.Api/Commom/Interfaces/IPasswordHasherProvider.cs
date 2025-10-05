namespace FIAPCloudGames.Users.Api.Commom.Interfaces;

public interface IPasswordHasherProvider
{
    string Hash(string password);
    bool Verify(string password, string hashedPassword);
}
