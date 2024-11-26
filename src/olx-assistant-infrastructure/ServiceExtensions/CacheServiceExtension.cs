using olx_assistant_application.Interfaces.IRepositories;
using olx_assistant_infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System.Net.Security;
using Grpc.Net.Client;

namespace olx_assistant_infrastructure.ServiceExtensions;
public static class CacheServiceExtension
{
    public static void CacheServiceConfig(
    this IServiceCollection services,
    IConfiguration configuration
    )
    {
        var connectionString = configuration.GetConnectionString("CacheService");

        var grpcChannel = GrpcChannel.ForAddress(connectionString, new GrpcChannelOptions
        {
            //LoggerFactory = LoggerFactory.Create(builder =>
            //{
            //    builder.AddConsole();
            //    builder.SetMinimumLevel(LogLevel.Debug);
            //}),
            HttpHandler = new SocketsHttpHandler
            {
                PooledConnectionLifetime = TimeSpan.FromMinutes(1),
                KeepAlivePingTimeout = TimeSpan.FromSeconds(30),
                KeepAlivePingPolicy = HttpKeepAlivePingPolicy.WithActiveRequests,
                EnableMultipleHttp2Connections = true,
                PooledConnectionIdleTimeout = TimeSpan.FromMinutes(5),
                SslOptions = new SslClientAuthenticationOptions
                {
                    RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true
                }
            }
        });

        services.AddSingleton(grpcChannel);
        services.AddSingleton(new isChased.isChasedClient(grpcChannel));
        services.AddTransient<IRedisCashedRepository, RedisCashedRepository>();
    }
}
