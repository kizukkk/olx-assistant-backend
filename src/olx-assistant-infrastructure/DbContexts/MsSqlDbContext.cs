using olx_assistant_domain.Entities;
using Microsoft.EntityFrameworkCore;
using olx_assistant_domain.Entities.Common;

namespace olx_assistant_infrastructure.DbContexts;
public class MsSqlDbContext : DbContext
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Target> Targets { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<Keyword> Keywords { get; set; }
    public DbSet<TargetTask> TargetTasks { get; set; }

    public MsSqlDbContext(DbContextOptions<MsSqlDbContext> options) 
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder model)
    {
        model.Entity<Product>()
            .Property(p => p.Price)
            .HasColumnType("money");

        model.Entity<Product>()
            .ToTable(tb => tb.HasTrigger("trgAfterInsert"));

        model.Entity<TargetTask>()
            .HasKey(p => p.TaskId);

        base.OnModelCreating(model);
    }
}
