using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiEcommerce.Entities
{
    public class Comment
    {
        public Comment()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }
        public long Id { get; set; }
        public string Content { get; set; }
        public int? Rating { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public long ProductId { get; set; }
        public Product Product { get; set; }


        public virtual HashSet<Comment> Replies { get; set; }
        public long UserId { get; set; }
        public User User { get; set; }
        public long RatingId { get; set; }

        public string RenderContent()
        {
            throw new NotImplementedException();
        }
    }
}
