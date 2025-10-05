using FIAPCloudGames.Users.Api.Commom.Interfaces;
using FIAPCloudGames.Users.Api.Features.Users.Queries;

namespace FIAPCloudGames.Promotions.Api.Infrastructure.Services;

public sealed class GameService(HttpClient httpClient) : IGameService
{
    public async Task<IEnumerable<GameResponse>> GetGameByIds(IEnumerable<Guid> gamesIds)
    {
        var payload = new GetGameByIdsRequest(gamesIds);

        HttpResponseMessage response = await httpClient.PostAsJsonAsync("games/by-ids", payload);

        response.EnsureSuccessStatusCode();

        IEnumerable<GameResponse>? games = await response.Content.ReadFromJsonAsync<IEnumerable<GameResponse>>();

        return games ?? Enumerable.Empty<GameResponse>();
    }
}
public sealed record GetGameByIdsRequest(IEnumerable<Guid> GamesIds);
