namespace BlazorShop.Server.Services.ProductServices
{
    public interface IProductService
    {
        Task<ServiceResponse<List<Product>>> GetProductsAsync();
        Task<ServiceResponse<Product>> GetProductAsync(int productID);
        Task<ServiceResponse<List<Product>>> GetProductsByCategoryAsync(string categoryUrl);
        Task<ServiceResponse<List<Product>>> SearchProductsAsync(string searchText);
        Task<ServiceResponse<List<string>>> GetProductSearchSuggestionsAsync(string searchText);
        Task<ServiceResponse<List<Product>>> GetFeaturdeProductsAsync();
    }
}
