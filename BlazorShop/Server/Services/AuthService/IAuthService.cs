namespace BlazorShop.Server.Services.AuthService
{
    public interface IAuthService
    {
        //registry method
        Task<ServiceResponse<int>> Register(User user, string password);
        //if user already exists
        Task<bool> UserExists(string email);
        Task<ServiceResponse<string>> Login(string email, string password); //the Json webtoken later will be a string
        Task<ServiceResponse<bool>> ChangePassword(int userId, string newPassword);
    }
}
