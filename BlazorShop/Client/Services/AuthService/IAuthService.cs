namespace BlazorShop.Client.Services.AuthService
{
    public interface IAuthService
    {
        Task<ServiceResponse<int>> Register(UserRegister request);
        Task<ServiceResponse<string>> Login(UserLogin request);
        Task<ServiceResponse<bool>> ChangePassword(UserChangePassword request); //using our UserChangePassword-model
        Task<bool> IsUserAuthenticated();
    }
}
