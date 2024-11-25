using olx_assistant_application.Interfaces.IRepositories;
using olx_assistant_infrastructure.ServiceExtensions;
using olx_assistant_application.Interfaces.IServices;
using olx_assistant_infrastructure.Repositories;
using olx_assistant_infrastructure.DbContexts;
using olx_assistant_application.Services;
using olx_assistant_application.Mapper;
using Microsoft.EntityFrameworkCore;
using FastEndpoints;
using Hangfire;


#region Application Builder

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddFastEndpoints();

builder.Services.MsSqlDatabaseConfigure(builder.Configuration);
builder.Services.CacheServiceConfig(builder.Configuration);

builder.Services.AddHangfire(opt => opt.UseSqlServerStorage(builder.Configuration.GetConnectionString("Hangfire")));
builder.Services.AddHangfireServer();

builder.Services.AddScoped<IProductMatchingService, ProductMatchingService>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();

builder.Services.AddAutoMapper(typeof(MapperProfile));

#endregion


#region Application Instance & Run

var app = builder.Build();

app.UseFastEndpoints();

app.UseHangfireDashboard();

DatabaseExtension.DatabaseMigrate(app.Services.CreateScope());

app.Lifetime.ApplicationStopped.Register(() =>
{
    Console.WriteLine("Server shutdown...");
    var context = app.Services.CreateScope().ServiceProvider.GetService<MsSqlDbContext>();
    var tableNames = context!.Model.GetEntityTypes()
    .Select(t => t.GetTableName())
    .Distinct()
    .ToList();

    foreach (var tableName in tableNames)
    {
        context.Database.ExecuteSqlRaw($"DELETE FROM {tableName};");
        context.SaveChanges();
    }
    Console.WriteLine("Database is cleaned!");
});

app.Run();

#endregion

