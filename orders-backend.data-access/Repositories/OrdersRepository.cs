using Dapper;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using OrdersBackend.DataAccess.Factories;
using OrdersBackend.Domain.Models;
using OrdersBackend.Domain.Repositories;
using Serilog;

namespace OrdersBackend.DataAccess.Repositories;

public class OrdersRepository : IOrdersRepository
{
    private readonly IDbConnectionFactory _factory;

    public OrdersRepository(IDbConnectionFactory factory)
    {
        _factory = factory;
    }

    public async Task<Order> Add(Order order)
    {
        try
        {
            await using NpgsqlConnection connection = _factory.CreateConnectionProductDb();

            const string query = @"insert into ""Orders"" (""UserId"", ""OrdersNumber"", ""TotalAmount"", ""Country"", ""FirstName"", ""LastName"", ""Company"", ""DeliveryOption"", ""Address"", ""Appartment"", ""PostalCode"", ""City"", ""Phone"", ""TaxAmout"", ""ShippingAmount"", ""DiscountAmount"", ""FinalAmount"", ""StatusId"", ""PromoCode"", ""CreatedAt"", ""UpdatedAt"")
values (@userId, @ordersNumber, @totalAmount, @country, @firstName, @lastName, @company, @deliveryOption, @address, @appartment, @postalCode, @city, @phone, @taxAmout, @shippingAmount, @discountAmount, @finalAmount, @statusId, @promoCode, @createdAt, @updatedAt);";

            System.Console.WriteLine(query);

            System.Console.WriteLine(order.UserId);

            int rowsAffected = await connection.ExecuteAsync(query, order);

            if (rowsAffected == 1)
            {
                return order;
            }

            return new Order();
        }
        catch (Exception e)
        {
            Log.Error($"Error In OrdersRepository::Add {e.Message}");
            Console.WriteLine($"Error In OrdersRepository::Add {e.Message}");
            return new Order();
        }
    }

    public async Task<bool> Cancel(Guid id)
    {
        try
        {
            await using NpgsqlConnection connection = _factory.CreateConnectionProductDb();

            const string query = @"update ""Orders"" set ""StatusId"" = 5 where ""Id"" = @orderId;";

            int rowsAffected = await connection.ExecuteAsync(query, new { orderId = id });

            return rowsAffected == 1;
        }
        catch (Exception e)
        {
            Log.Error($"Error In OrdersRepository::Cancel {e.Message}");
            return false;
        }
    }

    public async Task<bool> ChangeOrderStatus(int orderId, int statusId)
    {
        try
        {
            await using NpgsqlConnection connection = _factory.CreateConnectionProductDb();

            const string query = @"update ""Orders"" set ""StatusId"" = @statusId where ""Id"" = @orderId;";

            int rowsAffected = await connection.ExecuteAsync(query, new { orderId, statusId });

            return rowsAffected == 1;
        }
        catch (Exception e)
        {
            Log.Error($"Something Went Wrong In OrdersRepository::ChangeOrderStatus {e.Message}");
            Console.WriteLine($"Something Went Wrong In OrdersRepository::ChangeOrderStatus {e.Message}");
            return false;
        }
    }

    public async Task<List<Order>> GetUsersOrders(int userId, int pageSize, int pageIndex)
    {
        try
        {
            await using NpgsqlConnection connection = _factory.CreateConnectionProductDb();

            const string query = @"
                select *
                from ""Orders""
                where ""UserId"" = @userId
                limit @pageSize offset @offset
            ";

            int offset = pageSize * pageIndex;

            IEnumerable<Order> orders = await connection.QueryAsync<Order>(query, new { pageSize, offset, userId });

            return orders.ToList();
        }
        catch (Exception e)
        {
            Log.Error($"Error In OrdersRepository::GetUsersOrders {e.Message}");
            return [];
        }
    }
}