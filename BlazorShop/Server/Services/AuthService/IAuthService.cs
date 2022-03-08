namespace BlazorShop.Server.Services.AuthService
{
    public interface IAuthService
    {
        //registry method
        Task<ServiceResponse<int>> Register(User user, string password);
        //if user already exists
        Task<bool> UserExists(string email);
    }
}
