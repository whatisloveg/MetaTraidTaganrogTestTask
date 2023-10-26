using MetaTraidTaganrogTestTask.Interfaces;
using MetaTraidTaganrogTestTask.Models;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json.Linq;

namespace MetaTraidTaganrogTestTask.ApplicationContext
{
    public class BookRepository : IBookRepository
    {
        private string _connectionString;
        public BookRepository()
        {
            string json = System.IO.File.ReadAllText("appsettings.json");
            JObject jsonObject = JObject.Parse(json);
            _connectionString = jsonObject["DbConnctionString"].ToString();
        }
        public void AddBook(Book book)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "INSERT INTO Books (Name, Description, IsTaken, TakerClientId) VALUES (@Name, @Description, @IsTaken, @TakerClientId)";
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@Name", book.Name);
                command.Parameters.AddWithValue("@Description", book.Description);
                command.Parameters.AddWithValue("@IsTaken", book.IsTaken);
                command.Parameters.AddWithValue("@TakerClientId", book.TakerClientId);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }
        public void EditBookDescription(int bookId, string description)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "UPDATE Books SET Description = @Description WHERE Id = @BookId";
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@BookId", bookId);
                command.Parameters.AddWithValue("@Description", description);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void TakeBook(int bookId, int clientId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "UPDATE Books SET IsTaken= 1, TakerClientId = @ClientId WHERE Id = @BookId";
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@BookId", bookId);
                command.Parameters.AddWithValue("@ClientId", clientId);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }
        public void ReturnBook(int bookId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "UPDATE Books SET IsTaken = 0, TakerClientId = 0 WHERE Id = @BookId";
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@BookId", bookId);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

      

        public List<Book> GetAllBooks()
        {
            List<Book> books = new List<Book>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Books";
                SqlCommand command = new SqlCommand(query, connection);

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        books.Add(new Book
                        {
                            Id = (int)reader["Id"],
                            Name = reader["Name"].ToString(),   
                            Description = reader["Description"].ToString(),
                            IsTaken = (bool)reader["IsTaken"],
                            TakerClientId = (int)reader["TakerClientId"]

                        });
                    }
                }
            }

            return books;
        }

       
    }
}
