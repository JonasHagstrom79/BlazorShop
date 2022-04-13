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

        public async Task GetProductTypes()
        {
            var result = await _http
                .GetFromJsonAsync<ServiceResponse<List<ProductType>>>("api/producttype");//from our product-type controller url="api/producttype"
            ProductTypes = result.Data;
        }

        public async Task AddProductType(ProductType productType)
        {
            var response = await _http.PostAsJsonAsync("api/producttype", productType); //With the product type object(productType)
            ProductTypes = (await response.Content
                .ReadFromJsonAsync<ServiceResponse<List<ProductType>>>()).Data;
            OnChange.Invoke();
        }

        public async Task UpdateProductType(ProductType productType)
        {
            var response = await _http.PutAsJsonAsync("api/producttype", productType); //With the product type object(productType) PUTasJsonAsyn also
            ProductTypes = (await response.Content
                .ReadFromJsonAsync<ServiceResponse<List<ProductType>>>()).Data;
            OnChange.Invoke();
        }

        public ProductType CreateNewProductType() //Only happenon the client
        {
            var newProductType = new ProductType { IsNew = true, Editing = true }; //To be able to edit

            ProductTypes.Add(newProductType);
            OnChange.Invoke();
            return newProductType;
        }
    }
}
