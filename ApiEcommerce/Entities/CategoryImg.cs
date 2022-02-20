using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiEcommerce.Entities
{
    public class CategoryImg : FileUpload
    {
        public long CategoryId { get; set; }
        public Category Category { get; set; }       
    }
}
