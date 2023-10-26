using Microsoft.Data.SqlClient;
using Newtonsoft.Json.Linq;

namespace MetaTraidTaganrogTestTask.ApplicationContext
{
    public class DatabaseInitializer
    {
        private string _connectionString;
        public DatabaseInitializer() 
        {
            _connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Integrated Security=True;";
        }
        public void InitializeDatabase()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var createDbCmd = new SqlCommand(
                    "IF NOT EXISTS (SELECT 1 FROM sys.databases WHERE name = 'LibraryDatabase') " +
                    "CREATE DATABASE LibraryDatabase;", connection))
                {
                    createDbCmd.ExecuteNonQuery();
                }

                using (var useCmd = new SqlCommand("USE LibraryDatabase", connection))
                {
                    useCmd.ExecuteNonQuery();
                }
                using (var createClientsTableCmd = new SqlCommand(
                    "IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Clients') " +
                    "CREATE TABLE Clients (" +
                    "Id INT PRIMARY KEY IDENTITY(1,1)," +
                    "Name NVARCHAR(255)," +
                    "Surname NVARCHAR(255));", connection))
                {
                    createClientsTableCmd.ExecuteNonQuery();
                }

                using (var createBooksTableCmd = new SqlCommand(
                    "IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Books') " +
                    "CREATE TABLE Books (" +
                    "Id INT PRIMARY KEY IDENTITY(1,1)," +
                    "Name NVARCHAR(255)," +
                    "Description NVARCHAR(MAX)," +
                    "IsTaken BIT," +
                    "TakerClientId INT);", connection))
                {
                    createBooksTableCmd.ExecuteNonQuery();
                }
            }
        
        }
    }
}
