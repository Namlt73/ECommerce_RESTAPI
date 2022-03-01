using ApiEcommerce.Dtos.CategoryDtos;
using ApiEcommerce.Dtos.Handles;
using ApiEcommerce.Dtos.TagDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiEcommerce.Dtos.PageDtos
{
    public class HomeResponse : SuccessResponse
    {
        public List<CategoryDto> Categories { get; set; }

        public List<TagDto> Tags { get; set; }

        public static HomeResponse Build(List<Entities.Tag> tags, List<Entities.Category> categories)
        {
            var tagDtos = new List<TagDto>(tags.Count);
            var categoryDtos = new List<CategoryDto>(tags.Count);
            foreach (var tag in tags)
            {
                tagDtos.Add(TagDto.Build(tag));
            }

            foreach (var category in categories)
            {
                categoryDtos.Add(CategoryDto.Build(category));
            }

            return new HomeResponse
            {
                Tags = tagDtos,
                Categories = categoryDtos
            };
        }       
    }
}
