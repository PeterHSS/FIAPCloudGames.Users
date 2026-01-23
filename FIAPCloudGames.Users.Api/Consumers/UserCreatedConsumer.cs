using EasyNetQ;
using FIAPCloudGames.Users.Api.Features.Users.Commands.Create;
using FluentEmail.Core;

namespace FIAPCloudGames.Users.Api.Consumers;

public class UserCreatedConsumer(IServiceScopeFactory serviceScopeFactory, IBus bus) : BackgroundService
{
    public async Task Handle(UserCreatedEvent @event)
    {
        try
        {
            using var scope = serviceScopeFactory.CreateScope();

            var fluentEmail = scope.ServiceProvider.GetRequiredService<IFluentEmail>();

            var sendResponse = await fluentEmail
                .To(@event.Email)
                .Subject("Welcome to FIAP Cloud Games!")
                .Body($"Hello {@event.Name}, welcome to FIAP Cloud Games! Your user ID is {@event.Id}.")
                .SendAsync();
        }
        catch (Exception)
        {
            throw;
        }
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await bus.PubSub.SubscribeAsync<UserCreatedEvent>(subscriptionId: "user-created-queue",
            onMessage: async @event => await Handle(@event));
    }
}
