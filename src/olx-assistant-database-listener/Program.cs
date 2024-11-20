using Microsoft.Data.SqlClient;
using StackExchange.Redis;


ListeningServices.RedisInit();
ListeningServices.StartListening();


static class ListeningServices
{
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

    public static void StartListening()
    {
        SqlDependency.Start(SqlConnectionString);

        using (SqlConnection connection = new SqlConnection(SqlConnectionString))
        {
            connection.Open();

            Console.WriteLine("Notification Waiting...");
            while (true)
            {
                using (SqlCommand command = new SqlCommand(@"
            WAITFOR (
                RECEIVE TOP(1)
                    CONVERT(NVARCHAR(MAX), message_body) AS MessageBody
                FROM TargetQueue
            ), TIMEOUT 5000;", connection))
                {
                    var result = command.ExecuteScalar();
                    if (result != null && !string.IsNullOrEmpty(result.ToString()))
                    {
                        int data = 0;

                        try
                        {
                            data = int.Parse((string)result);
                        }catch (Exception ex)
                        {
                            Console.WriteLine($"Failed Parse data from result - {result}");
                            Console.WriteLine(ex);
                        }

                        db.SetAdd("product_id", data);
                        Console.WriteLine($"Received a new record: {result}");
                    }
                }
            }
        }
    }
}

