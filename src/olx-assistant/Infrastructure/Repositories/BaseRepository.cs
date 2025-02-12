using Microsoft.EntityFrameworkCore;
using olx_assistant_application.Interfaces.IRepositories;
using olx_assistant_domain.Entities.Common;
using olx_assistant_infrastructure.DbContexts;

namespace olx_assistant_infrastructure.Repositories;
public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
{
    private readonly MsSqlDbContext _context;
    private readonly DbSet<T> _entities;

    public BaseRepository(MsSqlDbContext context)
    {
        _context = context;
        _entities = _context.Set<T>();
    }

    public void Create(T obj) => _entities.Add(obj);

    public void Delete(int id) =>
        _entities.Remove(_entities.First(e => e.Id == id));

    public async Task<T> GetById(int id) =>
        await _entities.FirstAsync(e => e.Id == id);

    public void Update(T obj) => _entities.Update(obj);

    public void SaveChanges() => _context.SaveChanges();
}