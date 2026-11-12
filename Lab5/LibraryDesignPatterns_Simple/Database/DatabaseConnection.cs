using Microsoft.Data.SqlClient;
namespace LibraryDesignPatterns.Database;

// Singleton: only one database manager object.
public sealed class DatabaseConnection
{
    private static readonly DatabaseConnection _instance = new();
    public static DatabaseConnection Instance => _instance;
    private DatabaseConnection() { }

    // Change .\SQLEXPRESS to your SQL Server instance name.
    private readonly string _connectionString =
        @"Server=(localdb)\MSSQLLocalDB;Database=LibraryDB;Trusted_Connection=True;Encrypt=True;TrustServerCertificate=True;";

    // SQL connections are short-lived and reused internally through pooling.
    public SqlConnection OpenConnection()
    {
        var connection = new SqlConnection(_connectionString);
        connection.Open();
        return connection;
    }
}
