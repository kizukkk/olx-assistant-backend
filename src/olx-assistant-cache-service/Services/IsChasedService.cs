using Grpc.Core;
using olx_assistant_cache_service;

namespace olx_assistant_cache_service.Services;

public class IsChasedService : isChased.isChasedBase
{
    private readonly ILogger<IsChasedService> _logger;
    public IsChasedService(ILogger<IsChasedService> logger)
    {
        _logger = logger;
    }

    public override Task<FieldIsCashedReply> FieldIsCashed(FieldIsCashedRequest request, ServerCallContext context)
    {
        _logger.LogInformation($"Received request for ID: {request.Id}");
        return Task.FromResult(new FieldIsCashedReply
        {
            Status = true // або ваша логіка перевірки
        });
    }
}
