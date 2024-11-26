using StackExchange.Redis;
using Grpc.Core;

namespace olx_assistant_cache_service.Services;

public class IsCachedService : isChased.isChasedBase
{
    private readonly IConnectionMultiplexer _connectionMultiplexer;
    private readonly IDatabase _database;

    private readonly ILogger<IsCachedService> _logger;
    public IsCachedService(
        ILogger<IsCachedService> logger,
        IConnectionMultiplexer connectionMultiplexer)
    {
        _logger = logger;
        _connectionMultiplexer = connectionMultiplexer;
        _database = _connectionMultiplexer.GetDatabase();
    }

    public override Task<FieldIsCachedReply> FieldIsCached(FieldIsCachedRequest request, ServerCallContext context)
    {
        var result = _database.SetContains("product_id", request.Id);

        _logger.LogInformation($"Received request for ID: {request.Id}\n\tCache: {result}");
        
        return Task.FromResult(new FieldIsCachedReply
        {
            Status = result,
        });
    }
}
