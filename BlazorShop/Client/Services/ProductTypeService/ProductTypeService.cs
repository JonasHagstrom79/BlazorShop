namespace BlazorShop.Client.Services.ProductTypeService
{
    public class ProductTypeService : IProductTypeService
    {
        private readonly HttpClient _http;

        public List<ProductType> ProductTypes { get; set; } = new List<ProductType>();

        public event Action OnChange;

        public ProductTypeService(HttpClient http)
        {
            _http = http;
        }

        public async Task GetProductTyes()
        {
            var result = await _http
                .GetFromJsonAsync<ServiceResponse<List<ProductType>>>("api/producttype");//from our product-type controller url="api/producttype"
            ProductTypes = result.Data;
        }
    }
}
