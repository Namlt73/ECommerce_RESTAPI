using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

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
