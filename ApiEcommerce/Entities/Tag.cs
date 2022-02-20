using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApiEcommerce.Entities
{
    public class Tag
    {
        public Tag()
        {
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        [Required] 
        public long Id { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<TagImg> TagImgs { get; set; }
        public ICollection<ProductTag> ProductTags { get; set; }
    }
}
