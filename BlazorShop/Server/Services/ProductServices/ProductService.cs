namespace BlazorShop.Server.Services.ProductServices
{
    public class ProductService : IProductService
    {
        private readonly DataContext _context;

        public ProductService(DataContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets one Product from DB
        /// </summary>
        /// <param name="productID"></param>
        /// <returns>Product</returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ServiceResponse<Product>> GetProductAsync(int productID)
        {
            var response = new ServiceResponse<Product>();
            var product = await _context.Products.FindAsync(productID);
            if (product == null) 
            {
                response.Success = false;
                response.Message = "Sorry, this product does not exist";
            }
            else
            {
                response.Data = product;
            }
            return response;
        }

        /// <summary>
        /// Get the Products from DB
        /// </summary>
        /// <returns>List of Products</returns>
        public async Task<ServiceResponse<List<Product>>> GetProductsAsync()
        {
            var response = new ServiceResponse<List<Product>>
            {
                Data = await _context.Products.ToListAsync()
            };
            return response;
        }

        public async Task<ServiceResponse<List<Product>>> GetProductsByCategoryAsync(string categoryUrl)
        {
            var response = new ServiceResponse<List<Product>>
            {
                Data = await _context.Products
                    .Where(p => p.Category.Url.ToLower().Equals(categoryUrl.ToLower())) //Filter the products
                    .ToListAsync()
            };
            return response;
        }
    }
}
