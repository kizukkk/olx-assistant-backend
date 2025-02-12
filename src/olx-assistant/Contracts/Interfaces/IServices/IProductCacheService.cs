namespace olx_assistant_contracts.Interfaces.IServices;
public interface IProductCacheService
{
    public Task<bool> ProductIsCached(int id);

}
