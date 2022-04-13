namespace BlazorShop.Server.Services.ProductServices
{
    public interface IProductService
    {
        Task<ServiceResponse<List<Product>>> GetProductsAsync();
        Task<ServiceResponse<Product>> GetProductAsync(int productID);
        Task<ServiceResponse<List<Product>>> GetProductsByCategoryAsync(string categoryUrl);
        Task<ServiceResponse<ProductSearchResultDto>> SearchProductsAsync(string searchText, int page);
        Task<ServiceResponse<List<string>>> GetProductSearchSuggestionsAsync(string searchText);
        Task<ServiceResponse<List<Product>>> GetFeaturdeProductsAsync();
        Task<ServiceResponse<List<Product>>> GetAdminProductsAsync();
        Task<ServiceResponse<Product>> CreateProduct(Product product);
        Task<ServiceResponse<Product>> UpdateProduct(Product product);
        Task<ServiceResponse<bool>> DeleteProduct(int productId);
    }
}
