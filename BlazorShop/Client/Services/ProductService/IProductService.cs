namespace BlazorShop.Client.Services.ProductService
{
    public interface IProductService
    {
        event Action ProductsChanged; //add event listerner
        //The productlist itself
        List<Product> Products { get; set; }
        public string Message { get; set; } //To display search result
        public int CurrentPage { get; set; }
        public int PageCount { get; set; }
        string LastSearchText { get; set; }
        Task GetProducts(string? categoryUrl = null); //If no category url are given display all products
        Task<ServiceResponse<Product>> GetProduct(int productId);
        Task SearchProducts(string searchText, int page); //NOT async in name
        Task<List<string>> GetProductSearchSuggestions(string searchText); //NOT async in name
    }
}
