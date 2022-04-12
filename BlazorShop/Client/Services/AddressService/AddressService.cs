namespace BlazorShop.Client.Services.AddressService
{
    public class AddressService : IAddressService
    {
        private readonly HttpClient _http;

        public AddressService(HttpClient http)
        {
            _http = http;
        }
        public async Task<Address> AddOrUpdateAddress(Address address)
        {
            var response = await _http.PostAsJsonAsync("api/address", address); //With the addres sas anobject
            return response.Content
                .ReadFromJsonAsync<ServiceResponse<Address>>().Result.Data; //We get the result and the data
        }

        public async Task<Address> GetAddress()
        {
            var response = await _http
                .GetFromJsonAsync<ServiceResponse<Address>>("api/address"); //url is api/address
            return response.Data;
        }
    }
}
