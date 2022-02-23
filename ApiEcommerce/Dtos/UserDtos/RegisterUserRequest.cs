using ApiEcommerce.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ApiEcommerce.Dtos.UserDtos
{
    public class RegisterUserRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [JsonProperty(PropertyName = "username")]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.",
            MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [JsonProperty(PropertyName = "confirm_password")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password not match.")]
        public string ConfirmPassword { get; set; }


        [Display(Name = "User Role")]
        public List<SelectListItem> UserRoles { get; set; }
        public Role Role { get; set; }
        public string RoleName { get; set; }
    }
}
