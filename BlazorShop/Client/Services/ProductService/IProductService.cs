namespace BlazorShop.Client.Services.ProductService
{
    public interface IProductService
    {        
        event Action ProductsChanged; //an event so that the components know what have changed
       
        List<Product> Products { get; set; }  //The productlist itself
        Task GetProductsAsync(string categoryUrl = null);
        Task<ServiceResponse<Product>> GetProductAsync(int productId);
    }
}
