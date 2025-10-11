
namespace OrdersBackend.Api.Routes;

public static class ApiRoutes
{
    public static class OrdersController
    {
        public const string Base = "Orders";

        public const string Add = "Add";
        public const string Cancel = "Cancel";
        public const string GetUsersOrders = "GetUsersOrders/{userId}";
        public const string GetOrders = "GetOrders/{userId}";
        public const string ChangeOrderStatus = "ChangeOrderStatus";
    }

    public static class StatusController
    {
        public const string Base = "Status";

        public const string GetAll = "GetAll";
    }
}
