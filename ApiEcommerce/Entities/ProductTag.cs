using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiEcommerce.Entities
{
    public class ProductTag
    {
        public long ProductId { get; set; }
        public Product Product { get; set; }
        public long TagId { get; set; }
        public Tag Tag { get; set; }
    }
}
