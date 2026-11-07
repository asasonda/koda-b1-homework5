using System.Data;
using Microsoft.Data.SqlClient;
using Warteg.Api.DTOs;
using Warteg.Api.Models;

namespace Warteg.Api.Repositories;

public sealed class SqlMenuRepository : IMenuRepository
{
    private readonly string _connectionString;

    public SqlMenuRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("Warteg") ?? throw new InvalidOperationException("connection menghilang");
    }
    public async Task<bool> AddMenuAsync(Menu menu, CancellationToken cancellationToken)
    {
        const string sql = """
            INSERT INTO dbo.Menu(MenuCode, Name, Price, Stock, CategoryId)
            VALUES (@menuCode, @name, @price, @stock, @categoryId)
            """;

        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);
        await using var command = new SqlCommand(sql, connection);

        AddMenuParameter(command, menu);

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

    public async Task<bool> DeleteMenuAsync(string menuCode, CancellationToken cancellationToken)
    {
        const string sql = """
            DELETE FROM dbo.Menu
            WHERE MenuCode = @menuCode
            """;

        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);
        await using var command = new SqlCommand(sql, connection);
        AddMenuCode(command, menuCode);
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

    public async Task<IReadOnlyList<MenuCategoryResponse>> GetAllWithCategoryAsync(CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT
                p.MenuCode,
                p.Name,
                p.Price,
                p.Stock,
                c.Name
            FROM dbo.Menu p
            JOIN dbo.Categories c
                ON c.id = p.CategoryId
            """;

        var menu = new List<MenuCategoryResponse>();
        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);
        await using var command = new SqlCommand(sql, connection);
        await using var reader = await command.ExecuteReaderAsync(cancellationToken);

        while (await reader.ReadAsync(cancellationToken))
        {
            menu.Add(new MenuCategoryResponse(
                reader.GetString(0),
                reader.GetString(1),
                reader.GetDecimal(2),
                reader.GetInt32(3),
                reader.GetString(4)
            ));
        }

        return menu;
    }

    public async Task<Menu?> GetByIdAsync(string menuCode, CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT m.MenuCode, m.Name, m.Price, m.Stock, c.Name
            FROM dbo.Menu m
            join Categories c on c.id = m.CategoryId
            WHERE MenuCode = @menuCode
            """;

        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);
        await using var command = new SqlCommand(sql, connection);
        AddMenuCode(command, menuCode);
        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        if (!await reader.ReadAsync(cancellationToken))
        {
            return null;
        }
        return ReadMenu(reader);
    }

    public async Task<bool> UpdateMenuAsync(Menu menu, CancellationToken cancellationToken)
    {
        const string sql = """
            UPDATE dbo.Menu
            SET Name = @name, Price = @price, Stock = @stock, CategoryId = @categoryId
            WHERE MenuCode = @menuCode
            """;

        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);
        await using var command = new SqlCommand(sql, connection);
        AddMenuParameter(command, menu);

        var affectedRows = await command.ExecuteNonQueryAsync(cancellationToken);
        return affectedRows == 1;
    }


    private static void AddMenuParameter(SqlCommand command, Menu menu)
    {
        AddMenuCode(command, menu.MenuCode);
        command.Parameters.Add("@name", SqlDbType.NVarChar, 100).Value = menu.Name;
        var price = command.Parameters.Add("@price", SqlDbType.Decimal);
        command.Parameters.Add("@stock", SqlDbType.Int).Value = menu.Stock;
        command.Parameters.Add("@categoryId", SqlDbType.Int).Value = menu.CategoryId;

        price.Precision = 18;
        price.Scale = 2;
        price.Value = menu.Price;
    }

    private static void AddMenuCode(SqlCommand command,string menuCode)
    {
        command.Parameters.Add("@menuCode", SqlDbType.NVarChar, 50).Value = menuCode.Trim().ToUpperInvariant();
    }

    private static Menu ReadMenu(SqlDataReader reader)
    {
        return new Menu(
            reader.GetString(0),
            reader.GetString(1),
            reader.GetDecimal(2),
            reader.GetInt32(3),
            reader.GetInt32(4)
        );
    }
}