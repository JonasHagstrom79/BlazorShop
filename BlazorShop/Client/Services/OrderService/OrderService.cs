using Microsoft.AspNetCore.Components;

namespace BlazorShop.Client.Services.OrderService
{
    public class OrderService : IOrderService
    {
        private readonly HttpClient _http;
        private readonly AuthenticationStateProvider _authStateProvider;
        private readonly NavigationManager _navigationManager;

        //Need the http, auth and navigation 
        public OrderService(HttpClient http, AuthenticationStateProvider AuthStateProvider, NavigationManager navigationManager)
        {
            _http = http;
            _authStateProvider = AuthStateProvider;
            _navigationManager = navigationManager;
        }

        public async Task<OrderDetailsResponseDto> GetOrderDetails(int orderId)
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<OrderDetailsResponseDto>>($"api/order/{orderId}"); //With route
            return result.Data;
        }

        public async Task<List<OrderOverviewResponseDto>> GetOrders()
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<List<OrderOverviewResponseDto>>>("api/order");
            return result.Data;
        }

        public async Task<string> PlaceOrder()
        {
            if (await IsUserAuthenticated())
            {
                var result = await _http.PostAsync("api/payment/checkout", null); //payment-controller, route checkout
                var url = await result.Content.ReadAsStringAsync();
                return url;
            }
            else
            {
                return "login";
            }
        }

        private async Task<bool> IsUserAuthenticated()
        {
            return (await _authStateProvider.GetAuthenticationStateAsync()).User.Identity.IsAuthenticated;
        }
    }
}
