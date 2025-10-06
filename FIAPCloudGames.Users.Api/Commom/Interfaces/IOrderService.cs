namespace FIAPCloudGames.Users.Api.Commom.Interfaces;

public interface IOrderService
{
    Task<bool> CreateOrderAsync(Guid userId, IEnumerable<Guid> gameIds);
}
