namespace BlazorShop.Client.Services.CategoryService
{
    public interface ICategoryService
    {
        //acess from the sevices
        List<Category> categories { get; set; }
        Task GetCategoriesAsync();
    }
}
