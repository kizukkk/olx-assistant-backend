#region Application Builder

using FastEndpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddFastEndpoints();

#endregion


#region Application Instance & Run

var app = builder.Build();

app.UseFastEndpoints();

app.Run();

#endregion

