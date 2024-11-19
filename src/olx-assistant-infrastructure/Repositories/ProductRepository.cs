using olx_assistant_application.Interfaces.IRepositories;
using olx_assistant_infrastructure.DbContexts;
using olx_assistant_domain.Entities;

namespace olx_assistant_infrastructure.Repositories;
public class ProductRepository : BaseRepository<Product>, IProductRepository
{
    private readonly MsSqlDbContext _context;

    public ProductRepository(MsSqlDbContext context)
        : base(context)
    {
        _context = context;
    }

}
