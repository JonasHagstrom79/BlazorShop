﻿using Microsoft.AspNetCore.Http;
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
                
        [HttpGet]//default get method
        public async Task<ActionResult<ServiceResponse<List<OrderOverviewResponseDto>>>> GetOrders() 
        {
            var result = await _orderService.GetOrders();
            return Ok(result);
        }

        [HttpGet("{orderId}")]//route, we have api/orders/order an the orderId
        public async Task<ActionResult<ServiceResponse<OrderDetailsProductResponseDto>>> GetOrderDetails(int orderId)
        {
            var result = await _orderService.GetOrderDetails(orderId);
            return Ok(result);
        }
    }
}
