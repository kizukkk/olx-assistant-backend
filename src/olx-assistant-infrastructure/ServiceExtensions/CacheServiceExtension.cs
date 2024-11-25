using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using olx_assistant_application.Interfaces.IRepositories;
using olx_assistant_infrastructure.DbContexts;
using olx_assistant_infrastructure.Repositories;

namespace olx_assistant_infrastructure.ServiceExtensions;
public static class CacheServiceExtension
{
    public static void CacheServiceConfig(
    this IServiceCollection services,
    IConfiguration configuration
    )
    {
        var connectionString = configuration.GetConnectionString("CacheService");

        services.AddSingleton<IRedisCashedRepository>(
            new RedisCashedRepository(connectionString));
    }
}
