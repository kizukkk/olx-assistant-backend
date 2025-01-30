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
        try
        {
            _connectionMultiplexer = connectionMultiplexer;
            _database = _connectionMultiplexer.GetDatabase();
        }
        catch(RedisConnectionException ex)
        {
            Console.WriteLine("Ops! Some problems with Redis DB connection. " +
                $"Ensure that DB is running and connection string is valid.\n {ex.Message}");
        }
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
