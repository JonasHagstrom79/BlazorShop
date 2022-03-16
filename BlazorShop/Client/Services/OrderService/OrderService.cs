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

        public async Task PlaceOrder()
        {
            if (await IsUserAuthenticated())
            {
                await _http.PostAsync("api/order", null);
            }
            else
            {
                _navigationManager.NavigateTo("login");//Navigate to loginpage
            }
        }

        private async Task<bool> IsUserAuthenticated()
        {
            return (await _authStateProvider.GetAuthenticationStateAsync()).User.Identity.IsAuthenticated;
        }
    }
}
