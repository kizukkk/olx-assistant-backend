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
    var configuration = builder.Configuration.GetConnectionString("Redis");
    return ConnectionMultiplexer.Connect(configuration!);
});

#endregion

#region Build

var app = builder.Build();

var messageRepository = new ListeningServices();
var task = Task.Run(() => messageRepository.StartListening());

try
{
    ListeningServices.RedisInit();
}
catch (Exception ex)
{
    Console.WriteLine("Ops! Some problems with Redis DB connection. " +
                    $"Ensure that DB is running and connection string is valid.\n\n {ex.StackTrace}");

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
    private SqlTableDependency<Product> sqlTableDependency;
    static readonly string SqlConnectionString = "Server=AFRODITA\\LOCALSQLSERVER;Database=olx_assistance_db;Trusted_Connection=True;User Id=app_server;Password=202124; TrustServerCertificate=True";

    static readonly ConnectionMultiplexer _redis = ConnectionMultiplexer.Connect($"localhost:6379");
    static readonly IDatabase db = _redis.GetDatabase();

    public static void RedisInit()
    {
        db.KeyDelete("product_id");

        using (var connection = new SqlConnection(SqlConnectionString))
        {
            connection.Open();

            var sql = "SELECT [ProductId] FROM [olx_assistance_db].[dbo].[Products]";

            using var command = new SqlCommand(sql, connection);
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                int data = (int)reader["ProductId"];
                db.SetAdd("product_id", data);
                Console.WriteLine($"Cashing Product: {data}");
            }
            connection.Close();
        }
    }
    public void StartListening()
    {
        sqlTableDependency =
            new(SqlConnectionString, "Products");

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
            db.SetAdd("product_id", e.Entity.ProductId);
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
