using TableDependency.SqlClient.Base.EventArgs;
using olx_assistant_cache_service.Services;
using olx_assistant_domain.Entities;
using TableDependency.SqlClient;
using Microsoft.Data.SqlClient;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

#region Application Service

builder.Services.AddGrpc();

builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
  var redisConnection = builder.Configuration.GetConnectionString("Redis");
  Console.Write($"\n{redisConnection}\n");
  return ConnectionMultiplexer.Connect(redisConnection!);
});

builder.Services.AddSingleton(sp =>
{
  var sqlConnection = builder.Configuration.GetConnectionString("MSSQL");
  Console.Write($"\n{sqlConnection}\n");
  return sqlConnection;
});

builder.Services.AddSingleton<ListeningServices>();

#endregion

#region Build

var app = builder.Build();
var messageRepository = app.Services.GetRequiredService<ListeningServices>();
var task = Task.Run(() => messageRepository.StartListening());

try
{
  messageRepository.RedisInit();
}
catch (Exception ex)
{
  Console.WriteLine($"Ops! Some problems with Redis DB connection. Ensure that DB is running and connection string is valid.\n\n {ex.StackTrace}, {ex}");
  Console.WriteLine("Press Enter to force close.");
  Console.ReadLine();
  Environment.Exit(0);
}

app.Lifetime.ApplicationStopped.Register(() => messageRepository.Dispose());

app.MapGrpcService<IsCachedService>();
app.MapGet("/", () => { });

app.Run();

#endregion

public class ListeningServices : IDisposable
{
  private readonly SqlTableDependency<Product> sqlTableDependency;
  private readonly string _sqlConnectionString;
  private readonly IDatabase _db;

  public ListeningServices(string sqlConnectionString, IConnectionMultiplexer redis)
  {
    _sqlConnectionString = sqlConnectionString;
    _db = redis.GetDatabase();
  }

  public void RedisInit()
  {
    _db.KeyDelete("product_id");

    using (var connection = new SqlConnection(_sqlConnectionString))
    {
      connection.Open();

      var sql = "SELECT [ProductId] FROM [olx_assistance_db].[dbo].[Products]";

      using var command = new SqlCommand(sql, connection);
      using var reader = command.ExecuteReader();

      while (reader.Read())
      {
        int data = (int)reader["ProductId"];
        _db.SetAdd("product_id", data);
        Console.WriteLine($"Cashing Product: {data}");
      }
      connection.Close();
    }
  }

  public void StartListening()
  {
    var sqlTableDependency = new SqlTableDependency<Product>(_sqlConnectionString, "Products");

    sqlTableDependency.OnStatusChanged += (sender, e) =>
    {
      Console.WriteLine($"Status changed: {e.Status}");
    };

    sqlTableDependency.OnError += (sender, e) =>
    {
      Console.WriteLine($"Error: {e.Message}");
    };
    sqlTableDependency.OnChanged += HandleOnChanged;

    sqlTableDependency.Start();
  }

  private void HandleOnChanged(object sender, RecordChangedEventArgs<Product> e)
  {
    if (e.ChangeType == TableDependency.SqlClient.Base.Enums.ChangeType.Insert)
    {
      _db.SetAdd("product_id", e.Entity.ProductId);
      Console.WriteLine($"Received a new record: {e.Entity.ProductId}");
    }
  }

  #region IDisposable

  private bool disposedValue = false;

  protected virtual void Dispose(bool disposing)
  {
    if (!disposedValue)
    {
      if (disposing && sqlTableDependency != null)
      {
        sqlTableDependency.Stop();
        sqlTableDependency.Dispose();
      }
      disposedValue = true;
    }
  }

  public void Dispose()
  {
    Dispose(true);
  }

  #endregion
}