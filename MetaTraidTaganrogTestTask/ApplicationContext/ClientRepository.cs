using MetaTraidTaganrogTestTask.Interfaces;
using MetaTraidTaganrogTestTask.Models;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json.Linq;

namespace MetaTraidTaganrogTestTask.ApplicationContext
{
    public class ClientRepository : IClientRepository
    {
        private string _connectionString;
        public ClientRepository()
        {
            string json = System.IO.File.ReadAllText("appsettings.json");
            JObject jsonObject = JObject.Parse(json);
            _connectionString = jsonObject["DbConnctionString"].ToString();
        }
        public void AddClient(Client client)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "INSERT INTO Clients (Name, Surname) VALUES (@Name, @Surname)";
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@Name", client.Name);
                command.Parameters.AddWithValue("@Surname", client.Surname);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public List<Client> GetAllClients()
        {
            List<Client> clients = new List<Client>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Clients";
                SqlCommand command = new SqlCommand(query, connection);

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        clients.Add(new Client
                        {
                            Id = (int)reader["Id"],
                            Name = reader["Name"].ToString(),
                            Surname = reader["Surname"].ToString()
                        });
                    }
                }
            }

            return clients;
        }
    }
}
