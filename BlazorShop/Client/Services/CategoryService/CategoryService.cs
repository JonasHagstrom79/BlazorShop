namespace BlazorShop.Client.Services.CategoryService
{
    public class CategoryService : ICategoryService
    {
        #region Properties
        public List<Category> categories { get; set; }
        private readonly HttpClient _http;
        #endregion

        #region Constructor
        public CategoryService(HttpClient http) //For the call
        {
            _http = http;
        }
        #endregion
                
        /// <summary>
        /// Get a List of categories from the DB to the client
        /// </summary>
        /// <returns>List of categories</returns>
        public async Task GetCategoriesAsync()
        {
            var response = await _http.GetFromJsonAsync<ServiceResponse<List<Category>>>("api/category");
            if (response != null && response.Data != null)
            {
                categories = response.Data;
            }            
        }
    }
}
