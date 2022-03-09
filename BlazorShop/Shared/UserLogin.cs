using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorShop.Shared
{
    public class UserLogin
    {
        //small class, only used for the request
        [Required] //for the validation form
        public string Email { get; set; } = string.Empty;
        [Required] //for the validation form
        public string Password { get; set; } = string.Empty;
    }
}
