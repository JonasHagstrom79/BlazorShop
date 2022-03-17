using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlazorShop.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        //Access to the orderservice
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<bool>>> PlaceOrder() 
        {
            var result = await _orderService.PlaceOrder();
            return Ok(result);
        }

        [HttpGet]//default get method
        public async Task<ActionResult<ServiceResponse<List<OrderOverviewResponseDto>>>> GetOrders() 
        {
            var result = await _orderService.GetOrders();
            return Ok(result);
        }
    }
}
