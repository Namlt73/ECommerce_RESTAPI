using ApiEcommerce.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiEcommerce.Dtos.UserDtos
{
    public class UserBasicInfor
    {
        [JsonProperty(PropertyName = "username")]
        public string UserName { get; set; }

        public long Id { get; set; }

        public static UserBasicInfor Build(User user)
        {
            return new UserBasicInfor
            {
                UserName = user.UserName,
                Id = user.Id
            };
        }
    }
}
