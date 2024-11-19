using Microsoft.Data.SqlClient;

const string SqlConnectionString_Watch = "Server=AFRODITA\\LOCALSQLSERVER;Database=olx_assistance_db;Trusted_Connection=True;User Id=app_server;Password=202124; TrustServerCertificate=True";
const string RedisConnectionString_Write = "write";

SqlDependency.Start(SqlConnectionString_Watch);

using (SqlConnection connection = new SqlConnection(SqlConnectionString_Watch))
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
                Console.WriteLine($"Received a new record: {result}");
            }
        }
    }
}
