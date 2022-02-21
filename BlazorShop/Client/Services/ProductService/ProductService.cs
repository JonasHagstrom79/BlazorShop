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

        /// <summary>
        /// Get the products from the Database
        /// </summary>
        /// <returns>Products</returns>
        public async Task GetProducts()
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<List<Product>>>("api/product");
            if (result != null && result.Data != null)
            {
                Products = result.Data;
            }            
        }
    }
}
