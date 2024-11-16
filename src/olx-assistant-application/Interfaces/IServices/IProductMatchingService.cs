using olx_assistant_application.DTOs.Shared;
using olx_assistant_domain.Entities;

namespace olx_assistant_application.Interfaces.IServices;
public interface IProductMatchingService
{
    public Task<List<ProductResponse>> StartMatchingByTargetAsync(Target target);
}
