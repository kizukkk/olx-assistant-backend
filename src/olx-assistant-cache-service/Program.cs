using olx_assistant_cache_service.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<IsCachedService>();
app.MapGet("/", () => "Service is working!");

app.Run();
