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
        public async Task<ActionResult<ServiceResponse<List<Product>>>> GetProductsAsync() //adds from swagger
        {
            //Get products from datacontext
            var result = await _productService.GetProductsAsync();
            return Ok(result);
            //Ok är 200, går att returnera endra saker vid olika responses, coolt
        }

        [HttpGet("{productId}")]//the [Route] added to the [HtttpGet]       
        public async Task<ActionResult<ServiceResponse<List<Product>>>> GetProductAsync(int productId) //adds from swagger
        {
            //Get one product from server
            var result = await _productService.GetProductAsync(productId);
            return Ok(result);            
        }

        [HttpGet("category/{categoryUrl}")] //To get the products from the server
        public async Task<ActionResult<ServiceResponse<List<Product>>>> GetProductsByCategoryAsync(string categoryUrl) //adds from swagger
        {
            //Get one product from server
            var result = await _productService.GetProductsByCategoryAsync(categoryUrl);
            return Ok(result);
        }
    }
}
