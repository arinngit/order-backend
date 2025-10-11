using Npgsql;

namespace OrdersBackend.DataAccess.Factories;

public interface IDbConnectionFactory
{
    NpgsqlConnection CreateConnectionAuthDb();

    NpgsqlConnection CreateConnectionProductDb();
}