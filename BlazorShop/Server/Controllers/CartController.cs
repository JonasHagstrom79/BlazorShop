using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BlazorShop.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)//add ctor were we inject the ICartService
        {
            _cartService = cartService;
        }

        //First Post-Method
        [HttpPost("products")] //api/cart/products, we want to turn our cart items to products
        public async Task<ActionResult<ServiceResponse<List<CartProductResponseDto>>>> GetCartProducts(List<CartItem> cartItems)
        {
            var result = await _cartService.GetCartProducts(cartItems);
            return Ok(result);
        }

        [HttpPost] //default post of the controller
        public async Task<ActionResult<ServiceResponse<List<CartProductResponseDto>>>> StoreCartItems(List<CartItem> cartItems)
        {            
            var result = await _cartService.StoreCartItems(cartItems);//send the userId to the service
            return Ok(result);
        }

        [HttpGet("count")]
        public async Task<ActionResult<ServiceResponse<int>>> GetCartItemsCount() 
        { 
            return await _cartService.GetCartItemsCount();
        }

        [HttpGet]//Default get method
        public async Task<ActionResult<ServiceResponse<List<CartProductResponseDto>>>>GetDbCartProducts()
        {
            var result = await _cartService.GetDbCartProducts();
            return Ok(result);
        }

        [HttpPost("add")]
        public async Task<ActionResult<ServiceResponse<bool>>>AddToCart(CartItem cartItems)
        {            
            var result = await _cartService.AddToCart(cartItems);
            return Ok(result);
        }

        [HttpPut("update-quantity")] //giving it a route, put instead of post because it is not adding something
        public async Task<ActionResult<ServiceResponse<bool>>>UpdateQuantity(CartItem cartItems)
        {
            var result = await _cartService.UpdateQuantity(cartItems);
            return Ok(result);
        }

        [HttpDelete("{productId}/{productTypeId}")] //Delete and add a route where we set the parameters
        public async Task<ActionResult<ServiceResponse<bool>>> RemoveItemFromCart(int productId, int productTypeId)
        {
            var result = await _cartService.RemoveItemFromCart(productId, productTypeId);
            return Ok(result);
        }
    }
}
