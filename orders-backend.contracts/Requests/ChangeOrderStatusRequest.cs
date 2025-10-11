namespace OrdersBackend.Contracts.Requests;

public record ChangeOrderStatusRequest(int OrderId, int StatusId);