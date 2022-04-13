using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlazorShop.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //Empty API conroller
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        //Webservice call
        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<Category>>>> GetCategoriesAsync()
        {
            var result = await _categoryService.GetCategoriesAsync();
            return Ok(result);
        }

        [HttpGet("admin"), Authorize(Roles = "Admin")]//Important with route
        public async Task<ActionResult<ServiceResponse<List<Category>>>> GetAdminCategories()
        {
            var result = await _categoryService.GetAdminCategories();
            return Ok(result);
        }

        [HttpDelete("admin/{id}"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<List<Category>>>> DeleteCategory(int id)
        {
            var result = await _categoryService.DeleteCategory(id);
            return Ok(result);
        }

        [HttpPost("admin"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<List<Category>>>> AddCategory(Category category)
        {
            var result = await _categoryService.AddCategory(category);
            return Ok(result);
        }

        [HttpPut("admin"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<List<Category>>>> UpdateCategory(Category category)
        {
            var result = await _categoryService.UpdateCategory(category);
            return Ok(result);
        }
    }
}
