using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiEcommerce.Dtos.CommentDtos
{
    public class CommentDto
    {
        public string Content { get; set; }
        public int? Rating { get; set; }
    }
}
