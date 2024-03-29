﻿namespace BlazorShop.Server.Services.AuthService
{
    public interface IAuthService
    {
        //registry method
        Task<ServiceResponse<int>> Register(User user, string password);
        //if user already exists
        Task<bool> UserExists(string email);
        Task<ServiceResponse<string>> Login(string email, string password); //the Json webtoken later will be a string
        Task<ServiceResponse<bool>> ChangePassword(int userId, string newPassword);
        int GetUserId(); //gets the user id
        string GetUserEmail(); //MAYBE this is were it goes wrong
        Task<User> GetUserByEmail(string email);
    }
}
