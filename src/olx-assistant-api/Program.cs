#region Application Builder

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

#endregion


#region Application Instance & Run

var app = builder.Build();

app.MapControllers();

app.Run();

#endregion

