using Dapper;
using Npgsql;
using OrdersBackend.DataAccess.Factories;
using OrdersBackend.Domain.Models;
using OrdersBackend.Domain.Repositories;
using Serilog;

namespace OrdersBackend.DataAccess.Repositories;

public class StatusRepository : IStatusRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public StatusRepository(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<List<Status>> GetAll()
    {
        try
        {
            await using NpgsqlConnection connection = _dbConnectionFactory.CreateConnectionProductDb();

            const string query = @"select * from ""OrderStatuses""";

            IEnumerable<Status> statuses = await connection.QueryAsync<Status>(query);

            return statuses.ToList();
        }
        catch (Exception e)
        {
            Log.Error($"Error In StatusRepository::GetAll {e.Message}");
            Console.WriteLine($"Error In StatusRepository::GetAll {e.Message}");
            return [];
        }
    }
}