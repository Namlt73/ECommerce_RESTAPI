using ApiEcommerce.Dtos.Handles;
using ApiEcommerce.Dtos.ProductDtos;
using ApiEcommerce.Dtos.UserDtos;
using ApiEcommerce.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiEcommerce.Dtos.CommentDtos
{
    public class CommentDetailsDto : SuccessResponse
    {
        public long Id { get; set; }
        public ProductBasicInfor Product { get; set; }
        public string Content { get; set; }
        public int? Rating { get; set; }

        public UserBasicInfor User { get; set; }
        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public static CommentDetailsDto Build(Comment comment, bool includeProduct = false,
            bool includeUser = false)
        {
            var dto = new CommentDetailsDto
            {
                Id = comment.Id,
                Content = comment.Content,
                Rating = comment.Rating,
                CreatedAt = comment.CreatedAt,
                UpdatedAt = comment.UpdatedAt
            };

            if (includeProduct)
                dto.Product = new ProductBasicInfor
                {
                    Id = comment.ProductId,
                    Name = comment.Product.Name,
                    Slug = comment.Product.Slug,
                };
            if (includeUser)
                dto.User = UserBasicInfor.Build(comment.User);

            return dto;
        }
    }
}
