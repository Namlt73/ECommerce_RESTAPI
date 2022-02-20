using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiEcommerce.Entities
{
    public class ProductCategory
    {
        public long ProductId { get; set; }
        public Product Product { get; set; }
        public long CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
