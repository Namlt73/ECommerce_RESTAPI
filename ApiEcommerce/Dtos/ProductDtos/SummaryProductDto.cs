using ApiEcommerce.Dtos.CategoryDtos;
using ApiEcommerce.Dtos.TagDtos;
using ApiEcommerce.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiEcommerce.Dtos.ProductDtos
{
    public class SummaryProductDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public int Price { get; set; }
        public int Stock { get; set; }

        public int CommentsCount { get; set; }

        public List<string> Categories { get; set; }
        public IEnumerable<string> Tags { get; set; }

        public IEnumerable<string> ImageUrls { get; set; }

        public DateTime PublishAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public static SummaryProductDto Build(Product product)
        {
            return new SummaryProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Slug = product.Slug,
                Price = product.Price,
                Stock = product.Stock,
                CommentsCount = product.CommentsCount,
                Categories = CategoryOnlyNameDto.BuildAsStringList(product.ProductCategories),
                Tags = TagOnlyNameDto.BuildAsStringList(product.ProductTags),
                ImageUrls = product.ProductImages.Select(pi => pi.FilePath),
                PublishAt = product.PublishAt,
            };
        }
    }
}
