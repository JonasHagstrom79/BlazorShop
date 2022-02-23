namespace BlazorShop.Client.Services.CategorySerrvice
{
    public interface ICategoryService
    {
        //acess the category from the service
        List<Category> Categories{ get; set; }
        Task GetCategories();
    }
}
