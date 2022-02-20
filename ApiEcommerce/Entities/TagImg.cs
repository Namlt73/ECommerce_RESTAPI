using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiEcommerce.Entities
{
    public class TagImg : FileUpload
    {
        public long TagId { get; set; }
        public Tag Tag { get; set; }
    }
}
