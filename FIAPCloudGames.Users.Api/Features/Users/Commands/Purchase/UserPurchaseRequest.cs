namespace FIAPCloudGames.Users.Api.Features.Users.Commands.Purchase;

public record UserPurchaseRequest(IEnumerable<Guid> gamesIds);

