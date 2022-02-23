using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace ApiEcommerce.Dtos.UserDtos
{
    public class LoginRequest
    {
        [Required]
        [JsonProperty(PropertyName = "username")]
        public string UserName { get; set; }

        [Required]
        [JsonProperty(PropertyName = "password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
