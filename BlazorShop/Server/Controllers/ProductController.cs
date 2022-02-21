using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlazorShop.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly DataContext _context;

        public ProductController(DataContext context)//dependency injection, made global
        {
            _context = context;
        }
        [HttpGet]        
        public async Task<ActionResult<List<Product>>> GetProduct() //adds from swagger
        {
            //Get products from datacontext
            var products = await _context.Products.ToListAsync(); 
            return Ok(products);
            //Ok är 200, går att returnera endra saker vid olika responses, coolt
        }
    }
}
