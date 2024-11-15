using FastEndpoints;
using olx_assistant_application.Interfaces;
using olx_assistant_application.Mapper;
using olx_assistant_application.Services;


#region Application Builder

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddFastEndpoints();

#endregion


#region Application Instance & Run

var app = builder.Build();

app.UseFastEndpoints();

app.Run();

#endregion

