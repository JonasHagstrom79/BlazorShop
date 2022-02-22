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

        public event Action ProductsChanged;


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
        public async Task GetProductsAsync(string? categoryUrl = null) //for the event listener
        {
            var result = categoryUrl == null ?
                await _http.GetFromJsonAsync<ServiceResponse<List<Product>>>("api/product") : //If the call is null we use this line
                await _http.GetFromJsonAsync<ServiceResponse<List<Product>>>($"api/product/category/{categoryUrl}"); //If the call is not null
            if (result != null && result.Data != null)
            {
                Products = result.Data;
            }
            //Invoke the event at the end
            ProductsChanged.Invoke(); //Everythin subscribed to the event knows that smthing has changed
        }        
    }
}
