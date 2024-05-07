namespace BlazorApp1.Repositories;
using Microsoft.Data.SqlClient;
using BlazorApp1.Models_and_DTOs;

public class BooksRepository : IBooksRepository
{
    private readonly IConfiguration _configuration;
    public BooksRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<bool> DoesBookExist(int id)
    {
        var query = "SELECT 1 FROM Book WHERE ID = @ID";

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
	    var query = "SELECT 1 FROM Owner WHERE ID = @ID";

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
	    var query = "SELECT 1 FROM [Procedure] WHERE ID = @ID";

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
					    nam = reader["name"].ToString()
				    });
			    }
		    }
	    }
	    GenresBookDTO bookWithGenres = new GenresBookDTO
	    {
		    PK = id,
		    title = "Tytuł książki",
		    OwnerId = 1,
		    Genres = genres
	    };
 
	    return bookWithGenres;
    }

    public async Task AddNewBookWithProcedures(NewBookWithGenres newBookWithGenres)
    {
	    var insert = @"INSERT INTO Animal VALUES(@Name, @Type, @AdmissionDate, @OwnerId);
					   SELECT @@IDENTITY AS ID;";
	    
	    await using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
	    await using SqlCommand command = new SqlCommand();
	    
	    command.Connection = connection;
	    command.CommandText = insert;
	    
	    command.Parameters.AddWithValue("@Name", newBookWithGenres.PK);
	    command.Parameters.AddWithValue("@Type", newBookWithGenres.title);
	    command.Parameters.AddWithValue("@OwnerId", newBookWithGenres.OwnerId);
	    
	    await connection.OpenAsync();

	    var transaction = await connection.BeginTransactionAsync();
	    command.Transaction = transaction as SqlTransaction;
	    
	    try
	    {
		    var id = await command.ExecuteScalarAsync();
    
		    foreach (var genre in newBookWithGenres.Genres)
		    {
			    command.Parameters.Clear();
			    command.CommandText = "INSERT INTO Procedure_Animal VALUES(@ProcedureId, @AnimalId, @Date)";
			    command.Parameters.AddWithValue("@GenreId", genre.PK);
			    command.Parameters.AddWithValue("@AnimalId", id);
			    command.Parameters.AddWithValue("@nam", genre.nam);

			    await command.ExecuteNonQueryAsync();
		    }

		    await transaction.CommitAsync();
	    }
	    catch (Exception)
	    {
		    await transaction.RollbackAsync();
		    throw;
	    }
    }

    public async Task<int> AddBook(NewBookDTO book)
    {
	    var insert = @"INSERT INTO Animal VALUES(@Name, @Type, @AdmissionDate, @OwnerId);
					   SELECT @@IDENTITY AS ID;";
	    
	    await using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
	    await using SqlCommand command = new SqlCommand();
	    
	    command.Connection = connection;
	    command.CommandText = insert;
	    
	    command.Parameters.AddWithValue("@Name", book.PK);
	    command.Parameters.AddWithValue("@Type", book.title);
	    command.Parameters.AddWithValue("@OwnerId", book.OwnerId);
	    
	    await connection.OpenAsync();
	    
	    var id = await command.ExecuteScalarAsync();

	    if (id is null) throw new Exception();
	    
	    return Convert.ToInt32(id);
    }

    public async Task AddGenreBook(int bookId, Genre genre)
    {
	    var query = $"INSERT INTO Procedure_Animal VALUES(@ProcedureID, @AnimalID, @Date)";

	    await using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
	    await using SqlCommand command = new SqlCommand();

	    command.Connection = connection;
	    command.CommandText = query;
	    command.Parameters.AddWithValue("@ProcedureID", genre.PK);
	    command.Parameters.AddWithValue("@AnimalID", bookId);
	    command.Parameters.AddWithValue("@Date", genre.nam);

	    await connection.OpenAsync();

	    await command.ExecuteNonQueryAsync();
    }
}