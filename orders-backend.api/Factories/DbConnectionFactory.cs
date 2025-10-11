using Microsoft.Extensions.Options;
using OrdersBackend.DataAccess.Factories;
using Npgsql;
using OrdersBackend.Insfrastructure.Options;
using Sprache;

namespace OrdersBackend.Api.Factories;

public class DbConnectionFactory : IDbConnectionFactory
{
    private readonly string _connectionStringForAuth;
    private readonly string _connectionStringForProduct;

    public DbConnectionFactory(IOptions<ConnectionStringOptions> options)
    {
        _connectionStringForAuth = options.Value.ConnectionStringForAuthDb;
        _connectionStringForProduct = options.Value.ConnectionStringForProductDb;
    }

    public NpgsqlConnection CreateConnectionAuthDb()
    {
        return new NpgsqlConnection(_connectionStringForAuth);
    }

    public NpgsqlConnection CreateConnectionProductDb()
    {
        Console.WriteLine($"connection product {_connectionStringForProduct}");
        return new NpgsqlConnection(_connectionStringForProduct);
    }
}