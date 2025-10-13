using Azure.Monitor.OpenTelemetry.AspNetCore;
using Carter;
using FIAPCloudGames.Infrastructure;
using FIAPCloudGames.Users.Api.Commom.ExtensionMethods;
using FIAPCloudGames.Users.Api.Commom.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDependencyInjection(builder.Configuration);

builder.Services
    .AddOpenTelemetry()
    .UseAzureMonitor(configureAzureMonitor => configureAzureMonitor.ConnectionString = builder.Configuration["ApplicationInsights:ConnectionString"]);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI();

    app.ApplyMigrations();
}

app.UseMiddleware<RequestLogContextMiddleware>();

app.UseExceptionHandler();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapCarter();

app.MapControllers();

app.Run();