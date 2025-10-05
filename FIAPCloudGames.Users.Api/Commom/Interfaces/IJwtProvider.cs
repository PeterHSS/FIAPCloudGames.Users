using FIAPCloudGames.Users.Api.Features.Users.Models;

namespace FIAPCloudGames.Users.Api.Commom.Interfaces;

public interface IJwtProvider
{
    string Create(User user);
}