using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlazorShop.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {        
        private readonly IProductService _productService;

        public ProductController(IProductService productService)//dependency injection, made global
        {            
            _productService = productService;
        }
        [HttpGet]        
        public async Task<ActionResult<ServiceResponse<List<Product>>>> GetProducts() //adds from swagger
        {
            //Get products from datacontext
            var result = await _productService.GetProductAsync();
            return Ok(result);
            //Ok är 200, går att returnera endra saker vid olika responses, coolt
        }
    }
}
