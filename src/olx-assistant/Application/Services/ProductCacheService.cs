using olx_assistant_application.Interfaces.IRepositories;
using olx_assistant_contracts.Interfaces.IServices;

namespace olx_assistant_application.Services;
public class ProductCacheService : IProductCacheService
{
    private readonly IRedisCashedRepository _cashedRepository;

    public ProductCacheService(IRedisCashedRepository repo) => 
        _cashedRepository = repo;

    public async Task<bool> ProductIsCached(int id)
    {
       return await _cashedRepository.ProductIsProcessed(id);
    }
}
