using Microsoft.ApplicationInsights.Extensibility;
using Serilog;

namespace FIAPCloudGames.Api.Extensions;

public static class SerilogExtensions
{
    public static IHostBuilder AddSerilog(this IHostBuilder hostBuilder)
    {
        hostBuilder.UseSerilog((context, configuration) =>
        {
            configuration.ReadFrom.Configuration(context.Configuration);

            TelemetryConfiguration telemetryConfiguration = TelemetryConfiguration.CreateDefault();

            telemetryConfiguration.ConnectionString = context.Configuration["ApplicationInsights:ConnectionString"];

            configuration.WriteTo.ApplicationInsights(telemetryConfiguration, TelemetryConverter.Traces);
        });
        
        return hostBuilder;
    }
}
