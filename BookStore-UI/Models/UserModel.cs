using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore_UI.Models
{
    public class LoginModel
    {
    }
    public class RegistrationModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(15, ErrorMessage = "Your password is limited {0} to {0} characters.", MinimumLength = 6)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmed Password")]
        [Compare("Password", ErrorMessage = "The password are not match.")]
        public string ConfirnedPassword { get; set; }
    }
}
