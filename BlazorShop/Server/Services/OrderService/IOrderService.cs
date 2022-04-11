namespace BlazorShop.Server.Services.OrderService
{
    public interface IOrderService
    {
        Task<ServiceResponse<bool>> PlaceOrder(int userID); //userID for it to work with the webhook from stripe and email
        Task<ServiceResponse<List<OrderOverviewResponseDto>>> GetOrders();
        Task<ServiceResponse<OrderDetailsResponseDto>> GetOrderDetails(int orderId);
    }
}
