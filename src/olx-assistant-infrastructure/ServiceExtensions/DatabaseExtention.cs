using Microsoft.Extensions.DependencyInjection;
using olx_assistant_infrastructure.DbContexts;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

namespace olx_assistant_infrastructure.ServiceExtensions;
public static class DatabaseExtension
{
    public static void MsSqlDatabaseConfigure(
        this IServiceCollection services,
        IConfiguration configuration
        )
    {
        var connectionString = configuration.GetConnectionString("MsSql");

        //TODO : Implement Retry and Log logics   
        services.AddDbContext<MsSqlDbContext>(opt =>
        {
            opt.UseSqlServer(connectionString);
        });
    }

}
