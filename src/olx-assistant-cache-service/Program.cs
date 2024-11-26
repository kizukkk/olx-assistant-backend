using olx_assistant_cache_service.Services;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

#region Application Service

builder.Services.AddGrpc();

builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var configuration = builder.Configuration.GetConnectionString("Redis");
    return ConnectionMultiplexer.Connect(configuration);
});

#endregion


var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<IsCachedService>();
app.MapGet("/", () => "Service is working!");

app.Run();
