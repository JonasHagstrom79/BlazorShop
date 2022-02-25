namespace BlazorShop.Client.Services.ProductService
{
    public class ProductService : IProductService
    {
        private readonly HttpClient _http;

        //We want to make our http call
        public ProductService(HttpClient http)
        {
            _http = http;
        }
        public List<Product> Products { get; set; }
        public string Message { get; set; } = "Loading products...";

        public event Action ProductsChanged; //Event lisener

        public Task<ServiceResponse<Product>> GetProduct(int productId)
        {
            throw new NotImplementedException();
        }




        /// <summary>
        /// Gets a Product on the client
        /// </summary>
        /// <param name="productId"></param>
        /// <returns>Product</returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ServiceResponse<Product>> GetProductAsync(int productId)
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<Product>>($"api/product/{productId}"); //add a parameter to the string
            return result;
        }

        /// <summary>
        /// Get the products from the Database
        /// </summary>
        /// <returns>List Products</returns>

        public async Task GetProducts(string? categoryUrl = null)
        {
            var result = categoryUrl == null ? //use ternery operator 
                await _http.GetFromJsonAsync<ServiceResponse<List<Product>>>("api/product/featured") : //if null use this call(IproductService)
                await _http.GetFromJsonAsync<ServiceResponse<List<Product>>>($"api/product/category/{categoryUrl}"); //if not null use this(IproductService)

            if (result != null && result.Data != null)
            {
                Products = result.Data;
            }

            //Invoke event at the end, need to be or the site will crash
            ProductsChanged.Invoke();//Go to Index.razor
        }

        public async Task<List<string>> GetProductSearchSuggestions(string searchText)
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<List<string>>>($"api/product/searchsuggestions/{searchText}");
            return result.Data; //
        }

        public async Task SearchProducts(string searchText)
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<List<Product>>>($"api/product/search/{searchText}");
            if (result != null && result.Data != null)
            {
                Products = result.Data;
            }
            if (Products.Count == 0)
            {
                Message = "No products found.";
            }
            ProductsChanged?.Invoke(); //invokes the event handeler
        }

    }
}
