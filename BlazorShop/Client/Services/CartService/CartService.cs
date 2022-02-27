using Blazored.LocalStorage;

namespace BlazorShop.Client.Services.CartService
{
    public class CartService : ICartService
    {
        private readonly ILocalStorageService _localStorage;

        public CartService(ILocalStorageService localStorage) //we need the localstorage
        {
            _localStorage = localStorage;
        }
        
        public event Action OnChange;

        public async Task AddToCart(CartItem cartItem)
        {
            var cart = await _localStorage.GetItemAsync<List<CartItem>>("cart");
            if (cart == null) //if no cart creates a new
            {
                cart = new List<CartItem>(); //initilaze
            }
            cart.Add(cartItem);
            //sets the cart
            await _localStorage.SetItemAsync("cart", cart);
            //to update the cart
            OnChange.Invoke();
        }

        public async Task<List<CartItem>> GetCartItems()
        {
            var cart = await _localStorage.GetItemAsync<List<CartItem>>("cart");
            if (cart == null) //if no cart creates a new
            {
                cart = new List<CartItem>(); //initilaze
            }
            return cart;
        }
    }
}
