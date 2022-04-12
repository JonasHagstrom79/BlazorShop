namespace BlazorShop.Client.Services.CategorySerrvice
{
    public interface ICategoryService
    {
        //Onchange event
        event Action OnChange;

        //acess the category from the service
        List<Category> Categories{ get; set; }
        List<Category> AdminCategories { get; set; }
        Task GetCategories();

        Task GetAdminCategories();
        Task AddCategory(Category category);
        Task UpdateCategory(Category category);
        Task DeleteCategory(int categoryId);
        Category CreateNewCategory(); //For the client
    }
}
