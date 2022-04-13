namespace BlazorShop.Server.Services.ProductServices
{
    public class ProductService : IProductService
    {
        private readonly DataContext _context;

        public ProductService(DataContext context)
        {
            _context = context;
        }

        public async Task<ServiceResponse<List<Product>>> GetFeaturdeProductsAsync()
        {
            var response = new ServiceResponse<List<Product>>
            {
                Data = await _context.Products
                    .Where(p => p.Featured && p.Visible && !p.Deleted) //Only visible and not deleted
                    .Include(p => p.Variants.Where(v => v.Visible && !v.Deleted))
                    .ToListAsync()
            };
            return response;
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
            var product = await _context.Products
                .Include(p => p.Variants.Where(v => v.Visible && !v.Deleted)) //For every product we want to add the varinats, if not we will have an empty list. Also only visible and not deleted varainte(v)
                .ThenInclude(v => v.ProductType) //for the variants we want to include the product type
                .FirstOrDefaultAsync(p => p.Id == productID && !p.Deleted && p.Visible);  //to find the proper single product, and not deleted products and visible
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
                Data = await _context.Products
                .Where(p => p.Visible && !p.Deleted) //visible and not deleted
                .Include(p => p.Variants.Where(v => v.Visible && !v.Deleted))
                .ToListAsync() //We dont need the product types here because we wont show them on the client
            };
            return response;
        }

        public async Task<ServiceResponse<List<Product>>> GetProductsByCategoryAsync(string categoryUrl)
        {
            var response = new ServiceResponse<List<Product>>
            {
                Data = await _context.Products
                    .Where(p => p.Category.Url.ToLower().Equals(categoryUrl.ToLower()) &&
                    p.Visible && !p.Deleted)//compare url and put them into a list
                    .Include(p => p.Variants.Where(v => v.Visible && !v.Deleted)) //Adds the variants
                    .ToListAsync()
            };
            return response;
        }

        /// <summary>
        /// Get every word from a seach
        /// </summary>
        /// <param name="searchText"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ServiceResponse<List<string>>> GetProductSearchSuggestionsAsync(string searchText)
        {
            var products = await FindProductsBySearchTextAsync(searchText);

            List<string> result = new List<string>();
            //For every product we find here we heck if the title contains our search term
            foreach (var product in products)
            {
                if(product.Title.Contains(searchText, StringComparison.OrdinalIgnoreCase)) //same as ToLower() comparison
                {
                    result.Add(product.Title); //Got the title
                }
                if (product.Description != null)
                {
                    var punctioation = product.Description.Where(char.IsPunctuation)
                        .Distinct().ToArray();
                    var words = product.Description.Split() //With the punctiation we get all the words of the description
                        .Select(s => s.Trim(punctioation)); //select everything and remove the punctioation

                    foreach (var word in words)
                    {
                        if (word.Contains(searchText, StringComparison.OrdinalIgnoreCase) //if any words contains the searchtext, add to result
                            && !result.Contains(word))
                        {
                            result.Add(word);
                        }
                    }
                }
            }
            return new ServiceResponse<List<string>> { Data = result };
        }

        /// <summary>
        /// Search title and description of Product
        /// </summary>
        /// <param name="searchText"></param>
        /// <returns></returns>
        public async Task<ServiceResponse<ProductSearchResultDto>> SearchProductsAsync(string searchText, int page) //Add to controller always!
        {
            //The number of results per page
            var pageResults = 2f;
            var pageCount = Math.Ceiling((await FindProductsBySearchTextAsync(searchText)).Count / pageResults);

            var products = await _context.Products
                                .Where(p => p.Title.ToLower().Contains(searchText.ToLower()) //Filter title
                                ||
                                    p.Description.ToLower().Contains(searchText.ToLower()) &&
                                    p.Visible && !p.Deleted) //Filter decription
                                .Include(p => p.Variants)
                                .Skip((page - 1) * (int)pageResults) //For each page we skip two products
                                .Take((int)pageResults) //from the result we take the next two
                                .ToListAsync();

            //title and description search
            var response = new ServiceResponse<ProductSearchResultDto>
            {
                Data = new ProductSearchResultDto
                {
                    Products = products,
                    CurrentPage = page,
                    Pages = (int)pageCount
                }
            };

            return response;
        }

        private async Task<List<Product>> FindProductsBySearchTextAsync(string searchText)
        {
            return await _context.Products
                                .Where(p => p.Title.ToLower().Contains(searchText.ToLower()) //Filter title
                                ||
                                    p.Description.ToLower().Contains(searchText.ToLower()) &&
                                    p.Visible && !p.Deleted) //Filter decription
                                .Include(p => p.Variants) //include the variants
                                .ToListAsync();
        }
    }
}
