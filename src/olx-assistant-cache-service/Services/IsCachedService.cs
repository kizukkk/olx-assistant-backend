using Grpc.Core;
using olx_assistant_cache_service;

namespace olx_assistant_cache_service.Services;

public class IsCachedService : isChased.isChasedBase
{
    private readonly ILogger<IsCachedService> _logger;
    public IsCachedService(ILogger<IsCachedService> logger)
    {
        _logger = logger;
    }

    public override Task<FieldIsCachedReply> FieldIsCached(FieldIsCachedRequest request, ServerCallContext context)
    {
        _logger.LogInformation($"Received request for ID: {request.Id}");
        return Task.FromResult(new FieldIsCachedReply
        {
            Status = true,
        });
    }
}
