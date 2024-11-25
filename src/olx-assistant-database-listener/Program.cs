using Microsoft.Data.SqlClient;
using StackExchange.Redis;
using TableDependency.SqlClient;
using olx_assistant_domain.Entities;
using TableDependency.SqlClient.Base.EventArgs;


ListeningServices.RedisInit();

var messageRepository = new ListeningServices();
Task task = Task.Run(() => messageRepository.StartListening());

Console.WriteLine("Listening for changes. Press Enter to stop.");
Console.ReadLine();

messageRepository.Dispose();

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
                int data = (int) reader["ProductId"];
                db.SetAdd("product_id", data);
                Console.WriteLine($"Cashing Product: {data}");
            }
            connection.Close();
        }
    }
    public void StartListening()
    {
        sqlTableDependency = 
            new (SqlConnectionString, "Products");

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

