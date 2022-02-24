namespace BlazorShop.Client.Services.ProductService
{
    public interface IProductService
    {
        event Action ProductsChanged; //add event listerner
        //The productlist itself
        List<Product> Products { get; set; }
        Task GetProducts(string? categoryUrl = null); //If no category url are given display all products
        Task<ServiceResponse<Product>> GetProduct(int productId);
    }
}
