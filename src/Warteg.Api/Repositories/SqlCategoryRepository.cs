namespace Warteg.Api.Repositories;

using System.Data;
using Warteg.Api.Models;
using Microsoft.Data.SqlClient;


public sealed class SqlCategoryRepository : ICategoryRepository
{
    private readonly string _connectionString;

    public SqlCategoryRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("Warteg") ?? throw new InvalidOperationException("Connection menghilang");
    }

    public async Task<bool> AddCategoryAsync(Category category, CancellationToken cancellationToken)
    {
        const string sql = """
            INSERT INTO dbo.Categories(CategoryCode, Name)
            VALUES (@categoryCode, @name)
            """;

        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);
        await using var command = new SqlCommand(sql, connection);

        AddCategoryParameter(command, category);

        try
        {
            var affectedRows = await command.ExecuteNonQueryAsync(cancellationToken);
            return affectedRows == 1;
        }
        catch (SqlException e) when (e.Number is 2601 or 2627)
        {
            return false;
        }
        catch (SqlException e)
        {
            Console.WriteLine($"SQL Error: {e.Number} - {e.Message}");
            return false;
        }
    }

    public async Task<bool> DeleteCategoryAsync(string categoryCode, CancellationToken cancellationToken)
    {
        const string sql = """
            DELETE FROM dbo.Categories
            WHERE CategoryCode = @categoryCode
            """;

        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);
        await using var command = new SqlCommand(sql, connection);
        AddCategoryCode(command, categoryCode);
        try
        {
            var affectedRows = await command.ExecuteNonQueryAsync(cancellationToken);
            return affectedRows == 1;
        }
        catch (SqlException e) when (e.Number == 547)
        {
            return false;
        }
    }

    public async Task<IReadOnlyList<Category>> GetAllAsync(CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT CategoryCode, Name
            FROM dbo.Categories
            """;

        var categories = new List<Category>();
        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);
        await using var command = new SqlCommand(sql, connection);
        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        while (await reader.ReadAsync(cancellationToken))
        {
            categories.Add(ReadCategory(reader));
        }

        return categories;
    }

    public async Task<Category?> GetByIdAsync(string categoryCode,CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT CategoryCode, Name
            FROM dbo.Categories
            WHERE CategoryCode = @categoryCode
            """;

        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);
        await using var command = new SqlCommand(sql, connection);
        AddCategoryCode(command, categoryCode);
        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        if (!await reader.ReadAsync(cancellationToken))
        {
            return null;
        }
        return ReadCategory(reader);
    }

    public async Task<bool> UpdateCategoryAsync(Category category, CancellationToken cancellationToken)
    {
        const string sql = """
            UPDATE dbo.Categories
            SET Name = @name
            WHERE CategoryCode = @categoryCode
            """;

        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);
        await using var command = new SqlCommand(sql, connection);
        AddCategoryParameter(command, category);

        var affectedRows = await command.ExecuteNonQueryAsync(cancellationToken);
        return affectedRows == 1;
    }

    private static void AddCategoryParameter(SqlCommand command, Category category)
    {
        AddCategoryCode(command, category.CategoryCode);
        command.Parameters.Add("@name", SqlDbType.NVarChar, 100).Value = category.Name;
    }

    private static void AddCategoryCode(SqlCommand command,string categoryCode)
    {
        command.Parameters.Add("@categoryCode", SqlDbType.NVarChar, 50).Value = categoryCode.Trim().ToUpperInvariant();
    }

    private static Category ReadCategory(SqlDataReader reader)
    {
        return new Category(
            reader.GetString(0),
            reader.GetString(1)
        );
    }
}
