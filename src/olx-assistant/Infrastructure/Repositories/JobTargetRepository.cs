using olx_assistant_contracts.Interfaces.IRepositories;
using olx_assistant_domain.Entities.Common;
using olx_assistant_infrastructure.DbContexts;

namespace olx_assistant_infrastructure.Repositories;
public class JobTargetRepository : IJobTargetRepository
{
    private readonly MsSqlDbContext _context;

    public JobTargetRepository(MsSqlDbContext context)
    {
        _context = context;
    }
    public void RegisterTask(TargetJob targetTask)
    {
        _context.TargetTasks.Add(targetTask);
        _context.SaveChanges();
    }

}
