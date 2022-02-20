using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiEcommerce.Entities
{
    public class UserImg
    {
        public long UserId;
        public User User { get; set; }
    }
}
