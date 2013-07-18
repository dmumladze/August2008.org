using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace August2008.Models
{
    public class RegisterUser
    {
        public int UserId { get; set; }

        [Required]
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public string SocialLink { get; set; }
        public string Gender { get; set; }
        public string AccessToken { get; set; }
        public string Provider { get; set; }
        public string ProviderId { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}