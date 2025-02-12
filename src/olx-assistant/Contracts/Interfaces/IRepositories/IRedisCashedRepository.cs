namespace olx_assistant_application.Interfaces.IRepositories;
public interface IRedisCashedRepository
{
    public Task<Boolean> ProductIsProcessed(int id);

}
