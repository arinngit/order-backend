using Npgsql;
using OrdersBackend.Domain.Models;
using OrdersBackend.Domain.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using OrdersBackend.Domain.Constants;
using OrdersBackend.DataAccess.Factories;
using Dapper;

namespace OrdersBackend.DataAccess.Repositories;

public class RolesRepository : IRolesRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public RolesRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<Role> GetById(int Id)
    {
        try
        {
            await using NpgsqlConnection connection = _connectionFactory.CreateConnectionAuthDb();

            const string query = "select * from \"Roles\" where \"Id\" = @id;";

            return await connection.QueryFirstOrDefaultAsync<Role>(query, new { Id }) ?? new Role();
        }
        catch (Exception e)
        {
            Log.Error($"Error In RolesRepository::GetById {e.Message}");
            return new Role();
        }
    }
}
