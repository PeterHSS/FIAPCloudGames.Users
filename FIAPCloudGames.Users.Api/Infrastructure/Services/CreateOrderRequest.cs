namespace FIAPCloudGames.Orders.Api.Features.Commands.Create;

public record CreateOrderRequest(Guid UserId, IEnumerable<Guid> GamesId);
