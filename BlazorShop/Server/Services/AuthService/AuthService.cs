using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace BlazorShop.Server.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor; //access values from the webpage

        public AuthService(DataContext context, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)//also inject IConfiguration for the token-secret
        {
            _context = context;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Gets the userID from httpContextAccessor
        /// </summary>
        /// <returns>int</returns>
        public int GetUserId() => int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
        /// <summary>
        /// Gets the userEmail from httpContextAccessor
        /// </summary>
        /// <returns>string</returns>
        public string GetUserEmail() => _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);

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
                response.Data = CreateToken(user); //the token from secrets.json
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

        private string CreateToken(User user) 
        {
            List<Claim> claims = new List<Claim> //Store in the Json webtoken
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8
                .GetBytes(_configuration.GetSection("AppSettings:Token").Value));
            //Use key to create a new instance of sign in credentials
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            //create the token
            var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddDays(1),//Valid one day
                    signingCredentials: creds);
            //Our Json webtoken
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }

        public async Task<ServiceResponse<bool>> ChangePassword(int userId, string newPassword)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return new ServiceResponse<bool>
                {
                    Success = false,
                    Message = "User not found"
                };
            }
            CreatePasswordHash(newPassword, out byte[] passwordHash, out byte[] passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await _context.SaveChangesAsync();

            return new ServiceResponse<bool> { Data = true, Message = "Password has been changed."};
        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email.Equals(email));
        }
    }
}
