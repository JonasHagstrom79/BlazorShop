namespace BlazorShop.Client.Services.CategorySerrvice
{
    public class CategoryService : ICategoryService
    {
        private readonly HttpClient _http;

        public List<Category> Categories { get; set; } = new List<Category>(); //Initilaze the categories as a new list

        public CategoryService(HttpClient http)
        {
            _http = http;
        }

        public async Task GetCategories()//Get the categories from serviceresponse JSon
        {
            var response = await _http.GetFromJsonAsync<ServiceResponse<List<Category>>>("api/Category");
            if (response != null && response.Data != null)
            {
                Categories = response.Data;
            }
            
        }
    }
}
