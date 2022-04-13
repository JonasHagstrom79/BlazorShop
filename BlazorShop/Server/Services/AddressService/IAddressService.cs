namespace BlazorShop.Server.Services.AddressService
{
    public interface IAddressService
    {
        //Get adress method
        Task<ServiceResponse<Address>> GetAddress();
        Task<ServiceResponse<Address>> AddOrUpdateAddress(Address address);
    }
}
