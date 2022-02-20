using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiEcommerce.Entities
{
    public class ProductImg : FileUpload
    {
        public long ProductId { get; set; }
        public Product Product { get; set; }
    }
}
