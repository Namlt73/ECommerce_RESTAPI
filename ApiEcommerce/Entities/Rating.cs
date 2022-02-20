using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApiEcommerce.Entities
{
    public class Rating
    {
        public long Id { get; set; }
        public DateTime CreatedAt { get; set; }
        [Display(Name = "Rating")] 
        public long Value { get; set; }

        public long UserId { get; set; }
        public User User { get; set; }
        public long ProductId { get; set; }
        public Product Product { get; set; }
        public long CommentId { get; set; }
        public Comment Comment { get; set; }
    }
}
