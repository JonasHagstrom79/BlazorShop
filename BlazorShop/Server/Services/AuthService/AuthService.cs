using System.Security.Cryptography;

namespace BlazorShop.Server.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly DataContext _context;

        public AuthService(DataContext context)
        {
            _context = context;
        }

        public async Task<ServiceResponse<string>> Login(string email, string password)
        {
            var response = new ServiceResponse<string>();
            //check if the user exists
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email.ToLower().Equals(email.ToLower()));
            if (user == null)
            {
                response.Success = false;
                response.Message = "User not found";
            }
            else if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                response.Success = false;
                response.Message = "Wrong password";
            }    
            else
            {
                response.Data = "token"; //the token from secrets.json
            }                              
            return response;
        }

        public async Task<ServiceResponse<int>> Register(User user, string password)
        {
            //check if the user already exists
            if (await UserExists(user.Email))
            {
                return new ServiceResponse<int> // // //
                { 
                    Success = false, 
                    Message = "User already exists" 
                };
            }
            CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            //now we add the new user
            _context.Users.Add(user);
            //we did a change to the database tabel, so neet to call SaveChanges()
            await _context.SaveChangesAsync();

            return new ServiceResponse<int> 
            { 
                Data = user.Id, 
                Message ="Registration successful" 
            };
        }

        public async Task<bool> UserExists(string email)
        {
            if (await _context.Users.AnyAsync(user => user.Email.ToLower()
                    .Equals(email.ToLower())))
            {
                return true;
            }
            return false;
        }

        //To create the password hash
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt) 
        {
            //uses a cryptography algorithm
            using(var hmac = new HMACSHA512()) 
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt) 
        {
            using (var hmac = new HMACSHA512(passwordSalt)) 
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash); //SequenceEqual instead of a for loop
            }
        }
    }
}
