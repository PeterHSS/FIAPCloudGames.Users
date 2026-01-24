using Carter;
using EasyNetQ;
using FIAPCloudGames.Infrastructure;
using FIAPCloudGames.Users.Api.Commom.ExtensionMethods;
using FIAPCloudGames.Users.Api.Commom.Middlewares;
using FIAPCloudGames.Users.Api.Consumers;
using Npgsql;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDependencyInjection(builder.Configuration);

builder.Services.AddEasyNetQ(builder.Configuration.GetConnectionString("RabbitMQ")!).UseLegacyTypeNaming();

builder.Logging.AddOpenTelemetry(logging =>
{
    logging.IncludeScopes = true;
    logging.IncludeFormattedMessage = true;
});

builder.Services
    .AddOpenTelemetry()
    .WithMetrics(metrics =>
        metrics
            .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("FIAPCloudGames.Users.Api"))
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddRuntimeInstrumentation()
            .AddProcessInstrumentation()
            .AddNpgsqlInstrumentation()
            .AddPrometheusExporter())
    .WithTracing(tracing =>
        tracing
            .AddHttpClientInstrumentation()
            .AddAspNetCoreInstrumentation()
            .AddEntityFrameworkCoreInstrumentation()
            .AddNpgsql());

builder.Services
    .AddFluentEmail(builder.Configuration["Email:SenderEmail"], builder.Configuration["Email:Sender"])
    .AddSmtpSender(() =>
    {
        return new System.Net.Mail.SmtpClient
        {
            Host = builder.Configuration["Email:Host"]!,
            Port = builder.Configuration.GetValue<int>("Email:Port"),
            EnableSsl = true,
            Credentials = new System.Net.NetworkCredential
            {
                UserName = builder.Configuration["Email:Username"],
                Password = builder.Configuration["Email:Password"]
            }
        };
    });

builder.Services.AddHostedService<UserCreatedConsumer>();

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

app.UseOpenTelemetryPrometheusScrapingEndpoint("/users/metrics");

app.UseMiddleware<RequestLogContextMiddleware>();

app.UseExceptionHandler();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapCarter();

app.MapControllers();

app.Run();