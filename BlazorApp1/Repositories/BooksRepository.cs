using BlazorApp1.Models_and_DTOs;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorApp1.Repositories
{
    public class BooksRepository : IBooksRepository
    {
        private readonly IConfiguration _configuration;

        public BooksRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<bool> DoesBookExist(int id)
        {
            var query = "SELECT 1 FROM books WHERE PK = @ID";

            await using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
            await using SqlCommand command = new SqlCommand();

            command.Connection = connection;
            command.CommandText = query;
            command.Parameters.AddWithValue("@ID", id);

            await connection.OpenAsync();

            var res = await command.ExecuteScalarAsync();

            return res is not null;
        }

        public async Task<bool> DoesOwnerExist(int id)
        {
            var query = "SELECT 1 FROM publishing_houses WHERE PK = @ID";

            await using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
            await using SqlCommand command = new SqlCommand();

            command.Connection = connection;
            command.CommandText = query;
            command.Parameters.AddWithValue("@ID", id);

            await connection.OpenAsync();

            var res = await command.ExecuteScalarAsync();

            return res is not null;
        }

        public async Task<bool> DoesGenreExist(int id)
        {
            var query = "SELECT 1 FROM genres WHERE PK = @ID";

            await using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
            await using SqlCommand command = new SqlCommand();

            command.Connection = connection;
            command.CommandText = query;
            command.Parameters.AddWithValue("@ID", id);

            await connection.OpenAsync();

            var res = await command.ExecuteScalarAsync();

            return res is not null;
        }

        public async Task<GenresBookDTO> GetGenresBook(int id)
        {
            List<GenreDTO> genres = new List<GenreDTO>();

            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default")))
            {
                await connection.OpenAsync();

                string query = @"SELECT genres.PK, genres.name 
                                 FROM genres 
                                 INNER JOIN books_genres ON genres.PK = books_genres.FK_genre 
                                 WHERE books_genres.FK_book = @BookId;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@BookId", id);
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        genres.Add(new GenreDTO
                        {
                            PK = Convert.ToInt32(reader["PK"]),
                            name = reader["name"].ToString()
                        });
                    }
                }
            }

            // Dummy data for title and OwnerId; replace with actual queries if needed
            GenresBookDTO bookWithGenres = new GenresBookDTO
            {
                PK = id,
                title = "Dummy Book Title", // Replace with actual book title query if needed
                OwnerId = 1, // Replace with actual owner query if needed
                Genres = genres
            };

            return bookWithGenres;
        }

        public async Task<int> AddBook(NewBookDTO book)
        {
            var insert = @"INSERT INTO books (title) 
                           OUTPUT INSERTED.PK
                           VALUES (@Title);";

            await using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
            await using SqlCommand command = new SqlCommand();

            command.Connection = connection;
            command.CommandText = insert;

            command.Parameters.AddWithValue("@Title", book.title);

            await connection.OpenAsync();

            var id = await command.ExecuteScalarAsync();

            if (id is null) throw new Exception("Failed to insert book");

            return Convert.ToInt32(id);
        }

        public async Task AddGenreBook(int bookId, Genre genre)
        {
            var query = @"INSERT INTO books_genres (FK_book, FK_genre) 
                          VALUES (@BookId, @GenreId)";

            await using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
            await using SqlCommand command = new SqlCommand();

            command.Connection = connection;
            command.CommandText = query;
            command.Parameters.AddWithValue("@BookId", bookId);
            command.Parameters.AddWithValue("@GenreId", genre.PK);

            await connection.OpenAsync();

            await command.ExecuteNonQueryAsync();
        }
    }
}
