using olx_assistant_application.Interfaces.IRepositories;
using System.Threading.Tasks;
using Grpc.Net.Client;

namespace olx_assistant_infrastructure.Repositories;
public class RedisCashedRepository : IRedisCashedRepository
{
    private readonly string _gRpcAddress;

    public RedisCashedRepository(string gRpcAddress)
    {
        _gRpcAddress = gRpcAddress;
    }

    public async Task<bool> ProductIsProcessed(int id)
    {
        using var channel = GrpcChannel.ForAddress(_gRpcAddress);
        var client = new isChased.isChasedClient(channel);
        
        var reply = await client.FieldIsCachedAsync(
            new FieldIsCachedRequest() { Id = 12344 });

        return reply.Status;
    }
}
