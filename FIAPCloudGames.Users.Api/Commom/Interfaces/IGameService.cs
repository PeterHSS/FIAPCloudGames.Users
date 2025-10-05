using FIAPCloudGames.Users.Api.Features.Users.Queries;

namespace FIAPCloudGames.Users.Api.Commom.Interfaces;

public interface IGameService
{
    Task<IEnumerable<GameResponse>> GetGameByIds(IEnumerable<Guid> gamesIds);
}
