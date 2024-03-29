﻿using Microsoft.AspNetCore.Authorization;
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

        [HttpGet("admin"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<List<Product>>>> GetAdminProductsAsync() 
        {
            //Get products from datacontext
            var result = await _productService.GetAdminProductsAsync();
            return Ok(result);
        }

        [HttpPost("admin"), Authorize(Roles = "Admin")] //Post for creating
        public async Task<ActionResult<ServiceResponse<Product>>> CreateProductAsync(Product product)
        {
            //Get products from datacontext
            var result = await _productService.CreateProduct(product);
            return Ok(result);
        }

        [HttpPut("admin"), Authorize(Roles = "Admin")] //Put is for update
        public async Task<ActionResult<ServiceResponse<Product>>> UpdateProductAsync(Product product)
        {
            //Get products from datacontext
            var result = await _productService.UpdateProduct(product);
            return Ok(result);
        }

        [HttpDelete("{id}"), Authorize(Roles = "Admin")] //id in the route also
        public async Task<ActionResult<ServiceResponse<bool>>> DeleteProductAsync(int id)
        {
            //Get products from datacontext
            var result = await _productService.DeleteProduct(id);
            return Ok(result);
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

        public async Task<ActionResult<ServiceResponse<Product>>> GetProduct(int productId) //adds from swagger, found one error here, not a list

        {
            //Get one product from server
            var result = await _productService.GetProductAsync(productId);
            return Ok(result);            
        }


        [HttpGet("category/{categoryUrl}")]

        public async Task<ActionResult<ServiceResponse<List<Product>>>> GetProductsByCategoryAsync(string categoryUrl) //adds from swagger
        {
            //Get one product from server
            var result = await _productService.GetProductsByCategoryAsync(categoryUrl);
            return Ok(result);
        }


        [HttpGet("search/{searchText}/{page}")]
        public async Task<ActionResult<ServiceResponse<ProductSearchResultDto>>> SearchProductsAsync(string searchText, int page = 1) //from search ADDED SERVICERESPONSE HERE
        {
            //Get one product from server
            var result = await _productService.SearchProductsAsync(searchText, page);
            return Ok(result);
        }
                  
        [HttpGet("searchsuggestions/{searchText}")]
        public async Task<ActionResult<ServiceResponse<List<Product>>>> GetProductSearchSuggestionsAsync(string searchText) //from searchSuggestion
        {
            //Get one product from server
            var result = await _productService.GetProductSearchSuggestionsAsync(searchText);
            return Ok(result);
        }

        [HttpGet("featured")]
        public async Task<ActionResult<ServiceResponse<List<Product>>>> GetFeaturdeProductsAsync() //from searchSuggestion
        {
            //Get featured products from server
            var result = await _productService.GetFeaturdeProductsAsync();
            return Ok(result);
        }

    }
}
