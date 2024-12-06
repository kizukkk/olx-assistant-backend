using olx_assistant_domain.Entities.Common;

namespace olx_assistant_contracts.Interfaces.IRepositories;
public interface IJobTargetRepository
{
    public void RegisterTask(TargetJob targetJob);
}
