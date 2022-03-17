namespace BlazorShop.Client.Services.OrderService
{
    public interface IOrderService
    {
        Task PlaceOrder();
        Task<List<OrderOverviewResponseDto>> GetOrders();
    }
}
