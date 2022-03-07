using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorShop.Shared
{
    public class UserRegister
    {
        //Add data annotations, email is required for registration, also checks if its valid
        [Required, EmailAddress(ErrorMessage = "E-postfältet är inte en giltig e-postadress")]
        public string Email { get; set; } = string.Empty;
        [Required, StringLength(100, MinimumLength = 6, ErrorMessage = "Lösenordet måste innehålla minst 6 tecken")]
        public string Password { get; set; } = string.Empty;
        [Compare("Password", ErrorMessage = "The passwords do not match")]
        public string ComfirmPassword { get; set; } = string.Empty;
    }
}
