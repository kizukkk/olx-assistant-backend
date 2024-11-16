using olx_assistant_application.Interfaces.IServices;
using olx_assistant_application.Services;
using olx_assistant_application.Mapper;
using FastEndpoints;



#region Application Builder

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddFastEndpoints();

builder.Services.AddScoped<IProductMatchingService, ProductMatchingService>();

builder.Services.AddAutoMapper(typeof(MapperProfile));

#endregion


#region Application Instance & Run

var app = builder.Build();

app.UseFastEndpoints();

app.Run();

#endregion

