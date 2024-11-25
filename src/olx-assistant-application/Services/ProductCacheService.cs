using olx_assistant_application.Interfaces.IRepositories;

namespace olx_assistant_application.Services;
public class ProductCacheService
{
    private readonly IRedisCashedRepository _cashedRepository;

    public ProductCacheService(IRedisCashedRepository repo) => 
        _cashedRepository = repo;

    public async Task<bool> ProductIsCached(int id)
    {
       return await _cashedRepository.ProductIsProcessed(id);
    }
}
