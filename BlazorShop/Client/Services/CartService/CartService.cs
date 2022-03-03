using Blazored.LocalStorage;

namespace BlazorShop.Client.Services.CartService
{
    public class CartService : ICartService
    {
        private readonly ILocalStorageService _localStorage;
        private readonly HttpClient _http;

        public CartService(ILocalStorageService localStorage, HttpClient http) //we need the localstorage
        {
            _localStorage = localStorage;
            _http = http;
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

        public async Task<List<CartProductResponseDto>> GetCartProducts() //Need to inject the Http-client in the constructor also
        {
            var cartItems = await _localStorage.GetItemAsync<List<CartItem>>("cart");
            var response = await _http.PostAsJsonAsync("api/cart/products", cartItems); //second is the obkject to send through the body
            var cartProducts = 
                await response.Content.ReadFromJsonAsync<ServiceResponse<List<CartProductResponseDto>>>(); //var response is a http message so need to read from json
            return cartProducts.Data;
        }

        public async Task RemoveProductFromCart(int productId, int productTypeId)
        {
            //get our cart
            var cart = await _localStorage.GetItemAsync<List<CartItem>>("cart");
            if (cart == null)
            {
                return;
            }
            //The item we want to remove, finds with productId && productTypeId
            var cartItem = cart.Find(x => x.ProductId == productId
                && x.ProductTypeId == productTypeId);
            if (cartItem != null)
            {
                cart.Remove(cartItem);
                //after we remove it we set the item again
                await _localStorage.SetItemAsync("cart", cart);
                OnChange.Invoke();//cart counter will re-render itself
            }



        }
    }
}
