using Serilog;
using Serilog.Sinks.OpenSearch;

namespace FIAPCloudGames.Api.Extensions;

public static class SerilogExtensions
{
    public static IHostBuilder AddSerilog(this IHostBuilder hostBuilder)
    {
        hostBuilder.UseSerilog((context, configuration) =>
        {
            Serilog.Debugging.SelfLog.Enable(msg => Console.WriteLine(msg));

            configuration.WriteTo.Console(restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information);

            configuration.MinimumLevel.Information();

            configuration
                .MinimumLevel.Override("Microsoft.AspNetCore", Serilog.Events.LogEventLevel.Error)
                .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Error)
                .MinimumLevel.Override("System", Serilog.Events.LogEventLevel.Error);

            configuration
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .Enrich.WithProcessName()
                .Enrich.WithThreadId()
                .Enrich.WithThreadName()
                .Enrich.WithEnvironmentName()
                .Enrich.WithProperty("Application", "FIAPCloudGames.Users.Api");

            configuration.WriteTo.OpenSearch(new OpenSearchSinkOptions(new Uri(context.Configuration["Opensearch:Host"]!))
            {
                AutoRegisterTemplate = true,
                IndexFormat = "fgc-users-api-{0:yyyy.MM.dd}",
                ModifyConnectionSettings = conn => 
                    conn
                        .ServerCertificateValidationCallback((o, certificate, chain, errors) => true)
                        .BasicAuthentication(context.Configuration["Opensearch:Username"], context.Configuration["Opensearch:Password"])
            });
        });

        return hostBuilder;
    }
}
