using Serilog;
using Serilog.Sinks.OpenSearch;

namespace FIAPCloudGames.Api.Extensions;

public static class SerilogExtensions
{
    public static IHostBuilder AddSerilog(this IHostBuilder hostBuilder)
    {
        hostBuilder.UseSerilog((context, configuration) =>
        {
            configuration.WriteTo.OpenSearch(new OpenSearchSinkOptions(new Uri("https://localhost:9200"))
            {
                AutoRegisterTemplate = true,
                IndexFormat = "fgc-users-api-{0:yyyy.MM.dd}",
                ModifyConnectionSettings = conn => conn.ServerCertificateValidationCallback((o, certificate, chain, errors) => true).BasicAuthentication(context.Configuration["Opensearch:Username"], context.Configuration["Opensearch:Password"])
            });
        });

        return hostBuilder;
    }
}
