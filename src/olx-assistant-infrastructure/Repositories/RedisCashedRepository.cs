using olx_assistant_application.Interfaces.IRepositories;

namespace olx_assistant_infrastructure.Repositories;
public class RedisCashedRepository : IRedisCashedRepository
{
    private readonly isChased.isChasedClient _client;

    public RedisCashedRepository(isChased.isChasedClient client)
    {
        _client = client;
    }

    public async Task<bool> ProductIsProcessed(int id)
    {        
        var reply = await _client.FieldIsCachedAsync(
            new FieldIsCachedRequest() { Id = id });

        return reply.Status;
    }
}
