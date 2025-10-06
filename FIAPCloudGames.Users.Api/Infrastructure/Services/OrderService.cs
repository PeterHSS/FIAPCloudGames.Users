using FIAPCloudGames.Orders.Api.Features.Commands.Create;
using FIAPCloudGames.Users.Api.Commom.Interfaces;
using Serilog;

namespace FIAPCloudGames.Users.Api.Infrastructure.Services;

public class OrderService(HttpClient client) : IOrderService
{
    public async Task<bool> CreateOrderAsync(Guid userId, IEnumerable<Guid> gameIds)
    {
		try
		{
            var payload = new CreateOrderRequest(userId, gameIds);

            var response = await client.PostAsJsonAsync("orders", payload);

            response.EnsureSuccessStatusCode();

            return true;
        }
		catch (Exception)
		{
            Log.Error("Error creating order for user {UserId} with games {@GameIds}", userId, gameIds);
            
            return false;
		}
    }
}


