using Dapper;
using Npgsql;
using OrdersBackend.DataAccess.Factories;
using OrdersBackend.Domain.Models;
using OrdersBackend.Domain.Repositories;
using Serilog;

namespace OrdersBackend.DataAccess.Repositories;

public class UsersRepository : IUsersRepository
{
    private readonly IDbConnectionFactory _factory;

    public UsersRepository(IDbConnectionFactory factory)
    {
        _factory = factory;
    }
    
    public async Task<User> GetById(int id)
    {
        try
        {
            await using NpgsqlConnection connection = _factory.CreateConnectionAuthDb();

            const string query = @"select * from ""Users"" where ""Id"" = @id;";

            return await connection.QueryFirstOrDefaultAsync<User>(query, new {id}) ?? new User();
        }
        catch (Exception e)
        {
            Log.Error($"Error In UsersRepository::GetById {e.Message}");
            return new User();
        }
    }
}