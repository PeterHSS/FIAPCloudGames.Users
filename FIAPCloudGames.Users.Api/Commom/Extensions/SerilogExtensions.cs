using Microsoft.ApplicationInsights.Extensibility;
using Serilog;
using Serilog.Sinks.Elasticsearch;

namespace FIAPCloudGames.Api.Extensions;

public static class SerilogExtensions
{
    public static IHostBuilder AddSerilog(this IHostBuilder hostBuilder)
    {
        hostBuilder.UseSerilog((context, configuration) =>
        {
            configuration.ReadFrom.Configuration(context.Configuration);

            configuration.WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(context.Configuration["Elasticsearch:Uri"]!))
            {
                AutoRegisterTemplate = true,
                IndexFormat = "fgc-users-api-{0:yyyy.MM.dd}"
            });
        });
        
        return hostBuilder;
    }
}
