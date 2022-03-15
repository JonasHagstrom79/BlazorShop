namespace BlazorShop.Client.Services.CartService
{
    public interface ICartService
    {
        //Add event similar to the one in products
        event Action OnChange; //whenever something changes in the cart this would be the event we would invoke
        Task AddToCart(CartItem cartItem);  //Method to add something to the cart
        Task<List<CartItem>> GetCartItems(); //recieves all the cartitems
        Task<List<CartProductResponseDto>> GetCartProducts(); //Get the products of the cart
        Task RemoveProductFromCart(int productId, int productTypeId);
        Task UpdateQuantity(CartProductResponseDto product);
        Task StoreCartItems(bool emtyLocalCart);
        Task GetCartItemsCount();
    }
}
