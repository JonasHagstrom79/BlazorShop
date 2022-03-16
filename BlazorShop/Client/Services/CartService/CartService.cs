using Blazored.LocalStorage;

namespace BlazorShop.Client.Services.CartService
{
    public class CartService : ICartService
    {
        private readonly ILocalStorageService _localStorage;
        private readonly HttpClient _http;
        private readonly AuthenticationStateProvider _authStateProvider;

        public CartService(ILocalStorageService localStorage, HttpClient http, AuthenticationStateProvider authStateProvider) //we need the localstorage, and to know if the user is authenticated
        {
            _localStorage = localStorage;
            _http = http;
            _authStateProvider = authStateProvider;
        }
        
        public event Action OnChange;


        public async Task AddToCart(CartItem cartItem)//If the user is not auth=items from localstorage, If aut = get the items from the database
        {
            if (await IsUserAuthenticated())
            {
                await _http.PostAsJsonAsync("api/cart/add", cartItem); //postst with the "add" from AddToCart+add from server(Cartcontroller)
            }
            else
            {
                var cart = await _localStorage.GetItemAsync<List<CartItem>>("cart");
                if (cart == null) //if no cart creates a new
                {
                    cart = new List<CartItem>(); //initilaze
                }
                //Checks if the same item already exists in the cart
                var sameItem = cart.Find(x => x.ProductId == cartItem.ProductId &&
                    x.ProductTypeId == cartItem.ProductTypeId);
                if (sameItem == null)
                {
                    cart.Add(cartItem);
                }
                else
                {
                    sameItem.Quantity += cartItem.Quantity;
                }
                //sets the cart
                await _localStorage.SetItemAsync("cart", cart);
            }            
            //to update the cart
            await GetCartItemsCount();
        }        
              
        public async Task GetCartItemsCount()
        {
            if (await IsUserAuthenticated())
            {
                var result = await _http.GetFromJsonAsync<ServiceResponse<int>> ("api/cart/count");
                var count = result.Data;
                //Set the count to the localstorage
                await _localStorage.SetItemAsync<int>("cartItemsCount", count);
            }
            else
            {
                var cart = await _localStorage.GetItemAsync<List<CartItem>>("cart");
                await _localStorage.SetItemAsync<int>("cartItemsCount", cart != null ? cart.Count : 0 ); //The value frpm localstorage, NOT the database. if not null sets to cart.count, otherwise to 0
            }
            OnChange.Invoke();
        }

        public async Task<List<CartProductResponseDto>> GetCartProducts() //Need to inject the Http-client in the constructor also
        {
            //Check if the user is authenticated
            if (await IsUserAuthenticated())
            {
                var response = await _http.GetFromJsonAsync<ServiceResponse<List<CartProductResponseDto>>>("api/cart"); //second is the obkject to send through the body
                return response.Data;
            }
            else
            {
                var cartItems = await _localStorage.GetItemAsync<List<CartItem>>("cart");
                if (cartItems == null)
                {
                    return new List<CartProductResponseDto>();
                }
                var response = await _http.PostAsJsonAsync("api/cart/products", cartItems); //second is the obkject to send through the body
                var cartProducts =
                    await response.Content.ReadFromJsonAsync<ServiceResponse<List<CartProductResponseDto>>>(); //var response is a http message so need to read from json
                return cartProducts.Data;
            }
            
        }

        public async Task RemoveProductFromCart(int productId, int productTypeId)
        {
            //check if the user is authenticated
            if (await IsUserAuthenticated())
            {
                //make our call, with a route of parameters
                await _http.DeleteAsync($"api/cart/{productId}/{productTypeId}");
            }
            else
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
                }
            }            
        }

        public async Task StoreCartItems(bool emtyLocalCart = false)
        {
            //get the local cart
            var localCart = await _localStorage.GetItemAsync<List<CartItem>>("cart");
            if (localCart == null)
            {
                return;
            }
            //with that information we store all the cartitems in the database
            await _http.PostAsJsonAsync("api/cart", localCart);
            //if we want to empty the localcart
            if (emtyLocalCart)
            {
                await _localStorage.RemoveItemAsync("cart");
            }
        }

        public async Task UpdateQuantity(CartProductResponseDto product)
        {
            //checks if user is aut.
            if (await IsUserAuthenticated())
            {
                //creates a new request object(our cart item)
                var request = new CartItem
                {
                    ProductId = product.ProductId,
                    Quantity = product.Quantity,
                    ProductTypeId = product.ProductTypeId
                };
                //make the call with the route from CartController on the server([HttpPut("update-quantity")], UpateQuantiy
                await _http.PutAsJsonAsync("api/cart/update-quantity", request); //Sending our request item
            }
            else
            {
                var cart = await _localStorage.GetItemAsync<List<CartItem>>("cart");
                if (cart == null)
                {
                    return;
                }
                //The item we want to update, finds with productId && productTypeId
                var cartItem = cart.Find(x => x.ProductId == product.ProductId
                    && x.ProductTypeId == product.ProductTypeId);
                if (cartItem != null)
                {
                    cartItem.Quantity = product.Quantity;
                    //after we update it we set the item again
                    await _localStorage.SetItemAsync("cart", cart);
                }
            }            
        }

        private async Task<bool> IsUserAuthenticated()
        {
            return (await _authStateProvider.GetAuthenticationStateAsync()).User.Identity.IsAuthenticated;
        }
    }
}
