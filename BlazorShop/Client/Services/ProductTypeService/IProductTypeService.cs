namespace BlazorShop.Client.Services.ProductTypeService
{
    public interface IProductTypeService
    {
        event Action OnChange;

        public List<ProductType> ProductTypes { get; set; }

        //The method
        Task GetProductTypes();
    }
}
