namespace BlazorShop.Server.Services.CategoryServices
{
    public class CategoryService : ICategoryService
    {
        private readonly DataContext _context; 

        public CategoryService(DataContext context)
        {
            _context = context;
        }
        public async Task<ServiceResponse<List<Category>>> GetCategoriesAsync()
        {
            //To get acess to our categories
            var categories = await _context.Categories.ToListAsync();
            return new ServiceResponse<List<Category>>
            {
                Data = categories
            };
        }
    }
}
