using olx_assistant_application.Interfaces.IRepositories;
using olx_assistant_infrastructure.ServiceExtensions;
using olx_assistant_application.Interfaces.IServices;
using olx_assistant_infrastructure.Repositories;
using olx_assistant_infrastructure.DbContexts;
using olx_assistant_application.Services;
using olx_assistant_application.Mapper;
using Microsoft.EntityFrameworkCore;
using FastEndpoints;


#region Application Builder

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddFastEndpoints();

builder.Services.MsSqlDatabaseConfigure(builder.Configuration);

builder.Services.AddScoped<IProductMatchingService, ProductMatchingService>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();

builder.Services.AddAutoMapper(typeof(MapperProfile));

#endregion


#region Application Instance & Run

var app = builder.Build();

app.UseFastEndpoints();

DatabaseExtension.DatabaseMigrate(app.Services.CreateScope());

app.Run();

#endregion

