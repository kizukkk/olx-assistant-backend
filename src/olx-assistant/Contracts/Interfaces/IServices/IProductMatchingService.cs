using olx_assistant_domain.Entities;

namespace olx_assistant_application.Interfaces.IServices;
public interface IProductMatchingService
{
    public void StartMatchingByTarget(Target target);
    public void StartFastMatchingByTarget(Target target);
}
