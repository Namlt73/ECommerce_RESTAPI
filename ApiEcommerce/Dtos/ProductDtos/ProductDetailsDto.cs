using ApiEcommerce.Dtos.CategoryDtos;
using ApiEcommerce.Dtos.Handles;
using ApiEcommerce.Dtos.TagDtos;
using ApiEcommerce.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiEcommerce.Dtos.ProductDtos
{
    public class ProductDetailsDto : SuccessResponse
    {
        public long Id { get; set; }
        public string Slug { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime ModifiedAt { get; set; }
        public DateTime PublishedAt { get; set; }
        public IEnumerable<string> Categories { get; set; }
        public List<string> Tags { get; set; }

        public IEnumerable<string> ImageUrls { get; set; }
        public IEnumerable<Comment> Comments { get; set; }

        public static ProductDetailsDto Build(Product product)
        {
            var comments = new List<Comment>();
            if (product.Comments != null)
            {
                foreach (var comment in product.Comments)
                {
                    comments.Add(comment);
                }
            }

            return new ProductDetailsDto
            {
                Id = product.Id,
                Name = product.Name,
                Slug = product.Slug,
                Description = product.Description,
                PublishedAt = product.PublishAt,
                Comments = comments,
                Tags = TagOnlyNameDto.BuildAsStringList(product.ProductTags),
                Categories = CategoryOnlyNameDto.BuildAsStringList(product.ProductCategories),
                ImageUrls = product.ProductImages.Select(pi => pi.FilePath)
            };
        }
    }
}
