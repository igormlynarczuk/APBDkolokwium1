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
        var query = "SELECT 1 FROM Animal WHERE ID = @ID";

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
	    command.Parameters.AddWithValue("@AdmissionDate", newBookWithGenres.AdmissionDate);
	    command.Parameters.AddWithValue("@OwnerId", newBookWithGenres.OwnerId);
	    
	    await connection.OpenAsync();

	    var transaction = await connection.BeginTransactionAsync();
	    command.Transaction = transaction as SqlTransaction;
	    
	    try
	    {
		    var id = await command.ExecuteScalarAsync();
    
		    foreach (var procedure in newAnimalWithProcedures.Procedures)
		    {
			    command.Parameters.Clear();
			    command.CommandText = "INSERT INTO Procedure_Animal VALUES(@ProcedureId, @AnimalId, @Date)";
			    command.Parameters.AddWithValue("@ProcedureId", procedure.ProcedureId);
			    command.Parameters.AddWithValue("@AnimalId", id);
			    command.Parameters.AddWithValue("@Date", procedure.Date);

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

    public async Task<int> AddAnimal(NewAnimalDTO animal)
    {
	    var insert = @"INSERT INTO Animal VALUES(@Name, @Type, @AdmissionDate, @OwnerId);
					   SELECT @@IDENTITY AS ID;";
	    
	    await using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
	    await using SqlCommand command = new SqlCommand();
	    
	    command.Connection = connection;
	    command.CommandText = insert;
	    
	    command.Parameters.AddWithValue("@Name", animal.Name);
	    command.Parameters.AddWithValue("@Type", animal.Type);
	    command.Parameters.AddWithValue("@AdmissionDate", animal.AdmissionDate);
	    command.Parameters.AddWithValue("@OwnerId", animal.OwnerId);
	    
	    await connection.OpenAsync();
	    
	    var id = await command.ExecuteScalarAsync();

	    if (id is null) throw new Exception();
	    
	    return Convert.ToInt32(id);
    }

    public async Task AddProcedureAnimal(int animalId, ProcedureWithDate procedure)
    {
	    var query = $"INSERT INTO Procedure_Animal VALUES(@ProcedureID, @AnimalID, @Date)";

	    await using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
	    await using SqlCommand command = new SqlCommand();

	    command.Connection = connection;
	    command.CommandText = query;
	    command.Parameters.AddWithValue("@ProcedureID", procedure.ProcedureId);
	    command.Parameters.AddWithValue("@AnimalID", animalId);
	    command.Parameters.AddWithValue("@Date", procedure.Date);

	    await connection.OpenAsync();

	    await command.ExecuteNonQueryAsync();
    }
}